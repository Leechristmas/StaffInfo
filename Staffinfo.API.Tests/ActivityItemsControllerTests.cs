using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Staffinfo.API.Controllers;
using Staffinfo.API.Models;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Models.Common;
using Staffinfo.DAL.Repositories;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.API.Tests
{
    [TestClass]
    public class ActivityItemsControllerTests
    {
        private IUnitRepository _repository;
        private ActivityItemsController _controller;

        /// <summary>
        /// Setup an implementation of IQueryable
        /// </summary>
        /// <typeparam name="T">type of the set</typeparam>
        /// <param name="set">DbSet of items</param>
        /// <param name="data">fake data for the set</param>
        private void ConfigureTheDbSet<T>(Mock<DbSet<T>> set, List<T> data) where T : Entity
        {
            set.As<IDbAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(() => new TestDbAsyncEnumerator<T>(data.GetEnumerator()));//lambda is necessary for giving a new enumerator (otherwise items will not be yielded)
            set.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<T>(data.AsQueryable().Provider));
            set.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
            set.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
            set.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());//one more necessary lambda
            
            //create/delete/find operations
            set.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(data.Add);
            set.Setup(m => m.Remove(It.IsAny<T>())).Returns<T>(t =>
            {
                data.Remove(t);
                return t;
            });
            set.Setup(b => b.FindAsync(It.IsAny<object[]>()))    //substitution of the .SelectAsync(id) method
                .Returns<object[]>(ids => set.Object.FirstOrDefaultAsync(b => b.Id == (int)ids[0]));
        }

        [TestInitialize]
        public void SetUp()
        {
            var worksData = GetWorksFakeData();
            var militarydata = GetMilitaryFakeData();
            var sertificationData = GetSertificationsFakeData();
            var outFromOfficeData = GetOutFromOfficeFakeData();
            var disciplineData = GetDisciplineFakeData();
            var mesAchievementsData = GetMesAchievementsFakeData();
            var educationData = GetEducationFakeData();
            var contractsData = GetContractsFakeData();
            var relativesData = GetRelativesFakeData();

            //configuration of IQueryable sets
            var works = new Mock<DbSet<WorkTerm>>();
            ConfigureTheDbSet(works, worksData);

            var military = new Mock<DbSet<MilitaryService>>();
            ConfigureTheDbSet(military, militarydata);

            var sertifications = new Mock<DbSet<Sertification>>();
            ConfigureTheDbSet(sertifications, sertificationData);

            var outFromOffice = new Mock<DbSet<OutFromOffice>>();
            ConfigureTheDbSet(outFromOffice, outFromOfficeData);

            var discipline = new Mock<DbSet<DisciplineItem>>();
            ConfigureTheDbSet(discipline, disciplineData);

            var mesAchievements = new Mock<DbSet<MesAchievement>>();
            ConfigureTheDbSet(mesAchievements, mesAchievementsData);

            var education = new Mock<DbSet<EducationItem>>();
            ConfigureTheDbSet(education, educationData);

            var contracts = new Mock<DbSet<Contract>>();
            ConfigureTheDbSet(contracts, contractsData);

            var relatives = new Mock<DbSet<Relative>>();
            ConfigureTheDbSet(relatives, relativesData);

            //configuration of the context
            var context = new Mock<StaffContext>();
            context.Setup(c => c.Set<WorkTerm>()).Returns(() => works.Object);
            context.Setup(c => c.Set<MilitaryService>()).Returns(() => military.Object);
            context.Setup(c => c.Set<Sertification>()).Returns(() => sertifications.Object);
            context.Setup(c => c.Set<OutFromOffice>()).Returns(() => outFromOffice.Object);
            context.Setup(c => c.Set<DisciplineItem>()).Returns(() => discipline.Object);
            context.Setup(c => c.Set<MesAchievement>()).Returns(() => mesAchievements.Object);
            context.Setup(c => c.Set<EducationItem>()).Returns(() => education.Object);
            context.Setup(c => c.Set<Contract>()).Returns(() => contracts.Object);
            context.Setup(c => c.Set<Relative>()).Returns(() => relatives.Object);

            //configuration of the repository
            _repository = new StaffUnitRepository(null, null, null, new Repository<MesAchievement>(context.Object),
                new Repository<MilitaryService>(context.Object), null, null, null, null,
                new Repository<WorkTerm>(context.Object),
                null, new Repository<DisciplineItem>(context.Object), new Repository<OutFromOffice>(context.Object),
                new Repository<Sertification>(context.Object), new Repository<EducationItem>(context.Object),
                new Repository<Contract>(context.Object), new Repository<Relative>(context.Object));

            _controller = new ActivityItemsController(_repository);
        }
        
        [TestMethod]
        public void GetWorks_ShouldReturnItemsByEmployeeId()
        {
            IEnumerable<WorkTermViewModel> worksFirstPiece, worksSecondPiece;

            worksFirstPiece = _controller.GetWorks(1).Result;
            worksSecondPiece = _controller.GetWorks(3).Result;

            Assert.IsNotNull(worksFirstPiece, "worksFirstPiece != null");
            Assert.IsNotNull(worksSecondPiece, "worksSecondPiece != null");
            Assert.IsTrue(worksFirstPiece.Count() == 2, "worksFirstPiece.Count() == 2");
            Assert.IsTrue(worksSecondPiece.Count() == 1, "worksSecondPiece.Count() == 1");
        }

        [TestMethod]
        public async Task AddWork_ShouldAdd1ItemAndReturn3ItemsForEmployee()
        {
            IEnumerable<WorkTermViewModel> works;
            WorkTermViewModel work = null;

            await _controller.PostWork(new WorkTerm
            {
                Id = 1,
                EmployeeId = 1,
                StartDate = new DateTime(2014, 1, 1),
                FinishDate = new DateTime(2015, 1, 1),
                Post = "test"
            });
            works = _controller.GetWorks(1).Result.ToList();
            work = works.FirstOrDefault(w => String.Equals(w.Post, "test"));

            Assert.IsNotNull(works, "works != null");
            Assert.IsTrue(works.Count() == 3, "works.Count() == 3");
            Assert.IsNotNull(work, "work != null");
        }

        [TestMethod]
        public async Task RemoveWork_ShouldRemove1ItemAndReturn0ItemsForEmployee()
        {
            IEnumerable<WorkTermViewModel> works;

            await _controller.DeleteWork(3);
            works = _controller.GetWorks(3).Result;

            Assert.IsNotNull(works, "works != null");
            Assert.IsTrue(!works.Any(), "!works.Any()");
        }

        [TestMethod]
        public void GetMilitary_ShouldReturnItemsByEmployeeId()
        {
            IEnumerable<MilitaryServiceViewModel> militaryFirstPiece, militarySecondPiece;

            militaryFirstPiece = _controller.GetMilitary(1).Result;
            militarySecondPiece = _controller.GetMilitary(2).Result;

            Assert.IsNotNull(militaryFirstPiece, "militaryFirstPiece != null");
            Assert.IsNotNull(militarySecondPiece, "militarySecondPiece != null");
            Assert.IsTrue(militaryFirstPiece.Count() == 2, "militaryFirstPiece.Count() == 2");
            Assert.IsTrue(militarySecondPiece.Count() == 1, "militarySecondPiece.Count() == 1");
        }

        [TestMethod]
        public async Task AddMilitary_ShouldAdd1ItemAndReturn3ItemsForEmployee()
        {
            IEnumerable<MilitaryServiceViewModel> militaries;
            MilitaryServiceViewModel military = null;

            await _controller.PostMilitary(new MilitaryService
            {
                Id = 4,
                EmployeeId = 1,
                StartDate = new DateTime(2014, 1, 1),
                FinishDate = new DateTime(2015, 1, 1),
                Rank = "test"
            });
            militaries = _controller.GetMilitary(1).Result.ToList();
            military = militaries.FirstOrDefault(w => String.Equals(w.Rank, "test"));

            Assert.IsNotNull(militaries, "militaries != null");
            Assert.IsTrue(militaries.Count() == 3, "militaries.Count() == 3");
            Assert.IsNotNull(military, "military != null");
        }

        [TestMethod]
        public async Task RemoveMilitary_ShouldRemove1ItemAndReturn0ItemsForEmployee()
        {
            IEnumerable<MilitaryServiceViewModel> militaries;

            await _controller.DeleteMilitary(3);
            militaries = _controller.GetMilitary(3).Result;

            Assert.IsNotNull(militaries, "militaries != null");
            Assert.IsTrue(!militaries.Any(), "!militaries.Any()");
        }

        [TestMethod]
        public void GetSertifications_ShouldReturnItemsByEmployeeId()
        {
            IEnumerable<SertificationViewModel> sertificationFirstPiece, sertificationSecondPiece;

            sertificationFirstPiece = _controller.GetSertifications(1).Result;
            sertificationSecondPiece = _controller.GetSertifications(2).Result;

            Assert.IsNotNull(sertificationFirstPiece, "sertificationFirstPiece != null");
            Assert.IsNotNull(sertificationSecondPiece, "sertificationSecondPiece != null");
            Assert.IsTrue(sertificationFirstPiece.Count() == 2, "sertificationFirstPiece.Count() == 2");
            Assert.IsTrue(sertificationSecondPiece.Count() == 1, "sertificationSecondPiece.Count() == 1");
        }

        [TestMethod]
        public async Task AddSertification_ShouldAdd1ItemAndReturn3ItemsForEmployee()
        {
            IEnumerable<SertificationViewModel> sertifications;
            SertificationViewModel sertification = null;

            await _controller.PostSertification(new SertificationViewModel(new Sertification
            {
                Id = 4, EmployeeId = 1, DueDate = DateTime.Now, Description = "test"
            }));
            sertifications = _controller.GetSertifications(1).Result.ToList();
            sertification = sertifications.FirstOrDefault(w => String.Equals(w.Description,"test"));

            Assert.IsNotNull(sertifications, "sertifications != null");
            Assert.IsTrue(sertifications.Count() == 3, "sertifications.Count() == 3");
            Assert.IsNotNull(sertification, "sertification != null");
        }

        [TestMethod]
        public async Task RemoveSertification_ShouldRemove1ItemAndReturn0ItemsForEmployee()
        {
            IEnumerable<SertificationViewModel> sertifications;

            await _controller.DeleteSertification(3);
            sertifications = _controller.GetSertifications(3).Result;

            Assert.IsNotNull(sertifications, "sertifications != null");
            Assert.IsTrue(!sertifications.Any(), "!sertifications.Any()");
        }

        [TestMethod]
        public void GetOutFromOffice_ShouldReturnItemsByEmployeeId()
        {
            IEnumerable<OutFromOfficeViewModel> outFromOfficeFirstPiece, outFromOfficeSecondPiece;

            outFromOfficeFirstPiece = _controller.GetOutFromOffice(1).Result;
            outFromOfficeSecondPiece = _controller.GetOutFromOffice(2).Result;

            Assert.IsNotNull(outFromOfficeFirstPiece, "outFromOfficeFirstPiece != null");
            Assert.IsNotNull(outFromOfficeSecondPiece, "outFromOfficeSecondPiece != null");
            Assert.IsTrue(outFromOfficeFirstPiece.Count() == 2, "outFromOfficeFirstPiece.Count() == 2");
            Assert.IsTrue(outFromOfficeSecondPiece.Count() == 1, "outFromOfficeSecondPiece.Count() == 1");
        }

        [TestMethod]
        public async Task AddOutFromOffice_ShouldAdd1ItemAndReturn3ItemsForEmployee()
        {
            IEnumerable<OutFromOfficeViewModel> outFromOfficeItems;
            OutFromOfficeViewModel outFromOfficeItem = null;

            await _controller.PostOutFromOffice(new OutFromOfficeViewModel(new OutFromOffice
            {
                Id = 0,
                EmployeeId = 1,
                StartDate = new DateTime(2010, 1, 2),
                FinishDate = new DateTime(2011, 2, 2),
                Description = "test"
            }));
            outFromOfficeItems = _controller.GetOutFromOffice(1).Result.ToList();
            outFromOfficeItem = outFromOfficeItems.FirstOrDefault(w => String.Equals(w.Description, "test"));

            Assert.IsNotNull(outFromOfficeItems, "outFromOfficeItems != null");
            Assert.IsTrue(outFromOfficeItems.Count() == 3, "outFromOfficeItems.Count() == 3");
            Assert.IsNotNull(outFromOfficeItem, "outFromOfficeItem != null");
        }

        [TestMethod]
        public async Task RemoveOutFromOffice_ShouldRemove1ItemAndReturn0ItemsForEmployee()
        {
            IEnumerable<OutFromOfficeViewModel> outFromOfficeItems;

            await _controller.DeleteOutFromOfice(3);
            outFromOfficeItems = _controller.GetOutFromOffice(3).Result;

            Assert.IsNotNull(outFromOfficeItems, "outFromOfficeItems != null");
            Assert.IsTrue(!outFromOfficeItems.Any(), "!outFromOfficeItems.Any()");
        }

        [TestMethod]
        public void GetDisciplineItems_ShouldReturnItemsByEmployeeId()
        {
            IEnumerable<DisciplineItemViewModel> disciplineItemsFirstPiece, disciplineItemsSecondPiece;

            disciplineItemsFirstPiece = _controller.GetDisciplineItems(1).Result;
            disciplineItemsSecondPiece = _controller.GetDisciplineItems(2).Result;

            Assert.IsNotNull(disciplineItemsFirstPiece, "disciplineItemsFirstPiece != null");
            Assert.IsNotNull(disciplineItemsSecondPiece, "disciplineItemsSecondPiece != null");
            Assert.IsTrue(disciplineItemsFirstPiece.Count() == 2, "disciplineItemsFirstPiece.Count() == 2");
            Assert.IsTrue(disciplineItemsSecondPiece.Count() == 1, "disciplineItemsSecondPiece.Count() == 1");
        }

        [TestMethod]
        public async Task AddDisciplineItem_ShouldAdd1ItemAndReturn3ItemsForEmployee()
        {
            IEnumerable<DisciplineItemViewModel> disciplineItems;
            DisciplineItemViewModel disciplineItem = null;

            await _controller.PostDisciplineItem(new DisciplineItemViewModel(new DisciplineItem
            {
                Id = 0,
                EmployeeId = 1,
                ItemType = "G",
                Description = "test"
            }));
            disciplineItems = _controller.GetDisciplineItems(1).Result.ToList();
            disciplineItem = disciplineItems.FirstOrDefault(w => String.Equals(w.Description, "test"));

            Assert.IsNotNull(disciplineItems, "disciplineItems != null");
            Assert.IsTrue(disciplineItems.Count() == 3, "disciplineItems.Count() == 3");
            Assert.IsNotNull(disciplineItem, "disciplineItem != null");
        }

        [TestMethod]
        public async Task RemoveDisciplineItem_ShouldRemove1ItemAndReturn0ItemsForEmployee()
        {
            IEnumerable<DisciplineItemViewModel> disciplineItems;

            await _controller.DeleteOutFromOfice(3);
            disciplineItems = _controller.GetDisciplineItems(3).Result;

            Assert.IsNotNull(disciplineItems, "disciplineItems != null");
            Assert.IsTrue(!disciplineItems.Any(), "!disciplineItems.Any()");
        }

        [TestMethod]
        public void GetMesAchievements_ShouldReturnItemsByEmployeeId()
        {
            IEnumerable<MesAchievementViewModel> mesAchievementsFirstPiece, mesAchievementsSecondPiece;

            mesAchievementsFirstPiece = _controller.GetMesAchiements(1).Result;
            mesAchievementsSecondPiece = _controller.GetMesAchiements(2).Result;

            Assert.IsNotNull(mesAchievementsFirstPiece, "mesAchievementsFirstPiece != null");
            Assert.IsNotNull(mesAchievementsSecondPiece, "mesAchievementsSecondPiece != null");
            Assert.IsTrue(mesAchievementsFirstPiece.Count() == 2, "mesAchievementsFirstPiece.Count() == 2");
            Assert.IsTrue(mesAchievementsSecondPiece.Count() == 1, "mesAchievementsSecondPiece.Count() == 1");
        }

        [TestMethod]
        public async Task AddMesAchievement_ShouldAdd1ItemAndReturn3ItemsForEmployee()
        {
            IEnumerable<MesAchievementViewModel> mesAchievements;
            MesAchievementViewModel mesAchievement = null;

            await _controller.PostMesAchievement(new MesAchievement
            {
                EmployeeId = 1,
                StartDate = new DateTime(2001, 1, 2),
                FinishDate = null,
                Description = "test"
            });
            mesAchievements = _controller.GetMesAchiements(1).Result.ToList();
            mesAchievement = mesAchievements.FirstOrDefault(w => String.Equals(w.Description, "test"));

            Assert.IsNotNull(mesAchievements, "mesAchievements != null");
            Assert.IsTrue(mesAchievements.Count() == 3, "mesAchievements.Count() == 3");
            Assert.IsNotNull(mesAchievement, "mesAchievement != null");
        }

        [TestMethod]
        public async Task RemoveMesAchievement_ShouldRemove1ItemAndReturn0ItemsForEmployee()
        {
            IEnumerable<MesAchievementViewModel> mesAchievements;

            await _controller.DeleteMesAchievement(3);
            mesAchievements = _controller.GetMesAchiements(3).Result;

            Assert.IsNotNull(mesAchievements, "mesAchievements != null");
            Assert.IsTrue(!mesAchievements.Any(), "!mesAchievements.Any()");
        }

        [TestMethod]
        public void GetEducationItems_ShouldReturnItemsByEmployeeId()
        {
            IEnumerable<EducationViewModel> educationItemsFirstPiece, educationItemsSecondPiece;

            educationItemsFirstPiece = _controller.GetEducationItems(1).Result;
            educationItemsSecondPiece = _controller.GetEducationItems(2).Result;

            Assert.IsNotNull(educationItemsFirstPiece, "educationItemsFirstPiece != null");
            Assert.IsNotNull(educationItemsSecondPiece, "educationItemsSecondPiece != null");
            Assert.IsTrue(educationItemsFirstPiece.Count() == 2, "educationItemsFirstPiece.Count() == 2");
            Assert.IsTrue(educationItemsSecondPiece.Count() == 1, "educationItemsSecondPiece.Count() == 1");
        }

        [TestMethod]
        public async Task AddEducationItem_ShouldAdd1ItemAndReturn3ItemsForEmployee()
        {
            IEnumerable<EducationViewModel> educationItems;
            EducationViewModel educationItem = null;

            await _controller.PostEducation(new EducationViewModel(new EducationItem
            {
                Id = 0, EmployeeId = 1, Description = "test"
            }));
            educationItems = _controller.GetEducationItems(1).Result.ToList();
            educationItem = educationItems.FirstOrDefault(w => String.Equals(w.Description, "test"));

            Assert.IsNotNull(educationItems, "educationItems != null");
            Assert.IsTrue(educationItems.Count() == 3, "educationItems.Count() == 3");
            Assert.IsNotNull(educationItem, "educationItem != null");
        }

        [TestMethod]
        public async Task RemoveEducationItem_ShouldRemove1ItemAndReturn0ItemsForEmployee()
        {
            IEnumerable<EducationViewModel> educationItems;

            await _controller.DeleteEducation(3);
            educationItems = _controller.GetEducationItems(3).Result;

            Assert.IsNotNull(educationItems, "educationItems != null");
            Assert.IsTrue(!educationItems.Any(), "!educationItems.Any()");
        }

        [TestMethod]
        public void GetContracts_ShouldReturnItemsByEmployeeId()
        {
            IEnumerable<ContractViewModel> contractsFirstPiece, contractsSecondPiece;

            contractsFirstPiece = _controller.GetContracts(1).Result;
            contractsSecondPiece = _controller.GetContracts(2).Result;

            Assert.IsNotNull(contractsFirstPiece, "contractsFirstPiece != null");
            Assert.IsNotNull(contractsSecondPiece, "contractsSecondPiece != null");
            Assert.IsTrue(contractsFirstPiece.Count() == 2, "contractsFirstPiece.Count() == 2");
            Assert.IsTrue(contractsSecondPiece.Count() == 1, "contractsSecondPiece.Count() == 1");
        }

        [TestMethod]
        public async Task AddContract_ShouldAdd1ItemAndReturn3ItemsForEmployee()
        {
            IEnumerable<ContractViewModel> contracts;
            ContractViewModel contract = null;

            await _controller.PostContract(new ContractViewModel(new Contract
            {
                EmployeeId = 1, Description = "test"
            }));
            contracts = _controller.GetContracts(1).Result.ToList();
            contract = contracts.FirstOrDefault(w => String.Equals(w.Description, "test"));

            Assert.IsNotNull(contracts, "contracts != null");
            Assert.IsTrue(contracts.Count() == 3, "contracts.Count() == 3");
            Assert.IsNotNull(contract, "contract != null");
        }

        [TestMethod]
        public async Task RemoveContract_ShouldRemove1ItemAndReturn0ItemsForEmployee()
        {
            IEnumerable<ContractViewModel> contracts;

            await _controller.DeleteContract(3);
            contracts = _controller.GetContracts(3).Result;

            Assert.IsNotNull(contracts, "contracts != null");
            Assert.IsTrue(!contracts.Any(), "!contracts.Any()");
        }

        [TestMethod]
        public void GetRelatives_ShouldReturnItemsByEmployeeId()
        {
            IEnumerable<RelativeViewModel> relativesFirstPiece, relativesSecondPiece;

            relativesFirstPiece = _controller.GetRelatives(1).Result;
            relativesSecondPiece = _controller.GetRelatives(2).Result;

            Assert.IsNotNull(relativesFirstPiece, "relativesFirstPiece != null");
            Assert.IsNotNull(relativesSecondPiece, "relativesSecondPiece != null");
            Assert.IsTrue(relativesFirstPiece.Count() == 2, "relativesFirstPiece.Count() == 2");
            Assert.IsTrue(relativesSecondPiece.Count() == 1, "relativesSecondPiece.Count() == 1");
        }

        [TestMethod]
        public async Task AddRelative_ShouldAdd1ItemAndReturn3ItemsForEmployee()
        {
            IEnumerable<RelativeViewModel> relatives;
            RelativeViewModel relative = null;

            await _controller.PostRelative(new RelativeViewModel(new Relative
            {
                EmployeeID = 1, Lastname = "test"
            }));
            relatives = _controller.GetRelatives(1).Result.ToList();
            relative = relatives.FirstOrDefault(w => String.Equals(w.Lastname, "test"));

            Assert.IsNotNull(relatives, "contracts != null");
            Assert.IsTrue(relatives.Count() == 3, "contracts.Count() == 3");
            Assert.IsNotNull(relative, "contract != null");
        }

        [TestMethod]
        public async Task RemoveRelative_ShouldRemove1ItemAndReturn0ItemsForEmployee()
        {
            IEnumerable<RelativeViewModel> relatives;

            await _controller.DeleteContract(3);
            relatives = _controller.GetRelatives(3).Result;

            Assert.IsNotNull(relatives, "relatives != null");
            Assert.IsTrue(!relatives.Any(), "!relatives.Any()");
        }

        #region FakeData

        private List<Relative> GetRelativesFakeData() => new List<Relative>
        {
            new Relative
            {
                Id = 1,
                EmployeeID = 1,
                Lastname = "Пупкина",
                Firstname = "Елизавета",
                Middlename = "Астафьевна",
                BirthDate = new DateTime(1979, 1, 1),
                Status = "Супруг(а)"
            },
            new Relative
            {
                Id = 2,
                EmployeeID = 1,
                Lastname = "Пупкина",
                Firstname = "Александра",
                Middlename = "Ивановна",
                BirthDate = new DateTime(1979, 1, 1),
                Status = "Дочь"
            },
            new Relative
            {
                Id = 3,
                EmployeeID = 2,
                Lastname = "Петров",
                Firstname = "Артем",
                Middlename = "Викторович",
                BirthDate = new DateTime(1979, 1, 1),
                Status = "Сын"
            }
        };

        private List<Contract> GetContractsFakeData() => new List<Contract>
        {
            new Contract
            {
                Id = 1,
                EmployeeId = 1,
                StartDate = new DateTime(2016, 1, 1),
                FinishDate = new DateTime(2017, 1, 1)
            },
            new Contract
            {
                Id = 2,
                EmployeeId = 1,
                StartDate = new DateTime(2016, 1, 1),
                FinishDate = new DateTime(2017, 1, 1)
            },
            new Contract
            {
                Id = 3,
                EmployeeId = 2,
                StartDate = new DateTime(2016, 1, 1),
                FinishDate = new DateTime(2017, 1, 1)
            }
        };

        private List<EducationItem> GetEducationFakeData() => new List<EducationItem>
        {
            new EducationItem
            {
                Id = 1,
                EmployeeId = 1,
                StartDate = new DateTime(2013, 1, 1),
                FinishDate = new DateTime(2017, 6, 20)
            },
            new EducationItem
            {
                Id = 2,
                EmployeeId = 1,
                StartDate = new DateTime(2013, 1, 1),
                FinishDate = new DateTime(2017, 6, 20)
            },
            new EducationItem
            {
                Id = 3,
                EmployeeId = 2,
                StartDate = new DateTime(2013, 1, 1),
                FinishDate = new DateTime(2017, 6, 20)
            }
        };

        private List<MesAchievement> GetMesAchievementsFakeData() => new List<MesAchievement>
        {
            new MesAchievement {Id = 1, EmployeeId = 1, StartDate = new DateTime(2001, 1,1), FinishDate = null},
            new MesAchievement {Id = 2, EmployeeId = 1, StartDate = new DateTime(2001, 1,1), FinishDate = null},
            new MesAchievement {Id = 3, EmployeeId = 2, StartDate = new DateTime(2001, 1,1), FinishDate = null}
        };

        private List<DisciplineItem> GetDisciplineFakeData() => new List<DisciplineItem>
        {
            new DisciplineItem
            {
                Id = 1,
                EmployeeId = 1,
                Title = "title1",
                ItemType = "G",
                AwardOrFine = 123213,
                Date = DateTime.Now
            },
            new DisciplineItem
            {
                Id = 2,
                EmployeeId = 1,
                Title = "title2",
                ItemType = "V",
                AwardOrFine = 12321,
                Date = DateTime.Now
            },
            new DisciplineItem
            {
                Id = 3,
                EmployeeId = 2,
                Title = "title3",
                ItemType = "G",
                AwardOrFine = 10000,
                Date = DateTime.Now
            }
        };

        private List<OutFromOffice> GetOutFromOfficeFakeData() => new List<OutFromOffice>
        {
            new OutFromOffice
            {
                Id = 1,
                Cause = "H",
                EmployeeId = 1,
                StartDate = new DateTime(2013, 1, 1),
                FinishDate = new DateTime(2013, 2, 2)
            },
            new OutFromOffice
            {
                Id = 2,
                Cause = "S",
                EmployeeId = 1,
                StartDate = new DateTime(2013, 1, 1),
                FinishDate = new DateTime(2013, 2, 2)
            },
            new OutFromOffice
            {
                Id = 3,
                Cause = "H",
                EmployeeId = 2,
                StartDate = new DateTime(2013, 1, 1),
                FinishDate = new DateTime(2013, 2, 2)
            }
        };

        private List<Sertification> GetSertificationsFakeData() => new List<Sertification>
        {
            new Sertification {Id = 1, EmployeeId = 1, DueDate = DateTime.Now},
            new Sertification {Id = 2, EmployeeId = 1, DueDate = DateTime.Now},
            new Sertification {Id = 3, EmployeeId = 2, DueDate = DateTime.Now}
        };

        private List<MilitaryService> GetMilitaryFakeData() => new List<MilitaryService>
        {
            new MilitaryService
            {
                Id = 1,
                EmployeeId = 1,
                StartDate = new DateTime(2014, 1, 2),
                FinishDate = new DateTime(2015, 7, 2)
            },
            new MilitaryService
            {
                Id = 2,
                EmployeeId = 1,
                StartDate = new DateTime(2014, 1, 2),
                FinishDate = new DateTime(2015, 7, 2)
            },
            new MilitaryService
            {
                Id = 3,
                EmployeeId = 2,
                StartDate = new DateTime(2014, 1, 2),
                FinishDate = new DateTime(2015, 7, 2)
            }
        };


        private List<WorkTerm> GetWorksFakeData() => new List<WorkTerm>
        {
            new WorkTerm
            {
                Id = 1,
                EmployeeId = 1,
                StartDate = new DateTime(2015, 1, 1),
                FinishDate = new DateTime(2016, 1, 1)
            },
            new WorkTerm
            {
                Id = 2,
                EmployeeId = 1,
                StartDate = new DateTime(2015, 1, 1),
                FinishDate = new DateTime(2016, 1, 1)
            },
            new WorkTerm
            {
                Id = 3,
                EmployeeId = 3,
                StartDate = new DateTime(2015, 1, 1),
                FinishDate = new DateTime(2016, 1, 1)
            }
        };

        #endregion
        
        #region Infrastructure

        internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
        {
            private readonly IQueryProvider _inner;

            internal TestDbAsyncQueryProvider(IQueryProvider inner)
            {
                _inner = inner;
            }

            public IQueryable CreateQuery(Expression expression)
            {
                return new TestDbAsyncEnumerable<TEntity>(expression);
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                return new TestDbAsyncEnumerable<TElement>(expression);
            }

            public object Execute(Expression expression)
            {
                return _inner.Execute(expression);
            }

            public TResult Execute<TResult>(Expression expression)
            {
                return _inner.Execute<TResult>(expression);
            }

            public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
            {
                return Task.FromResult(Execute(expression));
            }

            public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
            {
                return Task.FromResult(Execute<TResult>(expression));
            }
        }

        internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
        {
            public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
                : base(enumerable)
            {
            }

            public TestDbAsyncEnumerable(Expression expression)
                : base(expression)
            {
            }

            public IDbAsyncEnumerator<T> GetAsyncEnumerator()
            {
                return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
            }

            IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
            {
                return GetAsyncEnumerator();
            }

            IQueryProvider IQueryable.Provider
            {
                get { return new TestDbAsyncQueryProvider<T>(this); }
            }
        }

        internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _inner;

            public TestDbAsyncEnumerator(IEnumerator<T> inner)
            {
                _inner = inner;
            }

            public void Dispose()
            {
                _inner.Dispose();
            }

            public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(_inner.MoveNext());
            }

            public T Current
            {
                get { return _inner.Current; }
            }

            object IDbAsyncEnumerator.Current
            {
                get { return Current; }
            }
        }

        #endregion
        
    }
}