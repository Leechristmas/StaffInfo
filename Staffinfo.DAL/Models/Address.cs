using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Models
{
    /// <summary>
    /// Employee's home address
    /// </summary>
    public class Address: Entity
    {
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Flat { get; set; }
    }
}