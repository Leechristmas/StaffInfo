using Staffinfo.DAL.Models;

namespace Staffinfo.DAL.Mapping
{
    public class SertificationMap: BaseMap<Sertification>
    {
        public SertificationMap() : base("tbl_Sertification")
        {
            this.Property(t => t.DueDate).IsRequired().HasColumnName("DueDate");
            this.Property(t => t.Description).HasColumnName("Description");

            this.Property(t => t.EmployeeId).IsOptional().HasColumnName("EmployeeID");
            this.HasOptional(t => t.Employee).WithMany().HasForeignKey(t => t.EmployeeId);
        }
    }
}