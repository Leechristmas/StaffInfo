using Microsoft.AspNet.Identity.EntityFramework;
using NLog;
using Staffinfo.API.Abstract;
using Staffinfo.API.Models;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Staffinfo.API.Controllers
{
    public enum PermissionAction
    {
        Remove = 0,
        Add
    }


    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private readonly IAuthRepository _repo = null;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly ILogger _logger;

        private readonly string[] _roles = new[] {"admin", "editor", "reader"};

        public UserController(IAuthRepository repo, ILogger logger, IRepository<Employee> emplRepo)
        {
            _repo = repo;
            _logger = logger;
            _employeeRepository = emplRepo;
        }

        [HttpGet]
        [Route("all")]
        public async Task<List<UserViewModel>> GetUsers()
        {
            var users = await _repo.GetUsersViewModelsAsync();

            return users;
        }

        [HttpPost]
        [Route("all/permissions")]
        public async Task SetPermission([FromBody]AddRemovePermissionModel model)
        {
            var user = await _repo.FindUser(model.UserId);
            if(!_roles.Contains(model.Role)) throw new Exception("Incorrect role!");
            if (user == null) throw new Exception("The user not found!");

            if (model.Action == PermissionAction.Add)
                await _repo.AddRoleToUser(model.UserId, model.Role);
            else if (model.Action == PermissionAction.Remove)
                await _repo.RemoveRoleFromUser(model.UserId, model.Role);
        }

        [HttpGet]
        [Route("not-registered")]
        public async Task<List<NamedEntity>> GetNotRegisteredUsers()
        {
            var registered = (await _repo.GetUsersViewModelsAsync()).Where(u => u.EmployeeId != null).Select(u => u.EmployeeId);
            var query = await _employeeRepository.WhereAsync(e => !registered.Contains(e.Id));

            List<NamedEntity> empls = query.Select(e => new NamedEntity { Id = e.Id, Name = $"{e.EmployeeLastname} - {e.ActualPost?.PostName}" }).ToList();

            return empls;
        }

        [HttpPost]
        [Route("register")]
        public async Task Register([FromBody] UserViewModel userModel)
        {
            var result = await _repo.RegisterUser(userModel);
        }

        [HttpDelete]
        [Route("all/{accountId}")]
        public async Task DeleteAccount([FromUri] string accountId)
        {
            var result = await _repo.DeleteUser(accountId);
        }


        [HttpPost]
        [Route("all/change-password")]
        public async Task ChangePassword([FromBody]ChangePasswordModel model)
        {
            var result = await _repo.ChangePassword(model.UserId, model.CurrentPassword, model.NewPassword);

            if(!result.Succeeded) throw new Exception(String.Join(";", result.Errors));
        }

    }
}
