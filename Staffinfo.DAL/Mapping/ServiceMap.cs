using Staffinfo.DAL.Models;

namespace Staffinfo.DAL.Mapping
{
    public class ServiceMap: BaseMap<Service>
    {
        public ServiceMap(): base("tbl_Service")
        {
            this.Property(t => t.ServiceName).IsRequired().HasColumnName("ServiceName");
            this.Property(t => t.ServiceShortName).IsOptional().HasColumnName("ServiceShortName");
            this.Property(t => t.ServiceGroupId).IsRequired().HasColumnName("ServiceGroupId");
            this.Property(t => t.Description).IsOptional().HasColumnName("Description");
        }
    }
}