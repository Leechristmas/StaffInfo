using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public IEnumerable<T> Select()
        {
            return Table.ToList();
        }

        public T Select(int id)
        {
            return Table.Find(id);
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return Table.Where(predicate).ToList();
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

        public void Save()
        {
            StaffContext.SaveChanges();
        }
    }
}