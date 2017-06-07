using System;
using System.ComponentModel.DataAnnotations;
using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Models
{
    public class Relative: Entity
    {
        [Required]
        [MaxLength(30)]
        public string Lastname { get; set; }

        [Required]
        [MaxLength(30)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(30)]
        public string Middlename { get; set; } 
        public DateTime BirthDate { get; set; }

        [Required]
        [MaxLength(15)]
        public string Status { get; set; }

        public int? EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

    }
}