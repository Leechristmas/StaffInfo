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
    /// Repository for locations
    /// </summary>
    public class LocationRepository: IRepository<Location>
    {
        private readonly StaffContext _staffContext;

        public LocationRepository(StaffContext staffContext)
        {
            _staffContext = staffContext;
        }

        public IEnumerable<Location> Select()
        {
            return _staffContext.Locations;
        }

        public Location Select(int id)
        {
            return _staffContext.Locations.Find(id);
        }

        public IEnumerable<Location> Find(Func<Location, bool> predicate)
        {
            return _staffContext.Locations.Where(predicate).ToList();
        }

        public void Create(Location item)
        {
            _staffContext.Locations.Add(item);
        }

        public void Update(Location item)
        {
            _staffContext.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Location location = _staffContext.Locations.Find(id);

            if (location != null)
                _staffContext.Locations.Remove(location);
        }
    }
}