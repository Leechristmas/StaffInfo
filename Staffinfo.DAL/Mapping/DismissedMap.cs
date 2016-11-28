using Staffinfo.DAL.Models;

namespace Staffinfo.DAL.Mapping
{
    public class DismissedMap: BaseMap<Dismissed>
    {
        public DismissedMap(): base("tbl_Dismissed")
        {
            //properties
            this.Property(t => t.DismissedLastname).IsRequired().HasColumnName("DismissedLastname");
            this.Property(t => t.DismissedFirstname).IsRequired().HasColumnName("DismissedFirstname");
            this.Property(t => t.DismissedMiddlename).IsRequired().HasColumnName("DismissedMiddlename");
            this.Property(t => t.BirthDate).IsOptional().HasColumnName("BirthDate");
            this.Property(t => t.DismissalDate).IsOptional().HasColumnName("DismissalDate");
            this.Property(t => t.Clause).IsOptional().HasColumnName("Clause");
            this.Property(t => t.ClauseDescription).IsOptional().HasColumnName("ClauseDescription");
        }
    }
}