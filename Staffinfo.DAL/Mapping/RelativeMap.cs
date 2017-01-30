using Staffinfo.DAL.Models;

namespace Staffinfo.DAL.Mapping
{
    public class RelativeMap: BaseMap<Relative>
    {
        public RelativeMap() : base("tbl_Relative")
        {
            this.Property(t => t.Lastname).IsRequired().HasColumnName("Lastname");
            this.Property(t => t.Firstname).IsRequired().HasColumnName("Firstname");
            this.Property(t => t.Middlename).IsRequired().HasColumnName("Middlename");
            this.Property(t => t.BirthDate).IsRequired().HasColumnName("BirthDate");
            this.Property(t => t.Status).IsRequired().HasColumnName("Status");

            this.Property(t => t.EmployeeID).IsOptional().HasColumnName("EmployeeID");
            this.HasOptional(t => t.Employee).WithMany().HasForeignKey(t => t.EmployeeID);
        }
    }
}