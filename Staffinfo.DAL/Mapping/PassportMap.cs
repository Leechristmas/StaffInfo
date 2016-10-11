using Staffinfo.DAL.Models;

namespace Staffinfo.DAL.Mapping
{
    public class PassportMap: BaseMap<Passport>
    {
        public PassportMap():base("tbl_Passport")
        {
            this.Property(t => t.PassportNumber).IsRequired().HasColumnName("PassportNumber");
            this.Property(t => t.PassportOrganization).IsRequired().HasColumnName("PassportOrganization");
        }
    }
}