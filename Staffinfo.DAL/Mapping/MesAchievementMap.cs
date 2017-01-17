using Staffinfo.DAL.Models;

namespace Staffinfo.DAL.Mapping
{
    public class MesAchievementMap: BaseMap<MesAchievement>
    {
        public MesAchievementMap(): base("tbl_MesAchievement")
        {
            this.Property(t => t.StartDate).IsRequired().HasColumnName("StartDate");
            this.Property(t => t.FinishDate).IsOptional().HasColumnName("FinishDate");

            //navigation properties
            this.Property(t => t.EmployeeId).IsOptional().HasColumnName("EmployeeID");
            HasOptional(t => t.Employee).WithMany().HasForeignKey(t => t.EmployeeId);

            this.Property(t => t.LocationId).IsOptional().HasColumnName("LocationID");
            HasOptional(t => t.Location).WithMany().HasForeignKey(t => t.LocationId);

            this.Property(t => t.PostId).IsOptional().HasColumnName("PostID");
            HasOptional(t => t.Post).WithMany().HasForeignKey(t => t.PostId);

            this.Property(t => t.RankId).IsOptional().HasColumnName("RankID");
            HasOptional(t => t.Rank).WithMany().HasForeignKey(t => t.RankId);
        }
    }
}