using System;
using System.Runtime.Serialization;
using Staffinfo.DAL.Models;

namespace Staffinfo.API.Models
{
    [DataContract]
    public class OutFromOfficeViewModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "employeeId")]
        public int? EmployeeId { get; set; }

        [DataMember(Name = "Cause")]
        public string Cause { get; set; }

        [DataMember(Name = "startDate")]
        public DateTime StartDate { get; set; }

        [DataMember(Name = "finishDate")]
        public DateTime FinishDate { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }


        public OutFromOfficeViewModel()
        {
            
        }

        public OutFromOfficeViewModel(OutFromOffice outFromOffice)
        {
            Id = outFromOffice.Id;
            EmployeeId = outFromOffice.EmployeeId;
            StartDate = outFromOffice.StartDate;
            FinishDate = outFromOffice.FinishDate;
            Cause = outFromOffice.Cause;
            Description = outFromOffice.Description;
        }

    }
}