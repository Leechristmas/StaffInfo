using System.Data.Entity.ModelConfiguration;
using Staffinfo.DAL.Models;

namespace Staffinfo.DAL.Mapping
{
    public class AddressMap: BaseMap<Address>
    {
        public AddressMap(string tableName) : base(tableName)
        { }

        public AddressMap(): this("tbl_Address")
        {
            //properties
            this.Property(t => t.City).IsRequired().HasColumnName("City");
            this.Property(t => t.Street).IsRequired().HasColumnName("Street");
            this.Property(t => t.House).IsRequired().HasColumnName("House");
            this.Property(t => t.Flat).IsOptional().HasColumnName("Flat");
        }
    }
}