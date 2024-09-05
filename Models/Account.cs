using BankManagementSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankManagementSystem.Models
{
    public class Account
    {
        public Account()
        {
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public AccountType AccountType { get; set; }

        [Required]
        public decimal Balance { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
