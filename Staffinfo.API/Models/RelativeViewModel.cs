using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Staffinfo.DAL.Models;

namespace Staffinfo.API.Models
{
    [DataContract]
    public class RelativeViewModel: BaseViewModel
    {
        [Required]
        [MaxLength(30)]
        [DataMember]
        public string Lastname { get; set; }

        [Required]
        [MaxLength(30)]
        [DataMember]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(30)]
        [DataMember]
        public string Middlename { get; set; }

        [DataMember]
        public DateTime BirthDate { get; set; }

        [Required]
        [MaxLength(15)]
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public int? EmployeeId { get; set; }

        public RelativeViewModel()
        {
            
        }

        public RelativeViewModel(Relative relative)
        {
            Id = relative.Id;
            Lastname = relative.Lastname;
            Firstname = relative.Firstname;
            Middlename = relative.Middlename;
            BirthDate = relative.BirthDate;
            Status = relative.Status;
            EmployeeId = relative.EmployeeID;
        }

    }
}