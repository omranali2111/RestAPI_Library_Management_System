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
                // Validate user credentials
                var user = dbContext.Users.SingleOrDefault(u => u.Email == email && u.Password == password);

                if (user == null)
                {
                    return Unauthorized(); // Return 401 Unauthorized if user not found
                }

            // Create claims for the user
                 var claims = new List<Claim>();
                new Claim(ClaimTypes.Email, user.Email);
           
                

             
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this-is-my-token"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Set token expiration time (adjust as needed)
                var expiration = DateTime.UtcNow.AddHours(1);

                // Create the JWT token
                var token = new JwtSecurityToken(
                    issuer: "omran",        
                    audience: "all",    
                    claims: claims,
                    expires: expiration,
                    signingCredentials: creds
                );

                // Return the generated token
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = expiration
                });
            }
    }
}

