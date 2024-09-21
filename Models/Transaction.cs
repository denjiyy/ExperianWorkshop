using BankManagementSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankManagementSystem.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        
        public TransactionType TransactionType { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Recipient { get; set; }

        [MaxLength(80)]
        public string TransactionDescription { get; set; }

        [Required]
        public TransactionStatus TransactionStatus { get; set; }

        [Required]
        public CurrencyType CurrencyType { get; set; }

        [Required]
        public DateOnly Timestamp { get; set; }

        [Required]
        public int AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }
    }
}
