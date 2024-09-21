using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankManagementSystem.Models;
using BankManagementSystem.Models.Enums;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using BankManagementSystem.DataProcessor;

namespace BankManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly BankSystemContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CardsController(BankSystemContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<User> GetLoggedInUser()
        {
            var userId = _httpContextAccessor.HttpContext!.Session.GetString("UserId");
            if (userId != null)
            {
                return await _context.Users.FindAsync(int.Parse(userId));
            }
            return null;
        }

        // GET: api/Cards
        [HttpGet]
        public async Task<IActionResult> GetCards()
        {
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            IQueryable<Card> query = _context.Cards.Include(c => c.Account);

            if (!loggedInUser.IsAdministrator)
            {
                query = query.Where(c => c.Account.UserId == loggedInUser.Id);
            }

            var cards = await query
                .Select(c => new CardsDto
                {
                    CardNumber = c.CardNumber,
                    CardType = c.CardType.ToString(),
                    ExpiryDate = c.ExpiryDate,
                    Status = c.Status.ToString(),
                    IssueDate = c.IssueDate,
                    CVV = "Hidden"
                })
                .ToListAsync();

            return Ok(cards);
        }

        // GET: api/Cards/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCard(int id)
        {
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var card = await _context.Cards
                .Include(c => c.Account)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (card == null)
            {
                return NotFound();
            }

            if (!loggedInUser.IsAdministrator && card.Account.UserId != loggedInUser.Id)
            {
                return Forbid();
            }

            var cardDto = new CardsDto
            {
                CardNumber = card.CardNumber,
                CardType = card.CardType.ToString(),
                ExpiryDate = card.ExpiryDate,
                Status = card.Status.ToString(),
                IssueDate = card.IssueDate,
                CVV = "Hidden" //??? is this correct?
            };

            return Ok(cardDto);
        }

        // POST: api/Cards
        [HttpPost]
        public async Task<IActionResult> CreateCard([FromBody] CardsDto dto)
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

            // Get the first account associated with the logged-in user (you can modify this logic as needed)
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == loggedInUser.Id);
            if (account == null)
            {
                return BadRequest("No account associated with this user.");
            }

            if (_context.Cards.Any(c => c.CardNumber == dto.CardNumber))
            {
                return Conflict("Card with the same number already exists.");
            }

            var card = new Card
            {
                CardNumber = dto.CardNumber,
                CardType = (CardType)Enum.Parse(typeof(CardType), dto.CardType, true),
                ExpiryDate = dto.ExpiryDate,
                CVV = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.CVV),
                IssueDate = dto.IssueDate,
                Status = (Status)Enum.Parse(typeof(Status), dto.Status, true),
                AccountId = account.Id // Automatically assign the card to the user's account
            };

            _context.Cards.Add(card);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCard), new { id = card.Id }, dto);
        }

        // PUT: api/Cards/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCard(int id, [FromBody] CardsDto dto)
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

            var card = await _context.Cards.Include(c => c.Account).Where(c => c.Id == id).FirstOrDefaultAsync();
            if (card == null)
            {
                return NotFound();
            }

            // Non-admin users can only edit cards from their own accounts
            if (!loggedInUser.IsAdministrator && card.Account.UserId != loggedInUser.Id)
            {
                return Forbid();
            }

            card.CardNumber = dto.CardNumber;
            card.CardType = (CardType)Enum.Parse(typeof(CardType), dto.CardType, true);
            card.ExpiryDate = dto.ExpiryDate;
            card.Status = (Status)Enum.Parse(typeof(Status), dto.Status, true);
            card.CVV = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.CVV);
            card.IssueDate = dto.IssueDate;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardExists(id))
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

        // DELETE: api/Cards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCard(int id)
        {
            var loggedInUser = await GetLoggedInUser();
            if (loggedInUser == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var card = await _context.Cards.Include(c => c.Account).Where(c => c.Id == id).FirstOrDefaultAsync();
            if (card == null)
            {
                return NotFound();
            }

            // Non-admin users can only delete cards from their own accounts
            if (!loggedInUser.IsAdministrator && card.Account.UserId != loggedInUser.Id)
            {
                return Forbid();
            }

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CardExists(int id)
        {
            return _context.Cards.Any(e => e.Id == id);
        }
    }
}
