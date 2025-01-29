using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProject.Data;
using WebApiProject.Models;

namespace WebApiProject.Repositories
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeesRepository(ApplicationDbContext context) { _context = context; }

        public async Task<Employees> CreateEmployee(Employees employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task RemoveEmployee(int id)
        {
            var user = await _context.Employees.FindAsync(id);
            
            if (user != null)
            {
                _context.Employees.Remove(user);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Employees>> GetAllEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employees?> GetEmployeeById(int id)
        {         
            var existingEmployee = await _context.Employees.FindAsync(id);

            return existingEmployee; 
        }

        public async Task UpdateEmployee([FromBody] Employees employees)
        {
           
            var existingEmployee = await _context.Employees.FindAsync(employees.Id);

            if (existingEmployee != null)
            {
                // Update only the necessary fields
                existingEmployee.UserName = employees.UserName;
                existingEmployee.Password = employees.Password;
                existingEmployee.Email = employees.Email;

                await _context.SaveChangesAsync();

            }
        }
    }
}
