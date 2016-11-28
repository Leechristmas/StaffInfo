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
    [Route("api/dismissed")]
    public class DismissedController: ApiController
    {
        private readonly IUnitRepository _repository;

        public DismissedController(IUnitRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<Dismissed>> GetDismissed(int offset, int limit, string query)
        {
            IEnumerable<Dismissed> all;
            if (String.IsNullOrEmpty(query))
                all = await _repository.DismissedRepository.SelectAsync();
            else
                all =
                    await
                        _repository.DismissedRepository.WhereAsync(e => e.DismissedLastname.Contains(query));

            System.Web.HttpContext.Current.Response.Headers.Add("X-Total-Count", all.Count().ToString());

            return all.Skip(offset).Take(limit);
        }

        [HttpDelete]
        [Route("api/dismissed/{id:int}")]
        public async Task Delete(int id)
        {
            await _repository.DismissedRepository.Delete(id);
            await _repository.DismissedRepository.SaveAsync();
        }

    }
}