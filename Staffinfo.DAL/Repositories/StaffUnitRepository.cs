using System;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.DAL.Repositories
{
    /// <summary>
    /// Unit repository (Unit Of Work pattern)
    /// </summary>
    public class StaffUnitRepository: IUnitRepository
    {
        private readonly StaffContext _staffContext;

        public StaffUnitRepository()
        {
            _staffContext = new StaffContext();
        }

        private AddressRepository _addressRepository;
        private EmployeeRepository _employeeRepository;
        private LocationRepository _locationRepository;
        private MesAchievementRepository _mesAchievementRepository;
        private MilitaryServiceRepository _militaryServiceRepository;
        private PassportRepository _passportRepository;
        private PostRepository _postRepository;
        private RankRepository _rankRepository;
        private ServiceRepository _serviceRepository;
        private WorkTermRepository _workTermRepository;

        public IRepository<Address> AddressRepository => _addressRepository ?? new AddressRepository(_staffContext);
        public IRepository<Employee> EmployeeRepository => _employeeRepository ?? new EmployeeRepository(_staffContext);
        public IRepository<Location> LocationRepository => _locationRepository ?? new LocationRepository(_staffContext);

        public IRepository<MesAchievement> MesAchievementRepository
            => _mesAchievementRepository ?? new MesAchievementRepository(_staffContext);

        public IRepository<MilitaryService> MilitaryServiceRepository
            => _militaryServiceRepository ?? new MilitaryServiceRepository(_staffContext);

        public IRepository<Passport> PassportRepository => _passportRepository ?? new PassportRepository(_staffContext);
        public IRepository<Post> PostRepository => _postRepository ?? new PostRepository(_staffContext);
        public IRepository<Rank> RankRepository => _rankRepository ?? new RankRepository(_staffContext);
        public IRepository<Service> ServiceRepository => _serviceRepository ?? new ServiceRepository(_staffContext);
        public IRepository<WorkTerm> WorkTermRepository => _workTermRepository ?? new WorkTermRepository(_staffContext);

        public void Save()
        {
            _staffContext.SaveChanges();
        }


        #region IDisposable

        private bool _disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _staffContext.Dispose();
                }
                this._disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}