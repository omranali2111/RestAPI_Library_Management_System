using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestAPI_Library_Management_System.models;
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
        public IActionResult GenerateJwtToken(string email, string password)
        {
            var user = dbContext.Users.SingleOrDefault(u => u.Email == email && u.Password == password);

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
                return Unauthorized("Invalid credentials.");
            }
        }

    }
}

