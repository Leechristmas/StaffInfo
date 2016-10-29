using System.Runtime.Serialization;

namespace Staffinfo.API.Models
{
    [DataContract]
    public class BaseViewModel
    {
        [DataMember(Name = "id")]
        public int? Id { get; set; } 
    }
}