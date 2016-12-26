using System.Data.Entity.ModelConfiguration;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Mapping
{
    public class DisciplineItemMap: BaseMap<DisciplineItem>
    {
        public DisciplineItemMap(): base("tbl_GratitudesAndPunishment")
        {
            this.Property(t => t.Title).IsRequired().HasColumnName("Title");
            this.Property(t => t.ItemType).HasColumnName("ItemType");
            this.Property(t => t.Date).IsRequired().HasColumnName("Date");
            this.Property(t => t.AwardOrFine).IsRequired().HasColumnName("AwardOrFine");
            this.Property(t => t.Description).HasColumnName("Description");

            //navigation properties
            this.Property(t => t.EmployeeId).IsOptional().HasColumnName("EmployeeID");
            HasOptional(t => t.Employee).WithMany().HasForeignKey(t => t.EmployeeId);
        }

    }
}