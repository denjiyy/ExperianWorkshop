using System.Transactions;

namespace BankManagementSystem.DataProcessor
{
    public class AccountsDto
    {
        public AccountsDto()
        {
            Transactions = new HashSet<TransactionsDto>();
        }

        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public decimal OverdraftLimit { get; set; }
        public string CurrencyType { get; set; }
        public DateOnly DateOpened { get; set; }
        public string Status { get; set; }
        public HashSet<TransactionsDto> Transactions { get; set; }
        public int UserId { get; set; }
    }
}
