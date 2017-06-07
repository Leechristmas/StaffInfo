using System;
using System.Runtime.Serialization;
using Staffinfo.DAL.Models;

namespace Staffinfo.API.Models
{
    [DataContract]
    public class EducationViewModel: BaseViewModel
    {
        [DataMember]
        public string Institution { get; set; }

        [DataMember]
        public string Speciality { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime FinishDate { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int? EmployeeId { get; set; }

        [DataMember]
        public int? EducationLevelId { get; set; }
        
        [DataMember]
        public string EducationLevel { get; set; }
        
        public EducationViewModel()
        {
            
        }

        public EducationViewModel(EducationItem item)
        {
            Id = item.Id;
            Institution = item.Institution;
            Speciality = item.Speciality;
            StartDate = item.StartDate;
            EducationLevelId = item.LevelCode;
            EducationLevel = $"{item.EducationLevel.Transcript}" +
                             (String.IsNullOrEmpty(item.EducationLevel.Description)
                                 ? ""
                                 : $"({item.EducationLevel.Description})");
            FinishDate = item.FinishDate;
            EmployeeId = item.EmployeeId;
            Description = item.Description;
        }
    }
}