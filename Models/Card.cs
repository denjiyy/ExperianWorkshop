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

        public string hashedCardNumber { get; set; }

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
        public DateOnly ExpiryDate { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }
    }
}
