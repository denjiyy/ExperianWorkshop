namespace BankManagementSystem.Models
{
    public class DepositCalculationRequest
    {
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
        public int TimePeriodInYears { get; set; }
        public int TimesCompounded { get; set; } = 1;
    }
}
