using Staffinfo.DAL.Models;

namespace Staffinfo.DAL.Mapping
{
    public class WorkTermMap: BaseMap<WorkTerm>
    {
        public WorkTermMap(): base("tbl_WorkTerm")
        {
            this.Property(t => t.Post).HasColumnName("Post");
            this.Property(t => t.StartDate).IsRequired().HasColumnName("StartDate");
            this.Property(t => t.FinishDate).IsRequired().HasColumnName("FinishDate");
            this.Property(t => t.Description).HasColumnName("Description");

            //navigation properties
            this.Property(t => t.EmployeeId).IsOptional().HasColumnName("EmployeeID");
            HasOptional(t => t.Employee).WithMany().HasForeignKey(t => t.EmployeeId);

            this.Property(t => t.LocationId).IsOptional().HasColumnName("LocationID");
            HasOptional(t => t.Location).WithMany().HasForeignKey(t => t.LocationId);
        }
    }
}