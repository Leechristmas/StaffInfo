using Staffinfo.DAL.Models;

namespace Staffinfo.DAL.Mapping
{
    public class PostMap: BaseMap<Post>
    {
        public PostMap(): base("tbl_Post")
        {
            this.Property(t => t.PostName).IsRequired().HasColumnName("PostName");
            this.Property(t => t.PostWeight).IsRequired().HasColumnName("PostWeight");

            //navigation properties
            this.Property(t => t.ServiceId).IsOptional().HasColumnName("ServiceID");
            HasOptional(t => t.Service).WithMany().HasForeignKey(t => t.ServiceId);
        }
    }
}