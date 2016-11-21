using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac.Core.Lifetime;
using Staffinfo.API.Models;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.API.Controllers
{
    [Route("api/employees")]
    public class EmployeeController : ApiController
    {
        private readonly IUnitRepository _repository;

        public EmployeeController(IUnitRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Employees
        /// <summary>
        /// Returns all ACTUAL employees
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<EmployeeViewModelMin>> GetActualEmployees(int offset, int limit)
        {
            var all = await _repository.EmployeeRepository.WhereAsync(e => e.RetirementDate == null);

            System.Web.HttpContext.Current.Response.Headers.Add("X-Total-Count", all.Count().ToString());

            return all.Skip(offset).Take(limit).Select(e => new EmployeeViewModelMin
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
            });
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
                Description = value.Description
            };
            _repository.EmployeeRepository.Create(newEmpl);
            await _repository.EmployeeRepository.SaveAsync();
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
                }
                _repository.AddressRepository.Update(address);
                await _repository.AddressRepository.SaveAsync();
            }

            if (value.PassportId != null)
            {
                Passport passport = await _repository.PassportRepository.SelectAsync(value.PassportId.Value);
                if (passport != null)
                {
                    passport.PassportNumber = value.PassportNumber;
                    passport.PassportOrganization = value.PassportOrganization;
                }
                _repository.PassportRepository.Update(passport);
                await _repository.PassportRepository.SaveAsync();
            }

            Employee original = await _repository.EmployeeRepository.SelectAsync(id);
            if (original != null)
            {
                original.EmployeeFirstname = value.EmployeeFirstname;
                original.EmployeeLastname = value.EmployeeLastname;
                original.EmployeeMiddlename = value.EmployeeMiddlename;
                original.BirthDate = value.BirthDate;
                original.Description = value.Description;
            }
            _repository.EmployeeRepository.Update(original);
            await _repository.EmployeeRepository.SaveAsync();
        }

        // DELETE: api/Employee/5
        [HttpDelete]
        [Route("api/employees/{id:int}")]
        public async Task Delete(int id)
        {
            _repository.EmployeeRepository.Delete(id);
            await _repository.EmployeeRepository.SaveAsync();
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
            _repository.MesAchievementRepository.Delete(id);
            await _repository.MesAchievementRepository.SaveAsync();
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
            _repository.MilitaryServiceRepository.Delete(id);
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
            _repository.WorkTermRepository.Delete(id);
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
