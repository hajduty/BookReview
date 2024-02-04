using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration configuration)
        {
            _config = configuration;
        }

        [HttpPost]
        [Route("register")]
        public Task<ActionResult<User>> Register(User registration)
        {
            var data = new DbHelper();

            if (EmailAlreadyExists(registration.Email, data))
            {
                return Task.FromResult<ActionResult<User>>(Conflict());
            };

            User reg = registration;
            reg.Admin = "false";

            data.InsertData(reg, "Registers");

            return Task.FromResult<ActionResult<User>>(Ok(registration));
        }

        [HttpPost]
        [Route("login")]
        public Task<ActionResult<User>> Login(User login)
        {
            var data = new DbHelper();
            User result = data.GetData<User>("Registers","email = @Email", new { Email = login.Email , Password = login.Password});

            if (result != null)
            {
                return Task.FromResult<ActionResult<User>>(Ok(CreateToken(result)));
            }
            return Task.FromResult<ActionResult<User>>(BadRequest());
        }

        private string CreateToken(User user)
        {
            // Retrieve issuer and audience from appsettings
            string issuer = _config.GetSection("AppSettings:Issuer").Value;
            string audience = _config.GetSection("AppSettings:Audience").Value;

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("admin", user.Admin)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private static bool EmailAlreadyExists(string email, DbHelper data)
        {
            User result = data.GetData<User>("Registers","email = @Email", new { Email = email });

            return result != null;
        }
    }
}
