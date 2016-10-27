using System;
using System.Runtime.Serialization;
using Staffinfo.DAL.Models;

namespace Staffinfo.API.Models
{
    [DataContract]
    public sealed class EmployeeViewModel
    {
        [DataMember(Name = "EmployeeFirstname")]
        public string EmployeeFirstname { get; set; }

        [DataMember(Name = "EmployeeLastname")]
        public string EmployeeLastname { get; set; }

        [DataMember(Name = "EmployeeMiddlename")]
        public string EmployeeMiddlename { get; set; }

        [DataMember(Name = "BirthDate")]
        public DateTime BirthDate { get; set; }

        [DataMember(Name = "IsPensioner")]
        public bool IsPensioner { get; set; }
        //public byte[] EmployeePhoto { get; set; }
        //public string PhotoMimeType { get; set; }
        
        [DataMember(Name = "ActualRankId")]
        public int? ActualRankId { get; set; }

        [DataMember(Name = "ActualRank")]
        public string ActualRank { get; set; }

        [DataMember(Name = "ActualPostId")]
        public int? ActualPostId { get; set; }

        [DataMember(Name = "ActualPost")]
        public string ActualPost { get; set; }

        [DataMember(Name = "PassportId")]
        public int? PassportId { get; set; }

        [DataMember(Name = "Passport")]
        public string Passport { get; set; }

        [DataMember(Name = "AddressId")]
        public int? AddressId { get; set; }

        [DataMember(Name = "Address")]
        public string Address { get; set; }

        public EmployeeViewModel()
        {
            
        }

        public EmployeeViewModel(Employee employee)
        {
            EmployeeFirstname = employee.EmployeeFirstname;
            EmployeeLastname = employee.EmployeeLastname;
            EmployeeMiddlename = employee.EmployeeMiddlename;
            BirthDate = employee.BirthDate;
            IsPensioner = employee.IsPensioner;
            ActualRankId = employee.ActualRankId;
            ActualRank = employee.ActualRank?.RankName;
            ActualPostId = employee.ActualPostId;
            ActualPost = employee.ActualPost?.PostName;
            PassportId = employee.PassportId;
            Passport = employee.Passport?.PassportNumber;
            AddressId = employee.AddressId;
            Address = $"г. {employee.Address?.City}, ул. {employee.Address?.Street}, д. {employee.Address?.House}" +
                      (String.IsNullOrEmpty(employee.Address?.Flat) ? "" : $"/{employee.Address.Flat}");
        }
    }
}