using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class Repository<T>: IStaffRepository, IRepository<T> where T : Entity
    {
        [Inject]
        public StaffContext StaffContext { get; set; }

        public Repository()
        {
            Table = StaffContext.Set<T>();
        }

        public Repository(StaffContext context)
        {
            StaffContext = context;
            Table = StaffContext.Set<T>();
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
        public async Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate)
        {
            return await Table.AsQueryable().Where(predicate).AsQueryable().ToListAsync();
        }

        public T Create(T item)
        {
            return Table.Add(item);
        }

        public void Update(T item)
        {
            StaffContext.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            T item = Table.Find(id);
            if (item != null)
            {
                Table.Remove(item);
            }
        }

        public async Task SaveAsync()
        {
            await StaffContext.SaveChangesAsync();
        }
    }
}