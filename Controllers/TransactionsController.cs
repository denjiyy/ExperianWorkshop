using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankManagementSystem.Models;
using BankManagementSystem.Models.Enums;
using BankManagementSystem.DataProcessor;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace BankManagementSystem.Controllers
{
    // API Controller for managing transactions
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        // Injected dependencies for database context and HTTP context accessor
        private readonly BankSystemContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Constructor to inject BankSystemContext and IHttpContextAccessor into the controller
        public TransactionsController(BankSystemContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
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

        // GET: api/Transactions
        // Retrieves a list of transactions for the logged-in user
        [HttpGet]
        [Authorize] // Only authorized users can access this method
        public async Task<ActionResult<IEnumerable<TransactionsDto>>> GetTransactions()
        {
            // Get the logged-in user
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in."); // Return 401 if the user is not logged in
            }

            // Query the transactions for the logged-in user's account
            var transactions = await _context.Transactions
                .Include(t => t.Account)
                .Where(t => t.Account.UserId == loggedInUser.Id)
                .ToListAsync();

            // Convert transactions to DTOs
            var transactionDtos = transactions.Select(t => new TransactionsDto
            {
                TransactionType = t.TransactionType.ToString(),
                Amount = t.Amount,
                TransactionDescription = t.TransactionDescription,
                TransactionStatus = t.TransactionStatus.ToString(),
                CurrencyType = t.CurrencyType.ToString(),
                Timestamp = t.Timestamp
            }).ToList();

            return Ok(transactionDtos); // Return the transaction DTOs
        }

        // GET: api/Transactions/5
        // Retrieves a specific transaction by ID for the logged-in user
        [HttpGet("{id}")]
        [Authorize] // Only authorized users can access this method
        public async Task<ActionResult<TransactionsDto>> GetTransaction(int id)
        {
            // Get the logged-in user
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in."); // Return 401 if the user is not logged in
            }

            // Find the transaction by ID and ensure it belongs to the logged-in user
            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .Where(t => t.Id == id && t.Account.UserId == loggedInUser.Id)
                .FirstOrDefaultAsync();

            if (transaction == null)
            {
                return NotFound("Transaction not found or access denied."); // Return 404 if not found or access denied
            }

            // Convert the transaction to a DTO
            var transactionDto = new TransactionsDto
            {
                TransactionType = transaction.TransactionType.ToString(),
                Amount = transaction.Amount,
                TransactionDescription = transaction.TransactionDescription,
                TransactionStatus = transaction.TransactionStatus.ToString(),
                CurrencyType = transaction.CurrencyType.ToString(),
                Timestamp = transaction.Timestamp
            };

            return Ok(transactionDto); // Return the transaction DTO
        }

        // POST: api/Transactions/{accountId}
        // Creates a new transaction for the specified account of the logged-in user
        [HttpPost("{accountId}")]
        [Authorize] // Only authorized users can access this method
        public async Task<IActionResult> CreateTransaction(int accountId, [FromBody] TransactionsDto dto)
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

            // Find the account associated with the provided accountId and the logged-in user
            var account = await _context.Accounts
                                         .FirstOrDefaultAsync(a => a.Id == accountId && a.UserId == loggedInUser.Id); // Ensure the account belongs to the logged-in user
            if (account == null)
            {
                return NotFound("No associated account found for the specified account ID."); // Return 404 if no account found
            }

            // Ensure TransactionType is valid
            if (!Enum.TryParse<TransactionType>(dto.TransactionType, true, out var transactionType))
            {
                return BadRequest("Invalid TransactionType."); // Return 400 Bad Request for invalid TransactionType
            }

            // Ensure TransactionStatus is valid
            if (!Enum.TryParse<TransactionStatus>(dto.TransactionStatus, true, out var transactionStatus))
            {
                return BadRequest("Invalid TransactionStatus."); // Return 400 Bad Request for invalid TransactionStatus
            }

            // Ensure CurrencyType is valid
            if (!Enum.TryParse<CurrencyType>(dto.CurrencyType, true, out var currencyType))
            {
                return BadRequest("Invalid CurrencyType."); // Return 400 Bad Request for invalid CurrencyType
            }

            // Check if the account has sufficient balance
            if (account.Balance < dto.Amount)
            {
                return BadRequest("Insufficient balance for the transaction."); // Return 400 Bad Request for insufficient balance
            }

            // Create a new transaction object from the provided data
            var transaction = new Transaction
            {
                TransactionType = transactionType,
                Amount = dto.Amount,
                TransactionDescription = dto.TransactionDescription,
                TransactionStatus = transactionStatus,
                CurrencyType = currencyType,
                Timestamp = dto.Timestamp,
                AccountId = account.Id, // Set AccountId from the found account
                Recipient = dto.Recipient
            };

            // Add the new transaction to the database
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync(); // Save changes to the database

            // Deduct the transaction amount from the account balance
            account.Balance -= dto.Amount; // Reduce account balance
            _context.Accounts.Update(account); // Update account
            await _context.SaveChangesAsync(); // Save changes to the account

            // Return the created transaction with its location in the response header
            return Created(nameof(GetTransaction), new
            {
                id = transaction.Id,
                transactionType = transaction.TransactionType.ToString(),
                amount = transaction.Amount,
                transactionDescription = transaction.TransactionDescription,
                transactionStatus = transaction.TransactionStatus.ToString(),
                currencyType = transaction.CurrencyType.ToString(),
                timestamp = transaction.Timestamp,
                recipient = transaction.Recipient,
                accountId = account.Id // Include AccountId for clarity
            }); // Return 201 Created with location
        }

        // PUT: api/Transactions/5
        // Updates an existing transaction; only administrators are allowed to update transactions
        [HttpPut("{id}")]
        [Authorize] // Only authorized users can access this method
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] TransactionsDto dto)
        {
            // Validate the request body
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return 400 if the model is invalid
            }

            // Get the logged-in user
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in."); // Return 401 if the user is not logged in
            }

            // Only administrators are allowed to update transactions
            if (loggedInUser.IsAdministrator == false)
            {
                Forbid("Only administrators can update the transactions."); // Return 403 if not an admin
            }

            // Find the transaction by ID and ensure it belongs to the logged-in user
            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .FirstOrDefaultAsync(t => t.Id == id && t.Account.UserId == loggedInUser.Id);

            if (transaction == null)
            {
                return NotFound("Transaction not found or access denied."); // Return 404 if not found
            }

            // Validate and parse the enum values for the transaction
            if (!Enum.TryParse(dto.TransactionType, true, out TransactionType transactionType))
            {
                return BadRequest("Invalid TransactionType."); // Return 400 if invalid
            }
            if (!Enum.TryParse(dto.TransactionStatus, true, out TransactionStatus transactionStatus))
            {
                return BadRequest("Invalid TransactionStatus."); // Return 400 if invalid
            }
            if (!Enum.TryParse(dto.CurrencyType, true, out CurrencyType currencyType))
            {
                return BadRequest("Invalid CurrencyType."); // Return 400 if invalid
            }

            // Update the transaction details
            transaction.TransactionType = transactionType;
            transaction.Amount = dto.Amount;
            transaction.TransactionDescription = dto.TransactionDescription;
            transaction.TransactionStatus = transactionStatus;
            transaction.CurrencyType = currencyType;
            transaction.Timestamp = dto.Timestamp;

            // Save the changes to the database
            try
            {
                await _context.SaveChangesAsync(); // Attempt to save the changes
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id)) // If the transaction no longer exists, return 404
                {
                    return NotFound();
                }
                else
                {
                    throw; // Rethrow any other exceptions
                }
            }

            return NoContent(); // Return 204 No Content if the update was successful
        }

        // DELETE: api/Transactions/5
        // Deletes a transaction for the logged-in user
        [HttpDelete("{id}")]
        [Authorize] // Only authorized users can access this method
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            // Get the logged-in user
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in."); // Return 401 if the user is not logged in
            }

            // Find the transaction by ID and ensure it belongs to the logged-in user
            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .FirstOrDefaultAsync(t => t.Id == id && t.Account.UserId == loggedInUser.Id);

            if (transaction == null)
            {
                return NotFound("Transaction not found or access denied."); // Return 404 if not found
            }

            // Remove the transaction from the database
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync(); // Save changes to the database

            return NoContent(); // Return 204 No Content if the deletion was successful
        }

        // Helper method to check if a transaction exists
        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id); // Returns true if the transaction exists
        }
    }
}
