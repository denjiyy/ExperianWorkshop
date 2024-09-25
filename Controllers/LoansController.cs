using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankManagementSystem.Models;
using BankManagementSystem.Models.Enums;
using BankManagementSystem.DataProcessor;
using System.IdentityModel.Tokens.Jwt;

namespace BankManagementSystem.Controllers
{
    // Controller for managing loan-related operations in the banking system
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly BankSystemContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public LoansController(BankSystemContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        private async Task<User> GetLoggedInUser()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
                return null!;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;

            if (userIdClaim != null && int.TryParse(userIdClaim, out var userId))
            {
                return await _context.Users.FindAsync(userId);
            }

            return null!;
        }

        private DateOnly GetFirstOfNextMonth(DateOnly date)
        {
            return new DateOnly(date.Year, date.Month, 1).AddMonths(1);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<object>>> GetLoans()
        {
            // Get the logged-in user
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            // Query the loans and include the related User and Payments entities
            IQueryable<Loan> query = _context.Loans
                .Include(l => l.User) // Includes the User entity
                .Include(l => l.Payments); // Include the Payments related to the loan

            // If the logged-in user is not an administrator, filter by userId
            if (!loggedInUser.IsAdministrator)
            {
                query = query.Where(l => l.UserId == loggedInUser.Id);
            }

            // Retrieve the loans
            var loans = await query.ToListAsync();

            // Return loan data along with payments one by one
            var loanDetails = loans.Select(loan => new
            {
                Id = loan.Id,
                Amount = loan.Amount,
                InterestRate = loan.InterestRate,
                LoanDate = loan.DateApproved,
                CurrencyType = loan.CurrencyType,
                NextPaymentDate = loan.NextPaymentDate,
                EndDate = loan.EndDate,
                TotalPaid = loan.TotalPaid,
                UserId = loan.UserId,
                Payments = loan.Payments.Select(payment => new
                {
                    PaymentId = payment.Id,
                    Amount = payment.Amount,
                    PaymentDate = payment.Date,
                    PaymentMethod = payment.PaymentMethod
                }).ToList()
            }).ToList();

            // Return the list of loan details along with their payments
            return Ok(loanDetails);
        }

        // GET: api/Loans/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<object>> GetLoan(int id)
        {
            // Get the logged-in user
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            // Retrieve the loan with the specified id and include the related User entity
            var loan = await _context.Loans.Include(l => l.User).FirstOrDefaultAsync(l => l.Id == id);

            // If the loan is not found, return 404
            if (loan == null)
            {
                return NotFound();
            }

            // If the user is not an administrator and does not own the loan, return 403
            if (!loggedInUser.IsAdministrator && loan.UserId != loggedInUser.Id)
            {
                return Forbid();
            }

            // Prepare loan details to return
            var loanDetails = new
            {
                Id = loan.Id,
                Amount = loan.Amount,
                InterestRate = loan.InterestRate,
                LoanDate = loan.DateApproved,
                CurrencyType = loan.CurrencyType,
                NextPaymentDate = loan.NextPaymentDate,
                EndDate = loan.EndDate,
                TotalPaid = loan.TotalPaid,
                UserId = loan.UserId,
                Payments = loan.Payments.Select(payment => new
                {
                    PaymentId = payment.Id,
                    Amount = payment.Amount,
                    PaymentDate = payment.Date,
                    PaymentMethod = payment.PaymentMethod
                }).ToList()
            };

            return Ok(loanDetails);
        }

        // POST: api/Loans
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Loan>> CreateLoan([FromBody] LoansDto loanDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var activeLoansCount = await _context.Loans.CountAsync(l => l.UserId == loggedInUser.Id && l.LoanStatus == LoanStatus.Active);
            if (activeLoansCount >= 10)
            {
                return BadRequest("You cannot have more than 10 active loans.");
            }

            var riskController = new RiskController(_context, _contextAccessor);

            // Await the result of the asynchronous method
            ActionResult<decimal> riskScoreResult = await riskController.CalculateRiskScore(loanDto);

            if (riskScoreResult.Result is BadRequestResult)
            {
                return BadRequest("Error in risk score calculation.");
            }

            decimal riskScore = riskScoreResult.Value;

            if (riskScore < 45)
            {
                return BadRequest("Loan denied due to high risk assessment.");
            }

            var loan = new Loan
            {
                Amount = loanDto.Amount,
                InterestRate = loanDto.InterestRate,
                CurrencyType = Enum.TryParse(loanDto.CurrencyType, true, out CurrencyType currencyType)
                    ? currencyType
                    : throw new ArgumentException("Invalid currency type."),
                DateApproved = loanDto.DateApproved,
                NextPaymentDate = GetFirstOfNextMonth(loanDto.StartDate),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(loanDto.DurationInMonths)),
                TotalPaid = loanDto.TotalPaid,
                LoanType = Enum.TryParse(loanDto.LoanType, true, out LoanType loanType)
                    ? loanType
                    : throw new ArgumentException("Invalid loan type."),
                LoanStatus = LoanStatus.Pending,
                UserId = loggedInUser.Id // Set the user ID of the loan owner
            };

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return Created(nameof(GetLoan), new { id = loan.Id,
                interestRate = loan.InterestRate,
                amount = loan.Amount,
                totalPaid = loan.TotalPaid,
                currencyType = loan.CurrencyType,
                dateApproved = loan.DateApproved,
                nextPaymentDate = loan.NextPaymentDate,
                endDate = loan.EndDate,
                loanType = loan.LoanType,
                loanStatus = loan.LoanStatus,
                userId = loan.UserId,
                payments = loan.Payments
            });
        }

        // PUT: api/Loans/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateLoan(int id, [FromBody] LoansDto loanDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return 400 Bad Request if the model state is invalid
            }

            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in."); // Return 401 Unauthorized if user is not authenticated
            }

            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound(); // Return 404 if the loan is not found
            }

            if (!loggedInUser.IsAdministrator && loan.UserId != loggedInUser.Id)
            {
                return Forbid(); // Return 403 Forbidden if access is denied
            }

            loan.Amount = loanDto.Amount;
            loan.InterestRate = loanDto.InterestRate;
            loan.CurrencyType = Enum.TryParse(loanDto.CurrencyType, true, out CurrencyType currencyType)
                ? currencyType
                : throw new ArgumentException("Invalid currency type."); // Validate currency type
            loan.DateApproved = loanDto.DateApproved;
            loan.NextPaymentDate = GetFirstOfNextMonth(loanDto.StartDate); // Update the next payment date
            loan.EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(loanDto.DurationInMonths));
            loan.LoanType = Enum.TryParse(loanDto.LoanType, true, out LoanType loanType)
                ? loanType
                : throw new ArgumentException("Invalid loan type."); // Validate loan type
            loan.LoanStatus = Enum.TryParse(loanDto.LoanStatus, true, out LoanStatus loanStatus)
                ? loanStatus
                : throw new ArgumentException("Invalid loan status."); // Validate loan status

            try
            {
                await _context.SaveChangesAsync(); // Save the updated loan to the database
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanExists(id))
                {
                    return NotFound(); // Return 404 if the loan does not exist
                }
                throw; // Rethrow the exception for unhandled scenarios
            }

            return NoContent(); // Return 204 No Content on successful update
        }

        // DELETE: api/Loans/5
        [HttpDelete("{id}")]
        [Authorize] // Only authorized users can access this method
        public async Task<IActionResult> DeleteLoan(int id)
        {
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in."); // Return 401 Unauthorized if user is not authenticated
            }

            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound(); // Return 404 if the loan does not exist
            }

            if (!loggedInUser.IsAdministrator && loan.UserId != loggedInUser.Id)
            {
                return Forbid(); // Return 403 Forbidden if access is denied
            }

            loan.LoanStatus = LoanStatus.PaidOff;

            try
            {
                await _context.SaveChangesAsync(); // Save the changes to the database
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanExists(id))
                {
                    return NotFound(); // Return 404 if the loan does not exist
                }
                throw; // Rethrow the exception for unhandled scenarios
            }

            return NoContent(); // Return 204 No Content on successful deletion
        }

        // Private helper method to check if a loan exists by its ID
        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.Id == id); // Returns true if the loan exists, false otherwise
        }
    }
}