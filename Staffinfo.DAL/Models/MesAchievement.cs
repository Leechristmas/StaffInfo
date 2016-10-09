using System;
using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Models
{
    public class MesAchievement: Entity
    {
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string Description { get; set; }

        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int? LocationId { get; set; }
        public Location Location { get; set; }

        public int? PostId { get; set; }
        public Post Post { get; set; }

        public int? RankId { get; set; }
        public Rank Rank { get; set; }

    }
}