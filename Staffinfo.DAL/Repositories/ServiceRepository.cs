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
    /// Repository for services
    /// </summary>
    public class ServiceRepository: IRepository<Service>
    {
        private readonly StaffContext _staffContext;

        public ServiceRepository(StaffContext staffContext)
        {
            _staffContext = staffContext;
        }

        public IEnumerable<Service> Select()
        {
            return _staffContext.Services;
        }

        public Service Select(int id)
        {
            return _staffContext.Services.Find(id);
        }

        public IEnumerable<Service> Find(Func<Service, bool> predicate)
        {
            return _staffContext.Services.Where(predicate).ToList();
        }

        public void Create(Service item)
        {
            _staffContext.Services.Add(item);
        }

        public void Update(Service item)
        {
            _staffContext.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Service service = _staffContext.Services.Find(id);

            if (service != null)
                _staffContext.Services.Remove(service);
        }
    }
}