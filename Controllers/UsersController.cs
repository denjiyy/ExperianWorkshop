using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using BankManagementSystem.Models;
using BankManagementSystem.Models.Enums;
using BankManagementSystem.DataProcessor.Import;

namespace BankManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize] // Commenting out authorization for testing
    public class UsersController : ControllerBase
    {
        private readonly BankSystemContext _context;

        public UsersController(BankSystemContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            // Commenting out role-based authorization for testing
            // Only administrators can access the list of users
            // if (!User.IsInRole("Administrator"))
            // {
            //     return Forbid();
            // }

            // Temporarily return all users without role checking for testing
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            // Commenting out user identity validation for testing
            // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // If the user is not an administrator, they can only access their own information
            // if (!User.IsInRole("Administrator") && int.Parse(userId) != id)
            // {
            //     return Forbid();
            // }

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
            // Only administrators can create new users
            // if (!User.IsInRole("Administrator"))
            // {
            //     return Forbid();
            // }

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
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UsersDto dto)
        {
            // Commenting out user identity validation for testing
            // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Only administrators or the user themselves can update user information
            // if (!User.IsInRole("Administrator") && int.Parse(userId) != id)
            // {
            //     return Forbid();
            // }

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
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Commenting out role-based authorization for testing
            // var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Only administrators can delete users
            // if (!User.IsInRole("Administrator"))
            // {
            //     return Forbid();
            // }

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

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}