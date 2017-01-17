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
        public async Task AddEmployee([FromBody]EmployeeViewModel value)
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
        public async Task EditEmployee(int id, [FromBody]EmployeeViewModel value)
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
    }
}
