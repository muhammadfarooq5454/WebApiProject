﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiProject.Interfaces;
using WebApiProject.Models;
using WebApiProject.Request_Models;
using WebApiProject.Services;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public string Login([FromBody] LoginRequest loginRequest)
        {
            return _authService.Login(loginRequest);
        }

        [HttpPost("addUser")]
        public User AddUser([FromBody] User value)
        {
            return _authService.AddUser(value);
        }
    }
}
