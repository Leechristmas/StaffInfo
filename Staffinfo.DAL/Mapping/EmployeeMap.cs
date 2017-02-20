using System.ComponentModel.DataAnnotations.Schema;
using Staffinfo.DAL.Models;

namespace Staffinfo.DAL.Mapping
{
    public class EmployeeMap: BaseMap<Employee>
    {
        public EmployeeMap(): base("tbl_Employee")
        {
            //this.MapToStoredProcedures(p => p.Insert(sp => sp.HasName("sp_InsertEmployee")
            //    .Parameter(pm => pm.EmployeeFirstname, "firstname")
            //    .Parameter(pm => pm.EmployeeLastname, "lastname")
            //    .Parameter(pm => pm.EmployeeMiddlename, "middlename")
            //    .Parameter(pm => pm.Description, "description")
            //    .Parameter(pm => pm.ActualPostId, "actualPostId")
            //    .Parameter(pm => pm.ActualRankId, "actualRankId")
            //    .Parameter(pm => pm.AddressId, "addressId")
            //    .Parameter(pm => pm.BirthDate, "birthDate")
            //    .Parameter(pm => pm.EmployeePhoto, "employeePhoto")
            //    .Parameter(pm => pm.PhotoMimeType, "photoMimeType")
            //    .Parameter(pm => pm.FirstPhoneNumber, "firstPhoneNumber")
            //    .Parameter(pm => pm.SecondPhoneNumber, "secondPhoneNumber")
            //    .Parameter(pm => pm.Gender, "gender")
            //    .Parameter(pm => pm.PersonalNumber, "personalNumber")
            //    .Parameter(pm => pm.PassportId, "passportId")
            //    .Parameter(pm => pm.RetirementDate, "retirementDate")));

            this.Property(t => t.EmployeeFirstname).HasColumnName("EmployeeFirstname");
            this.Property(t => t.EmployeeLastname).HasColumnName("EmployeeLastname");
            this.Property(t => t.EmployeeMiddlename).HasColumnName("EmployeeMiddlename");
            this.Property(t => t.BirthDate).IsRequired().HasColumnName("BirthDate");
            this.Property(t => t.RetirementDate).IsOptional().HasColumnName("RetirementDate");
            this.Property(t => t.PhotoMimeType).IsOptional().HasColumnName("PhotoMimeType");
            this.Property(t => t.EmployeePhoto).IsOptional().HasColumnName("EmployeePhoto");
            this.Property(t => t.Description).IsOptional().HasColumnName("Description");
            this.Property(t => t.FirstPhoneNumber).IsOptional().HasColumnName("FirstPhoneNumber");
            this.Property(t => t.SecondPhoneNumber).IsOptional().HasColumnName("SecondPhoneNumber");
            this.Property(t => t.Gender).IsRequired().HasColumnName("Gender");
            this.Property(t => t.PersonalNumber).IsRequired().HasColumnName("PersonalNumber");
            this.Property(t => t.Seniority).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);//.IsRequired().HasColumnName("Seniority");

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