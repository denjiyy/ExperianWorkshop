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
    public class TransactionsController : ControllerBase
    {
        private readonly BankSystemContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransactionsController(BankSystemContext context, IHttpContextAccessor httpContextAccessor)
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

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionsDto>>> GetTransactions()
        {
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var transactions = await _context.Transactions
                .Include(t => t.Account)
                .Where(t => t.Account.UserId == loggedInUser.Id)
                .ToListAsync();

            var transactionDtos = transactions.Select(t => new TransactionsDto
            {
                TransactionType = t.TransactionType.ToString(),
                Amount = t.Amount,
                TransactionDescription = t.TransactionDescription,
                TransactionStatus = t.TransactionStatus.ToString(),
                CurrencyType = t.CurrencyType.ToString(),
                Timestamp = t.Timestamp
            }).ToList();

            return Ok(transactionDtos);
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionsDto>> GetTransaction(int id)
        {
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .Where(t => t.Id == id && t.Account.UserId == loggedInUser.Id)
                .FirstOrDefaultAsync();

            if (transaction == null)
            {
                return NotFound("Transaction not found or access denied.");
            }

            var transactionDto = new TransactionsDto
            {
                TransactionType = transaction.TransactionType.ToString(),
                Amount = transaction.Amount,
                TransactionDescription = transaction.TransactionDescription,
                TransactionStatus = transaction.TransactionStatus.ToString(),
                CurrencyType = transaction.CurrencyType.ToString(),
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

            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
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
                .FirstOrDefaultAsync(a => a.UserId == loggedInUser.Id);

            if (account == null)
            {
                return NotFound("No account found for the logged-in user.");
            }

            var transaction = new Transaction
            {
                TransactionType = transactionType,
                Amount = dto.Amount,
                TransactionDescription = dto.TransactionDescription,
                TransactionStatus = transactionStatus,
                CurrencyType = currencyType,
                Timestamp = dto.Timestamp,
                AccountId = account.Id,
                Recipient = dto.Recipient
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            var transactionDto = new TransactionsDto
            {
                TransactionType = transaction.TransactionType.ToString(),
                Amount = transaction.Amount,
                TransactionDescription = transaction.TransactionDescription,
                TransactionStatus = transaction.TransactionStatus.ToString(),
                CurrencyType = transaction.CurrencyType.ToString(),
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

            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .FirstOrDefaultAsync(t => t.Id == id && t.Account.UserId == loggedInUser.Id);

            if (transaction == null)
            {
                return NotFound("Transaction not found or access denied.");
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
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .FirstOrDefaultAsync(t => t.Id == id && t.Account.UserId == loggedInUser.Id);

            if (transaction == null)
            {
                return NotFound("Transaction not found or access denied.");
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