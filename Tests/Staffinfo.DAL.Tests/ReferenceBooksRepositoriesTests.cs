using System;
using System.Text;
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
    /// Summary description for ReferenceBooksTests
    /// </summary>
    [TestClass]
    public class ReferenceBooksRepositoriesTests
    {
        private IUnitRepository _repository;

        [TestInitialize]
        public void SetUp()
        {
            var locationRepository = new Repository<Location>(new StaffContext());
            var postRepository = new Repository<Post>(new StaffContext());
            var rankRepository = new Repository<Rank>(new StaffContext());
            var serviceRepository = new Repository<Service>(new StaffContext());
            
            _repository = new StaffUnitRepository(null, null, locationRepository, null, null, null, postRepository,
                rankRepository, serviceRepository, null, null, null, null, null, null, null, null);
        }

        #region TestContext

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
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
        public void GetAllLocations_ShouldReturnAllLocationsFromDB()
        {
            List<Location> locations = null;

            locations = _repository.LocationRepository.SelectAsync().Result.ToList();

            Assert.IsNotNull(locations, "The list of item is \"NULL\"");
            Assert.IsTrue(locations.Count > 0, "No items have been returned.");
        }

        [TestMethod]
        public void GetAllPosts_ShouldReturnAllPostsFromDB()
        {
            List<Post> posts = null;

            posts = _repository.PostRepository.SelectAsync().Result.ToList();

            Assert.IsNotNull(posts, "The list of item is \"NULL\"");
            Assert.IsTrue(posts.Count > 0, "No items have been returned.");
        }

        [TestMethod]
        public void GetAllRanks_ShouldReturnAllRanksFromDB()
        {
            List<Rank> ranks = null;

            ranks = _repository.RankRepository.SelectAsync().Result.ToList();

            Assert.IsNotNull(ranks, "The list of item is \"NULL\"");
            Assert.IsTrue(ranks.Count > 0, "No items have been returned.");
        }

        [TestMethod]
        public void GetAllServices_ShouldReturnAllServicesFromDB()
        {
            List<Service> services = null;

            services = _repository.ServiceRepository.SelectAsync().Result.ToList();

            Assert.IsNotNull(services, "The list of item is \"NULL\"");
            Assert.IsTrue(services.Count > 0, "No items have been returned.");
        }

    }
}
