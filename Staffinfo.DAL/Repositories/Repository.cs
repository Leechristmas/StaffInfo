using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Ninject;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Models.Common;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.DAL.Repositories
{
    /// <summary>
    /// Generic repository for every model
    /// </summary>
    /// <typeparam name="T">the model type</typeparam>
    public class Repository<T>: IStaffRepository, IRepository<T>, IDisposable where T : Entity
    {
        [Inject]
        public StaffContext StaffContext { get; set; }

        public Database Database { get; set; }

        public Repository()
        {
            Table = StaffContext.Set<T>();
            Database = StaffContext.Database;
        }

        public Repository(StaffContext context)
        {
            StaffContext = context;
            Table = StaffContext.Set<T>();
            Database = StaffContext.Database;
        }

        public DbSet<T> Table { get; set; }

        public async Task<IEnumerable<T>> SelectAsync()
        {
            return await Table.ToListAsync();
        }

        public async Task<T> SelectAsync(int id)
        {
            return await Table.FindAsync(id);
        }

        //TODO:async recieving by a condition
        public async Task<IEnumerable<T>> WhereAsync(Func<T, bool> predicate)
        {
            return (await Table.ToListAsync()).Where(predicate);
        }

        public T Create(T item)
        {
            StaffContext.Entry(item).State = EntityState.Added;
            return Table.Add(item);
        }

        public void Update(T item, params string[] propToExclude)
        {
            StaffContext.Entry(item).State = EntityState.Modified;

            if (propToExclude != null && propToExclude.Length > 0)
            {
                foreach (var p in propToExclude)
                {
                    StaffContext.Entry(item).Property(p).IsModified = false;
                }
            }
        }

        public async Task Delete(int id)
        {
            T item = await Table.FindAsync(id);
            if (item != null)
            {
                Table.Remove(item);
            }
        }

        public async Task SaveAsync()
        {
            await StaffContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            StaffContext.Dispose();
        }
    }
}