namespace BankManagementSystem.Models
{
    public class DepositCalculationRequest
    {
        // deposit
        public decimal Amount { get; set; }

        // annual interest rate
        public decimal Rate { get; set; }

        // time in years for the deposit
        public int TimePeriodInYears { get; set; }

        // default compounding annually
        public int TimesCompounded { get; set; } = 1;
    }
}
