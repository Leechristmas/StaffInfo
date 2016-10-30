using System.ComponentModel.DataAnnotations;
using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Models
{
    /// <summary>
    /// Employee's home address
    /// </summary>
    public class Address: Entity
    {
        [MaxLength(60)]
        [Required]
        public string City { get; set; }

        [MaxLength(60)]
        [Required]
        public string Area { get; set; }

        [MaxLength(100)]
        [Required]
        public string DetailedAddress { get; set; }

        [MaxLength(6)]
        [Required]
        public string ZipCode { get; set; }
    }
}