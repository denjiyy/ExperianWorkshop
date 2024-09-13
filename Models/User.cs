using BankManagementSystem.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankManagementSystem.Models
{
    [Index("Email", IsUnique = true)]
    [Index("Username", IsUnique = true)]
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

        [Required]
        [MaxLength(30)]
        public string Password { get; set; }

        [Required]
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
        public DateTime DateOfBirth { get; set; }

        public ICollection<Account> Accounts { get; set; }

        public ICollection<Loan> Loans { get; set; }

        public ICollection<Card> Cards { get; set; }
    }
}
