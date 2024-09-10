namespace BankManagementSystem.DataProcessor.Import
{
    public class LoansDto
    {
        public LoansDto()
        {
            Payments = new HashSet<PaymentsDto>();    
        }

        public string LoanType { get; set; }
        public decimal Amount { get; set; }
        public decimal InterestRate { get; set; }
        public string CurrencyType { get; set; }
        public DateTime DateApproved { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime NextPaymentDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LoanStatus { get; set; }
        public HashSet<PaymentsDto> Payments { get; set; }
    }
}
