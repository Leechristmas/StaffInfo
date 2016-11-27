using System;
using System.Runtime.Serialization;

namespace Staffinfo.API.Models
{
    [DataContract]
    public class Dismissal
    {
        [DataMember(Name = "employeeId")]
        public int? EmployeeId { get; set; }

        [DataMember(Name = "dismissalDate")]
        public DateTime? DismissalDate { get; set; }

        [DataMember(Name = "clause")]
        public string Clause { get; set; }

        [DataMember(Name = "clauseDescription")]
        public string ClauseDescription { get; set; }

        public bool IsCorrect
            =>
                EmployeeId != null && DismissalDate != null && !String.IsNullOrEmpty(Clause) &&
                !String.IsNullOrEmpty(ClauseDescription);
    }
}