namespace BankManagementSystem.DataProcessor.Import
{
    public class PaymentsDto
    {
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyType { get; set; }
        public decimal Fee { get; set; }
        public DateTime Date { get; set; }
    }
}
