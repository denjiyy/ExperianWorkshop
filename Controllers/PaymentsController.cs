using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankManagementSystem.Models;
using BankManagementSystem.Models.Enums;
using BankManagementSystem.DataProcessor;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace BankManagementSystem.Controllers
{
    // Controller for managing payments in the banking system
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        // Dependency injection for database context and HTTP context accessor
        private readonly BankSystemContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Constructor to inject dependencies into the controller
        public PaymentsController(BankSystemContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // Private helper method to get the logged-in user based on the JWT token in the request
        private async Task<User> GetLoggedInUser()
        {
            // Extract the JWT token from the Authorization header
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
                return null!;

            // Parse the JWT token to get user claims
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;

            // If a valid user ID claim is found, return the corresponding user from the database
            if (userIdClaim != null && int.TryParse(userIdClaim, out var userId))
            {
                return await _context.Users.FindAsync(userId);
            }

            return null!; // Return null if the user could not be identified
        }

        // GET: api/Payments
        // Retrieves the payments made by the logged-in user
        [HttpGet]
        [Authorize] // Only authorized users can access this method
        public async Task<ActionResult<IEnumerable<PaymentsDto>>> GetPayments()
        {
            // Get the currently logged-in user
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in."); // Return 401 Unauthorized if user is not authenticated
            }

            // Retrieve payments made by the logged-in user
            var payments = await _context.Payments
                .Include(p => p.Loan) // Include the associated loan information
                .Where(p => p.Loan.UserId == loggedInUser.Id) // Filter payments by the user's loans
                .ToListAsync();

            // Convert payments to DTOs for returning to the client
            var paymentDto = payments.Select(p => new PaymentsDto
            {
                PaymentMethod = p.PaymentMethod.ToString(),
                Amount = p.Amount,
                Date = p.Date,
                CurrencyType = p.CurrencyType.ToString()
            }).ToList();

            return Ok(paymentDto); // Return the list of payments
        }

        // GET: api/Payments/5
        // Retrieves a specific payment by its ID for the logged-in user
        [HttpGet("{id}")]
        [Authorize] // Only authorized users can access this method
        public async Task<ActionResult<PaymentsDto>> GetPayment(int id)
        {
            // Get the logged-in user
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in."); // Return 401 if the user is not authenticated
            }

            // Find the payment by ID and ensure it belongs to the user's loan
            var payment = await _context.Payments
                .Include(p => p.Loan) // Include loan information
                .Where(p => p.Id == id && p.Loan.UserId == loggedInUser.Id) // Filter by the logged-in user
                .FirstOrDefaultAsync();

            // If the payment is not found, return 404 Not Found
            if (payment == null)
            {
                return NotFound("Payment not found or access denied.");
            }

            // Convert the payment to a DTO for returning to the client
            var paymentDto = new PaymentsDto
            {
                PaymentMethod = payment.PaymentMethod.ToString(),
                Amount = payment.Amount,
                Date = payment.Date,
                CurrencyType = payment.CurrencyType.ToString()
            };

            return Ok(paymentDto); // Return the payment DTO
        }

        [HttpPost("{loanId}")] // Expect a loanId as part of the route
        [Authorize] // Only authorized users can access this method
        public async Task<IActionResult> CreatePayment(int loanId, [FromBody] PaymentsDto dto)
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

            // Find the loan associated with the provided loanId
            var loan = await _context.Loans
                                      .FirstOrDefaultAsync(l => l.Id == loanId && l.UserId == loggedInUser.Id); // Check if the loan belongs to the user
            if (loan == null)
            {
                return NotFound("No associated loan found for the provided LoanId."); // Return 404 if no loan found
            }

            // Ensure PaymentMethod is valid
            if (!Enum.IsDefined(typeof(PaymentMethod), dto.PaymentMethod))
            {
                return BadRequest("Invalid payment method."); // Return 400 Bad Request for invalid payment method
            }

            // Create a new payment object from the provided data
            var payment = new Payment
            {
                PaymentMethod = (PaymentMethod)Enum.Parse(typeof(PaymentMethod), dto.PaymentMethod, true),
                Amount = dto.Amount,
                CurrencyType = (CurrencyType)Enum.Parse(typeof(CurrencyType), dto.CurrencyType, true),
                Date = DateOnly.FromDateTime(DateTime.UtcNow), // Set the current date
                LoanId = loan.Id // Associate with the loan found from the user
            };

            // Check for existing payment with the same details (optional)
            if (await _context.Payments.AnyAsync(p => p.LoanId == payment.LoanId && p.Amount == payment.Amount && p.Date == payment.Date))
            {
                return Conflict("A payment with the same details already exists."); // Return 409 Conflict if payment exists
            }

            // Add the new payment to the database
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync(); // Save changes to the database

            // Return the created payment with its location in the response header
            return Created(nameof(GetPayment), new
            {
                id = payment.Id,
                paymentMethod = payment.PaymentMethod,
                amount = payment.Amount,
                currencyType = payment.CurrencyType,
                date = payment.Date,
                loanId = payment.LoanId // Return the associated LoanId
            }); // Return 201 Created with location
        }

        // PUT: api/Payments/{id}
        // Updates an existing payment by its ID
        [HttpPut("{id}")]
        [Authorize] // Only authorized users can access this method
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] PaymentsDto dto)
        {
            // Retrieve the payment to update
            var paymentToUpdate = await _context.Payments
                .Include(p => p.Loan) // Include loan information
                .FirstOrDefaultAsync(x => x.Id == id);

            if (paymentToUpdate == null)
            {
                return NotFound(); // Return 404 if the payment is not found
            }

            // Update the payment details from the provided DTO
            paymentToUpdate.Amount = dto.Amount;
            paymentToUpdate.PaymentMethod = (PaymentMethod)Enum.Parse(typeof(PaymentMethod), dto.PaymentMethod, true);
            paymentToUpdate.CurrencyType = (CurrencyType)Enum.Parse(typeof(CurrencyType), dto.CurrencyType, true);
            paymentToUpdate.Date = dto.Date;

            // Recalculate the total payments made on the loan
            var totalPaymentsMade = await _context.Payments
                .Where(p => p.LoanId == paymentToUpdate.LoanId)
                .SumAsync(p => p.Amount);

            var originalLoanAmount = paymentToUpdate.Loan.Amount;
            var loanBalance = originalLoanAmount - totalPaymentsMade;

            // If the loan is fully paid off, update the loan status
            if (loanBalance <= 0)
            {
                paymentToUpdate.Loan.LoanStatus = LoanStatus.PaidOff;
                loanBalance = 0;
            }

            // Save the updated payment and loan information to the database
            await _context.SaveChangesAsync();

            // Return a success message along with the remaining loan balance
            return Ok(new
            {
                Message = "Payment updated successfully.",
                RemainingLoanBalance = loanBalance
            });
        }

        // DELETE: api/Payments/{id}
        // Deletes a payment by its ID
        [HttpDelete("{id}")]
        [Authorize] // Only authorized users can access this method
        public async Task<IActionResult> DeletePayment(int id)
        {
            // Get the logged-in user
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in."); // Return 401 Unauthorized if user is not authenticated
            }

            // Retrieve the payment by ID and ensure it belongs to the logged-in user's loan
            var payment = await _context.Payments
                .Include(p => p.Loan) // Include loan information
                .Where(p => p.Id == id && p.Loan.UserId == loggedInUser.Id) // Filter payments by the logged-in user
                .FirstOrDefaultAsync();

            if (payment == null)
            {
                return NotFound("Payment not found or access denied."); // Return 404 if the payment doesn't exist or user access is denied
            }

            // Remove the payment from the database
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return NoContent(); // Return 204 No Content on successful deletion
        }

        // Helper method to check if a payment exists by its ID
        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id); // Returns true if the payment exists, false otherwise
        }
    }
}
