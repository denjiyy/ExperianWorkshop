using BankManagementSystem.DataProcessor;
using BankManagementSystem.Models;
using BankManagementSystem.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RiskController : ControllerBase
    {
        private readonly BankSystemContext _context;

        private const decimal CreditScoreWeight = 0.5m;
        private const decimal TypeWeight = 0.2m;
        private const decimal AmountWeight = 0.15m;
        private const decimal TimeWeight = 0.15m;

        public RiskController(BankSystemContext context)
        {
            _context = context;
        }

        [HttpPost("calculate-risk")]
        public decimal CalculateRiskScore([FromBody] LoansDto loanDto, int userId)
        {
            if (loanDto == null) throw new ArgumentNullException(nameof(loanDto), "Loan data is required.");

            var user = _context.Users.Find(userId);
            if (user == null) throw new ArgumentNullException(nameof(user), "User not found.");

            var userLoans = _context.Loans.Where(l => l.UserId == userId).ToList();
            var userPayments = _context.Payments.Include(p => p.Loan).Where(p => p.Loan.UserId == userId).ToList();

            decimal creditScoreFactor = CreditScoreWeight * GetCreditScoreFactor(user);
            decimal typeFactor = TypeWeight * GetTypeFactor(loanDto.LoanType);
            decimal amountFactor = AmountWeight * GetAmountFactor(loanDto.Amount);
            decimal timeFactor = TimeWeight * GetTimeFactor(loanDto.StartDate, loanDto.EndDate);

            decimal riskScore = creditScoreFactor + typeFactor + amountFactor + timeFactor;
            riskScore += CalculateAdditionalRiskFactors(userLoans, userPayments, user);

            return riskScore;
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

        private decimal GetAmountFactor(decimal amount)
        {
            if (amount > 1000000) return 0;
            return amount switch
            {
                < 10000 => 40,
                < 50000 => 60,
                < 100000 => 80,
                _ => 100,
            };
        }

        private decimal GetTimeFactor(DateOnly startDate, DateOnly endDate)
        {
            var durationInMonths = ((endDate.Year - startDate.Year) * 12) + (endDate.Month - startDate.Month);

            return durationInMonths switch
            {
                > 420 => 80,
                > 120 => 60,
                _ => 40,
            };
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
        public async Task<IActionResult> UpdateCreditScore(int id, [FromBody] int? scoreChange)
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

            user.UpdateCreditScore(scoreChange.Value);
            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Credit score updated.", creditScore = user.CreditScore });
        }

        [HttpGet("user-risk/{userId}")]
        public ActionResult<decimal> GetUserRiskScore(int userId, [FromBody] LoansDto loanDto)
        {
            if (loanDto == null) return BadRequest("Loan data is required.");

            var user = _context.Users.Find(userId);
            if (user == null) return NotFound("User not found.");

            var riskScore = CalculateRiskScore(loanDto, userId);

            if (riskScore < 45)
            {
                return Ok(new { RiskScore = riskScore, Status = "Loan Denied", Message = "Loan denied due to high risk assessment." });
            }

            return Ok(new { RiskScore = riskScore, Status = "Loan Approved", Message = "Loan approved based on risk assessment." });
        }
    }
}