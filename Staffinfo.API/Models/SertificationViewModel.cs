using System;
using System.Runtime.Serialization;
using Staffinfo.DAL.Models;

namespace Staffinfo.API.Models
{
    [DataContract]
    public class SertificationViewModel : BaseViewModel
    {
        [DataMember]
        public int EmployeeId { get; set; }

        [DataMember]
        public DateTime DueDate { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Level { get; set; }

        public SertificationViewModel()
        {
            
        }

        public SertificationViewModel(Sertification sertification)
        {
            Id = sertification.Id;
            if (sertification.EmployeeId != null) EmployeeId = sertification.EmployeeId.Value;
            DueDate = sertification.DueDate;
            Description = sertification.Description;
            Level = sertification.Level;
        }

    }
}