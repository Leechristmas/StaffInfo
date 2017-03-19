using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Staffinfo.API.Models;
using System.Collections.Generic;

namespace Staffinfo.API.Abstract
{
    public interface IAuthRepository: IDisposable
    {
        Task<IdentityResult> RegisterUser(UserModel userModel);
        Task<IdentityResult> RegisterUserAsync(UserViewModel userModel);
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityUser> FindUser(string userName, string password);
        Task<List<IdentityUser>> GetUsersAsync();
        Task<List<UserViewModel>> GetUsersViewModelsAsync();
    }
}