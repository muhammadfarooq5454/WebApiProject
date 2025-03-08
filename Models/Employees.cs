using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApiProject.Models
{
    public class Employees
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
    }
}
