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
        public decimal OverdraftLimit { get; set; }

        [Required]
        public CurrencyType CurrencyType { get; set; }

        [Required]
        public DateOnly DateOpened { get; set; }

        //public decimal InterestRate { get; set; } : if any of the accounts have interest accrual (savings account for example)

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
