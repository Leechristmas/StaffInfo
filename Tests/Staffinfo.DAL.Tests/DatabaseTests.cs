using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.DAL.Tests
{
    /// <summary>
    /// Includes unit-tests for Unit repository
    /// </summary>
    [TestClass]
    public class DatabaseTests
    {
        private readonly IUnitRepository _staffUnitRepository = new StaffUnitRepository(
            new Repository<Address>(new StaffContext()), 
            new Repository<Employee>(new StaffContext()), 
            new Repository<Location>(new StaffContext()),
            new Repository<MesAchievement>(new StaffContext()),
            new Repository<MilitaryService>(new StaffContext()),
            new Repository<Passport>(new StaffContext()),
            new Repository<Post>(new StaffContext()),
            new Repository<Rank>(new StaffContext()),
            new Repository<Service>(new StaffContext()), 
            new Repository<WorkTerm>(new StaffContext()));

        //[TestMethod]
        //public void Select_GetAllAddresses()
        //{
        //    IEnumerable<Address> addresses;

        //    addresses = _staffUnitRepository.AddressRepository.Select();

        //    Assert.IsTrue(addresses.Any());
        //}

        //[TestMethod]
        //public void Add_And_RemoveAddress()
        //{
        //    Address address = new Address()
        //    {
        //        City = "Test_city",
        //        Street = "Test_street",
        //        House = "test_h",
        //        Flat = "t_f"
        //    };
        //    int addressId;

        //    address = _staffUnitRepository.AddressRepository.Create(address);
        //    _staffUnitRepository.AddressRepository.Save();

        //    addressId = address.Id;
        //    _staffUnitRepository.AddressRepository.Delete(addressId);
        //    _staffUnitRepository.AddressRepository.Save();
        //    Address removed = _staffUnitRepository.AddressRepository.Select(addressId);


        //    Assert.IsTrue(addressId > 0);
        //    Assert.IsNull(removed);
        //}

        [TestMethod]
        public void Select_Employees()
        {
            var t = _staffUnitRepository.EmployeeRepository.SelectAsync().Result;
            _staffUnitRepository.EmployeeRepository.Delete(2);
            Task.WaitAll(_staffUnitRepository.EmployeeRepository.SaveAsync());
            var t2 = _staffUnitRepository.EmployeeRepository.SelectAsync().Result;
        }

    }
}