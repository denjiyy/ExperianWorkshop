using BankManagementSystem.DataProcessor;
using BankManagementSystem.Models;
using BankManagementSystem.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace BankManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RiskController : ControllerBase
    {
        private readonly BankSystemContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private const decimal CreditScoreWeight = 0.5m;
        private const decimal TypeWeight = 0.2m;
        private const decimal AmountWeight = 0.15m;
        private const decimal TimeWeight = 0.15m;

        public RiskController(BankSystemContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        // Method to get logged in user from the JWT token
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

        [HttpPost("calculate-risk")]
        [Authorize]
        public async Task<ActionResult<decimal>> CalculateRiskScore([FromBody] LoansDto loanDto)
        {
            if (loanDto == null)
                return BadRequest("Loan data is required.");

            var user = await GetLoggedInUser();  // Use the method to get the logged-in user

            if (user == null)
                return Unauthorized("User identity not found in the token.");

            var userLoans = await _context.Loans.Where(l => l.UserId == user.Id).ToListAsync();
            var userPayments = await _context.Payments.Include(p => p.Loan).Where(p => p.Loan.UserId == user.Id).ToListAsync();

            decimal creditScoreFactor = CreditScoreWeight * GetCreditScoreFactor(user);
            decimal typeFactor = TypeWeight * GetTypeFactor(loanDto.LoanType);
            decimal amountFactor = AmountWeight * GetAmountFactor(loanDto.Amount, loanDto.LoanType);
            decimal timeFactor = TimeWeight * GetTimeFactor(loanDto.StartDate, loanDto.DurationInMonths, loanDto.LoanType);

            decimal riskScore = creditScoreFactor + typeFactor + amountFactor + timeFactor;
            riskScore += CalculateAdditionalRiskFactors(userLoans, userPayments, user);

            return Ok(riskScore);
        }

        private decimal GetCreditScoreFactor(User user)
        {
            return Math.Clamp(user.CreditScore, 0, 100);
        }

        private decimal GetTypeFactor(string loanType)
        {
            return loanType switch
            {
                "Personal" => 75,
                "Mortgage" => 50,
                "Student" => 100,
                _ => 50,
            };
        }

        private decimal GetAmountFactor(decimal amount, string loanType)
        {
            (decimal minAmount, decimal maxAmount) limits = loanType switch
            {
                "Personal" => (500, 75_000),
                "Student" => (1_000, 10_000),
                "Mortgage" => (10_000, 1_000_000),
                _ => (500, 75_000),
            };

            return CalculateScoreBasedOnAmount(amount, limits.minAmount, limits.maxAmount);
        }

        private decimal CalculateScoreBasedOnAmount(decimal amount, decimal minAmount, decimal maxAmount)
        {
            if (amount > maxAmount) return 1;
            if (amount < minAmount) return 100;

            return 100 - ((amount - minAmount) / (maxAmount - minAmount) * 99);
        }

        private decimal GetTimeFactor(DateOnly startDate, int durationInMonths, string loanType)
        {
            (int minMonths, int maxMonths) timeLimits = loanType switch
            {
                "Personal" => (3, 120),
                "Student" => (3, 60),
                "Mortgage" => (12, 420),
                _ => (3, 420),
            };

            return CalculateScoreBasedOnDuration(durationInMonths, timeLimits.minMonths, timeLimits.maxMonths);
        }

        private decimal CalculateScoreBasedOnDuration(int durationInMonths, int minMonths, int maxMonths)
        {
            if (durationInMonths > maxMonths) return 1;
            if (durationInMonths < minMonths) return 100;

            return 100 - ((durationInMonths - minMonths) / (decimal)(maxMonths - minMonths) * 99);
        }

        private decimal CalculateAdditionalRiskFactors(List<Loan> userLoans, List<Payment> userPayments, User user)
        {
            decimal additionalRisk = 0;

            var latePaymentsCount = userPayments.Count(p => p.Date > p.Loan.NextPaymentDate);
            if (latePaymentsCount > 5)
            {
                additionalRisk += 20;
                user.CreditScore -= 40;
            }
            else if (latePaymentsCount > 2)
            {
                additionalRisk += 10;
                user.CreditScore -= 20;
            }

            var onTimePaymentsCount = userPayments.Count(p => p.Date <= p.Loan.NextPaymentDate);
            if (onTimePaymentsCount > 5)
            {
                user.CreditScore += 30;
            }

            int distinctLoanTypes = userLoans.Select(l => l.LoanType).Distinct().Count();
            if (distinctLoanTypes >= 3)
            {
                user.CreditScore += 15;
            }

            int activeLoansCount = userLoans.Count(l => l.LoanStatus == LoanStatus.Active);
            if (activeLoansCount > 10)
            {
                user.CreditScore -= 15;
            }

            user.CreditScore = Math.Clamp(user.CreditScore, 0, 100);

            return additionalRisk;
        }

        [HttpPut("{id}/update-credit-score")]
        [Authorize]
        public async Task<IActionResult> UpdateCreditScore([FromBody] int? scoreChange, int id = 1)
        {
            if (!scoreChange.HasValue)
            {
                return BadRequest("Score change is required.");
            }

            if (scoreChange < 0 || scoreChange > 100)
            {
                return BadRequest("The credit score must be between 0 and 100.");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("User not found.");

            user.CreditScore = scoreChange.Value;
            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Credit score updated.", creditScore = user.CreditScore });
        }

        [HttpGet("user-risk")]
        public async Task<ActionResult<decimal>> GetUserRiskScore([FromBody] LoansDto loanDto)
        {
            if (loanDto == null)
                return BadRequest("Loan data is required.");

            var user = await GetLoggedInUser();  // Use the method to get the logged-in user

            if (user == null)
                return Unauthorized("User identity not found in the token.");

            var riskScore = await CalculateRiskScore(loanDto);

            if (riskScore.Value < 45)
            {
                return Ok(new { RiskScore = riskScore.Value, Status = "Loan Denied", Message = "Loan denied due to high risk assessment." });
            }

            return Ok(new { RiskScore = riskScore.Value, Status = "Loan Approved", Message = "Loan approved based on risk assessment." });
        }
    }
}