using System.Data.Entity.ModelConfiguration;
using Staffinfo.DAL.Models;

namespace Staffinfo.DAL.Mapping
{
    public class AddressMap: BaseMap<Address>
    {
        public AddressMap(): base("tbl_Address")
        {
            //properties
            this.Property(t => t.City).IsRequired().HasColumnName("City");
            this.Property(t => t.Area).IsRequired().HasColumnName("Area");
            this.Property(t => t.DetailedAddress).IsRequired().HasColumnName("DetailedAddress");
            this.Property(t => t.ZipCode).IsOptional().HasColumnName("ZipCode");
        }
    }
}