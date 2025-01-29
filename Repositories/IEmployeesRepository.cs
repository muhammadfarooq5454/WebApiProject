using Microsoft.AspNetCore.Mvc;
using WebApiProject.Models;

namespace WebApiProject.Repositories
{
    public interface IEmployeesRepository
    {
        Task<List<Employees>> GetAllEmployees();
        Task<Employees?> GetEmployeeById(int id);
        Task<Employees> CreateEmployee(Employees employees);
        Task UpdateEmployee(Employees employees);
        Task RemoveEmployee(int id);
    }
}
