namespace BankManagementSystem.DataProcessor
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
        public DateOnly DateApproved { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly NextPaymentDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string LoanStatus { get; set; }
        public HashSet<PaymentsDto> Payments { get; set; }
    }
}
