using BankManagementSystem.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankManagementSystem.Models
{
    [Index("CardNumber", IsUnique = true)]
    public class Card
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public CardType CardType { get; set; }

        [Required]
        public string CardNumber { get; set; }

        [Required]
        public string CVV { get; set; }

        [Required]
        public DateOnly IssueDate { get; set; }

        [Required]
        public DateOnly ExpiryDate { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public int AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }
    }
}
