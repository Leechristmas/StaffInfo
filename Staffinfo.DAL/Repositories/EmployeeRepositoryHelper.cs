using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.DAL.Repositories
{
    public static class EmployeeRepositoryHelper
    {
        /// <summary>
        /// Transfers the employee to dismissed
        /// </summary>
        /// <param name="employeeRepository"></param>
        /// <param name="employeeId">ID of the employee</param>
        /// <param name="dismissalDate">The date of the dimsissal</param>
        /// <param name="clause">the clause</param>
        /// <param name="clauseDescription">custom description of the clause</param>
        /// <returns></returns>
        public static async Task TransferToDismissed(this IRepository<Employee> employeeRepository, int employeeId, DateTime dismissalDate, string clause,
            string clauseDescription)
        {
            await employeeRepository.Database.ExecuteSqlCommandAsync(
               "dbo.pr_TransferEmployeeToDismissed @employeeId, @dismissalDate, @clause, @clauseDescription",
               new SqlParameter("@employeeId", 2),
               new SqlParameter("@dismissalDate", DateTime.Now),
               new SqlParameter("@clause", "333"),
               new SqlParameter("@clauseDescription", "azaza"));
        }
    }
}