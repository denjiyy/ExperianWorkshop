using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using BankManagementSystem.Models;
using BankManagementSystem.Models.Enums;
using BankManagementSystem.DataProcessor;

namespace BankManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly BankSystemContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentsController(BankSystemContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<User> GetLoggedInUser()
        {
            var userId = _httpContextAccessor.HttpContext?.Session.GetString("UserId");
            if (userId != null)
            {
                return await _context.Users.FindAsync(int.Parse(userId));
            }
            return null!;
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentsDto>>> GetPayments()
        {
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var payments = await _context.Payments
                .Include(p => p.Loan)
                .Where(p => p.Loan.UserId == loggedInUser.Id)
                .ToListAsync();

            var paymentDto = payments.Select(p => new PaymentsDto
            {
                PaymentMethod = p.PaymentMethod.ToString(),
                Amount = p.Amount,
                Date = p.Date,
                CurrencyType = p.CurrencyType.ToString()
            }).ToList();

            return Ok(paymentDto);
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentsDto>> GetPayment(int id)
        {
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var payment = await _context.Payments
                .Include(p => p.Loan)
                .Where(p => p.Id == id && p.Loan.UserId == loggedInUser.Id)
                .FirstOrDefaultAsync();

            if (payment == null)
            {
                return NotFound("Payment not found or access denied.");
            }

            var paymentDto = new PaymentsDto
            {
                PaymentMethod = payment.PaymentMethod.ToString(),
                Amount = payment.Amount,
                Date = payment.Date,
                CurrencyType = payment.CurrencyType.ToString()
            };

            return Ok(paymentDto);
        }

        // POST: api/Payments
        [HttpPost("{loanId}")]
        public async Task<ActionResult<Payment>> CreatePayment(int loanId, [FromBody] PaymentsDto paymentDto)
        {
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var loan = await _context.Loans.FindAsync(loanId);
            if (loan == null)
            {
                return NotFound("Loan not found.");
            }

            var payment = new Payment
            {
                Amount = paymentDto.Amount,
                Date = DateOnly.FromDateTime(DateTime.UtcNow),
                LoanId = loan.Id,
                CurrencyType = (CurrencyType)Enum.Parse(typeof(CurrencyType), paymentDto.CurrencyType, true),
                PaymentMethod = (PaymentMethod)Enum.Parse(typeof(CurrencyType), paymentDto.PaymentMethod, true)
            };

            _context.Payments.Add(payment);
            loan.TotalPaid += payment.Amount;

            if (loan.TotalPaid >= loan.Amount)
            {
                loan.LoanStatus = LoanStatus.PaidOff;
            }

            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, payment);
        }

        // PUT: api/Payments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] PaymentsDto dto)
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

            var payment = await _context.Payments
                .Include(p => p.Loan)
                .Where(p => p.Id == id && p.Loan.UserId == loggedInUser.Id)
                .FirstOrDefaultAsync();

            if (payment == null)
            {
                return NotFound("Payment not found or access denied.");
            }

            if (!Enum.TryParse(dto.CurrencyType, true, out CurrencyType currencyType))
            {
                return BadRequest("Invalid CurrencyType.");
            }

            payment.PaymentMethod = Enum.TryParse(dto.PaymentMethod, out PaymentMethod paymentMethod) ? paymentMethod : payment.PaymentMethod;
            payment.Amount = dto.Amount;
            payment.Date = dto.Date;
            payment.CurrencyType = currencyType;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
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

        // DELETE: api/Payments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var payment = await _context.Payments
                .Include(p => p.Loan)
                .Where(p => p.Id == id && p.Loan.UserId == loggedInUser.Id)
                .FirstOrDefaultAsync();

            if (payment == null)
            {
                return NotFound("Payment not found or access denied.");
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }
    }
}
