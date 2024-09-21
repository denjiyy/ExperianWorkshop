using Microsoft.AspNetCore.Mvc;
using BankManagementSystem.Models;
using BankManagementSystem.Models.Enums;

namespace BankManagementSystem.Controllers
{
    /// <summary>
    /// примерна формула за сложна лихва: A = P(1 + r/n)^(nt)
    /// където:
    /// P = начален депозит
    /// r = годишен лихвен % (decimal)
    /// n = колко пъти тази лихва се начислява на година
    /// t = време, за което са инвестирани парите, в години
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DepositCalculatorController : ControllerBase
    {
        private readonly BankSystemContext _context;

        public DepositCalculatorController(BankSystemContext context)
        {
            _context = context;
        }

        [HttpPost("calculate")]
        public IActionResult CalculateDeposit([FromBody] DepositCalculationRequest request)
        {
            if (request.Amount <= 0 || request.Rate <= 0 || request.TimePeriodInYears <= 0)
            {
                return BadRequest("Invalid input values. Amount, Rate, and TimePeriodInYears should be greater than zero.");
            }

            decimal principal = request.Amount;
            decimal rate = request.Rate / 100;
            int timesCompounded = request.TimesCompounded;
            int time = request.TimePeriodInYears;

            decimal totalAmount = principal * (decimal)Math.Pow((double)(1 + rate / timesCompounded), timesCompounded * time);

            return Ok(new
            {
                InitialAmount = principal,
                InterestRate = request.Rate,
                TimePeriodInYears = time,
                TotalAmount = totalAmount
            });
        }
    }
}
