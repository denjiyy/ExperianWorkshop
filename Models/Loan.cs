using BankManagementSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankManagementSystem.Models
{
    public class Loan
    {
        public Loan()
        {
            Payments = new HashSet<Payment>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public LoanType LoanType { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public decimal InterestRate { get; set; }

        [Required]
        public CurrencyType CurrencyType { get; set; }

        [Required]
        public DateOnly DateApproved { get; set; }

        [Required]
        public DateOnly StartDate { get; set; }

        [Required]
        public DateOnly NextPaymentDate { get; set; }

        [Required]
        public DateOnly EndDate { get; set; }

        [NotMapped]
        public decimal TotalPaid { get; set; }
 
        [Required]
        public LoanStatus LoanStatus { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public ICollection<Payment> Payments { get; set; }
    }
}
