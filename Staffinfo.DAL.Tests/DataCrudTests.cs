using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Infrastructure;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.DAL.Tests
{
    /// <summary>
    /// Includes unit-tests for Unit repository
    /// </summary>
    [TestClass]
    public class DataCrudTests
    {
        private readonly IUnitRepository _staffUnitRepository = DIConfig.Kernel.Get<IUnitRepository>();

        [TestMethod]
        public void Select_GeyAllAddresses()
        {
            StaffContext db = new StaffContext();

            var t = db.Addresses.ToList();

            //IEnumerable<Address> addresses;

            //addresses = _staffUnitRepository.AddressRepository.Select();

            //Assert.IsTrue(addresses.Any());
        }
    }
}