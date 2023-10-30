
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestAPI_Library_Management_System;
using RestAPI_Library_Management_System.models;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Text;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContextDbContext _dBContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;


        public UserController(UserManager<ApplicationUser> UserManager, 
                             SignInManager<ApplicationUser> signInManager, IConfiguration configuration,
                             AppDbContextDbContext dBContext)
        {
            _userManager = UserManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _dBContext = dBContext;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(SignIn signInModel)
        {
            var user = await _userManager.FindByEmailAsync(signInModel.Email);
            var password = await _userManager.CheckPasswordAsync(user, signInModel.Password);
            var roles = await _userManager.GetRolesAsync(user);

            var result = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);

            if (!password)
            {
                Console.WriteLine(result.ToString());
                return BadRequest("Invalid credentials");
            }

            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, signInModel.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            foreach (var item in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, item));
            }

            var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
            );

            var patron = _dBContext.Patrons.FirstOrDefault(p => p.UserId == user.Id);
            if (patron == null)
            {
                return BadRequest("Associated patron details not found.");
            }

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                role = roles.FirstOrDefault(),
                patronId = patron.Id
            });
        }


        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUP(SignUp Input)
        {
            var NewUser = new ApplicationUser
            {
                UserName = Input.Name,
                Email = Input.Email
            };

            var result = await _userManager.CreateAsync(NewUser, Input.Password);

            if (result.Succeeded)
            {

                await _userManager.AddToRoleAsync(NewUser, Input.Role);





                var userProfile = new Patron
                {
                    Name = Input.Name,
                    ContactNumber=Input.ContactNumber,
                    Age=Input.Age,
                    UserId = NewUser.Id // Set the foreign key to the user's Id
                };

                // Save the user profile data to the database
                _dBContext.Patrons.Add(userProfile);
                await _dBContext.SaveChangesAsync();

                return Ok(new
                {
                    message = "SignUp is Successful",
                    patronId = userProfile.Id
                });
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
