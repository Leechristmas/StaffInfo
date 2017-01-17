using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Staffinfo.DAL.Models;

namespace Staffinfo.API.Models
{
    [DataContract]
    public class DisciplineItemViewModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "employeeId")]
        public int? EmployeeId { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "itemType")]
        public string ItemType { get; set; }

        [DataMember(Name = "date")]
        public DateTime Date { get; set; }

        [DataMember(Name = "awardOrFine")]
        public long AwardOrFine { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        public DisciplineItemViewModel()
        {
            
        }

        public DisciplineItemViewModel(DisciplineItem disciplineItem)
        {
            Id = disciplineItem.Id;
            EmployeeId = disciplineItem.EmployeeId;
            ItemType = disciplineItem.ItemType;
            Title = disciplineItem.Title;
            AwardOrFine = disciplineItem.AwardOrFine;
            Description = disciplineItem.Description;
            Date = disciplineItem.Date;
        }
    }
}