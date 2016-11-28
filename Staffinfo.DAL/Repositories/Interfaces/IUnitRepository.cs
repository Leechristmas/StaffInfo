using System;
using Staffinfo.DAL.Models;

namespace Staffinfo.DAL.Repositories.Interfaces
{
    /// <summary>
    /// Contract of 'Unit Of Work' pattern
    /// </summary>
    public interface IUnitRepository : IDisposable
    {
        IRepository<Address> AddressRepository { get; }
        IRepository<Employee> EmployeeRepository { get; }
        IRepository<Dismissed> DismissedRepository { get; } 
        IRepository<Location> LocationRepository { get; }
        IRepository<MesAchievement> MesAchievementRepository { get; }
        IRepository<MilitaryService> MilitaryServiceRepository { get; }
        IRepository<Passport> PassportRepository { get; }
        IRepository<Post> PostRepository { get; }
        IRepository<Rank> RankRepository { get; }
        IRepository<Service> ServiceRepository { get; }
        IRepository<WorkTerm> WorkTermRepository { get; }
    }
}