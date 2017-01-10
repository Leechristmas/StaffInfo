using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Staffinfo.API.Models;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.API.Controllers
{
    [Route("api/employees")]
    [Authorize]
    public class EmployeesController : ApiController
    {
        private readonly IUnitRepository _repository;

        public EmployeesController(IUnitRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Employees
        /// <summary>
        /// Returns all ACTUAL employees
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<EmployeeViewModelMin>> GetActualEmployees(int offset, int limit, string query)
        {
            IEnumerable<Employee> all;
            if (String.IsNullOrEmpty(query))
                all = await _repository.EmployeeRepository.WhereAsync(e => e.RetirementDate == null);
            else
                all =
                    await
                        _repository.EmployeeRepository.WhereAsync(
                            e => e.RetirementDate == null && e.EmployeeLastname.StartsWith(query, StringComparison.OrdinalIgnoreCase));

            System.Web.HttpContext.Current.Response.Headers.Add("X-Total-Count", all.Count().ToString());

            var queryResult =  all.Skip(offset).Take(limit).Select(e => new EmployeeViewModelMin
            {
                Id = e.Id,
                EmployeeLastname = e.EmployeeLastname,
                EmployeeFirstname = e.EmployeeFirstname,
                EmployeeMiddlename = e.EmployeeMiddlename,
                ActualPost = e.ActualPost?.PostName,
                ActualRank = e.ActualRank?.RankName,
                ActualPostId = e.ActualPostId,
                ActualRankId = e.ActualRankId,
                BirthDate = e.BirthDate,
                Description = e.Description
            }).ToList();
            return queryResult;
        }

        // GET: api/Employees/5
        /// <summary>
        /// Returns employee by id
        /// </summary>
        /// <param name="id">id of desired employee</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/employees/{id:int}")]
        public async Task<EmployeeViewModelMin> GetEmployee(int id)
        {
            Employee employee = await _repository.EmployeeRepository.SelectAsync(id);
            return new EmployeeViewModel(employee);
        }

        // POST: api/Employee
        public async Task Post([FromBody]EmployeeViewModel value)
        {
            //adding passport
            Passport passport = new Passport
            {
                PassportNumber = value.PassportNumber,
                PassportOrganization = value.PassportOrganization
            };
            passport = _repository.PassportRepository.Create(passport);
            await _repository.PassportRepository.SaveAsync();

            //adding address
            Address address = new Address
            {
                City = value.City,
                Area = value.Area,
                DetailedAddress = value.DetailedAddress,
                ZipCode = value.ZipCode
            };
            address = _repository.AddressRepository.Create(address);
            await _repository.AddressRepository.SaveAsync();

            Employee newEmpl = new Employee
            {
                Id = 0,
                EmployeeFirstname = value.EmployeeFirstname,
                EmployeeLastname = value.EmployeeLastname,
                EmployeeMiddlename = value.EmployeeMiddlename,
                BirthDate = value.BirthDate,
                PassportId = passport.Id,
                AddressId = address.Id,
                Description = value.Description,
                EmployeePhoto = value.EmployeePhoto,
                FirstPhoneNumber = value.FirstPhoneNumber,
                SecondPhoneNumber = value.SecondPhoneNumber
            };
            _repository.EmployeeRepository.Create(newEmpl);
            await _repository.EmployeeRepository.SaveAsync();
        }

        // PUT: api/Employee/5
        [HttpPut]
        [Route("api/employees/{id:int}")]
        public async Task EditMilitary(int id, [FromBody]EmployeeViewModel value)
        {
            if (value.AddressId != null)
            {
                Address address = await _repository.AddressRepository.SelectAsync(value.AddressId.Value);
                if (address != null)
                {
                    address.City = value.City;
                    address.DetailedAddress = value.DetailedAddress;
                    address.Area = value.Area;
                    address.ZipCode = value.ZipCode;

                    _repository.AddressRepository.Update(address);
                    await _repository.AddressRepository.SaveAsync();
                }
            }

            if (value.PassportId != null)
            {
                Passport passport = await _repository.PassportRepository.SelectAsync(value.PassportId.Value);
                if (passport != null)
                {
                    passport.PassportNumber = value.PassportNumber;
                    passport.PassportOrganization = value.PassportOrganization;

                    _repository.PassportRepository.Update(passport);
                    await _repository.PassportRepository.SaveAsync();
                }
            }

            Employee original = await _repository.EmployeeRepository.SelectAsync(id);
            if (original != null)
            {
                original.EmployeeFirstname = value.EmployeeFirstname;
                original.EmployeeLastname = value.EmployeeLastname;
                original.EmployeeMiddlename = value.EmployeeMiddlename;
                original.BirthDate = value.BirthDate;
                original.Description = value.Description;
                original.EmployeePhoto = value.EmployeePhoto;
                original.FirstPhoneNumber = value.FirstPhoneNumber;
                original.SecondPhoneNumber = value.SecondPhoneNumber;

                _repository.EmployeeRepository.Update(original);
                await _repository.EmployeeRepository.SaveAsync();
            }
        }

        // DELETE: api/Employee/5
        [HttpDelete]
        [Route("api/employees/{id:int}")]
        public async Task Delete(int id)
        {
            await _repository.EmployeeRepository.Delete(id);
            await _repository.EmployeeRepository.SaveAsync();
        }

        [HttpPost]
        [Route("api/employees/retiredtransfer")]
        public async Task TransferToRetired([FromBody]Employee employee)
        {
            Employee original = await _repository.EmployeeRepository.SelectAsync(employee.Id);
            if (original != null && employee?.RetirementDate != null)
            {
                original.RetirementDate = employee.RetirementDate;

                _repository.EmployeeRepository.Update(original);
                await _repository.EmployeeRepository.SaveAsync();
            }
        }

        [HttpPost]
        [Route("api/employees/dismissedtransfer")]
        public async Task TransferToDismissed([FromBody]Dismissal dismissal)
        {
            if(!dismissal.IsCorrect) throw new Exception("Parameter is null");

            Employee original = await _repository.EmployeeRepository.SelectAsync(dismissal.EmployeeId.Value);
            if (original != null)
            {
                await _repository.EmployeeRepository.TransferToDismissed(dismissal.EmployeeId.Value, dismissal.DismissalDate.Value, dismissal.Clause, dismissal.ClauseDescription);
            }
        }

        [Route("api/employees/seniority/{employeeId:int}")]
        [HttpGet]
        public async Task<Seniority> GetSeniority(int employeeId)
        {
            int mes = await _repository.EmployeeRepository.GetExpirience(employeeId,
                EmployeeRepositoryHelper.Expirience.MESAchievements);
            int military = await _repository.EmployeeRepository.GetExpirience(employeeId,
                EmployeeRepositoryHelper.Expirience.Military);

            Seniority seniority = new Seniority
            {
                EmployeeId = employeeId,
                MESSeniorityDays = mes,
                MilitarySeniorityDays = military,
                WorkSeniorityDays = 0
            };

            return seniority;//
        }

        /// <summary>
        /// Returns total seniority statisctic by years
        /// </summary>
        /// <param name="scale">one term in a chart</param>
        /// <param name="min">min value of seniority</param>
        /// <param name="max">max value of seniority</param>
        /// <returns></returns>
        [Route("api/employees/seniority/statistic/total")]
        [HttpGet]
        public async Task<Dictionary<string, int>> GetTotalSeniorityStatistic(int scale = 5, int min = 0, int max = 30)
        {
            return await _repository.EmployeeRepository.GetSeniorityStatistic(scale, min, max, EmployeeRepositoryHelper.Seniority.Total);
        }

        /// <summary>
        /// Returns seniority statisctic by years for actual employees
        /// </summary>
        /// <param name="scale">one term in a chart</param>
        /// <param name="min">min value of seniority</param>
        /// <param name="max">max value of seniority</param>
        /// <returns></returns>
        [Route("api/employees/seniority/statistic/actual")]
        [HttpGet]
        public async Task<Dictionary<string, int>> GetActualSeniorityStatistic(int scale = 5, int min = 0, int max = 30)
        {
            return await _repository.EmployeeRepository.GetSeniorityStatistic(scale, min, max, EmployeeRepositoryHelper.Seniority.Actual);
        }

        [Route("api/employees/servicesstruct")]
        [HttpGet]
        public async Task<Dictionary<string, int>> GetServicesStruct()
        {
            return await _repository.EmployeeRepository.GetServicesStructure();
        }

        #region Reference Books

        /// <summary>
        /// Returns all ranks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/employees/ranks")]
        public async Task<IEnumerable<NamedEntity>> GetRanks()
        {
            IEnumerable<Rank> list = await _repository.RankRepository.SelectAsync();
            return list.OrderBy(r => r.RankWeight).Select(r => new NamedEntity {Name = r.RankName, Id = r.Id});
        }

        [HttpGet]
        [Route("api/employees/ranks/{id:int}")]
        public async Task<Rank> GetRank(int id)
        {
            return await _repository.RankRepository.SelectAsync(id);
        }

        [HttpGet]
        [Route("api/employees/services")]
        public async Task<IEnumerable<Service>> GetServices()
        {
            return await _repository.ServiceRepository.SelectAsync();
        }

        [HttpGet]
        [Route("api/employees/services/{id:int}")]
        public async Task<Service> GetService(int id)
        {
            return await _repository.ServiceRepository.SelectAsync(id);
        }

        [HttpGet]
        [Route("api/employees/posts")]
        public async Task<IEnumerable<NamedEntity>> GetPosts()
        {
            IEnumerable<Post> list =  await _repository.PostRepository.SelectAsync();
            return list.OrderBy(p => p.PostWeight).Select(p => new NamedEntity {Id = p.Id, Name = p.PostName});
        }

        [HttpGet]
        [Route("api/employees/postsforservice/{serviceId:int}")]
        public async Task<IEnumerable<NamedEntity>> GetPostsByServiceId(int serviceId)
        {
            IEnumerable<Post> list = await _repository.PostRepository.WhereAsync(p => p.ServiceId == serviceId);
            return list.OrderBy(p => p.PostWeight).Select(p => new NamedEntity { Id = p.Id, Name = p.PostName });
        }

        [HttpGet]
        [Route("api/employees/posts/{id:int}")]
        public async Task<Post> GetPost(int id)
        {
            return await _repository.PostRepository.SelectAsync(id);
        }

        [HttpPost]
        [Route("api/employees/mesachievements")]
        public async Task PostMesAchievement([FromBody]MesAchievement value)
        {
            MesAchievement mesAchievement = new MesAchievement
            {
                Id = 0,
                StartDate = value.StartDate,
                FinishDate = value.FinishDate,
                LocationId = value.LocationId,
                PostId = value.PostId,
                RankId = value.RankId,
                EmployeeId = value.EmployeeId
            };
            _repository.MesAchievementRepository.Create(mesAchievement);
            await _repository.MesAchievementRepository.SaveAsync();
        }

        [HttpGet]
        [Route("api/employees/mesachievements/{emplId:int}")]
        public async Task<IEnumerable<MesAchievementViewModel>> GetMesAchiements(int emplId)
        {
            IEnumerable<MesAchievement> mesAchievements = await _repository.MesAchievementRepository.WhereAsync(i => i.EmployeeId == emplId);
            return mesAchievements.Select(i => new MesAchievementViewModel(i));
        }

        [HttpDelete]
        [Route("api/employees/mesachievements/{id:int}")]
        public async Task DeleteMesAchievement(int id)
        {
            await _repository.MesAchievementRepository.Delete(id);
            await _repository.MesAchievementRepository.SaveAsync();
        }

        [HttpPost]
        [Route("api/employees/discipline")]
        public async Task PostDisciplineItem([FromBody] DisciplineItemViewModel model)
        {
            DisciplineItem disciplineItem = new DisciplineItem
            {
                Id = 0,
                EmployeeId = model.EmployeeId,
                Title = model.Title,
                ItemType = model.ItemType,
                Date = model.Date,
                AwardOrFine = model.AwardOrFine,
                Description = model.Description
            };
            _repository.DisciplineItemRepository.Create(disciplineItem);
            await _repository.DisciplineItemRepository.SaveAsync();
        }

        [HttpGet]
        [Route("api/employees/discipline/{emplId:int}")]
        public async Task<IEnumerable<DisciplineItemViewModel>> GetDisciplineItems(int emplId)
        {
            IEnumerable<DisciplineItem> disciplineItems =
                await _repository.DisciplineItemRepository.WhereAsync(i => i.EmployeeId == emplId);
            return disciplineItems.Select(i => new DisciplineItemViewModel(i));
        }

        [HttpDelete]
        [Route("api/employees/discipline/{id:int}")]
        public async Task DeleteDisciplineItem(int id)
        {
            await _repository.DisciplineItemRepository.Delete(id);
            await _repository.DisciplineItemRepository.SaveAsync();
        }

        [HttpPost]
        [Route("api/employees/outfromoffice")]
        public async Task PostOutFromOffice([FromBody] OutFromOfficeViewModel model)
        {
            OutFromOffice outFromOffice = new OutFromOffice
            {
                Id = 0,
                EmployeeId = model.EmployeeId,
                StartDate = model.StartDate,
                FinishDate = model.FinishDate,
                Cause = model.Cause,
                Description = model.Description
            };
            _repository.OutFromOfficeRepository.Create(outFromOffice);
            await _repository.OutFromOfficeRepository.SaveAsync();
        }

        [HttpGet]
        [Route("api/employees/outfromoffice/{employeeId:int}")]
        public async Task<IEnumerable<OutFromOfficeViewModel>> GetOutFromOffice(int employeeId)
        {
            IEnumerable<OutFromOffice> outFromOffice =
                    await _repository.OutFromOfficeRepository.WhereAsync(i => i.EmployeeId == employeeId);
            return outFromOffice.Select(i => new OutFromOfficeViewModel(i));
        }

        [HttpDelete]
        [Route("api/employees/outfromoffice/{id:int}")]
        public async Task DeleteOutFromOfice(int id)
        {
            await _repository.OutFromOfficeRepository.Delete(id);
            await _repository.OutFromOfficeRepository.SaveAsync();
        }

        [HttpGet]
        [Route("api/employees/sertification/{employeeId:int}")]
        public async Task<IEnumerable<SertificationViewModel>> GetSertifications(int employeeId)
        {
            IEnumerable<Sertification> sertifications =
                await _repository.SertificationRepository.WhereAsync(i => i.EmployeeId == employeeId);
            return sertifications.Select(i => new SertificationViewModel(i));
        }

        [HttpPost]
        [Route("api/employees/sertification")]
        public async Task PostSertification([FromBody]SertificationViewModel model)
        {
            Sertification sertification = new Sertification
            {
                Id = 0,
                EmployeeId = model.EmployeeId,
                DueDate = model.DueDate,
                Description = model.Description
            };
            _repository.SertificationRepository.Create(sertification);
            await _repository.SertificationRepository.SaveAsync();
        }

        [HttpDelete]
        [Route("api/employees/sertification/{id:int}")]
        public async Task DeleteSertification(int id)
        {
            await _repository.SertificationRepository.Delete(id);
            await _repository.SertificationRepository.SaveAsync();
        }

        [HttpPost]
        [Route("api/employees/military")]
        public async Task PostMilitary([FromBody] MilitaryService value)
        {
            MilitaryService militaryService = new MilitaryService
            {
                Id = 0,
                StartDate = value.StartDate,
                FinishDate = value.FinishDate,
                EmployeeId = value.EmployeeId,
                Description = value.Description,
                LocationId = value.LocationId,
                Rank = value.Rank
            };
            _repository.MilitaryServiceRepository.Create(militaryService);
            await _repository.MilitaryServiceRepository.SaveAsync();
        }

        // PUT: api/Employee/5
        [HttpPut]
        [Route("api/employees/{id:int}")]
        public async Task Put(int id, [FromBody]EmployeeViewModel value)
        {
            if (value.AddressId != null)
            {
                Address address = await _repository.AddressRepository.SelectAsync(value.AddressId.Value);
                if (address != null)
                {
                    address.City = value.City;
                    address.DetailedAddress = value.DetailedAddress;
                    address.Area = value.Area;
                    address.ZipCode = value.ZipCode;

                    _repository.AddressRepository.Update(address);
                    await _repository.AddressRepository.SaveAsync();
                }
            }

            if (value.PassportId != null)
            {
                Passport passport = await _repository.PassportRepository.SelectAsync(value.PassportId.Value);
                if (passport != null)
                {
                    passport.PassportNumber = value.PassportNumber;
                    passport.PassportOrganization = value.PassportOrganization;

                    _repository.PassportRepository.Update(passport);
                    await _repository.PassportRepository.SaveAsync();
                }
            }

            Employee original = await _repository.EmployeeRepository.SelectAsync(id);
            if (original != null)
            {
                original.EmployeeFirstname = value.EmployeeFirstname;
                original.EmployeeLastname = value.EmployeeLastname;
                original.EmployeeMiddlename = value.EmployeeMiddlename;
                original.BirthDate = value.BirthDate;
                original.Description = value.Description;
                original.EmployeePhoto = value.EmployeePhoto;
                original.FirstPhoneNumber = value.FirstPhoneNumber;
                original.SecondPhoneNumber = value.SecondPhoneNumber;

                _repository.EmployeeRepository.Update(original);
                await _repository.EmployeeRepository.SaveAsync();
            }
        }

        [HttpPut]
        [Route("api/employees/military/{id:int}")]
        public async Task PutMilitary(int id, [FromBody]MilitaryServiceViewModel model)
        {
            MilitaryService original = await _repository.MilitaryServiceRepository.SelectAsync(id);
            if (original != null)
            {
                original.StartDate = model.StartDate;
                original.FinishDate = model.FinishDate;
                original.EmployeeId = model.EmployeeId;
                original.LocationId = model.LocationId;
                original.Rank = model.Rank;
                original.Description = model.Description;

                _repository.MilitaryServiceRepository.Update(original);
                await _repository.MilitaryServiceRepository.SaveAsync();
            }
        }

        [HttpGet]
        [Route("api/employees/military/{emplId:int}")]
        public async Task<IEnumerable<MilitaryServiceViewModel>> GetMilitary(int emplId)
        {
            IEnumerable<MilitaryService> military = await _repository.MilitaryServiceRepository.WhereAsync(i => i.EmployeeId == emplId);
            return military.Select(i => new MilitaryServiceViewModel(i));
        }

        [HttpDelete]
        [Route("api/employees/military/{id:int}")]
        public async Task DeleteMilitary(int id)
        {
            await _repository.MilitaryServiceRepository.Delete(id);
            await _repository.MilitaryServiceRepository.SaveAsync();
        }

        [HttpGet]
        [Route("api/employees/works/{emplId:int}")]
        public async Task<IEnumerable<WorkTermViewModel>> GetWorks(int emplId)
        {
            IEnumerable<WorkTerm> works = await _repository.WorkTermRepository.WhereAsync(i => i.EmployeeId == emplId);
            return works.Select(i => new WorkTermViewModel(i));
        }

        [HttpDelete]
        [Route("api/employees/works/{id:int}")]
        public async Task DeleteWork(int id)
        {
            await _repository.WorkTermRepository.Delete(id);
            await _repository.WorkTermRepository.SaveAsync();
        }

        [Route("api/employees/works")]
        public async Task PostWork([FromBody] WorkTerm value)
        {
            WorkTerm workTerm = new WorkTerm
            {
                Id = 0,
                EmployeeId = value.EmployeeId,
                Description = value.Description,
                LocationId = value.LocationId,
                StartDate = value.StartDate,
                FinishDate = value.FinishDate,
                Post = value.Post
            };
            _repository.WorkTermRepository.Create(workTerm);
            await _repository.WorkTermRepository.SaveAsync();
        }

        [HttpGet]
        [Route("api/employees/locations")]
        public async Task<IEnumerable<NamedEntity>> GetLocations()
        {
            IEnumerable<Location> locations = await _repository.LocationRepository.SelectAsync();
            return locations.OrderBy(l => l.LocationName)
                .Select(l => new NamedEntity {Id = l.Id, Name = l.LocationName});
        }

        #endregion

    }
}
