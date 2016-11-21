using System.Runtime.Serialization;

namespace Staffinfo.API.Models
{
    [DataContract]
    public class NamedEntity
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        
        [DataMember(Name = "name")]
        public string Name { get; set; } 
    }
}