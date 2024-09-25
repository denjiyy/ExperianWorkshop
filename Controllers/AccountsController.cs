using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankManagementSystem.Models;
using BankManagementSystem.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using BankManagementSystem.DataProcessor;
using System.IdentityModel.Tokens.Jwt;
using MediaBrowser.Model.Dto;
using Newtonsoft.Json;

namespace BankManagementSystem.Controllers
{
    // Controller for managing account-related operations in the banking system
    [ApiController]
    [Route("api/[controller]")] // Defines the base route for the controller
    public class AccountsController : ControllerBase
    {
        private readonly BankSystemContext _context; // Database context for accessing account data
        private readonly IHttpContextAccessor _httpContextAccessor; // Accessor for HTTP context

        // Constructor to inject dependencies into the controller
        public AccountsController(BankSystemContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context; // Assign the injected context to the private field
            _httpContextAccessor = httpContextAccessor; // Assign the HTTP context accessor
        }

        // Private helper method to get the currently logged-in user based on the JWT token
        private async Task<UsersDto> GetLoggedInUser()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
            {
                Console.WriteLine("No token found in request.");
                return null;
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;

                Console.WriteLine($"UserId claim found: {userIdClaim}");

                if (userIdClaim != null && int.TryParse(userIdClaim, out var userId))
                {
                    var user = await _context.Users.FindAsync(userId);
                    Console.WriteLine($"Retrieved user: {user?.Id}");

                    if (user != null)
                    {
                        return new UsersDto()
                        {
                            Id = user.Id,
                            Username = user.Username,
                            FullName = user.FullName,
                            //Username = user.Username,
                            Email = user.Email,
                            Password = user.Password,
                            IsAdministrator = user.IsAdministrator,
                            PhoneNumber = user.PhoneNumber,
                            Address =  user.Address,
                            CreditScore = user.CreditScore,
                            Status = user.Status.ToString(),
                            DateOfBirth = user.DateOfBirth
                        };
                    }
                }
                else
                {
                    Console.WriteLine("Invalid or missing user ID claim.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing token: {ex.Message}");
            }

            return null;
        }

        // GET: api/Accounts
        // Retrieves a list of all accounts or only the accounts associated with the logged-in user if they are not an administrator
        [HttpGet]
        [Authorize] // Only authorized users can access this method
        public async Task<IActionResult> GetAccounts()
        {
            // Uncommented code: retrieves the logged-in user
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in."); // Return 401 Unauthorized if user is not authenticated
            }

            // Create a query to retrieve accounts
            IQueryable<Account> query = _context.Accounts;

            // Uncommented code: filter accounts for non-administrative users
            if (!loggedInUser.IsAdministrator)
            {
                query = query.Where(a => a.UserId == loggedInUser.Id);
            }

            // Execute the query and select relevant account details
            var accounts = await query
                .Select(account => new
                {
                    account.Id, // Expose account ID
                    account.AccountNumber, // Expose account number
                    account.Balance, // Expose account balance
                    account.AccountType, // Expose account type
                    account.Status, // Expose account status
                    account.DateOpened,
                    Cards = account.Cards.Select(card => new
                    {
                        card.Id,
                        card.CardNumber,
                        card.CardType,
                        card.IssueDate,
                        card.Status,
                        card.ExpiryDate,
                        card.AccountId
                    })// Expose the date the account was opened
                })
                .ToListAsync();

            return Ok(accounts); // Return the list of accounts
        }

        // GET: api/Accounts/5
        // Retrieves a specific account by its ID for the logged-in user
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetAccount(int id)
        {
            var account = await _context.Accounts
                .Include(a => a.Cards) // Include the Cards related to the account
                .FirstOrDefaultAsync(a => a.Id == id);

            if (account == null)
            {
                return NotFound(); // Return 404 if the account doesn't exist
            }

            return Ok(new
            {
                account.Id,
                account.AccountNumber,
                account.Balance,
                account.AccountType,
                account.Status,
                account.DateOpened,
                Cards = account.Cards.Select(card => new // Include cards in the response
                {
                    card.Id,
                    card.CardNumber,
                    card.CardType,
                    card.IssueDate,
                    card.Status,
                    card.ExpiryDate,
                    card.AccountId
                })
            });
        }

        // POST: api/Accounts
        // Creates a new account based on the provided account data
        [HttpPost]
        [Authorize] // Only authorized users can access this method
        public async Task<IActionResult> CreateAccount([FromBody] AccountsDto dto)
        {
            // Validate the model state
            if (!ModelState.IsValid)
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage)).ToList();
                return BadRequest(new { Errors = errors });
            }

            // Get the logged-in user
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in."); // Return 401 Unauthorized if user is not authenticated
            }

            dto.UserId = loggedInUser.Id;

            // Restrict non-administrators from setting the initial account balance
            if (!loggedInUser.IsAdministrator && dto.Balance > 0)
            {
                return Forbid("Only administrators can set the initial account balance."); // Return 403 Forbidden
            }

            // Ensure AccountType is valid
            if (!Enum.TryParse<AccountType>(dto.AccountType, true, out var accountType))
            {
                return BadRequest("Invalid account type."); // Return 400 Bad Request for invalid account type
            }

            // Ensure Status is valid
            if (!Enum.TryParse<Status>(dto.Status, true, out var status))
            {
                return BadRequest("Invalid account status."); // Return 400 Bad Request for invalid status
            }

            // Ensure CurrencyType is valid
            if (!Enum.TryParse<CurrencyType>(dto.CurrencyType, true, out var currencyType))
            {
                return BadRequest("Invalid currency type."); // Return 400 Bad Request for invalid currency type
            }

            // Create a new account object from the provided data
            var account = new Account
            {
                AccountNumber = dto.AccountNumber,
                AccountType = accountType,
                Balance = loggedInUser.IsAdministrator ? dto.Balance : 0,
                Status = status,
                DateOpened = DateOnly.FromDateTime(DateTime.UtcNow),
                CurrencyType = currencyType,
                UserId = loggedInUser.Id
            };

            // Check for existing account with the same account number
            if (await _context.Accounts.AnyAsync(a => a.AccountNumber == account.AccountNumber))
            {
                return Conflict("An account with this account number already exists."); // Return 409 Conflict if account exists
            }

            // Add the new account to the database
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync(); // Save changes to the database

            // Return the created account with its location in the response header
            return Created(nameof(GetAccount), new { id = account.Id,
                balance = account.Balance,
                accountNumber = account.AccountNumber,
                accountType = account.AccountType,
                status = account.Status,
                userId = account.UserId,
                currencyType = account.CurrencyType,
                dateOpened = account.DateOpened,
                cards = account.Cards,
                transactions = account.Transactions
            }); // Return 201 Created with location
        }


        // PUT: api/Accounts
        // Updates an existing account by its associated user ID
        [HttpPut]
        [Authorize] // Only authorized users can access this method
        public async Task<IActionResult> EditAccount([FromBody] AccountsDto dto)
        {
            // Validate the model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return 400 Bad Request if the model state is invalid
            }

            // Get the logged-in user
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in."); // Return 401 Unauthorized if user is not authenticated
            }

            // Retrieve the account associated with the logged-in user
            var account = await _context.Accounts
                .Where(a => a.UserId == loggedInUser.Id) // Assuming Accounts table has UserId field
                .FirstOrDefaultAsync();

            if (account == null)
            {
                return NotFound(); // Return 404 Not Found if the account does not exist
            }

            // Restrict non-administrators from updating the account balance
            if (!loggedInUser.IsAdministrator && dto.Balance != account.Balance)
            {
                return Forbid("Only administrators can update the account balance."); // Return 403 Forbidden if unauthorized balance update
            }

            // Allow the administrator to update the balance
            if (loggedInUser.IsAdministrator)
            {
                account.Balance = dto.Balance; // Update balance if user is admin
            }

            // Update other account fields regardless of user role
            account.AccountType = (AccountType)Enum.Parse(typeof(AccountType), dto.AccountType, true); // Update account type
            account.Status = (Status)Enum.Parse(typeof(Status), dto.Status, true); // Update account status

            try
            {
                await _context.SaveChangesAsync(); // Save the updated account to the database
            }
            catch (DbUpdateConcurrencyException) // Handle concurrency conflicts
            {
                if (!AccountExists(account.Id))
                {
                    return NotFound(); // Return 404 Not Found if the account does not exist
                }
                else
                {
                    throw; // Rethrow the exception for unhandled scenarios
                }
            }

            return NoContent(); // Return 204 No Content on successful update
        }

        // DELETE: api/Accounts/5
        // Soft deletes an existing account by its ID
        [HttpDelete("{id}")]
        [Authorize] // Only authorized users can access this method
        public async Task<IActionResult> DeleteAccount(int id)
        {
            // Get the logged-in user
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in."); // Return 401 Unauthorized if user is not authenticated
            }

            // Retrieve the account to delete
            var account = await _context.Accounts
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            if (account == null)
            {
                return NotFound(); // Return 404 Not Found if the account does not exist
            }

            // Ensure the logged-in user has permission to delete the account
            if (!loggedInUser.IsAdministrator && account.UserId != loggedInUser.Id)
            {
                return Forbid(); // Return 403 Forbidden if access is denied
            }

            // Soft delete: set the account's status to Closed
            account.Status = Status.Closed; // Assuming Status.Closed is defined in your Status enum
                                            // Optionally, set any other relevant fields for soft deletion, such as DeletionDate or IsDeleted flag.

            try
            {
                await _context.SaveChangesAsync(); // Save changes to the database
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound(); // Return 404 Not Found if the account does not exist
                }
                else
                {
                    throw; // Rethrow the exception for unhandled scenarios
                }
            }

            return NoContent(); // Return 204 No Content on successful deletion
        }

        // Private helper method to check if an account exists by its ID
        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id); // Returns true if the account exists, false otherwise
        }
    }
}
