using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.DAL.Tests
{
    /// <summary>
    /// Summary description for EmployeeRepositoryTests
    /// </summary>
    [TestClass]
    public class EmployeeRepositoryTests
    {
        private IUnitRepository _repository;

        [TestInitialize]
        public void SetUp()
        {
            var addressRepository = new Repository<Address>(new StaffContext());
            var passportRepository = new Repository<Passport>(new StaffContext());
            var employeeRepository = new Repository<Employee>(new StaffContext());
            _repository = new StaffUnitRepository(addressRepository, employeeRepository, null, null, null, passportRepository, null, null, null, null, null, null, null, null);
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
        public void GetAllEmployees_ShouldReturnAllEmployeesFromDB()
        {
            List<Employee> employees = null;

            employees = _repository.EmployeeRepository.SelectAsync().Result.ToList();

            Assert.IsNotNull(employees, "The list of item is \"NULL\"");
            Assert.IsTrue(employees.Count > 0, "No items have been returned.");
        }

        [TestMethod]
        public async Task CRUD_ShouldCreateGetUpdateDeleteAnEmployeeItem()
        {
            var id = CreateEployee().Result;
            var item = TestingCRUDHelper.GetItem_ShouldReturnAnItemById(_repository.EmployeeRepository, id);

            TestingCRUDHelper.GetAllItems_ShouldReturnAllItemsFromDB(_repository.EmployeeRepository);

            await Update_ShouldUpdateItem(item);
            await TestingCRUDHelper.Delete_ShouldDeleteSpecifiedItemFromDB(_repository.EmployeeRepository, id);
        }

        private async Task Update_ShouldUpdateItem(Employee employee)
        {
            employee.EmployeeLastname = "Ivanov";
            employee.EmployeeFirstname = "Ivan";
            employee.EmployeeMiddlename = "Ivanovich";
            employee.BirthDate = new DateTime(2000, 1, 1);
            
            _repository.EmployeeRepository.Update(employee);
            await _repository.EmployeeRepository.SaveAsync();

            Employee updated = _repository.EmployeeRepository.SelectAsync(employee.Id).Result;

            Assert.AreEqual(updated.EmployeeFirstname, employee.EmployeeFirstname, "The \"firstname\" has not been updated!");
            Assert.AreEqual(updated.EmployeeLastname, employee.EmployeeLastname, "The \"lastname\" has not been updated!");
            Assert.AreEqual(updated.EmployeeMiddlename, employee.EmployeeMiddlename, "The \"middlename\" has not been updated!");
            Assert.AreEqual(updated.BirthDate, employee.BirthDate, "The \"birthdate\" has not been updated!");
        }

        private async Task<int> CreateEployee()
        {
            Address addressToSave = new Address
            {
                Area = "Gomelskaya",
                City = "Gomel",
                DetailedAddress = "Vladzimirova st., 17/42",
                ZipCode = "100500"
            };

            Passport passportToSave = new Passport
            {
                PassportNumber = "AA1234567",
                PassportOrganization = "Gomelskij ROVD"
            };

            Employee employeeToSave = new Employee
            {
                EmployeeLastname = "Petrov",
                EmployeeFirstname = "Petr",
                EmployeeMiddlename = "Petrovich",
                BirthDate = DateTime.Now,
                Address = addressToSave,
                Passport = passportToSave
            };

            var created = _repository.EmployeeRepository.Create(employeeToSave);
            await _repository.EmployeeRepository.SaveAsync();
            
            Assert.IsNotNull(created, "Operation of creation returned \"NULL\"");
            Assert.IsNotNull(created.Passport, "Passport object of created is \"NULL\"");
            Assert.IsNotNull(created.Address, "Address object of created is \"NULL\"");
            Assert.IsTrue(created.Id > 0, "Object has invalid id!");  //that shows the item has been saved at the DB

            return created.Id;
        }

    }
}
