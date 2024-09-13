using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankManagementSystem.Models;
using BankManagementSystem.Models.Enums;
using BankManagementSystem.DataProcessor.Import;

namespace BankManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize] // Commenting out authorization for testing
    public class LoansController : ControllerBase
    {
        private readonly BankSystemContext _context;

        public LoansController(BankSystemContext context)
        {
            _context = context;
        }

        // GET: api/Loans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoans()
        {
            // Commenting out UserId extraction from JWT token for testing
            // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // if (string.IsNullOrEmpty(userId))
            // {
            //     return Unauthorized("UserId is missing from the token.");
            // }

            // Temporarily fetch all loans for testing without filtering by user
            var loans = await _context.Loans
                .Include(l => l.User)
                .ToListAsync();

            return Ok(loans);
        }

        // GET: api/Loans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetLoan(int id)
        {
            // Commenting out UserId extraction from JWT token for testing
            // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // if (string.IsNullOrEmpty(userId))
            // {
            //     return Unauthorized("UserId is missing from the token.");
            // }

            // Temporarily fetch loan without filtering by user for testing
            var loan = await _context.Loans
                .Where(l => l.Id == id)
                .Include(l => l.User)
                .FirstOrDefaultAsync();

            if (loan == null)
            {
                return NotFound();
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

            // Commenting out UserId extraction from JWT token for testing
            // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // if (string.IsNullOrEmpty(userId))
            // {
            //     return Unauthorized("UserId is missing from the token.");
            // }

            var loan = new Loan
            {
                Amount = loanDto.Amount,
                InterestRate = loanDto.InterestRate,
                CurrencyType = (CurrencyType)Enum.Parse(typeof(CurrencyType), loanDto.CurrencyType, true),
                DateApproved = loanDto.DateApproved,
                StartDate = loanDto.StartDate,
                NextPaymentDate = loanDto.NextPaymentDate,
                EndDate = loanDto.EndDate,
                LoanType = (LoanType)Enum.Parse(typeof(LoanType), loanDto.LoanType, true),
                LoanStatus = (LoanStatus)Enum.Parse(typeof(LoanStatus), loanDto.LoanStatus, true),
                // Temporarily skipping UserId association for testing
            };

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLoan), new { id = loan.Id }, loan);
        }

        // PUT: api/Loans/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLoan(int id, [FromBody] LoansDto loanDto)
        {
            // Commenting out UserId extraction from JWT token for testing
            // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // if (string.IsNullOrEmpty(userId))
            // {
            //     return Unauthorized("UserId is missing from the token.");
            // }

            var loan = await _context.Loans
                .Where(l => l.Id == id)
                .FirstOrDefaultAsync();

            if (loan == null)
            {
                return NotFound();
            }

            loan.Amount = loanDto.Amount;
            loan.InterestRate = loanDto.InterestRate;
            loan.CurrencyType = (CurrencyType)Enum.Parse(typeof(CurrencyType), loanDto.CurrencyType, true);
            loan.DateApproved = loanDto.DateApproved;
            loan.StartDate = loanDto.StartDate;
            loan.NextPaymentDate = loanDto.NextPaymentDate;
            loan.EndDate = loanDto.EndDate;
            loan.LoanType = (LoanType)Enum.Parse(typeof(LoanType), loanDto.LoanType, true);
            loan.LoanStatus = (LoanStatus)Enum.Parse(typeof(LoanStatus), loanDto.LoanStatus, true);

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
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Loans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            // Commenting out UserId extraction from JWT token for testing
            // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // if (string.IsNullOrEmpty(userId))
            // {
            //     return Unauthorized("UserId is missing from the token.");
            // }

            var loan = await _context.Loans
                .Where(l => l.Id == id)
                .FirstOrDefaultAsync();

            if (loan == null)
            {
                return NotFound();
            }

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.Id == id);
        }
    }
}
