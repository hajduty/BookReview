using api.Models.Auth;
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
        public Task<ActionResult<Register>> Register(Register registration)
        {
            SqlConnection con = new SqlConnection(_config.GetConnectionString("ReviewAuth").ToString());

            if (EmailAlreadyExists(registration.Email, con))
            {
                return Task.FromResult<ActionResult<Register>>(Conflict());
            }

            SqlCommand cmd = new("INSERT INTO Registration(username, password, email) VALUES('" + registration.Username + "','" + registration.Password + "','" + registration.Email  + "')", con);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i > 0)
                return Task.FromResult<ActionResult<Register>>(Ok(registration));

            return Task.FromResult<ActionResult<Register>>(BadRequest());
        }

        [HttpPost]
        [Route("login")]
        public Task<ActionResult<Login>> Login(Login login)
        {
            SqlConnection con = new SqlConnection(_config.GetConnectionString("ReviewAuth").ToString());

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Registration WHERE email = '" + login.Email + "' AND password = '" + login.Password + "'", con);

            con.Open();
            int count = (int)cmd.ExecuteScalar();
            con.Close();
            if (count > 0)
                return Task.FromResult<ActionResult<Login>>(Ok(CreateToken(login)));

            return Task.FromResult<ActionResult<Login>>(BadRequest());
        }

        private string CreateToken(Login login)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login.Email)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private bool EmailAlreadyExists(string email, SqlConnection con)
        {
            SqlCommand cmd = new("SELECT COUNT(*) FROM Registration WHERE email = '" + email + "'", con);

            con.Open();
            int count = (int)cmd.ExecuteScalar();
            con.Close();

            return count > 0;
        }
    }
}
