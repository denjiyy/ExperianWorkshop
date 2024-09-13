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
        public DateTime IssueDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
