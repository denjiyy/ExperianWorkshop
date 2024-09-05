using System.ComponentModel.DataAnnotations;

namespace BankManagementSystem.Models
{
    public class Customer : User
    {
        public Customer()
        {
            Accounts = new HashSet<Account>();
            Cards = new HashSet<Card>();
            Loans = new HashSet<Loan>();
        }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(30)]
        public string Address { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        public ICollection<Account> Accounts { get; set; }

        public ICollection<Loan> Loans { get; set; }

        public ICollection<Card> Cards { get; set; }
    }
}
