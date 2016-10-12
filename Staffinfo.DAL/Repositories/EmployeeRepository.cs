using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.DAL.Repositories
{
    /// <summary>
    /// Repository for employees entities
    /// </summary>
    public class EmployeeRepository: IRepository<Employee>
    {
        private readonly StaffContext _staffContext ;

        public EmployeeRepository(StaffContext staffContext)
        {
            _staffContext = staffContext;
        }

        public IEnumerable<Employee> Select()
        {
            return _staffContext.Employees;
        }

        public Employee Select(int id)
        {
            return _staffContext.Employees.Find(id);
        }

        public IEnumerable<Employee> Find(Func<Employee, bool> predicate)
        {
            return _staffContext.Employees.Where(predicate).ToList();
        }

        public void Create(Employee item)
        {
            _staffContext.Employees.Add(item);
        }

        public void Update(Employee item)
        {
            _staffContext.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Employee employee = _staffContext.Employees.Find(id);

            if (employee != null)
                _staffContext.Employees.Remove(employee);
        }
    }
}