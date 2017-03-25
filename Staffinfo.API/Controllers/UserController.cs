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
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private readonly IAuthRepository _repo = null;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly ILogger _logger;

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

    }
}
