using BankManagementSystem.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BankManagementSystem.Models
{
    public class BankSystemContext : DbContext
    {
        private const string BankManagementSystemContextConnection = @"Server=.;Database=BankingManagementSystem;Integrated Security=True;Encrypt=False;";

        public BankSystemContext()
        {

        }

        public BankSystemContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(BankManagementSystemContextConnection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
        new User
        {
            Id = 1,
            FullName = "John Doe",
            Username = "johndoe",
            Email = "john.doe@example.com",
            Password = "Password123!", // This will be hashed by EF
            IsAdministrator = false,
            PhoneNumber = "123-456-7890",
            Address = "123 Elm Street",
            CreditScore = 700,
            Status = Status.Active,
            DateOfBirth = new DateTime(1990, 1, 1)
        },
        new User
        {
            Id = 2,
            FullName = "Jane Smith",
            Username = "janesmith",
            Email = "jane.smith@example.com",
            Password = "Password123!", // This will be hashed by EF
            IsAdministrator = true,
            PhoneNumber = "987-654-3210",
            Address = "456 Oak Avenue",
            CreditScore = 750,
            Status = Status.Active,
            DateOfBirth = new DateTime(1985, 5, 15)
        }
    );
        }
    }
}