using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Repositories.Interfaces
{
    public interface IRepository<T> where T : Entity
    {
        DbSet<T> Table { get; set; }

        /// <summary>
        /// Returns all items
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> SelectAsync();

        /// <summary>
        /// Returns the item by id
        /// </summary>
        /// <param name="id">id of the item</param>
        /// <returns></returns>
        Task<T> SelectAsync(int id);

        /// <summary>
        /// Returns the items satisfying for the predicate
        /// </summary>
        /// <param name="predicate">search condition</param>
        /// <returns></returns>
        Task<IEnumerable<T>> WhereAsync(Func<T, Boolean> predicate);
        
        /// <summary>
        /// Creates new item and saves this one in database
        /// </summary>
        /// <param name="item">item to save</param>
        T Create(T item);

        /// <summary>
        /// Updates the item
        /// </summary>
        /// <param name="item">item to update</param>
        void Update(T item);

        /// <summary>
        /// Removes the item from database
        /// </summary>
        /// <param name="id">id of the item to remove</param>
        Task Delete(int id);

        /// <summary>
        /// Saves all changes
        /// </summary>
        Task SaveAsync();
    }
}