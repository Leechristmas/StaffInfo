using System;
using System.ComponentModel.DataAnnotations;
using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Models
{
    public class EducationItem: Entity
    {
        [Required]
        [MaxLength(100)]
        public string Institution { get; set; }

        [Required]
        [MaxLength(100)]
        public string Speciality { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime FinishDate { get; set; }
        
        [MaxLength(200)]
        public string Description { get; set; }

        public int? EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

    }
}