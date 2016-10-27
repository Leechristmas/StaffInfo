using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.API.Controllers
{
    public class EmployeeController : ApiController
    {
        private readonly IUnitRepository _repository;

        public EmployeeController(IUnitRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Employee
        public IEnumerable<string> Get()
        {
            return _repository.EmployeeRepository.Select().Select(e => e.EmployeeLastname);
        }

        // GET: api/Employee/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Employee
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Employee/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Employee/5
        public void Delete(int id)
        {
        }
    }
}
