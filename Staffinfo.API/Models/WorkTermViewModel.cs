using System;
using System.Runtime.Serialization;
using Staffinfo.DAL.Models;

namespace Staffinfo.API.Models
{
    public class WorkTermViewModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "startDate")]
        public DateTime StartDate { get; set; }

        [DataMember(Name = "finishDate")]
        public DateTime? FinishDate { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "employeeId")]
        public int? EmployeeId { get; set; }

        [DataMember(Name = "locationId")]
        public int? LocationId { get; set; }

        [DataMember(Name = "location")]
        public string Location { get; set; }

        [DataMember(Name = "rank")]
        public string Post { get; set; }

        public WorkTermViewModel()
        {

        }

        public WorkTermViewModel(WorkTerm workTerm)
        {
            Id = workTerm.Id;
            StartDate = workTerm.StartDate;
            FinishDate = workTerm.FinishDate;
            Description = workTerm.Description;
            EmployeeId = workTerm.EmployeeId;
            LocationId = workTerm.LocationId;
            Location = workTerm.Location?.LocationName;
            Post = workTerm.Post;
        }
    }
}