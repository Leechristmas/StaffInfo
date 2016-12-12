using System.Runtime.Serialization;

namespace Staffinfo.API.Models
{
    [DataContract]
    public class Seniority
    {
        [DataMember(Name = "employeeId")]
        public int EmployeeId { get; set; }

        [DataMember(Name = "mesSeniorityDays")]
        public int MESSeniorityDays { get; set; }

        [DataMember(Name = "militarySeniorityDays")]
        public int MilitarySeniorityDays { get; set; }

        [DataMember(Name = "workSeniorityDays")]
        public int WorkSeniorityDays { get; set; }
    }
}