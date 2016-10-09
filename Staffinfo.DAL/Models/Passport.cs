using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Models
{
    public class Passport: Entity
    {
        public string PassportNumber { get; set; }
        public string PassportOrganization { get; set; }
    }
}