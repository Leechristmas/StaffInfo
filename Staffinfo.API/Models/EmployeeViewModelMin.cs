using System;
using System.Runtime.Serialization;
using Staffinfo.DAL.Models;

namespace Staffinfo.API.Models
{
    [DataContract]
    public class EmployeeViewModelMin: BaseViewModel
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

        [DataMember(Name = "actualRankId")]
        public int? ActualRankId { get; set; }

        [DataMember(Name = "ActualPost")]
        public string ActualPost { get; set; }

        [DataMember(Name = "actualPostId")]
        public int? ActualPostId { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        public EmployeeViewModelMin()
        {
            
        }

        public EmployeeViewModelMin(Employee employee)
        {
            Id = employee.Id;
            EmployeeFirstname = employee.EmployeeFirstname;
            EmployeeLastname = employee.EmployeeLastname;
            EmployeeMiddlename = employee.EmployeeMiddlename;
            BirthDate = employee.BirthDate;
            ActualRank = employee.ActualRank?.RankName;
            ActualPost = employee.ActualPost?.PostName;
            ActualPostId = employee.ActualPostId;
            ActualRankId = employee.ActualRankId;
            Description = employee.Description;
        }

        /// <summary>
        /// Returns employee instance from specified view-model
        /// </summary>
        /// <param name="model">view-model</param>
        /// <returns></returns>
        public static Employee GetEmployeeFromModel(EmployeeViewModelMin model)
        {
            return new Employee()
            {
                ActualPostId = model.ActualPostId,
                ActualRankId = model.ActualRankId,
                EmployeeFirstname = model.EmployeeFirstname,
                EmployeeLastname = model.EmployeeLastname,
                EmployeeMiddlename = model.EmployeeMiddlename,
                BirthDate = model.BirthDate,
                Description = model.Description
            };
        }
    }
}