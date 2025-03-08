using WebApiProject.Models;

namespace WebApiProject.Interfaces
{
    public interface IEmployeeService
    {
        public List<Employees> GetAllEmployeeDetails();
        public Employees GetEmployeeDetails(int id);
        public Employees AddEmployee(Employees employee);
        public Employees UpdateEmployee(Employees employee);
        public bool DeleteEmployeeDetails(int id);
    }
}
