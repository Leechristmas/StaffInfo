using System;
using System.ComponentModel.DataAnnotations;
using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Models
{
    public class Sertification: Entity
    {
        public int? EmployeeId { get; set; }
     
        public virtual Employee Employee { get; set; }

        [Required]   
        public DateTime DueDate { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [MaxLength(15)]
        public string Level { get; set; }

    }
}