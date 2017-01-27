using Staffinfo.DAL.Models;

namespace Staffinfo.DAL.Mapping
{
    public class EducationItemMap: BaseMap<EducationItem>
    {
        public EducationItemMap() : base("tbl_Education")
        {
            this.Property(t => t.Institution).IsRequired().HasColumnName("Institution");
            this.Property(t => t.Speciality).IsRequired().HasColumnName("Speciality");
            this.Property(t => t.StartDate).IsRequired().HasColumnName("StartDate");
            this.Property(t => t.FinishDate).IsRequired().HasColumnName("FinishDate");
            this.Property(t => t.Description).IsRequired().HasColumnName("Description");

            //navigation properties
            this.Property(t => t.EmployeeId).IsOptional().HasColumnName("EmployeeID");
            HasOptional(t => t.Employee).WithMany().HasForeignKey(t => t.EmployeeId);
        }
    }
}