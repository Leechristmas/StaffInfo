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
        /// Expirience type
        /// </summary>
        public enum Expirience
        {
            Common = 0,
            MESAchievements,
            Military,
            Work
        }

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
               new SqlParameter("@employeeId", employeeId),
               new SqlParameter("@dismissalDate", dismissalDate),
               new SqlParameter("@clause", clause),
               new SqlParameter("@clauseDescription", clauseDescription));
        }

        /// <summary>
        /// Returns expirience (days) for the specified employee
        /// </summary>
        /// <param name="employeeRepository"></param>
        /// <param name="employeeId">ID of the employee</param>
        /// <param name="expirienceType">type of expirience</param>
        /// <returns></returns>
        public static async Task<int> GetExpirience(this IRepository<Employee> employeeRepository, int employeeId,
            Expirience expirienceType)
        {
            var days =
                    employeeRepository.Database.SqlQuery<int>(
                        "select dbo.fn_GetExpirienceByEmployeeID(@employeeId, @type);",
                        new SqlParameter("@employeeId", employeeId),
                        new SqlParameter("@type", (int)expirienceType));

            return await days.FirstOrDefaultAsync();
        }


    }
}