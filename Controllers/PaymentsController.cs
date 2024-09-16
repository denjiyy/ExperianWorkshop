using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using BankManagementSystem.Models;
using BankManagementSystem.Models.Enums;
using BankManagementSystem.DataProcessor;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BankManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Commenting out authorization for testing
    public class PaymentsController : ControllerBase
    {
        private readonly BankSystemContext _context;

        public PaymentsController(BankSystemContext context)
        {
            _context = context;
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentsDto>>> GetPayments()
        {
            // Commenting out UserId extraction from JWT token for testing
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("UserId is missing from the token.");
            }

            // Temporarily fetch all payments for testing without filtering by user
            var payments = await _context.Payments
                .Include(p => p.Loan)
                .ToListAsync();

            var paymentDtos = payments.Select(p => new PaymentsDto
            {
                Amount = p.Amount,
                Date = p.Date,
                CurrencyType = p.CurrencyType.ToString() // Convert enum to string
            }).ToList();

            return Ok(paymentDtos);
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentsDto>> GetPayment(int id)
        {
            // Commenting out UserId extraction from JWT token for testing
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("UserId is missing from the token.");
            }

            // Temporarily fetch payment without filtering by user for testing
            var payment = await _context.Payments
                .Include(p => p.Loan)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (payment == null)
            {
                return NotFound("Payment not found.");
            }

            var paymentDto = new PaymentsDto
            {
                PaymentMethod = payment.PaymentMethod.ToString(),
                Fee = payment.Fee,
                Amount = payment.Amount,
                Date = payment.Date,
                CurrencyType = payment.CurrencyType.ToString() // Convert enum to string
            };

            return Ok(paymentDto);
        }

        // POST: api/Payments
        [HttpPost]
        public async Task<ActionResult<PaymentsDto>> CreatePayment([FromBody] PaymentsDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Commenting out UserId extraction from JWT token for testing
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("UserId is missing from the token.");
            }

            if (!Enum.TryParse(dto.CurrencyType, true, out CurrencyType currencyType))
            {
                return BadRequest("Invalid CurrencyType.");
            }

            // Temporarily skip user-specific loan retrieval for testing
            var loan = await _context.Loans
                .FirstOrDefaultAsync(); // Modify this logic if you have a more specific way to find the loan

            if (loan == null)
            {
                return NotFound("No loan found.");
            }

            var payment = new Payment
            {
                Amount = dto.Amount,
                Date = dto.Date,
                CurrencyType = currencyType,
                LoanId = loan.Id
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            var paymentDto = new PaymentsDto
            {
                Amount = payment.Amount,
                Date = payment.Date,
                CurrencyType = payment.CurrencyType.ToString() // Convert enum to string
            };

            return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, paymentDto);
        }

        // PUT: api/Payments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] PaymentsDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Commenting out UserId extraction from JWT token for testing
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("UserId is missing from the token.");
            }

            var payment = await _context.Payments
                .Include(p => p.Loan)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
            {
                return NotFound();
            }

            if (!Enum.TryParse(dto.CurrencyType, true, out CurrencyType currencyType))
            {
                return BadRequest("Invalid CurrencyType.");
            }

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
            // Commenting out UserId extraction from JWT token for testing
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("UserId is missing from the token.");
            }

            var payment = await _context.Payments
                .Include(p => p.Loan)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
            {
                return NotFound();
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
