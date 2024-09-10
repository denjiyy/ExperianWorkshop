using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using BankManagementSystem.Models;
using BankManagementSystem.DataProcessor.Import;
using BankManagementSystem.Models.Enums;

namespace BankManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("UserId is missing from the token.");
            }

            var payments = await _context.Payments
                .Include(p => p.Loan)
                .Where(p => p.Loan.UserId == int.Parse(userId))
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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("UserId is missing from the token.");
            }

            var payment = await _context.Payments
                .Include(p => p.Loan)
                .Where(p => p.Id == id && p.Loan.UserId == int.Parse(userId))
                .FirstOrDefaultAsync();

            if (payment == null)
            {
                return NotFound("Payment not found or does not belong to the user.");
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

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("UserId is missing from the token.");
            }

            if (!Enum.TryParse(dto.CurrencyType, true, out CurrencyType currencyType))
            {
                return BadRequest("Invalid CurrencyType.");
            }

            // Determine which loan the payment belongs to (this could be handled differently based on your logic)
            var loan = await _context.Loans
                .Where(l => l.UserId == int.Parse(userId))
                .FirstOrDefaultAsync(); // Modify this logic if you have a more specific way to find the loan

            if (loan == null)
            {
                return NotFound("No loan found for the user.");
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

            if (payment.Loan.UserId != int.Parse(userId))
            {
                return Unauthorized("You do not have access to this payment.");
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

            if (payment.Loan.UserId != int.Parse(userId))
            {
                return Unauthorized("You do not have access to this payment.");
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
