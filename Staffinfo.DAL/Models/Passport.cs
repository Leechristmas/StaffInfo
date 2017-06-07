using System.ComponentModel.DataAnnotations;
using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Models
{
    public class Passport: Entity
    {
        [Required]
        [MinLength(9)]
        [MaxLength(9)]
        public string PassportNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string PassportOrganization { get; set; }

        [Required]
        [MinLength(14)]
        [MaxLength(14)]
        public string IdentityNumber { get; set; }
    }
}