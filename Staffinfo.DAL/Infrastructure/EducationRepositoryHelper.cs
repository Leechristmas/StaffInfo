using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.DAL.Infrastructure
{
    public static class EducationRepositoryHelper
    {
        /// <summary>
        /// Returns all educations levels
        /// </summary>
        /// <param name="educationRepository">repository</param>
        /// <returns></returns>
        public static async Task<IEnumerable<EducationLevel>> GetEducationLevels(this IRepository<EducationItem> educationRepository)
        {
            return
                await
                    educationRepository.Database.SqlQuery<EducationLevel>(@"SELECT * FROM dbo.tbl_EducationLevel")
                        .ToListAsync();
        }
    }
}