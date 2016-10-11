using Staffinfo.DAL.Models;

namespace Staffinfo.DAL.Mapping
{
    public class RankMap: BaseMap<Rank>
    {
        public RankMap():base("tbl_Rank")
        {
            this.Property(t => t.RankName).IsRequired().HasColumnName("RankName");
            this.Property(t => t.RankWeight).IsRequired().HasColumnName("RankWeight");
            this.Property(t => t.Term).IsRequired().HasColumnName("Term");
        }
    }
}