using System;
using System.Collections.Generic;
using Staffinfo.DAL.Models.Common;

namespace Staffinfo.DAL.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        /// <summary>
        /// Returns all items
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> Select();

        /// <summary>
        /// Returns the item by id
        /// </summary>
        /// <param name="id">id of the item</param>
        /// <returns></returns>
        T Select(int id);

        /// <summary>
        /// Returns the items satisfying for the predicate
        /// </summary>
        /// <param name="predicate">search condition</param>
        /// <returns></returns>
        IEnumerable<T> Find(Func<T, Boolean> predicate);

        /// <summary>
        /// Creates new item and saves this one in database
        /// </summary>
        /// <param name="item">item to save</param>
        void Create(T item);

        /// <summary>
        /// Updates the item
        /// </summary>
        /// <param name="item">item to update</param>
        void Update(T item);

        /// <summary>
        /// Removes the item from database
        /// </summary>
        /// <param name="id">id of the item to remove</param>
        void Delete(int id);
    }
}