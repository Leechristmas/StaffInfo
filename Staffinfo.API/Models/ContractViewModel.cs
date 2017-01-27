using System;
using System.Runtime.Serialization;
using Staffinfo.DAL.Models;

namespace Staffinfo.API.Models
{
    [DataContract]
    public class ContractViewModel: BaseViewModel
    {
        [DataMember]

        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime FinishDate { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int? EmployeeId { get; set; }

        public ContractViewModel()
        {
            
        }

        public ContractViewModel(Contract item)
        {
            Id = item.Id;
            StartDate = item.StartDate;
            FinishDate = item.FinishDate;
            EmployeeId = item.EmployeeId;
            Description = item.Description;
        }
    }
}