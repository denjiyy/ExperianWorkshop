using BankManagementSystem.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankManagementSystem.Models
{
    [Index(nameof(AccountNumber), IsUnique = true)]
    public class Account
    {
        public Account()
        {
            Cards = new HashSet<Card>();
            Transactions = new HashSet<Transaction>();
            DateOpened = DateOnly.FromDateTime(DateTime.UtcNow);
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string AccountNumber { get; set; }

        [Required]
        public AccountType AccountType { get; set; }

        [Required]
        public decimal Balance { get; set; }

        [Required]
        public CurrencyType CurrencyType { get; set; }

        [Required]
        public DateOnly DateOpened { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public ICollection<Card> Cards { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
