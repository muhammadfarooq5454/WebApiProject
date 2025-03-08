using WebApiProject.Models;
using WebApiProject.Request_Models;

namespace WebApiProject.Interfaces
{
    public interface IAuthService
    {
        User AddUser(User user);
        string Login(LoginRequest loginRequest);
    }
}
