using System;
using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Models
{
    public class MilitaryService: Entity
    {
        public string Rank { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string Description { get; set; }

        //navigation properties
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int? LocationId { get; set; }
        public Location Location { get; set; }
        
    }
}