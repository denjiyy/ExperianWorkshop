namespace BankManagementSystem.DataProcessor
{
    public class TransactionsDto
    {
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string TransactionDescription { get; set; }
        public string TransactionStatus { get; set; }
        public string CurrencyType { get; set; }
        public DateOnly Timestamp { get; set; }
    }
}
