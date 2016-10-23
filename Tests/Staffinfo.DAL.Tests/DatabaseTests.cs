using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
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
        //private readonly IUnitRepository _staffUnitRepository = new StaffUnitRepository();

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

    }
}