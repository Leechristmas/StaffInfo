using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Staffinfo.API.Models
{
    [DataContract]
    public class Dismissal
    {
        [DataMember(Name = "employeeId")]
        [Required]
        public int? EmployeeId { get; set; }

        [DataMember(Name = "dismissalDate")]
        [Required]
        public DateTime? DismissalDate { get; set; }

        [DataMember(Name = "clause")]
        [Required]
        public string Clause { get; set; }

        [DataMember(Name = "clauseDescription")]
        [Required]
        public string ClauseDescription { get; set; }
    }
}