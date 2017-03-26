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
        Task<IdentityResult> RegisterUser(UserViewModel userModel);
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityUser> FindUser(string userName, string password);
        Task<IdentityUser> FindUser(string userId);
        Task<List<IdentityUser>> GetUsersAsync();
        Task<List<UserViewModel>> GetUsersViewModelsAsync();
        Task<IdentityResult> DeleteUser(string userId);
        Task RemoveRoleFromUser(string userId, string role);
        Task AddRoleToUser(string userId, string role);
        Task<IdentityResult> ChangePassword(string userId, string currentPassword, string newPassword);
    }
}