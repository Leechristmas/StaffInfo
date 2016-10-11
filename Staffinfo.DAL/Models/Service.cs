using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Models
{
    public class Service: Entity
    {
        public string ServiceName { get; set; }
        public string ServiceShortName { get; set; }
        public int ServiceGroupId { get; set; }
        public string Description { get; set; }
    }
}