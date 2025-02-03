using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiProject.Data;
using WebApiProject.Models;
using WebApiProject.Repositories;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesRepository _employeeRepository;

        public EmployeesController(IEmployeesRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddEmployees([FromBody] Employees employee)
        {
            if (employee == null)
            {
                return BadRequest();
            }

            await _employeeRepository.CreateEmployee(employee);

            return CreatedAtAction(nameof(GetUserById), new { id = employee.Id }, employee);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Employees>> GetUserById(int id)
        {

            var employee = await _employeeRepository.GetEmployeeById(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Employees>>> GetAllUsers()
        {
            var employees = await _employeeRepository.GetAllEmployees();

            if (employees == null)
            {
                return NotFound();
            }
            return employees;
        }

        [HttpPost]
        public async Task<IActionResult> ModifyEmployee(Employees employee)
        {

            Employees? existingemp = await _employeeRepository.GetEmployeeById(employee.Id);

            if(existingemp != null)
            {
                await _employeeRepository.UpdateEmployee(employee);

                return Ok(new { Message = "Employee updated successfully." });
            }

            return NotFound(new {Message = "Employee Not Found" });            
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee(int id) 
        {
            await _employeeRepository.RemoveEmployee(id);

            return NoContent();
        }
    }
}