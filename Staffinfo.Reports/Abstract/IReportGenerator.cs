using System.IO;
using System.Threading.Tasks;

namespace Staffinfo.Reports.Abstract
{
    public interface IReportGenerator
    {
        Task<MemoryStream> GetTotalEmployeesListAsPdf();
        Task<MemoryStream> GetTotalEmployeesListAsXlsx();
    }
}