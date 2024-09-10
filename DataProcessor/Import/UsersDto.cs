namespace BankManagementSystem.DataProcessor.Import
{
    public class UsersDto
    {
        public UsersDto()
        {
            Cards = new HashSet<CardsDto>();
            Loans = new HashSet<LoansDto>();
            Accounts = new HashSet<AccountsDto>();
        }

        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdministrator { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int CreditScore { get; set; }
        public string Status { get; set; }
        public DateTime DateOfBirth { get; set; }
        public HashSet<AccountsDto> Accounts { get; set; }
        public HashSet<LoansDto> Loans { get; set; }
        public HashSet<CardsDto> Cards { get; set; }
    }
}
