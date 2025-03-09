using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiProject.Models;
using Microsoft.AspNetCore.Http;
using WebApiProject.Services;
using WebApiProject.Interfaces;
using WebApiProject.Request_Models;
using WebApiProject.Context;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApiProject.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtContext _jwtContext;
        private readonly IConfiguration _configuration;
        public AuthService(JwtContext jwtContext, IConfiguration configuration)
        {
            _jwtContext = jwtContext;
            _configuration = configuration;
        }

        //Adding roles in the Roles Table
        public Role AddRole(Role role)
        {
            var addedrole = _jwtContext.Roles.Add(role);
            _jwtContext.SaveChanges();
            return addedrole.Entity;
        }

        //Adding user in the Users Table
        public User AddUser(User user)
        {
            var addeduser = _jwtContext.Users.Add(user);
            _jwtContext.SaveChanges();
            return addeduser.Entity;
        }

        //Assigning roles for a particular user in a userrole table 
        public bool AssignRoletoUser(AddUserRole addUserRole)
        {
            try
            {
                var Listuserrole = new List<UserRole>();

                var user = _jwtContext.Users.FirstOrDefault(x => x.Id == addUserRole.UserId);
                if (user != null)
                {
                    foreach (var roleId in addUserRole.RoleIds)
                    {
                        var userrole = new UserRole()
                        {
                            RoleId = roleId,
                            UserId = user.Id
                        };

                        Listuserrole.Add(userrole);
                    }

                    _jwtContext.UserRoles.AddRange(Listuserrole);
                    _jwtContext.SaveChanges();
                    return true;
                }
                else
                {
                    throw new Exception("User not Valid");
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public dynamic Login(LoginRequest loginRequest)
        {
            if(loginRequest.Username != null && loginRequest.Password != null)
            {
                var user = _jwtContext.Users.FirstOrDefault(x => x.Name == loginRequest.Username &&
                    x.Password == loginRequest.Password);

                if (user != null)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("UserName",user.Name),
                        new Claim("Email", user.Email)
                    };

                    //Adding Roles as a Claim in Token
                    var userroles = _jwtContext.UserRoles.Where(x => x.UserId == user.Id).ToList();

                    //Use particular user ki tamam roles ki ids agai hain
                    var roleIds = userroles.Select(x => x.RoleId).ToList();

                    //Ab in Ids ki behalf pe unke roles uthaonga
                    var roles = _jwtContext.Roles.Where(x => roleIds.Contains(x.Id)).ToList();

                    foreach(var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Name));
                    }

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                    return new { token = jwtToken };
                }
                else
                {
                    throw new Exception("Credentials are not Valid");
                }
            }
            else
            {
                throw new Exception("User is not Valid");
            }
        }
    }
}
