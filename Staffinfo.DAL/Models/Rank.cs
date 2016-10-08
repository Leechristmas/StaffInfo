using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Models
{
    public class Rank: Entity
    {
        public string RankName { get; set; }
        public int RankWeight { get; set; }
        
        /// <summary>
        /// Due date for the rank (months count)
        /// </summary>
        public int DueDate { get; set; }
    }
}