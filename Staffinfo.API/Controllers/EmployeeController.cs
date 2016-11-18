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
                BirthDate = e.BirthDate
            });
        }

        /// <summary>
        /// Returns pensioners
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public async Task<IEnumerable<EmployeeViewModel>> GetPensioners()
        //{
        //    var all = await _repository.EmployeeRepository.WhereAsync(e => e.IsPensioner);
        //    return all.Select(e => new EmployeeViewModel
        //    {
        //        Id = e.Id,
        //        EmployeeLastname = e.EmployeeLastname,
        //        EmployeeFirstname = e.EmployeeFirstname,
        //        EmployeeMiddlename = e.EmployeeMiddlename,
        //        ActualPost = e.ActualPost.PostName,
        //        ActualRank = e.ActualRank.RankName,
        //        ActualPostId = e.ActualPostId,
        //        ActualRankId = e.ActualRankId,
        //        BirthDate = e.BirthDate
        //    });
        //}

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
        public EmployeeViewModelMin Post([FromBody]EmployeeViewModelMin value)
        {
            Employee newEmpl = _repository.EmployeeRepository.Create(EmployeeViewModelMin.GetEmployeeFromModel(value));
            _repository.EmployeeRepository.SaveAsync();

            return new EmployeeViewModelMin(newEmpl);
        }

        // PUT: api/Employee/5
        public void Put(int id, [FromBody]string value)
        {
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
        public async Task<IEnumerable<Rank>> GetRanks()
        {
            return await _repository.RankRepository.SelectAsync();
        }

        [HttpGet]
        [Route("api/employees/ranks/{id}")]
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
        [Route("api/employees/services/{id}")]
        public async Task<Service> GetService(int id)
        {
            return await _repository.ServiceRepository.SelectAsync(id);
        }

        [HttpGet]
        [Route("api/employees/posts")]
        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _repository.PostRepository.SelectAsync();
        }

        [HttpGet]
        [Route("api/employees/posts/{id}")]
        public async Task<Post> GetPost(int id)
        {
            return await _repository.PostRepository.SelectAsync(id);
        }


        #endregion

    }
}
    