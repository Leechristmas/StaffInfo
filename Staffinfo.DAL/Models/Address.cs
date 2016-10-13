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
        public string Street { get; set; }

        [MaxLength(10)]
        [Required]
        public string House { get; set; }

        [MaxLength(5)]
        public string Flat { get; set; }
    }
}