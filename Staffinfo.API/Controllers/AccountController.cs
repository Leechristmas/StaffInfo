using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using NLog;
using Staffinfo.API.Abstract;
using Staffinfo.API.Models;

namespace Staffinfo.API.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private readonly IAuthRepository _repo = null;
        private readonly ILogger _logger;

        public AccountController(IAuthRepository repo, ILogger logger)
        {
            _logger = logger;
            _repo = repo;
        }
        
        // POST api/Account/Register
        [Authorize(Roles = "admin")]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _repo.RegisterUser(userModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            _logger.Log(LogLevel.Info, $"Зарегестрирован пользователь \"{userModel.UserName}\"");

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [Route("Roles/{roleName}")]
        public async Task<IHttpActionResult> RegisterRole(string roleName)
        {
            IdentityResult result = await _repo.CreateRoleAsync(roleName);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }
            
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
