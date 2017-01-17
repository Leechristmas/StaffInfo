using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
        /// Seniority type
        /// </summary>
        public enum Seniority
        {
            Total = 0,
            Actual
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
                        "select dbo.fn_GetSeniorityByEmployeeID(@employeeId, @type);",
                        new SqlParameter("@employeeId", employeeId),
                        new SqlParameter("@type", (int)expirienceType));

            return await days.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Returns dictionary (service name - count employees of this service)
        /// </summary>
        /// <param name="employeeRepository"></param>
        /// <returns></returns>
        public static async Task<Dictionary<string, int>> GetServicesStructure(
            this IRepository<Employee> employeeRepository)
        {
            return
                await
                    employeeRepository.Database.SqlQuery<ServiceStructQueryResult>("dbo.pr_GetServicesStructure NULL").ToDictionaryAsync(a => a.Name, b => b.Count);
        }

        /// <summary>
        /// Returns notifications
        /// </summary>
        /// <param name="employeeRepository"></param>
        /// <param name="includeCustomNotifications">if include notifications created by user</param>
        /// <param name="includeSertification">if include sertification</param>
        /// <param name="includeBirthDates">if include birth dates</param>
        /// <returns></returns>
        public static async Task<List<Notification>> GetNotifications(this IRepository<Employee> employeeRepository, bool includeCustomNotifications = false, bool includeSertification = false, bool includeBirthDates = false)
        {
            return
                await
                    employeeRepository.Database.SqlQuery<Notification>(
                        "dbo.pr_GetNotifications @includeCustomNotifications, @includeSertification, @includeBirthDates;",
                        new SqlParameter("@includeCustomNotifications", includeCustomNotifications),
                        new SqlParameter("@includeSertification", includeSertification),
                        new SqlParameter("@includeBirthDates", includeBirthDates))
                        .ToListAsync();
        }

        public static async Task<Notification> AddNotification(this IRepository<Employee> employeeRepository, Notification notification)
        {
            return await employeeRepository.Database.SqlQuery<Notification>(
                 "dbo.pr_AddNotification @Author, @Title, @Details, @DueDate;",
                 new SqlParameter("@Author", notification.Author),
                 new SqlParameter("@Title", notification.Title),
                 new SqlParameter("@Details", notification.Details),
                 new SqlParameter("@DueDate", notification.DueDate)).FirstOrDefaultAsync();
        }

        public static async Task DeleteNotification(this IRepository<Employee> employeeRepository, int notificationId)
        {
            await employeeRepository.Database.ExecuteSqlCommandAsync("dbo.pr_DeleteNotification @notificationId",
                new SqlParameter("@notificationId", notificationId));
        }

        /// <summary>
        /// Returns seniority statistic
        /// </summary>
        /// <param name="employeeRepository"></param>
        /// <param name="scale">step</param>
        /// <param name="min">min value</param>
        /// <param name="max">max value</param>
        /// <param name="seniority">seniority type</param>
        /// <returns></returns>
        public static async Task<Dictionary<string, int>> GetSeniorityStatistic(this IRepository<Employee> employeeRepository, int scale, int min, int max, Seniority seniority)
        {
            List<int> list = null;

            switch (seniority)
            {
                case Seniority.Total:
                    list =
                    await
                        employeeRepository.Database.SqlQuery<int>(
                            "SELECT dbo.fn_GetSeniorityByEmployeeID(te.ID, 1) FROM tbl_Employee te;").ToListAsync();
                    break;
                case Seniority.Actual:
                    list =
                    await
                        employeeRepository.Database.SqlQuery<int>(
                            "SELECT dbo.fn_GetSeniorityByEmployeeID(te.ID, 1) FROM tbl_Employee te where te.RetirementDate is null;").ToListAsync();
                    break;
                default:
                    throw new Exception("Incorrect seniority type!");
            }

            Dictionary<string, int> statistic = new Dictionary<string, int>();

            int[] sorted = new int[max % scale == 0 ? max / scale : max / scale + 1];

            //TODO: days count
            foreach (var sen in list)
            {
                int position = (sen / 365) / scale;
                sorted[position] += 1;
            }

            var pos = 0;

            for (int i = min; i < max; i += scale)
            {
                statistic.Add((i + scale) <= max ? $"от {i} до {i + scale} лет" : $"от {i} до {max} лет", sorted[pos]);
                pos++;
            }

            return statistic;
        }


    }

    internal class ServiceStructQueryResult
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

}