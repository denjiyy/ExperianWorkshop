namespace BankManagementSystem.DataProcessor
{
    public class PaymentsDto
    {
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyType { get; set; }
        public decimal Fee { get; set; }
        public DateOnly Date { get; set; }
    }
}
