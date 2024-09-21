using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankManagementSystem.Models;
using BankManagementSystem.Models.Enums;
using BankManagementSystem.DataProcessor;

namespace BankManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly BankSystemContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public LoansController(BankSystemContext context, HttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        private async Task<User> GetLoggedInUser()
        {
            var userId = _contextAccessor.HttpContext?.Session.GetString("UserId");
            if (userId != null)
            {
                return await _context.Users.FindAsync(int.Parse(userId));
            }
            return null!;
        }

        private DateOnly GetFirstOfNextMonth(DateOnly date)
        {
            return new DateOnly(date.Year, date.Month, 1).AddMonths(1);
        }

        // GET: api/Loans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoans()
        {
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            IQueryable<Loan> query = _context.Loans.Include(l => l.User);

            if (!loggedInUser.IsAdministrator)
            {
                query = query.Where(l => l.UserId == loggedInUser.Id);
            }

            var loans = await query.ToListAsync();
            return Ok(loans);
        }

        // GET: api/Loans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetLoan(int id)
        {
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var loan = await _context.Loans.Include(l => l.User).FirstOrDefaultAsync(l => l.Id == id);
            if (loan == null)
            {
                return NotFound();
            }

            if (!loggedInUser.IsAdministrator && loan.UserId != loggedInUser.Id)
            {
                return Forbid();
            }

            return Ok(loan);
        }

        // POST: api/Loans
        [HttpPost]
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

            var riskController = new RiskController(_context);
            decimal riskScore = riskController.CalculateRiskScore(loanDto, loggedInUser.Id);

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
                StartDate = loanDto.StartDate,
                NextPaymentDate = GetFirstOfNextMonth(loanDto.StartDate),
                EndDate = loanDto.EndDate,
                LoanType = Enum.TryParse(loanDto.LoanType, true, out LoanType loanType)
                    ? loanType
                    : throw new ArgumentException("Invalid loan type."),
                LoanStatus = Enum.TryParse(loanDto.LoanStatus, true, out LoanStatus loanStatus)
                    ? loanStatus
                    : throw new ArgumentException("Invalid loan status."),
                UserId = loggedInUser.Id
            };

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLoan), new { id = loan.Id }, loan);
        }

        // PUT: api/Loans/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLoan(int id, [FromBody] LoansDto loanDto)
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

            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }

            if (!loggedInUser.IsAdministrator && loan.UserId != loggedInUser.Id)
            {
                return Forbid();
            }

            loan.Amount = loanDto.Amount;
            loan.InterestRate = loanDto.InterestRate;
            loan.CurrencyType = Enum.TryParse(loanDto.CurrencyType, true, out CurrencyType currencyType)
                ? currencyType
                : throw new ArgumentException("Invalid currency type.");
            loan.DateApproved = loanDto.DateApproved;
            loan.StartDate = loanDto.StartDate;
            loan.NextPaymentDate = GetFirstOfNextMonth(loanDto.StartDate);
            loan.EndDate = loanDto.EndDate;
            loan.LoanType = Enum.TryParse(loanDto.LoanType, true, out LoanType loanType)
                ? loanType
                : throw new ArgumentException("Invalid loan type.");
            loan.LoanStatus = Enum.TryParse(loanDto.LoanStatus, true, out LoanStatus loanStatus)
                ? loanStatus
                : throw new ArgumentException("Invalid loan status.");

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Loans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }

            if (!loggedInUser.IsAdministrator && loan.UserId != loggedInUser.Id)
            {
                return Forbid();
            }

            loan.LoanStatus = LoanStatus.PaidOff;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.Id == id);
        }
    }
}
