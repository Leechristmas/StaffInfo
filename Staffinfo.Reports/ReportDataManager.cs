using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories;

namespace Staffinfo.Reports
{
    public static class ReportDataManager
    {
        /// <summary>
        /// Returns data for the 'total employees' report
        /// </summary>
        /// <returns></returns>
        public async static Task<DataTable> GetTotalEmployeesData()
        {
            DataTable data = null;
            using (Repository<Employee> repository = new Repository<Employee>(new StaffContext()))
            {
                var query = repository.SelectAsync();

                data = new DataTable();
                data.Columns.Add("lastname", typeof (string));
                data.Columns.Add("firstname", typeof (string));
                data.Columns.Add("middlename", typeof (string));
                data.Columns.Add("birthdate", typeof (string));
                data.Columns.Add("currentrank", typeof (string));
                data.Columns.Add("currentpost", typeof (string));

                var employees = await query;

                foreach (var employee in employees)
                {
                    data.Rows.Add(employee.EmployeeLastname, employee.EmployeeFirstname, employee.EmployeeMiddlename,
                        employee.BirthDate.ToString("d", new CultureInfo("ru-RU")), employee.ActualRank,
                        employee.ActualPost);
                }
            }

            return data;
        }
    }
}