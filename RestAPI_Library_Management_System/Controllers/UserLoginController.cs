﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Microsoft.IdentityModel.Tokens;
using RestAPI_Library_Management_System.models;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestAPI_Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        public static AppDbContextDbContext dbContext;

        public UserLoginController(AppDbContextDbContext DB)
        {
            dbContext = DB;
        }


        [HttpPost("user-login")]
        public IActionResult GenerateJwtToken(User data)
        {
            Log.Information($"Received login request - Email: {data.Email}, Password: {data.Password}");
            var user = dbContext.Users.SingleOrDefault(u => u.Email ==data.Email && u.Password == data.Password);

            if (user != null)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));

                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                 {
                        new Claim("email", user.Email),
           
                 };

                var token = new JwtSecurityToken(
                    issuer: "omran",
                    audience: "all",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials
                );

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            else
            {
                Log.Warning($"Login failed - Invalid credentials.");
                return Unauthorized("Invalid credentials.");
            }
        }

    }
}

