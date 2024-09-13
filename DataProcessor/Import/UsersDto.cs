using System.ComponentModel.DataAnnotations;

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
        
        public int Id { get; set; }
        
        public string FullName { get; set; }
        public string Username { get; set; }

        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [MaxLength(30)]
        public string NewPassword { get; set; }
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
