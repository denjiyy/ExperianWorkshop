using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankManagementSystem.Models;
using BankManagementSystem.Models.Enums;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using BankManagementSystem.DataProcessor;
using System.IdentityModel.Tokens.Jwt;

namespace BankManagementSystem.Controllers
{
    // Controller for managing card-related operations in the banking system
    [ApiController]
    [Route("api/[controller]")] // Defines the base route for the controller
    public class CardsController : ControllerBase
    {
        private readonly BankSystemContext _context; // Database context for accessing card data
        private readonly IHttpContextAccessor _httpContextAccessor; // Accessor for HTTP context

        // Constructor to inject dependencies into the controller
        public CardsController(BankSystemContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context; // Assign the injected context to the private field
            _httpContextAccessor = httpContextAccessor; // Assign the HTTP context accessor
        }

        // Private helper method to get the currently logged-in user based on the JWT token
        private async Task<User> GetLoggedInUser()
        {
            // Extract the JWT token from the Authorization header
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
                return null!; // Return null if no token is found

            // Parse the JWT token to get user claims
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;

            // If a valid user ID claim is found, return the corresponding user from the database
            if (userIdClaim != null && int.TryParse(userIdClaim, out var userId))
            {
                var tokenCheck = await _context.Users.FindAsync(userId);
                return tokenCheck!;
            }

            return null!; // Return null if the user could not be identified
        }

        // GET: api/Cards
        // Retrieves all cards for the logged-in user or all cards if the user is an administrator
        [HttpGet]
        [Authorize] // Only authorized users can access this method
        public async Task<IActionResult> GetCards()
        {
            // Uncommented code: retrieves the logged-in user
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in."); // Return 401 Unauthorized if user is not authenticated
            }

            // Create a query to retrieve cards, including account details
            IQueryable<Card> query = _context.Cards.Include(c => c.Account);

            //Uncommented code: filter cards for non - administrative users
             if (!loggedInUser.IsAdministrator)
                {
                    query = query.Where(c => c.Account.UserId == loggedInUser.Id);
                }

            // Execute the query and transform the results into DTOs
            var cards = await query
                .Select(c => new CardsDto
                {
                    CardNumber = c.CardNumber, // Expose the card number
                    CardType = c.CardType.ToString(), // Expose the card type
                    ExpiryDate = c.ExpiryDate, // Expose the expiry date
                    Status = c.Status.ToString(), // Expose the status
                    IssueDate = c.IssueDate, // Expose the issue date
                    CVV = "Hidden",
                    AccountId = c.AccountId
                })
                .ToListAsync();

            return Ok(cards); // Return the list of card DTOs
        }

        // GET: api/Cards/5
        // Retrieves a specific card by its ID for the logged-in user
        [HttpGet("{id}")]
        [Authorize] // Only authorized users can access this method
        public async Task<IActionResult> GetCard(int id)
        {
            // Get the logged-in user
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in."); // Return 401 Unauthorized if user is not authenticated
            }

            // Find the card by ID and include account information
            var card = await _context.Cards
                .Include(c => c.Account)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (card == null)
            {
                return NotFound(); // Return 404 Not Found if the card doesn't exist
            }

            // Ensure the logged-in user has permission to view the card
            if (!loggedInUser.IsAdministrator && card.Account.UserId != loggedInUser.Id)
            {
                return Forbid(); // Return 403 Forbidden if access is denied
            }

            // Map the card to a DTO for response
            var cardDto = new CardsDto
            {
                CardNumber = card.CardNumber,
                CardType = card.CardType.ToString(),
                ExpiryDate = card.ExpiryDate,
                Status = card.Status.ToString(),
                IssueDate = card.IssueDate,
                CVV = "Hidden" // Hide CVV for security reasons
            };

            return Ok(cardDto); // Return the card details
        }

        // POST: api/Cards/{accountId}
        // Creates a new card for the specified account of the logged-in user
        [HttpPost("{accountId}")]
        [Authorize] // Only authorized users can access this method
        public async Task<IActionResult> CreateCard(int accountId, [FromBody] CardsDto dto)
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

            // Ensure CardType is valid
            if (!Enum.TryParse<CardType>(dto.CardType, true, out var cardType))
            {
                return BadRequest("Invalid card type."); // Return 400 Bad Request for invalid card type
            }

            // Create a new card object from the provided data
            var card = new Card
            {
                CardType = cardType,
                CardNumber = GenerateRandomCardNumber(),
                CVV = dto.CVV,
                IssueDate = DateOnly.FromDateTime(DateTime.UtcNow),
                ExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(4)),
                Status = Status.Active, // Default to active status or set it based on dto
                AccountId = account.Id // Set AccountId from the found account
            };

            // Check for existing card with the same card number
            if (await _context.Cards.AnyAsync(c => c.CardNumber == card.CardNumber))
            {
                return Conflict("A card with this card number already exists."); // Return 409 Conflict if card exists
            }

            // Add the new card to the database
            await _context.Cards.AddAsync(card);
            await _context.SaveChangesAsync(); // Save changes to the database

            // Return the created card with its location in the response header
            return Created(nameof(GetCard), new
            {
                id = card.Id,
                cardNumber = card.CardNumber,
                cardType = card.CardType,
                issueDate = card.IssueDate,
                expiryDate = card.ExpiryDate,
                status = card.Status,
                accountId = card.AccountId // Include AccountId for clarity
            }); // Return 201 Created with location
        }

        private string GenerateRandomCardNumber()
        {
            var random = new Random();
            return string.Concat(Enumerable.Range(0, 16).Select(_ => random.Next(0, 10).ToString()));
        }

        // PUT: api/Cards/5
        // Updates an existing card by its ID
        [HttpPut("{id}")]
        [Authorize] // Only authorized users can access this method
        public async Task<IActionResult> EditCard(int id, [FromBody] CardsDto dto)
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

            // Retrieve the card to update and include account information
            var card = await _context.Cards.Include(c => c.Account).Where(c => c.Id == id).FirstOrDefaultAsync();
            if (card == null)
            {
                return NotFound(); // Return 404 Not Found if the card does not exist
            }

            // Ensure the logged-in user has permission to edit the card
            if (!loggedInUser.IsAdministrator && card.Account.UserId != loggedInUser.Id)
            {
                return Forbid(); // Return 403 Forbidden if access is denied
            }

            // Update the card properties with the new values
            card.CardNumber = dto.CardNumber; // Update card number
            card.CardType = (CardType)Enum.Parse(typeof(CardType), dto.CardType, true); // Update card type
            card.ExpiryDate = dto.ExpiryDate; // Update expiry date
            card.Status = (Status)Enum.Parse(typeof(Status), dto.Status, true); // Update status
            card.CVV = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.CVV); // Hash the new CVV
            card.IssueDate = dto.IssueDate; // Update issue date

            try
            {
                await _context.SaveChangesAsync(); // Save the updated card to the database
            }
            catch (DbUpdateConcurrencyException) // Handle concurrency conflicts
            {
                if (!CardExists(id))
                {
                    return NotFound(); // Return 404 if the card does not exist
                }
                else
                {
                    throw; // Rethrow the exception for unhandled scenarios
                }
            }

            return NoContent(); // Return 204 No Content on successful update
        }

        // DELETE: api/Cards/{id}
        // Closes the specified card for the logged-in user
        [HttpDelete("{id}")]
        [Authorize] // Only authorized users can access this method
        public async Task<IActionResult> DeleteCard(int id)
        {
            // Get the logged-in user
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in."); // Return 401 Unauthorized if user is not authenticated
            }

            // Retrieve the card to close and include account information
            var card = await _context.Cards.Include(c => c.Account)
                                             .FirstOrDefaultAsync(c => c.Id == id);
            if (card == null)
            {
                return NotFound(); // Return 404 Not Found if the card does not exist
            }

            // Ensure the logged-in user has permission to close the card
            if (!loggedInUser.IsAdministrator && card.Account.UserId != loggedInUser.Id)
            {
                return Forbid(); // Return 403 Forbidden if access is denied
            }

            // Close the card by updating its status
            card.Status = Status.Closed; // Assuming 'Closed' is a valid status in the Status enum
            _context.Cards.Update(card); // Mark the card as modified
            await _context.SaveChangesAsync(); // Save changes to the database

            return NoContent(); // Return 204 No Content on successful closure
        }

        // Private helper method to check if a card exists by its ID
        private bool CardExists(int id)
        {
            return _context.Cards.Any(e => e.Id == id); // Returns true if the card exists, false otherwise
        }
    }
}
