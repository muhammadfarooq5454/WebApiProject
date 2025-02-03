using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace WebApiProject.Models
{
    public class Employees
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role {  get; set; }
    }
}
