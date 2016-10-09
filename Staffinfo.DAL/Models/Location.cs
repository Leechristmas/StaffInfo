using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Models
{
    public class Location: Entity
    {
        public string LocationName { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
    }
}