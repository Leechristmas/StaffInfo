using System;
using System.Runtime.Serialization;
using Staffinfo.DAL.Models;

namespace Staffinfo.API.Models
{
    [DataContract]
    public class EmployeeViewModel: EmployeeViewModelMin
    {
        [DataMember(Name = "passportId")]
        public int? PassportId { get; set; }

        [DataMember(Name = "gender")]
        public string Gender { get; set; }

        [DataMember(Name = "personalNumber")]
        public string PersonalNumber { get; set; }

        [DataMember(Name = "passportNumber")]
        public string PassportNumber { get; set; }
    
        [DataMember(Name = "passportOrganization")]
        public string PassportOrganization { get; set; }

        [DataMember(Name = "passportIdentityNumber")]
        public string PassportIdentityNumber { get; set; }

        [DataMember(Name = "addressId")]
        public int? AddressId { get; set; }

        [DataMember(Name = "city")]
        public string City { get; set; }

        [DataMember(Name = "area")]
        public string Area { get; set; }

        [DataMember(Name = "detailedAddress")]
        public string DetailedAddress { get; set; }

        [DataMember(Name = "zipCode")]
        public string ZipCode { get; set; }
        
        [DataMember(Name = "firstPhoneNumber")]
        public string FirstPhoneNumber { get; set; }

        [DataMember(Name = "secondPhoneNumber")]
        public string SecondPhoneNumber { get; set; }

        [DataMember(Name = "retirementDate")]
        public DateTime? RetirementDate { get; set; }

        [DataMember(Name = "employeePhoto")]
        public byte[] EmployeePhoto { get; set; }

        public EmployeeViewModel()
        { }

        public EmployeeViewModel(Employee employee): base(employee)
        {
            AddressId = employee.AddressId;
            City = employee.Address.City;
            Area = employee.Address.Area;
            DetailedAddress = employee.Address.DetailedAddress;
            ZipCode = employee.Address.ZipCode;
            PassportId = employee.PassportId;
            PassportNumber = employee.Passport.PassportNumber;
            PassportOrganization = employee.Passport.PassportOrganization;
            PassportIdentityNumber = employee.Passport.IdentityNumber;
            FirstPhoneNumber = employee.FirstPhoneNumber;
            SecondPhoneNumber = employee.SecondPhoneNumber;
            RetirementDate = employee.RetirementDate;
            EmployeePhoto = employee.EmployeePhoto;
            Gender = employee.Gender;
            PersonalNumber = employee.PersonalNumber;
        }
    }
}