using BankManagementSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankManagementSystem.Models
{
    public class Card
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public CardType CardType { get; set; }

        private string hashedCardNumber { get; set; }

        [Required]
        public string CardNumber
        {
            get
            {
                return hashedCardNumber;
            }
            set
            {
                hashedCardNumber = BCrypt.Net.BCrypt.EnhancedHashPassword(value);
            }
        }

        [Required]
        public string CVV { get; set; }

        [Required]
        public DateOnly IssueDate { get; set; }

        [Required]
        public DateOnly ExpiryDate { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
