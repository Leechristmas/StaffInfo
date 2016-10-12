using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.DAL.Repositories
{
    /// <summary>
    /// Repository for work terms
    /// </summary>
    public class WorkTermRepository: IRepository<WorkTerm>
    {
        private readonly StaffContext _staffContext;

        public WorkTermRepository(StaffContext staffContext)
        {
            _staffContext = staffContext;
        }

        public IEnumerable<WorkTerm> Select()
        {
            return _staffContext.WorkTerms;
        }

        public WorkTerm Select(int id)
        {
            return _staffContext.WorkTerms.Find(id);
        }

        public IEnumerable<WorkTerm> Find(Func<WorkTerm, bool> predicate)
        {
            return _staffContext.WorkTerms.Where(predicate).ToList();
        }

        public void Create(WorkTerm item)
        {
            _staffContext.WorkTerms.Add(item);
        }

        public void Update(WorkTerm item)
        {
            _staffContext.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            WorkTerm workTerm = _staffContext.WorkTerms.Find(id);

            if (workTerm != null)
                _staffContext.WorkTerms.Remove(workTerm);
        }
    }
}