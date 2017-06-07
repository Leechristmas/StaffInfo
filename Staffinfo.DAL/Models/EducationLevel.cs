using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Models
{
    public class EducationLevel: Entity
    {
        public string Transcript { get; set; }
        public short Weight { get; set; }
        public string Description { get; set; } 
    }
}