using BankManagementSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankManagementSystem.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public int AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }
    }
}
