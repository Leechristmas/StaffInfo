using System;
using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Models
{
    public class MesAchievement: Entity
    {
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string Description { get; set; }

        public int? EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public int? LocationId { get; set; }
        public virtual Location Location { get; set; }

        public int? PostId { get; set; }
        public virtual Post Post { get; set; }

        public int? RankId { get; set; }
        public virtual Rank Rank { get; set; }

    }
}