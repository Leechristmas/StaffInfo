using System.Runtime.Serialization;
using Staffinfo.DAL.Models;

namespace Staffinfo.API.Models
{
    [DataContract]
    public class EmployeeViewModel: EmployeeViewModelMin
    {
        [DataMember(Name = "passportId")]
        public int? PassportId { get; set; }

        [DataMember(Name = "passportNumber")]
        public string PassportNumber { get; set; }
    
        [DataMember(Name = "passportOrganization")]
        public string PassportOrganization { get; set; }

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
        }
    }
}