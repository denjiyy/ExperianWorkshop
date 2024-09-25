using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using BankManagementSystem.DataProcessor;
using BankManagementSystem.Models.Enums;

namespace BankManagementSystem.Controllers
{
    // API Controller for managing users
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // Injected dependencies for database context and configuration settings
        private readonly BankSystemContext _context;
        private readonly IConfiguration _configuration;

        // Constructor to inject BankSystemContext and IConfiguration into the controller
        public UsersController(BankSystemContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Method to get the currently logged-in user based on the JWT token in the request
        private async Task<User> GetLoggedInUser()
        {
            // Extract the JWT token from the Authorization header
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
                return null!;

            // Parse the token to extract claims
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;

            // Find the user from the database using the extracted user ID
            if (userIdClaim != null && int.TryParse(userIdClaim, out var userId))
            {
                return await _context.Users.FindAsync(userId);
            }

            return null!; // Return null if the user could not be found
        }

        // GET: api/Users
        // Retrieves a list of all users; restricted to administrators
        [HttpGet]
        [Authorize] // Only authorized users can access this method
        public async Task<ActionResult<IEnumerable<object>>> GetUsers()
        {
            // Get the logged-in user and check if they are an administrator
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null || !loggedInUser.IsAdministrator)
            {
                return Forbid(); // Return 403 if the user is not an admin
            }

            // Retrieve the list of all users from the database, including their loans
            var users = await _context.Users
                .Include(u => u.Loans)
                .Include(u => u.Accounts)
                .ToListAsync();

            // Create a response that returns every field from the user model
            var userDtos = users.Select(user => new
            {
                Id = user.Id,
                FullName = user.FullName,
                Username = user.Username,
                Email = user.Email,
                Password = user.Password, // Be cautious with sensitive data like passwords
                IsAdministrator = user.IsAdministrator,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                CreditScore = user.CreditScore,
                Status = user.Status,
                DateOfBirth = user.DateOfBirth,
                Accounts = user.Accounts.Select(account => new
                {
                    AccountId = account.Id,
                    AccountBalance = account.Balance,
                    AccountType = account.AccountType,
                    AccountNumber = account.AccountNumber,
                    DateOpened = account.DateOpened,
                    Status = account.Status
                }),
                Loans = user.Loans.Select(loan => new
                {
                    LoanId = loan.Id,
                    LoanAmount = loan.Amount,
                    LoanStatus = loan.LoanStatus,
                    DateApproved = loan.DateApproved,
                    LoanEndDate = loan.EndDate
                }).ToList()
            }).ToList();

            // Return the list of users along with their details
            return Ok(userDtos);
        }

        // GET: api/Users/5
        // Retrieves a specific user by ID; users can access only their own data, or admins can access any user
        [HttpGet("{id}")]
        [Authorize] // Only authorized users can access this method
        public async Task<ActionResult<User>> GetUser(int id)
        {
            // Get the logged-in user
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Forbid(); // Return 403 if the user is not logged in
            }

            // Check if the logged-in user is an administrator or if they are requesting their own data
            if (!loggedInUser.IsAdministrator && loggedInUser.Id != id)
            {
                return Forbid(); // Return 403 if the user does not have permission
            }

            // Find the user by their ID
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(); // Return 404 if the user does not exist
            }

            return Ok(user); // Return the user data
        }

        // POST: api/Users
        // Creates a new user; accessible to everyone (anonymous access)
        [HttpPost]
        [AllowAnonymous] // Allow access without authentication
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return 400 if the model is invalid
            }

            // Uncomment the following line to hash the password before saving
            // user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            // Add the new user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync(); // Save changes to the database

            // Return the newly created user with a 201 status code
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/Users/5
        // Updates an existing user's information; only administrators or the user themselves can update
        [HttpPut("{id}")]
        [Authorize] // Only authorized users can access this method
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            // Get the logged-in user
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Forbid(); // Return 403 if the user is not logged in
            }

            // Check if the user is an administrator or is updating their own information
            if (!loggedInUser.IsAdministrator && loggedInUser.Id != id)
            {
                return Forbid(); // Return 403 if the user does not have permission
            }

            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return 400 if the model is invalid
            }

            // Find the user by their ID
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(); // Return 404 if the user does not exist
            }

            // Update the user's information
            if (!string.IsNullOrEmpty(dto.Email))
            {
                user.Email = dto.Email;
            }

            if (!string.IsNullOrEmpty(dto.Username))
            {
                user.Username = dto.Username;
            }

            // If a new password is provided, hash and update it
            if (!string.IsNullOrEmpty(dto.NewPassword))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            }

            // Mark the user as modified in the database
            _context.Entry(user).State = EntityState.Modified;

            // Attempt to save the changes, handling any concurrency issues
            try
            {
                await _context.SaveChangesAsync(); // Save the updated user data
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound(); // Return 404 if the user no longer exists
                }
                else
                {
                    throw; // Rethrow the exception if it's not related to the user not existing
                }
            }

            return NoContent(); // Return 204 to indicate the update was successful
        }

        // DELETE: api/Users/5
        // Deletes (or closes) a user's account; only administrators can delete accounts
        [HttpDelete("{id}")]
        [Authorize] // Only authorized users can access this method
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Get the logged-in user and check if they are an administrator
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null || !loggedInUser.IsAdministrator)
            {
                return Forbid(); // Return 403 if the user is not an admin
            }

            // Find the user by their ID
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(); // Return 404 if the user does not exist
            }

            // Mark the user's status as closed
            user.Status = Status.Closed;
            await _context.SaveChangesAsync(); // Save the changes to the database

            return NoContent(); // Return 204 to indicate the user was deleted (or closed)
        }

        // Helper method to check if a user exists in the database by their ID
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id); // Returns true if the user exists, false otherwise
        }
    }
}