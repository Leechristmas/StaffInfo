using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Staffinfo.API.Abstract;
using Staffinfo.API.Controllers;
using Staffinfo.API.Models;

namespace Staffinfo.API.Tests
{
    [TestClass]
    public class AccountControllerTests
    {
        private AccountController _controller;
        
        [TestInitialize]
        public void SetUp()
        {
            var repo = new Mock<IAuthRepository>();
            repo.Setup(r => r.RegisterUser(It.IsAny<UserModel>()))
                .Returns<UserModel>(u => String.Equals(u.UserName, "qwerty")
                    ? Task.Run(() => IdentityResult.Success)
                    : Task.Run(() => IdentityResult.Failed("test")));
            //repo.Setup(r => r.FindUser(It.IsAny<string>(), It.IsAny<string>())).Returns<string, string>(
            //    (name, password) =>
            //    {
            //        return Task.Run((() => new IdentityUser(name)));
            //    });


            _controller = new AccountController(repo.Object, null);
        }
        
        [TestMethod]
        public void RegisterNewUser_ShouldReturnOK()
        {
            IHttpActionResult result;

            UserModel user = new UserModel
            {
                UserName = "qwerty",
                Password = "123456",
                ConfirmPassword = "123456"
            };
            result = _controller.Register(user).Result;

            Assert.IsTrue(result.GetType() == typeof(OkResult));
        }

        [TestMethod]
        public void RegisterNewUser_ShouldReturnFailed()
        {
            IHttpActionResult result;

            UserModel user = new UserModel
            {
                UserName = "aaa",
                Password = "123456",
                ConfirmPassword = "123456"
            };
            result = _controller.Register(user).Result;

            Assert.IsTrue(result.GetType() != typeof(OkResult));
        }
    }
}
