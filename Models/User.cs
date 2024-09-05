using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankManagementSystem.Models
{
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public abstract class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MaxLength(30)]
        public string Email { get; set; }

        private string hashedPassword { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string Password
        {
            get
            {
                return hashedPassword;
            }
            set
            {
                hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(value);
            }
        }

        public bool IsAdministrator { get; set; }
    }
}
