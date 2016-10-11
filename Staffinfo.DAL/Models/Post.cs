using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Models
{
    public class Post: Entity
    {
        public string PostName { get; set; }
        public int PostWeight { get; set; }

        public int? ServiceId { get; set; }
        public virtual Service Service { get; set; }
    }
}