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
        public User AddUser(User user)
        {
            var addeduser = _jwtContext.Users.Add(user);
            _jwtContext.SaveChanges();
            return addeduser.Entity;
        }

        public string Login(LoginRequest loginRequest)
        {
            if(loginRequest.Username != null && loginRequest.Password != null)
            {
                var user = _jwtContext.Users.FirstOrDefault(x => x.Email == loginRequest.Username &&
                    x.Password == loginRequest.Password);

                if (user != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("UserName",user.Name),
                        new Claim("Email", user.Email)
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                    return jwtToken;
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
