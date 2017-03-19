using Microsoft.AspNet.Identity.EntityFramework;
using NLog;
using Staffinfo.API.Abstract;
using Staffinfo.API.Models;
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
        private readonly ILogger _logger;

        public UserController(IAuthRepository repo, ILogger logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        [Route("all")]
        public async Task<List<UserViewModel>> GetUsers()
        {
            var users = await _repo.GetUsersViewModelsAsync();

            return users;
        }

    }
}
