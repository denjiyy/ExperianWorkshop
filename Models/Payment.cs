using BankManagementSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankManagementSystem.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public CurrencyType CurrencyType { get; set; }

        //to track any fees associated with the payment
        public decimal Fee { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int LoanId { get; set; }
        [ForeignKey(nameof(LoanId))]
        public Loan Loan { get; set; }
    }
}
