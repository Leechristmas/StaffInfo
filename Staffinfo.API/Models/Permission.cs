using Staffinfo.API.Controllers;

namespace Staffinfo.API.Models
{
    public class AddRemovePermissionModel
    {
        public string UserId { get; set; }
        public string Role { get; set; }
        public PermissionAction Action { get; set; }
    }
}