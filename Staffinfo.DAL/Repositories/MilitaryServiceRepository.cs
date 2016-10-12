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
    /// Repository for MilitaryServices
    /// </summary>
    public class MilitaryServiceRepository: IRepository<MilitaryService>
    {
        private readonly StaffContext _staffContext;

        public MilitaryServiceRepository(StaffContext staffContext)
        {
            _staffContext = staffContext;
        }

        public IEnumerable<MilitaryService> Select()
        {
            return _staffContext.MilitaryServices;
        }

        public MilitaryService Select(int id)
        {
            return _staffContext.MilitaryServices.Find(id);
        }

        public IEnumerable<MilitaryService> Find(Func<MilitaryService, bool> predicate)
        {
            return _staffContext.MilitaryServices.Where(predicate).ToList();
        }

        public void Create(MilitaryService item)
        {
            _staffContext.MilitaryServices.Add(item);
        }

        public void Update(MilitaryService item)
        {
            _staffContext.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            MilitaryService militaryService = _staffContext.MilitaryServices.Find(id);

            if (militaryService != null)
                _staffContext.MilitaryServices.Remove(militaryService);
        }
    }
}