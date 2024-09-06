using BankManagementSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankManagementSystem.Models
{
    public class User
    {
        public User()
        {
            Accounts = new HashSet<Account>();
            Cards = new HashSet<Card>();
            Loans = new HashSet<Loan>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MaxLength(30)]
        public string Email { get; set; }

        private string hashedPassword { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string Password
        {
            get
            {
                return hashedPassword;
            }
            set
            {
                hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(value);
            }
        }

        public bool IsAdministrator { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(30)]
        public string Address { get; set; }

        [Required]
        public int CreditScore { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        public ICollection<Account> Accounts { get; set; }

        public ICollection<Loan> Loans { get; set; }

        public ICollection<Card> Cards { get; set; }
    }
}
