using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories;
using Staffinfo.DAL.Repositories.Interfaces;
using Staffinfo.API.Models;

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

        [TestMethod]
        public async Task TransferToDismissed()
        {
            await _staffUnitRepository.EmployeeRepository.Database.ExecuteSqlCommandAsync(
                "dbo.pr_TransferEmployeeToDismissed @employeeId, @dismissalDate, @clause, @clauseDescription",
                new SqlParameter("@employeeId", 2),
                new SqlParameter("@dismissalDate", DateTime.Now),
                new SqlParameter("@clause", "333"),
                new SqlParameter("@clauseDescription", "azaza"));
        }

        [TestMethod]
        public void Add_Employee()
        {
            EmployeeViewModel e = new EmployeeViewModel
            {
                EmployeeLastname = "aaa",
                EmployeeFirstname = "bbb",
                EmployeeMiddlename = "ccc",
                BirthDate = DateTime.Now
            };
            var t = EmployeeViewModelMin.GetEmployeeFromModel(e);
            t.Id = 0;
            t.AddressId = 1;
            t.PassportId = 1;
            t.RetirementDate = null;
            var l1 = _staffUnitRepository.EmployeeRepository;

            Employee newEmpl =
                _staffUnitRepository.EmployeeRepository.Create(t);


            var l2 = _staffUnitRepository.EmployeeRepository;
            Task.WaitAll(_staffUnitRepository.EmployeeRepository.SaveAsync());
            var t2 = _staffUnitRepository.EmployeeRepository.SelectAsync().Result;
        }

    }
}