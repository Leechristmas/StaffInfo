using System.Data.Entity.ModelConfiguration;
using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Mapping
{
    public class BaseMap<T>: EntityTypeConfiguration<T> where T: Entity
    {
        protected BaseMap(string tableName)
        {
            this.ToTable(tableName);

            this.HasKey(t => t.Id);
        }
    }
}