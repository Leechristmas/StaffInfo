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
    /// Repository for passports
    /// </summary>
    public class PassportRepository: IRepository<Passport>
    {
        private readonly StaffContext _staffContext;

        public PassportRepository(StaffContext staffContext)
        {
            _staffContext = staffContext;
        }

        public IEnumerable<Passport> Select()
        {
            return _staffContext.Passports;
        }

        public Passport Select(int id)
        {
            return _staffContext.Passports.Find(id);
        }

        public IEnumerable<Passport> Find(Func<Passport, bool> predicate)
        {
            return _staffContext.Passports.Where(predicate).ToList();
        }

        public void Create(Passport item)
        {
            _staffContext.Passports.Add(item);
        }

        public void Update(Passport item)
        {
            _staffContext.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Passport passport = _staffContext.Passports.Find(id);

            if (passport != null)
                _staffContext.Passports.Remove(passport);
        }
    }
}