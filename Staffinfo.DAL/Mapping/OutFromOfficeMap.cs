using Staffinfo.DAL.Models;

namespace Staffinfo.DAL.Mapping
{
    public class OutFromOfficeMap: BaseMap<OutFromOffice>
    {
        public OutFromOfficeMap() : base("tbl_OutFromOffice")
        {
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.FinishDate).HasColumnName("FinishDate");
            this.Property(t => t.Cause).HasColumnName("Cause");
            this.Property(t => t.Description).HasColumnName("Description");

            this.Property(t => t.EmployeeId).IsOptional().HasColumnName("EmployeeID");
            HasOptional(t => t.Employee).WithMany().HasForeignKey(t => t.EmployeeId);
        }
    }
}