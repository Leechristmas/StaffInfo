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
    /// Repository for addresses
    /// </summary>
    public class AddressRepository: IRepository<Address>
    {
        private readonly StaffContext _staffContext;

        public AddressRepository(StaffContext staffContext)
        {
            _staffContext = staffContext;
        } 

        public IEnumerable<Address> Select()
        {
            return _staffContext.Addresses;
        }

        public Address Select(int id)
        {
            return _staffContext.Addresses.Find(id);
        }

        public IEnumerable<Address> Find(Func<Address, bool> predicate)
        {
            return _staffContext.Addresses.Where(predicate).ToList();
        }

        public void Create(Address item)
        {
            _staffContext.Addresses.Add(item);
        }

        public void Update(Address item)
        {
            _staffContext.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Address address = _staffContext.Addresses.Find(id);
            if (address != null)
                _staffContext.Addresses.Remove(address);
        }
    }
}