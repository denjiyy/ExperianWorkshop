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
    public class TransactionsController : ControllerBase
    {
        private readonly BankSystemContext _context;

        public TransactionsController(BankSystemContext context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionsDto>>> GetTransactions()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("UserId is missing from the token.");
            }

            var transactions = await _context.Transactions
                .Include(t => t.Account)
                .Where(t => t.Account.UserId == int.Parse(userId)) // Filter by the user's account
                .ToListAsync();

            var transactionDtos = transactions.Select(t => new TransactionsDto
            {
                TransactionType = t.TransactionType.ToString(), // Convert enum to string
                Amount = t.Amount,
                TransactionDescription = t.TransactionDescription,
                TransactionStatus = t.TransactionStatus.ToString(), // Convert enum to string
                CurrencyType = t.CurrencyType.ToString(), // Convert enum to string
                Timestamp = t.Timestamp
            }).ToList();

            return Ok(transactionDtos);
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionsDto>> GetTransaction(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("UserId is missing from the token.");
            }

            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .Where(t => t.Id == id && t.Account.UserId == int.Parse(userId)) // Ensure the transaction belongs to the user's account
                .FirstOrDefaultAsync();

            if (transaction == null)
            {
                return NotFound("Transaction not found or does not belong to the user.");
            }

            var transactionDto = new TransactionsDto
            {
                TransactionType = transaction.TransactionType.ToString(), // Convert enum to string
                Amount = transaction.Amount,
                TransactionDescription = transaction.TransactionDescription,
                TransactionStatus = transaction.TransactionStatus.ToString(), // Convert enum to string
                CurrencyType = transaction.CurrencyType.ToString(), // Convert enum to string
                Timestamp = transaction.Timestamp
            };

            return Ok(transactionDto);
        }

        // POST: api/Transactions
        [HttpPost]
        public async Task<ActionResult<TransactionsDto>> CreateTransaction([FromBody] TransactionsDto dto)
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

            if (!Enum.TryParse(dto.TransactionType, true, out TransactionType transactionType))
            {
                return BadRequest("Invalid TransactionType.");
            }

            if (!Enum.TryParse(dto.TransactionStatus, true, out TransactionStatus transactionStatus))
            {
                return BadRequest("Invalid TransactionStatus.");
            }

            if (!Enum.TryParse(dto.CurrencyType, true, out CurrencyType currencyType))
            {
                return BadRequest("Invalid CurrencyType.");
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.UserId == int.Parse(userId)); // Get the user's account

            if (account == null)
            {
                return NotFound("No account found for the user.");
            }

            var transaction = new Transaction
            {
                TransactionType = transactionType,
                Amount = dto.Amount,
                TransactionDescription = dto.TransactionDescription,
                TransactionStatus = transactionStatus,
                CurrencyType = currencyType,
                Timestamp = dto.Timestamp,
                AccountId = account.Id
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            var transactionDto = new TransactionsDto
            {
                TransactionType = transaction.TransactionType.ToString(), // Convert enum to string
                Amount = transaction.Amount,
                TransactionDescription = transaction.TransactionDescription,
                TransactionStatus = transaction.TransactionStatus.ToString(), // Convert enum to string
                CurrencyType = transaction.CurrencyType.ToString(), // Convert enum to string
                Timestamp = transaction.Timestamp
            };

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transactionDto);
        }

        // PUT: api/Transactions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] TransactionsDto dto)
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

            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return NotFound();
            }

            if (transaction.Account.UserId != int.Parse(userId))
            {
                return Unauthorized("You do not have access to this transaction.");
            }

            if (!Enum.TryParse(dto.TransactionType, true, out TransactionType transactionType))
            {
                return BadRequest("Invalid TransactionType.");
            }

            if (!Enum.TryParse(dto.TransactionStatus, true, out TransactionStatus transactionStatus))
            {
                return BadRequest("Invalid TransactionStatus.");
            }

            if (!Enum.TryParse(dto.CurrencyType, true, out CurrencyType currencyType))
            {
                return BadRequest("Invalid CurrencyType.");
            }

            transaction.TransactionType = transactionType;
            transaction.Amount = dto.Amount;
            transaction.TransactionDescription = dto.TransactionDescription;
            transaction.TransactionStatus = transactionStatus;
            transaction.CurrencyType = currencyType;
            transaction.Timestamp = dto.Timestamp;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("UserId is missing from the token.");
            }

            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return NotFound();
            }

            if (transaction.Account.UserId != int.Parse(userId))
            {
                return Unauthorized("You do not have access to this transaction.");
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
