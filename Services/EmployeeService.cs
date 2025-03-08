using Microsoft.EntityFrameworkCore;
using WebApiProject.Context;
using WebApiProject.Interfaces;
using WebApiProject.Models;

namespace WebApiProject.Services
{
    public class EmployeeService : IEmployeeService
    {
        public readonly JwtContext _jwtContext;
        public EmployeeService(JwtContext jwtContext)
        {
            _jwtContext = jwtContext;
        }

        public Employees AddEmployee(Employees employee)
        {
            var emp = _jwtContext.Employees.Add(employee);
            _jwtContext.SaveChanges();

            return emp.Entity;
        }

        public bool DeleteEmployeeDetails(int id)
        {
            try
            {
                var employee = _jwtContext.Employees.FirstOrDefault(x => x.Id == id);
                if(employee != null)
                {
                    _jwtContext.Employees.Remove(employee);
                    _jwtContext.SaveChanges();
                }
                else
                {
                    throw new Exception("Employee not found");
                }
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }

        public List<Employees> GetAllEmployeeDetails()
        {
            var employees =  _jwtContext.Employees.ToList();
            return employees;
        }

        public Employees GetEmployeeDetails(int id)
        {
            var employee = _jwtContext.Employees.FirstOrDefault(x => x.Id == id);
            if(employee == null)
            {
                throw new Exception("Employee not found");
            }
            else
            {
                return employee;
            }
        }

        public Employees UpdateEmployee(Employees employee)
        {
            var updated = _jwtContext.Employees.Update(employee);
            _jwtContext.SaveChanges();
            return updated.Entity;
        }
    }
}
