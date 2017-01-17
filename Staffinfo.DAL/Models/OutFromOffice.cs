using System;
using System.ComponentModel.DataAnnotations;
using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Models
{
    public class OutFromOffice: Entity
    {
        [Required]
        public int? EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime FinishDate { get; set; }

        [Required]
        [MaxLength(1)]
        public string Cause { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

    }
}