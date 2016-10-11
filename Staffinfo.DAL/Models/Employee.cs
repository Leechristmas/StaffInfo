using System;
using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Models
{
    public class Employee: Entity
    {
        public string EmployeeFirstname { get; set; }
        public string EmployeeLastname { get; set; }
        public string EmployeeMiddlename { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsPensioner { get; set; }
        public byte[] EmployeePhoto { get; set; }
        public string PhotoMimeType { get; set; }

        //navigation properties
        public int? ActualRankId { get; set; }
        public virtual Rank ActualRank { get; set; }

        public int? ActualPostId { get; set; }
        public virtual Post ActualPost { get; set; }

        public int? PassportId { get; set; }
        public virtual Passport Passport { get; set; }

        public int? AddressId { get; set; }
        public virtual Address Address { get; set; }
    }
}