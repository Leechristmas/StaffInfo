using Staffinfo.DAL.Models;

namespace Staffinfo.DAL.Mapping
{
    public class EmployeeMap: BaseMap<Employee>
    {
        public EmployeeMap(): base("tbl_Employee")
        {
            this.Property(t => t.EmployeeFirstname).HasColumnName("EmployeeFirstname");
            this.Property(t => t.EmployeeLastname).HasColumnName("EmployeeLastname");
            this.Property(t => t.EmployeeMiddlename).HasColumnName("EmployeeMiddlename");
            this.Property(t => t.BirthDate).IsRequired().HasColumnName("BirthDate");
            this.Property(t => t.RetirementDate).IsOptional().HasColumnName("RetirementDate");
            this.Property(t => t.PhotoMimeType).IsOptional().HasColumnName("PhotoMimeType");
            this.Property(t => t.EmployeePhoto).IsOptional().HasColumnName("EmployeePhoto");
            this.Property(t => t.Description).IsOptional().HasColumnName("Description");

            //navigation properties
            this.Property(t => t.ActualRankId).IsOptional().HasColumnName("ActualRankID");
            this.HasOptional(t => t.ActualRank).WithMany().HasForeignKey(t => t.ActualRankId);

            this.Property(t => t.ActualPostId).IsOptional().HasColumnName("ActualPostID");
            this.HasOptional(t => t.ActualPost).WithMany().HasForeignKey(t => t.ActualPostId);

            this.Property(t => t.PassportId).IsOptional().HasColumnName("PassportID");
            this.HasOptional(t => t.Passport).WithMany().HasForeignKey(t => t.PassportId);

            this.Property(t => t.AddressId).IsOptional().HasColumnName("AddressID");
            this.HasOptional(t => t.Address).WithMany().HasForeignKey(t => t.AddressId);
        }
    }
}