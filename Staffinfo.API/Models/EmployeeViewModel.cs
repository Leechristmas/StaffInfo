using System;
using System.Runtime.Serialization;
using Staffinfo.DAL.Models;

namespace Staffinfo.API.Models
{
    [DataContract]
    public sealed class EmployeeViewModel: BaseViewModel
    {
        [DataMember(Name = "EmployeeFirstname")]
        public string EmployeeFirstname { get; set; }

        [DataMember(Name = "EmployeeLastname")]
        public string EmployeeLastname { get; set; }

        [DataMember(Name = "EmployeeMiddlename")]
        public string EmployeeMiddlename { get; set; }

        [DataMember(Name = "BirthDate")]
        public DateTime BirthDate { get; set; }

        [DataMember(Name = "ActualRank")]
        public string ActualRank { get; set; }

        [DataMember(Name = "ActualPost")]
        public string ActualPost { get; set; }

        public EmployeeViewModel()
        {
            
        }

        public EmployeeViewModel(Employee employee)
        {
            EmployeeFirstname = employee.EmployeeFirstname;
            EmployeeLastname = employee.EmployeeLastname;
            EmployeeMiddlename = employee.EmployeeMiddlename;
            BirthDate = employee.BirthDate;
            ActualRank = employee.ActualRank?.RankName;
            ActualPost = employee.ActualPost?.PostName;
        }
    }
}