using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Ninject;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.DAL.Repositories
{
    /// <summary>
    /// Repository for addresses
    /// </summary>
    public class AddressRepository: IRepository<Address>, IStaffRepository
    {
        [Inject]
        public StaffContext StaffContext { get; set; }

        public AddressRepository(StaffContext staffContext)
        {
            StaffContext = staffContext;
        } 

        public IEnumerable<Address> Select()
        {
            return StaffContext.Addresses;
        }

        public Address Select(int id)
        {
            return StaffContext.Addresses.Find(id);
        }

        public IEnumerable<Address> Find(Func<Address, bool> predicate)
        {
            return StaffContext.Addresses.Where(predicate).ToList();
        }

        public void Create(Address item)
        {
            StaffContext.Addresses.Add(item);
        }

        public void Update(Address item)
        {
            StaffContext.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Address address = StaffContext.Addresses.Find(id);
            if (address != null)
                StaffContext.Addresses.Remove(address);
        }

    }
}