using Staffinfo.DAL.Models;

namespace Staffinfo.DAL.Mapping
{
    public class ContractMap : BaseMap<Contract>
    {
        public ContractMap() : base("tbl_Contract")
        {
            this.Property(t => t.StartDate).IsRequired().HasColumnName("StartDate");
            this.Property(t => t.FinishDate).IsRequired().HasColumnName("FinishDate");
            this.Property(t => t.Description).IsOptional().HasColumnName("Description");

            //navigation properties
            this.Property(t => t.EmployeeId).IsOptional().HasColumnName("EmployeeID");
            HasOptional(t => t.Employee).WithMany().HasForeignKey(t => t.EmployeeId);

        }
    }
}
