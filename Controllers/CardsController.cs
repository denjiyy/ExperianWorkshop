using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BankManagementSystem.DataProcessor.Import;
using BankManagementSystem.Models;
using BankManagementSystem.Models.Enums;
using BCrypt.Net;
using System.Security.Claims;

namespace BankManagementSystem.wwwroot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // Ensure JWT is required for access to this controller
    public class CardsController : ControllerBase
    {
        private readonly BankSystemContext _context;

        public CardsController(BankSystemContext context)
        {
            _context = context;
        }

        // GET: api/Cards
        [HttpGet]
        public async Task<IActionResult> GetCards()
        {
            var cards = await _context.Cards
                .Select(c => new CardsDto
                {
                    CardNumber = c.CardNumber,  // Only show masked or hashed numbers for security reasons
                    CardType = c.CardType.ToString(),
                    ExpiryDate = c.ExpiryDate,
                    Status = c.Status.ToString(),
                    IssueDate = c.IssueDate,
                    CVV = "Hidden" // Never return sensitive information like CVV
                })
                .ToListAsync();

            return Ok(cards);
        }

        // GET: api/Cards/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCard(int id)
        {
            var card = await _context.Cards
                .Where(c => c.Id == id)
                .Select(c => new CardsDto
                {
                    CardNumber = c.CardNumber,
                    CardType = c.CardType.ToString(),
                    ExpiryDate = c.ExpiryDate,
                    Status = c.Status.ToString(),
                    IssueDate = c.IssueDate,
                    CVV = "Hidden" // Again, never expose CVV
                })
                .FirstOrDefaultAsync();

            if (card == null)
            {
                return NotFound();
            }

            return Ok(card);
        }

        // POST: api/Cards
        [HttpPost]
        public async Task<IActionResult> CreateCard([FromBody] CardsDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Extract UserId from the JWT token
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("UserId is missing from the token.");
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
                UserId = int.Parse(userId)
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

            if (id <= 0)
            {
                return BadRequest("Invalid card ID.");
            }

            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("UserId is missing from the token.");
            }

            card.CardNumber = dto.CardNumber;
            card.CardType = (CardType)Enum.Parse(typeof(CardType), dto.CardType, true);
            card.ExpiryDate = dto.ExpiryDate;
            card.Status = (Status)Enum.Parse(typeof(Status), dto.Status, true);
            card.CVV = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.CVV);
            card.IssueDate = dto.IssueDate;
            card.UserId = int.Parse(userId);

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
            if (id <= 0)
            {
                return BadRequest("Invalid card ID.");
            }

            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
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
