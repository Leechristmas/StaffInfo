using System;
using System.Runtime.Serialization;
using Staffinfo.DAL.Models;

namespace Staffinfo.API.Models
{
    public class MilitaryServiceViewModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "startDate")]
        public DateTime StartDate { get; set; }

        [DataMember(Name = "finishDate")]
        public DateTime FinishDate { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "employeeId")]
        public int? EmployeeId { get; set; }

        [DataMember(Name = "locationId")]
        public int? LocationId { get; set; }

        [DataMember(Name = "location")]
        public string Location { get; set; }

        [DataMember(Name = "rank")]
        public string Rank { get; set; }

        public MilitaryServiceViewModel()
        {
            
        }

        public MilitaryServiceViewModel(MilitaryService militaryService)
        {
            Id = militaryService.Id;
            StartDate = militaryService.StartDate;
            FinishDate = militaryService.FinishDate;
            Description = militaryService.Description;
            EmployeeId = militaryService.EmployeeId;
            LocationId = militaryService.LocationId;
            Location = militaryService.Location?.LocationName;
            Rank = militaryService.Rank;
        }
    }
}