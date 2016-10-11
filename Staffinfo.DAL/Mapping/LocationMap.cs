using Staffinfo.DAL.Models;

namespace Staffinfo.DAL.Mapping
{
    public class LocationMap: BaseMap<Location>
    {
        public LocationMap():base("tbl_Location")
        {
            this.Property(t => t.LocationName).IsRequired().HasColumnName("LocationName");
            this.Property(t => t.Address).IsOptional().HasColumnName("Address");
            this.Property(t => t.Description).IsOptional().HasColumnName("Description");
        }

    }
}