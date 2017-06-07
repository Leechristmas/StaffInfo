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
            this.Property(t => t.Description).IsOptional().HasColumnName("Description");

            //navigation properties
            this.Property(t => t.EmployeeId).IsOptional().HasColumnName("EmployeeID");
            HasOptional(t => t.Employee).WithMany().HasForeignKey(t => t.EmployeeId);

            this.Property(t => t.LevelCode).IsOptional().HasColumnName("LevelCode");
            HasOptional(t => t.EducationLevel).WithMany().HasForeignKey(t => t.LevelCode);
        }
    }

    public class EducationLevelMap : BaseMap<EducationLevel>
    {
        public EducationLevelMap(): base("tbl_EducationLevel")
        {
            this.Property(t => t.Transcript).IsRequired().HasColumnName("Transcript");
            this.Property(t => t.Weight).IsRequired().HasColumnName("Weight");
            this.Property(t => t.Description).IsOptional().HasColumnName("Description");
        }
    }

}