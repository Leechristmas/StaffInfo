using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.DAL.Repositories
{
    public class RankRepository: IRepository<Rank>
    {
        private readonly StaffContext _staffContext;

        public RankRepository(StaffContext staffContext)
        {
            _staffContext = staffContext;
        }

        public IEnumerable<Rank> Select()
        {
            return _staffContext.Ranks;
        }

        public Rank Select(int id)
        {
            return _staffContext.Ranks.Find(id);
        }

        public IEnumerable<Rank> Find(Func<Rank, bool> predicate)
        {
            return _staffContext.Ranks.Where(predicate).ToList();
        }

        public void Create(Rank item)
        {
            _staffContext.Ranks.Add(item);
        }

        public void Update(Rank item)
        {
            _staffContext.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Rank rank = _staffContext.Ranks.Find(id);

            if (rank != null)
                _staffContext.Ranks.Remove(rank);
        }
    }
}