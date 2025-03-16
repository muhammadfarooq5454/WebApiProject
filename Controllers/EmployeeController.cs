using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiProject.Interfaces;
using WebApiProject.Models;
using WebApiProject.Services;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        public List<Employees> GetEmployees()
        {
            var employees = _employeeService.GetAllEmployeeDetails();
            return employees;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("{id}")]
        public Employees GetEmployee(int id) 
        { 
            var employee = _employeeService.GetEmployeeDetails(id);
            return employee;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public Employees AddEmployee([FromBody] Employees employees)
        {
            var employee = _employeeService.AddEmployee(employees);
            return employee;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public Employees UpdateEmployee([FromBody] Employees value)
        {
            var employee = _employeeService.UpdateEmployee(value);
            return employee;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public bool DeleteEmployee(int id)
        {
            var isDeleted = _employeeService.DeleteEmployeeDetails(id);
            return isDeleted;
        }
    }
}
