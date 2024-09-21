using BankManagementSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankManagementSystem.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public CurrencyType CurrencyType { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public int LoanId { get; set; }
        [ForeignKey(nameof(LoanId))]
        public Loan Loan { get; set; }
    }
}
