using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Staffinfo.API.Abstract;
using Staffinfo.API.Models;
using System.Linq;
using System.Data.Entity;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.API
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AuthContext _ctx;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepository<Employee> _employeeRepository;

        public AuthRepository(IRepository<Employee> employeeRepository)
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_ctx));
            _employeeRepository = employeeRepository;
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.UserName
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            if (result.Succeeded)
            {
                var registered = await _userManager.FindAsync(userModel.UserName, userModel.Password);
                result = await _userManager.AddToRoleAsync(registered.Id, @"reader");
            }

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            IdentityRole role = new IdentityRole(roleName);

            var result = await _roleManager.CreateAsync(role);

            return result;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }

        public async Task<List<IdentityUser>> GetUsersAsync()
        {
            var users = await _ctx.Users.ToListAsync();
            return users;
        }

        public async Task<List<UserViewModel>> GetUsersViewModelsAsync()
        {
            var usersIds = await _ctx.Users.ToListAsync();

            var users = usersIds.Select(u => new UserViewModel(u)).ToList();
            
            foreach(var u in users)
            {
                u.Roles = (await _userManager.GetRolesAsync(u.Id)).ToList();
                if(u.EmployeeId != null)
                {
                    var empl = await _employeeRepository.SelectAsync(u.EmployeeId.Value);

                    u.EmployeePhoto = empl.EmployeePhoto;
                    u.Firstname = empl.EmployeeFirstname;
                    u.Lastname = empl.EmployeeLastname;
                    u.Middlename = empl.EmployeeMiddlename;
                }
            }

            return users;
        }
        
        public async Task<IdentityResult> RegisterUser(UserViewModel userModel)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.Login,
                Email = userModel.Email
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            if (result.Succeeded)
            {
                var registered = await _userManager.FindAsync(userModel.Login, userModel.Password);
                result = await _userManager.AddToRolesAsync(registered.Id, userModel.Roles.ToArray());
                result = await _userManager.AddClaimAsync(registered.Id, new System.Security.Claims.Claim("employeeId", userModel.EmployeeId.Value.ToString()));
            }

            return result;
        }
    }
}