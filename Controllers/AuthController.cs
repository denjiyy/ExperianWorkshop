using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;


// THIS IS STRICTLY FOR TESTING PURPOSES!!!
namespace BankManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This is just a simple, hardcoded user store for testing purposes
        private readonly Dictionary<string, string> users = new Dictionary<string, string>
        {
            { "testuser", "password123" }  // username, password
        };

        // POST: api/Auth/Authenticate
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserLoginDto userLogin)
        {
            // Validate user credentials (In a real application, this would involve checking a database)
            if (!users.TryGetValue(userLogin.Username, out var password) || password != userLogin.Password)
            {
                return Unauthorized("Invalid username or password");
            }

            // Generate JWT token if authentication is successful
            var token = GenerateJwtToken(userLogin.Username);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(string username)
        {
            // Fetch the SecretKey from configuration and decode it
            var key = Convert.FromBase64String(_configuration["Jwt:SecretKey"]); // Decodes the Base64 string

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class UserLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
