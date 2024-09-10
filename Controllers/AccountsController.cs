using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BankManagementSystem.Models;
using BankManagementSystem.Models.Enums;
using System.Security.Claims;
using BankManagementSystem.DataProcessor.Import;

namespace BankManagementSystem.wwwroot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Secures this controller with JWT
    public class AccountsController : ControllerBase
    {
        private readonly BankSystemContext _context;

        public AccountsController(BankSystemContext context)
        {
            _context = context;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<IActionResult> GetAccounts()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("UserId is missing from the token.");
            }

            var accounts = await _context.Accounts
                .Where(a => a.UserId == int.Parse(userId))  // Filter accounts by logged-in user
                .Select(a => new
                {
                    a.Id,
                    a.AccountNumber,
                    a.Balance,
                    a.AccountType,
                    a.Status,
                    a.DateOpened,
                })
                .ToListAsync();

            return Ok(accounts);
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("UserId is missing from the token.");
            }

            var account = await _context.Accounts
                .Where(a => a.Id == id && a.UserId == int.Parse(userId))  // Ensure account belongs to the logged-in user
                .FirstOrDefaultAsync();

            if (account == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                account.Id,
                account.AccountNumber,
                account.Balance,
                account.AccountType,
                account.Status,
                account.DateOpened
            });
        }

        // POST: api/Accounts
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] AccountsDto dto)
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

            var account = new Account
            {
                AccountNumber = dto.AccountNumber,
                AccountType = (AccountType)Enum.Parse(typeof(AccountType), dto.AccountType, true),
                Balance = dto.Balance,
                Status = (Status)Enum.Parse(typeof(Status), dto.Status, true),
                UserId = int.Parse(userId), // Assign the UserId from the token
                DateOpened = DateTime.UtcNow
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }

        // PUT: api/Accounts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditAccount(int id, [FromBody] AccountsDto dto)
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

            var account = await _context.Accounts
                .Where(a => a.Id == id && a.UserId == int.Parse(userId)) // Ensure account belongs to the logged-in user
                .FirstOrDefaultAsync();

            if (account == null)
            {
                return NotFound();
            }

            account.AccountType = (AccountType)Enum.Parse(typeof(AccountType), dto.AccountType, true);
            account.Balance = dto.Balance;
            account.Status = (Status)Enum.Parse(typeof(Status), dto.Status, true);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
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

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("UserId is missing from the token.");
            }

            var account = await _context.Accounts
                .Where(a => a.Id == id && a.UserId == int.Parse(userId))  // Ensure account belongs to the logged-in user
                .FirstOrDefaultAsync();

            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }
    }
}
