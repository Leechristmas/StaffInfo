using System;
using System.ComponentModel.DataAnnotations;
using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Models
{
    //Gratitude or punishment
    public class DisciplineItem: Entity
    {
        public int? EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        [MaxLength(60)]
        [Required]
        public string Title { get; set; }
        
        /// <summary>
        /// 'G' - gratitude/ 'V' - punishment
        /// </summary>
        [Required]
        public string ItemType { get; set; }
        
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Money (in kopecks)
        /// </summary>
        [Required]
        public long AwardOrFine { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
        
    }
}