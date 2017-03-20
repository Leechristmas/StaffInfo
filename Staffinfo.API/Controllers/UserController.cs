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
        [Route("employees")]
        public async Task<List<NamedEntity>> GetEmployees()
        {
            var query = await _employeeRepository.SelectAsync();

            List<NamedEntity> empls = query.Select(e => new NamedEntity { Id = e.Id, Name = $"{e.EmployeeLastname} - {e.ActualPost?.PostName}" }).ToList();

            return empls;
        }

        [HttpPost]
        [Route("register")]
        public async Task Register([FromBody] UserViewModel userModel)
        {
            var result = await _repo.RegisterUser(userModel);
        }

    }
}
