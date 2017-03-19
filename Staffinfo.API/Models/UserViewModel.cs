using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Staffinfo.API.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public List<string> Roles { get; set; }
        public int? EmployeeId { get; set; }
        public byte[] EmployeePhoto { get; set; }

        public UserViewModel(string id, string login, string email, int? employeeId, string lastname, string firstname, string middlename, List<string> roles, byte[] employeePhoto)
        {
            Id = Id;
            Login = login;
            Email = email;
            Lastname = lastname;
            Firstname = firstname;
            Middlename = middlename;
            Roles = roles;
            EmployeeId = employeeId;
            EmployeePhoto = employeePhoto;
        }

        public UserViewModel(IdentityUser user)
        {
            Id = user.Id;
            Login = user.UserName;
            Email = user.Email;
            Lastname = user.Claims.FirstOrDefault(c => c.ClaimType == "lastname")?.ClaimValue;
            Firstname = user.Claims.FirstOrDefault(c => c.ClaimType == "firstname")?.ClaimValue;
            Middlename = user.Claims.FirstOrDefault(c => c.ClaimType == "middlename")?.ClaimValue;

            int emplId;

            if (Int32.TryParse(user.Claims.FirstOrDefault(c => c.ClaimType == "employeeId")?.ClaimValue, out emplId))
                EmployeeId = emplId;
            else
                EmployeeId = null;

        }

        public UserViewModel()
        {

        }
    }
}