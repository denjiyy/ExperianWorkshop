using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankManagementSystem.Models;
using BankManagementSystem.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using BankManagementSystem.DataProcessor;

namespace BankManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly BankSystemContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountsController(BankSystemContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<User> GetLoggedInUser()
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                return await _context.Users.FindAsync(int.Parse(userId));
            }
            return null;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<IActionResult> GetAccounts()
        {
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            IQueryable<Account> query = _context.Accounts;
            if (!loggedInUser.IsAdministrator)
            {
                query = query.Where(a => a.UserId == loggedInUser.Id);
            }

            var accounts = await query
                .Select(a => new
                {
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
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var account = await _context.Accounts
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            if (account == null)
            {
                return NotFound();
            }

            if (!loggedInUser.IsAdministrator && account.UserId != loggedInUser.Id)
            {
                return Forbid();
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

            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var account = new Account
            {
                AccountNumber = dto.AccountNumber,
                AccountType = (AccountType)Enum.Parse(typeof(AccountType), dto.AccountType, true),
                Balance = dto.Balance,
                Status = (Status)Enum.Parse(typeof(Status), dto.Status, true),
                UserId = loggedInUser.Id,
                DateOpened = DateOnly.FromDateTime(DateTime.Now),
                CurrencyType = (CurrencyType)Enum.Parse(typeof(CurrencyType), dto.CurrencyType, true)
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

            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var account = await _context.Accounts
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            if (account == null)
            {
                return NotFound();
            }

            if (!loggedInUser.IsAdministrator && account.UserId != loggedInUser.Id)
            {
                return Forbid();
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
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var account = await _context.Accounts
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            if (account == null)
            {
                return NotFound();
            }

            if (!loggedInUser.IsAdministrator && account.UserId != loggedInUser.Id)
            {
                return Forbid();
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