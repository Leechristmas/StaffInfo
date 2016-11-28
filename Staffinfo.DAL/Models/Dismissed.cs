using System;
using System.ComponentModel.DataAnnotations;
using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Models
{
    public class Dismissed: Entity
    {
        [MaxLength(30)]
        [Required]
        public string DismissedLastname { get; set; }

        [MaxLength(30)]
        [Required]
        public string DismissedFirstname { get; set; }

        [MaxLength(30)]
        [Required]
        public string DismissedMiddlename { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public DateTime DismissalDate { get; set; }

        [MaxLength(10)]
        public string Clause { get; set; }

        [MaxLength(150)]
        public string ClauseDescription { get; set; }
    }
}