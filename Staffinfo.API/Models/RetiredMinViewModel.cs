using System;
using System.Runtime.Serialization;
using Staffinfo.DAL.Models;

namespace Staffinfo.API.Models
{
    [DataContract]
    public class RetiredMinViewModel: BaseViewModel
    {
        [DataMember(Name = "EmployeeFirstname")]
        public string EmployeeFirstname { get; set; }

        [DataMember(Name = "EmployeeLastname")]
        public string EmployeeLastname { get; set; }

        [DataMember(Name = "EmployeeMiddlename")]
        public string EmployeeMiddlename { get; set; }

        [DataMember(Name = "BirthDate")]
        public DateTime BirthDate { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "retirementDate")]
        public DateTime? RetirementDate { get; set; }

        public RetiredMinViewModel()
        {
            
        }

        public RetiredMinViewModel(Employee employee)
        {
            Id = employee.Id;
            EmployeeFirstname = employee.EmployeeFirstname;
            EmployeeLastname = employee.EmployeeLastname;
            EmployeeMiddlename = employee.EmployeeMiddlename;
            BirthDate = employee.BirthDate;
            Description = employee.Description;
            RetirementDate = employee.RetirementDate;
        }
    }
}