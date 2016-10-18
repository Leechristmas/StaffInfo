using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ninject;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.Web.Controllers.Staffinfo
{
    [RoutePrefix("api/employees")]
    public class EmployeesController : ApiController
    {
        private readonly IUnitRepository _repository;

        public EmployeesController(IUnitRepository staffRepository)
        {
            _repository = staffRepository;
        }

        // GET: api/Employees
        public IEnumerable<Employee> Get()
        {
            var employees = _repository.EmployeeRepository.Select();

            return employees;
        }

        // GET: api/Employees/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Employees
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Employees/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Employees/5
        public void Delete(int id)
        {
        }
    }
}
