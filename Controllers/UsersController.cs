﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using BankManagementSystem.Models;
using BankManagementSystem.Models.Enums;
using BankManagementSystem.DataProcessor;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BankManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly BankSystemContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(BankSystemContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            // Commenting out role-based authorization for testing
            // Only administrators can access the list of users
            if (!User.IsInRole("Administrator"))
            {
                return Forbid();
            }

            // Temporarily return all users without role checking for testing
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            // Commenting out user identity validation for testing
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // If the user is not an administrator, they can only access their own information
            if (!User.IsInRole("Administrator") && int.Parse(userId) != id)
            {
                return Forbid();
            }

            // Temporarily allow access to any user for testing
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            // Commenting out role-based authorization for testing
            // Only administrators can create new users?
            //if (!User.IsInRole("Administrator"))
            //{
            //    return Forbid();
            //}

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UsersDto dto)
        {
            // Commenting out user identity validation for testing
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Only administrators or the user themselves can update user information
            if (!User.IsInRole("Administrator") && int.Parse(userId) != id)
            {
                return Forbid();
            }

            if (id != dto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Update user properties
            user.FullName = dto.FullName;
            user.Email = dto.Email;

            // Handle password update
            if (!string.IsNullOrEmpty(dto.NewPassword))
            {
                // Hash the new password
                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Commenting out role-based authorization for testing
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Only administrators can delete users
            if (!User.IsInRole("Administrator"))
            {
                return Forbid();
            }

            // Find the user by ID
            var user = await _context.Users.FindAsync(id);

            // If user is not found, return a 404 Not Found response
            if (user == null)
            {
                return NotFound();
            }

            user.Status = Status.Closed;

            // Save the changes asynchronously
            await _context.SaveChangesAsync();

            // Return 204 No Content to indicate success
            return NoContent();
        }

        //descent into insanity
        //[HttpGet]
        //[Route("test-anon")]
        //[AllowAnonymous]
        //public IActionResult TestAnonymous()
        //{
        //    return Ok("Anonymous route works");
        //}

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            User? user = _context.Users
                .FirstOrDefault(x => x.Email == loginDto.Email && x.Password == loginDto.Password);
            if (user != null)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("Email", user.Email.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddHours(1), signingCredentials: signIn);

                string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new { Token = tokenValue, User = user });
            }

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}