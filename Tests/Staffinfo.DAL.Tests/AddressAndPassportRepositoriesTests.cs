using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.DAL.Tests
{
    /// <summary>
    /// Summary description for AddressAndPassportRepositoriesTests
    /// </summary>
    [TestClass]
    public class AddressAndPassportRepositoriesTests
    {
        private IUnitRepository _repository;

        [TestInitialize]
        public void SetUp()
        {
            var addressRepository = new Repository<Address>(new StaffContext());
            var passportRepository = new Repository<Passport>(new StaffContext());
            _repository = new StaffUnitRepository(addressRepository, null, null, null, null, passportRepository, null, null, null, null, null, null, null, null, null, null);
        }

        #region TestContext
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        #endregion

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void GetAllAddresses_ShouldReturnAllAddressesFromDB()
        {
            List<Address> addresses = null;

            addresses = _repository.AddressRepository.SelectAsync().Result.ToList();

            Assert.IsNotNull(addresses);
            Assert.IsTrue(addresses.Count > 0, "No items have been returned.");
        }

        [TestMethod]
        public void GetAllPassports_ShouldReturnAllPassportsFromDB()
        {
            List<Passport> passports = null;

            passports = _repository.PassportRepository.SelectAsync().Result.ToList();

            Assert.IsNotNull(passports);
            Assert.IsTrue(passports.Count > 0, "No items have been returned.");
        }
    }
}
