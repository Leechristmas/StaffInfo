using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Staffinfo.API.Models;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.API.Controllers
{
    [Route("api/reference-books")]
    public class ReferenceBooksController : ApiController
    {
        private readonly IUnitRepository _repository;

        public ReferenceBooksController(IUnitRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Returns all ranks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/reference-books/ranks")]
        public async Task<IEnumerable<NamedEntity>> GetRanks()
        {
            IEnumerable<Rank> list = await _repository.RankRepository.SelectAsync();
            return list.OrderBy(r => r.RankWeight).Select(r => new NamedEntity { Name = r.RankName, Id = r.Id });
        }

        [HttpGet]
        [Route("api/reference-books/ranks/{id:int}")]
        public async Task<Rank> GetRank(int id)
        {
            return await _repository.RankRepository.SelectAsync(id);
        }

        [HttpGet]
        [Route("api/reference-books/services")]
        public async Task<IEnumerable<Service>> GetServices()
        {
            return await _repository.ServiceRepository.SelectAsync();
        }

        [HttpGet]
        [Route("api/reference-books/services/{id:int}")]
        public async Task<Service> GetService(int id)
        {
            return await _repository.ServiceRepository.SelectAsync(id);
        }

        [HttpGet]
        [Route("api/reference-books/posts")]
        public async Task<IEnumerable<NamedEntity>> GetPosts()
        {
            IEnumerable<Post> list = await _repository.PostRepository.SelectAsync();
            return list.OrderBy(p => p.PostWeight).Select(p => new NamedEntity { Id = p.Id, Name = p.PostName });
        }

        [HttpGet]
        [Route("api/reference-books/posts-for-service/{serviceId:int}")]
        public async Task<IEnumerable<NamedEntity>> GetPostsByServiceId(int serviceId)
        {
            IEnumerable<Post> list = await _repository.PostRepository.WhereAsync(p => p.ServiceId == serviceId);
            return list.OrderBy(p => p.PostWeight).Select(p => new NamedEntity { Id = p.Id, Name = p.PostName });
        }

        [HttpGet]
        [Route("api/reference-books/posts/{id:int}")]
        public async Task<Post> GetPost(int id)
        {
            return await _repository.PostRepository.SelectAsync(id);
        }

        [HttpGet]
        [Route("api/reference-books/locations")]
        public async Task<IEnumerable<NamedEntity>> GetLocations()
        {
            IEnumerable<Location> locations = await _repository.LocationRepository.SelectAsync();
            return locations.OrderBy(l => l.LocationName)
                .Select(l => new NamedEntity { Id = l.Id, Name = l.LocationName });
        }

    }
}
