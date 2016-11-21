using System;
using System.Runtime.Serialization;
using Staffinfo.DAL.Models;

namespace Staffinfo.API.Models
{
    [DataContract]
    public class MesAchievementViewModel
    {
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

        [DataMember(Name = "postId")]
        public int? PostId { get; set; }

        [DataMember(Name = "post")]
        public string Post { get; set; }

        [DataMember(Name = "rankId")]
        public int? RankId { get; set; }

        [DataMember(Name = "rank")]
        public string Rank { get; set; }

        public MesAchievementViewModel()
        {
            
        }

        public MesAchievementViewModel(MesAchievement mesAchievement)
        {
            StartDate = mesAchievement.StartDate;
            FinishDate = mesAchievement.FinishDate;
            Description = mesAchievement.Description;
            EmployeeId = mesAchievement.EmployeeId;
            LocationId = mesAchievement.LocationId;
            Location = mesAchievement.Location?.LocationName;
            PostId = mesAchievement.PostId;
            Post = mesAchievement.Post?.PostName;
            RankId = mesAchievement.RankId;
            Rank = mesAchievement.Rank?.RankName;   
        }


    }
}