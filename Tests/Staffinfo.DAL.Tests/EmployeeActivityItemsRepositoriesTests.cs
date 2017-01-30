﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Models.Common;
using Staffinfo.DAL.Repositories;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.DAL.Tests
{
    [TestClass]
    public class EmployeeActivityItemsRepositoriesTests
    {
        private IUnitRepository _repository;

        [TestInitialize]
        public void SetUp()
        {
            var addressRepository = new Repository<Address>(new StaffContext());
            var passportRepository = new Repository<Passport>(new StaffContext());
            var employeeRepository = new Repository<Employee>(new StaffContext());
            var locationRepository = new Repository<Location>(new StaffContext());
            var disciplineRepository = new Repository<DisciplineItem>(new StaffContext());
            var mesAchievementRepository = new Repository<MesAchievement>(new StaffContext());
            var militaryRepository = new Repository<MilitaryService>(new StaffContext());
            var outFromOffice = new Repository<OutFromOffice>(new StaffContext());
            var sertificationRepository = new Repository<Sertification>(new StaffContext());
            var workTermRepository= new Repository<WorkTerm>(new StaffContext());
            var educationRepository = new Repository<EducationItem>(new StaffContext());
            var contractRepository = new Repository<Contract>(new StaffContext());
            var relativeRepository = new Repository<Relative>(new StaffContext());

            _repository = new StaffUnitRepository(addressRepository, employeeRepository, locationRepository, mesAchievementRepository, militaryRepository,
                passportRepository, null, null, null, workTermRepository, null, disciplineRepository, outFromOffice, sertificationRepository, educationRepository, contractRepository, relativeRepository);
        }

        #region DisciplineItemsRepositoryTests

        [TestMethod]
        public async Task CRUD_ShouldCreateGetUpdateDeleteADisciplineItem()
        {
            var id = CreateDisciplineItem_ShouldCreateAndSaveADisciplineItem().Result;
            var item = TestingCRUDHelper.GetItem_ShouldReturnAnItemById(_repository.DisciplineItemRepository, id);

            TestingCRUDHelper.GetAllItems_ShouldReturnAllItemsFromDB(_repository.DisciplineItemRepository);

            await Update_ShouldUpdateItem(item);
            await TestingCRUDHelper.Delete_ShouldDeleteSpecifiedItemFromDB(_repository.DisciplineItemRepository, id);
        }

        private async Task Update_ShouldUpdateItem(DisciplineItem item)
        {
            item.Title = "test_title";
            item.Description = "testing of updating";

            _repository.DisciplineItemRepository.Update(item);
            await _repository.DisciplineItemRepository.SaveAsync();

            DisciplineItem updated = _repository.DisciplineItemRepository.SelectAsync(item.Id).Result;

            Assert.AreEqual(updated.Title, item.Title, "The \"title\" has not been updated!");
            Assert.AreEqual(updated.Description, item.Description, "The \"description\" has not been updated!");
        }
        
        private async Task<int> CreateDisciplineItem_ShouldCreateAndSaveADisciplineItem()
        {
            Employee employee = _repository.EmployeeRepository.SelectAsync().Result.ToList()[0];

            DisciplineItem item = new DisciplineItem
            {
                EmployeeId = employee.Id,
                Title = "test",
                ItemType = "G",
                AwardOrFine = 100,
                Date = DateTime.Now,
                Description = "test"
            };

            var created = _repository.DisciplineItemRepository.Create(item);
            await _repository.DisciplineItemRepository.SaveAsync();

            Assert.IsNotNull(created, "Operation of creation returned \"NULL\"");
            Assert.IsTrue(created.Id > 0, "Object has invalid id!");

            return created.Id;
        }

        #endregion

        #region MESAchievementsRepositoryTests
        
        [TestMethod]
        public async Task CRUD_ShouldCreateGetUpdateDeleteAMESAchievements()
        {
            var id = CreateMesAchievement_ShouldCreateAndSaveAMESAchievement().Result;
            var item = TestingCRUDHelper.GetItem_ShouldReturnAnItemById(_repository.MesAchievementRepository, id);

            TestingCRUDHelper.GetAllItems_ShouldReturnAllItemsFromDB(_repository.MesAchievementRepository);

            await Update_ShouldUpdateMesAchievement(item);
            await TestingCRUDHelper.Delete_ShouldDeleteSpecifiedItemFromDB(_repository.MesAchievementRepository, id);
        }
        
        private async Task Update_ShouldUpdateMesAchievement(MesAchievement item)
        {
            item.Description = "testing of updating";

            _repository.MesAchievementRepository.Update(item);
            await _repository.MesAchievementRepository.SaveAsync();

            MesAchievement updated = _repository.MesAchievementRepository.SelectAsync(item.Id).Result;
            
            Assert.AreEqual(updated.Description, item.Description, "The \"description\" has not been updated!");
        }
        
        private async Task<int> CreateMesAchievement_ShouldCreateAndSaveAMESAchievement()
        {
            Employee employee = _repository.EmployeeRepository.SelectAsync().Result.ToList()[0];

            Location location = _repository.LocationRepository.SelectAsync().Result.ToList()[0];

            MesAchievement item = new MesAchievement
            {
                EmployeeId = employee.Id,
                StartDate = DateTime.Now.AddDays(-1),
                FinishDate = DateTime.Now,
                LocationId = location.Id,
                PostId = 1,
                RankId = 1,
                Description = "test"
            };

            var created = _repository.MesAchievementRepository.Create(item);
            await _repository.MesAchievementRepository.SaveAsync();

            Assert.IsNotNull(created, "Operation of creation returned \"NULL\"");
            Assert.IsTrue(created.Id > 0, "Object has invalid id!");

            return created.Id;
        }

        #endregion

        #region MilitaryRepositoryTests
        
        [TestMethod]
        public async Task CRUD_ShouldCreateGetUpdateDeleteAMilitary()
        {
            var id = CreateMilitary_ShouldCreateAndSaveAMilitary().Result;
            var item = TestingCRUDHelper.GetItem_ShouldReturnAnItemById(_repository.MilitaryServiceRepository, id);

            TestingCRUDHelper.GetAllItems_ShouldReturnAllItemsFromDB(_repository.MilitaryServiceRepository);

            await Update_ShouldUpdateMilitary(item);
            await TestingCRUDHelper.Delete_ShouldDeleteSpecifiedItemFromDB(_repository.MilitaryServiceRepository, id);
        }

        private async Task Update_ShouldUpdateMilitary(MilitaryService item)
        {
            item.Description = "testing of updating";

            _repository.MilitaryServiceRepository.Update(item);
            await _repository.MilitaryServiceRepository.SaveAsync();

            MilitaryService updated = _repository.MilitaryServiceRepository.SelectAsync(item.Id).Result;

            Assert.AreEqual(updated.Description, item.Description, "The \"description\" has not been updated!");
        }
        
        private async Task<int> CreateMilitary_ShouldCreateAndSaveAMilitary()
        {
            Employee employee = _repository.EmployeeRepository.SelectAsync().Result.ToList()[0];

            Location location = _repository.LocationRepository.SelectAsync().Result.ToList()[0];

            MilitaryService item = new MilitaryService
            {
                EmployeeId = employee.Id,
                StartDate = DateTime.Now.AddDays(-1),
                FinishDate = DateTime.Now,
                LocationId = location.Id,
                Rank = "test_rank",
                Description = "test_military_rank"
            };

            var created = _repository.MilitaryServiceRepository.Create(item);
            await _repository.MilitaryServiceRepository.SaveAsync();

            Assert.IsNotNull(created, "Operation of creation returned \"NULL\"");
            Assert.IsTrue(created.Id > 0, "Object has invalid id!");

            return created.Id;
        }

        #endregion

        #region OutFromOfficeItemsRepositoryTests
        
        [TestMethod]
        public async Task CRUD_ShouldCreateGetUpdateDeleteAnOutFromOfficeItem()
        {
            var id = CreateOutFromOfficeItem_ShouldCreateAndSaveAnOutFromOfficeItem().Result;
            var item = TestingCRUDHelper.GetItem_ShouldReturnAnItemById(_repository.OutFromOfficeRepository, id);

            TestingCRUDHelper.GetAllItems_ShouldReturnAllItemsFromDB(_repository.OutFromOfficeRepository);

            await Update_ShouldUpdateOutFromOfficeItem(item);
            await TestingCRUDHelper.Delete_ShouldDeleteSpecifiedItemFromDB(_repository.OutFromOfficeRepository, id);
        }

        private async Task Update_ShouldUpdateOutFromOfficeItem(OutFromOffice item)
        {
            item.Description = "testing of updating";

            _repository.OutFromOfficeRepository.Update(item);
            await _repository.OutFromOfficeRepository.SaveAsync();

            OutFromOffice updated = _repository.OutFromOfficeRepository.SelectAsync(item.Id).Result;

            Assert.AreEqual(updated.Description, item.Description, "The \"description\" has not been updated!");
        }

        private async Task<int> CreateOutFromOfficeItem_ShouldCreateAndSaveAnOutFromOfficeItem()
        {
            Employee employee = _repository.EmployeeRepository.SelectAsync().Result.ToList()[0];

            Location location = _repository.LocationRepository.SelectAsync().Result.ToList()[0];

            OutFromOffice item = new OutFromOffice
            {
                EmployeeId = employee.Id,
                StartDate = DateTime.Now.AddDays(-1),
                FinishDate = DateTime.Now,
                Cause = "G",
                Description = "test_military_rank"
            };

            var created = _repository.OutFromOfficeRepository.Create(item);
            await _repository.OutFromOfficeRepository.SaveAsync();

            Assert.IsNotNull(created, "Operation of creation returned \"NULL\"");
            Assert.IsTrue(created.Id > 0, "Object has invalid id!");

            return created.Id;
        }


        #endregion

        #region SertificationsRepositoryTests

        [TestMethod]
        public async Task CRUD_ShouldCreateGetUpdateDeleteAnSertification()
        {
            var id = CreateSertification_ShouldCreateAndSaveASertification().Result;
            var item = TestingCRUDHelper.GetItem_ShouldReturnAnItemById(_repository.SertificationRepository, id);

            TestingCRUDHelper.GetAllItems_ShouldReturnAllItemsFromDB(_repository.OutFromOfficeRepository);

            await Update_ShouldUpdateSertification(item);
            await TestingCRUDHelper.Delete_ShouldDeleteSpecifiedItemFromDB(_repository.SertificationRepository, id);
        }

        private async Task Update_ShouldUpdateSertification(Sertification item)
        {
            item.Description = "testing of updating";

            _repository.SertificationRepository.Update(item);
            await _repository.SertificationRepository.SaveAsync();

            Sertification updated = _repository.SertificationRepository.SelectAsync(item.Id).Result;

            Assert.AreEqual(updated.Description, item.Description, "The \"description\" has not been updated!");
        }

        private async Task<int> CreateSertification_ShouldCreateAndSaveASertification()
        {
            Employee employee = _repository.EmployeeRepository.SelectAsync().Result.ToList()[0];

            Sertification item = new Sertification
            {
                EmployeeId = employee.Id,
                DueDate = DateTime.Now,
                Description = "test_military_rank"
            };

            var created = _repository.SertificationRepository.Create(item);
            await _repository.SertificationRepository.SaveAsync();

            Assert.IsNotNull(created, "Operation of creation returned \"NULL\"");
            Assert.IsTrue(created.Id > 0, "Object has invalid id!");

            return created.Id;
        }

        #endregion

        #region WorkTermsRepositoryTests

        [TestMethod]
        public async Task CRUD_ShouldCreateGetUpdateDeleteAWorkTerm()
        {
            var id = CreateWorkTerm_ShouldCreateAndSaveAWorkTerm().Result;
            var item = TestingCRUDHelper.GetItem_ShouldReturnAnItemById(_repository.WorkTermRepository, id);

            TestingCRUDHelper.GetAllItems_ShouldReturnAllItemsFromDB(_repository.WorkTermRepository);

            await Update_ShouldUpdateWorkTerm(item);
            await TestingCRUDHelper.Delete_ShouldDeleteSpecifiedItemFromDB(_repository.WorkTermRepository, id);
        }

        private async Task Update_ShouldUpdateWorkTerm(WorkTerm item)
        {
            item.Description = "testing of updating";

            _repository.WorkTermRepository.Update(item);
            await _repository.WorkTermRepository.SaveAsync();

            WorkTerm updated = _repository.WorkTermRepository.SelectAsync(item.Id).Result;

            Assert.AreEqual(updated.Description, item.Description, "The \"description\" has not been updated!");
        }

        private async Task<int> CreateWorkTerm_ShouldCreateAndSaveAWorkTerm()
        {
            Employee employee = _repository.EmployeeRepository.SelectAsync().Result.ToList()[0];

            Location location = _repository.LocationRepository.SelectAsync().Result.ToList()[0];

            WorkTerm item = new WorkTerm
            {
                EmployeeId = employee.Id,
                StartDate = DateTime.Now.AddDays(-1),
                FinishDate = DateTime.Now,
                LocationId = location.Id,
                Post = "test_post",
                Description = "test_military_rank"
            };

            var created = _repository.WorkTermRepository.Create(item);
            await _repository.WorkTermRepository.SaveAsync();

            Assert.IsNotNull(created, "Operation of creation returned \"NULL\"");
            Assert.IsTrue(created.Id > 0, "Object has invalid id!");

            return created.Id;
        }

        #endregion

        #region EduacationRepositoryTests

        [TestMethod]
        public async Task CRUD_ShouldCreateGetUpdateDeleteAnEducationItem()
        {
            var id = CreateEducationItem_ShouldCreateAndSaveAnEducationItem().Result;
            var item = TestingCRUDHelper.GetItem_ShouldReturnAnItemById(_repository.EducationRepository, id);

            TestingCRUDHelper.GetAllItems_ShouldReturnAllItemsFromDB(_repository.EducationRepository);

            await Update_ShouldUpdateEducationItem(item);
            await TestingCRUDHelper.Delete_ShouldDeleteSpecifiedItemFromDB(_repository.EducationRepository, id);
        }

        private async Task Update_ShouldUpdateEducationItem(EducationItem item)
        {
            item.Description = "testing of updating";

            _repository.EducationRepository.Update(item);
            await _repository.EducationRepository.SaveAsync();

            EducationItem updated = _repository.EducationRepository.SelectAsync(item.Id).Result;

            Assert.AreEqual(updated.Description, item.Description, "The \"description\" has not been updated!");
        }

        private async Task<int> CreateEducationItem_ShouldCreateAndSaveAnEducationItem()
        {
            EducationItem item = new EducationItem()
            {
                Institution = "inst_test",
                Speciality = "spec_test",
                StartDate = DateTime.Now.AddDays(-1),
                FinishDate = DateTime.Now,
                EmployeeId = 1,
                Description = "desc_test"
            };
            
            var created = _repository.EducationRepository.Create(item);
            await _repository.EducationRepository.SaveAsync();

            Assert.IsNotNull(created, "Operation of creation returned \"NULL\"");
            Assert.IsTrue(created.Id > 0, "Object has invalid id!");

            return created.Id;
        }

        #endregion

        #region ContractRepositoryTests

        [TestMethod]
        public async Task CRUD_ShouldCreateGetUpdateDeleteAContract()
        {
            var id = CreateContract_ShouldCreateAndSaveAContract().Result;
            var item = TestingCRUDHelper.GetItem_ShouldReturnAnItemById(_repository.ContractRepository, id);

            TestingCRUDHelper.GetAllItems_ShouldReturnAllItemsFromDB(_repository.ContractRepository);

            await Update_ShouldUpdateContract(item);
            await TestingCRUDHelper.Delete_ShouldDeleteSpecifiedItemFromDB(_repository.ContractRepository, id);
        }

        private async Task Update_ShouldUpdateContract(Contract item)
        {
            item.Description = "testing of updating";

            _repository.ContractRepository.Update(item);
            await _repository.ContractRepository.SaveAsync();

            Contract updated = _repository.ContractRepository.SelectAsync(item.Id).Result;

            Assert.AreEqual(updated.Description, item.Description, "The \"description\" has not been updated!");
        }

        private async Task<int> CreateContract_ShouldCreateAndSaveAContract()
        {
            Contract item = new Contract()
            {
                StartDate = DateTime.Now.AddDays(-1),
                FinishDate = DateTime.Now,
                EmployeeId = 1,
                Description = "desc_test"
            };

            var created = _repository.ContractRepository.Create(item);
            await _repository.ContractRepository.SaveAsync();

            Assert.IsNotNull(created, "Operation of creation returned \"NULL\"");
            Assert.IsTrue(created.Id > 0, "Object has invalid id!");

            return created.Id;
        }

        #endregion

        #region RelativesRepositoryTests

        [TestMethod]
        public async Task CRUD_ShouldCreateGetUpdateDeleteARelative()
        {
            var id = CreateRelative_ShouldCreateAndSaveARelative().Result;
            var item = TestingCRUDHelper.GetItem_ShouldReturnAnItemById(_repository.RelativeRepository, id);

            TestingCRUDHelper.GetAllItems_ShouldReturnAllItemsFromDB(_repository.RelativeRepository);

            await Update_ShouldUpdateRelative(item);
            await TestingCRUDHelper.Delete_ShouldDeleteSpecifiedItemFromDB(_repository.RelativeRepository, id);
        }

        private async Task Update_ShouldUpdateRelative(Relative item)
        {
            item.Lastname = "testing";

            _repository.RelativeRepository.Update(item);
            await _repository.RelativeRepository.SaveAsync();

            Relative updated = _repository.RelativeRepository.SelectAsync(item.Id).Result;

            Assert.AreEqual(updated.Lastname, item.Lastname, "The \"lastname\" has not been updated!");
        }

        private async Task<int> CreateRelative_ShouldCreateAndSaveARelative()
        {
            Relative item = new Relative()
            {
                Lastname = "Ололоев",
                Firstname = "Ололош",
                Middlename = "Ололоевич",
                EmployeeID = 1,
                BirthDate = DateTime.Now,
                Status = "сын"
            };

            var created = _repository.RelativeRepository.Create(item);
            await _repository.RelativeRepository.SaveAsync();

            Assert.IsNotNull(created, "Operation of creation returned \"NULL\"");
            Assert.IsTrue(created.Id > 0, "Object has invalid id!");

            return created.Id;
        }

        #endregion
    }
}
