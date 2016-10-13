using Staffinfo.DAL.Context;

namespace Staffinfo.DAL.Repositories.Interfaces
{
    public interface IStaffRepository
    {
        StaffContext StaffContext { get;set; } 
    }
}