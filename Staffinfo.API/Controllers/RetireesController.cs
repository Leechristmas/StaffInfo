using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Staffinfo.API.Models;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.API.Controllers
{
    [Route("api/retirees")]
    public class RetireesController: ApiController
    {
        private readonly IUnitRepository _repository;

        public RetireesController(IUnitRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<RetiredMinViewModel>> GetRetirees(int offset, int limit, string query)
        {
            IEnumerable<Employee> all;
            if (String.IsNullOrEmpty(query))
                all = await _repository.EmployeeRepository.WhereAsync(e => e.RetirementDate != null);
            else
                all =
                    await
                        _repository.EmployeeRepository.WhereAsync(
                            e => e.RetirementDate != null && e.EmployeeLastname.StartsWith(query, StringComparison.OrdinalIgnoreCase));

            System.Web.HttpContext.Current.Response.Headers.Add("X-Total-Count", all.Count().ToString());

            return all.Skip(offset).Take(limit).Select(e => new RetiredMinViewModel(e));
        }

    }
}