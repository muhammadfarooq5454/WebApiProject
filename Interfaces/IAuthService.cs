using WebApiProject.Models;
using WebApiProject.Request_Models;

namespace WebApiProject.Interfaces
{
    public interface IAuthService
    {
        User AddUser(User user);
        dynamic Login(LoginRequest loginRequest);
        Role AddRole(Role role);
        bool AssignRoletoUser(AddUserRole addUserRole);
    }
}
