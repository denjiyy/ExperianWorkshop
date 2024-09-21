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
            modelBuilder.Entity<Loan>(entity =>
            {
                entity.Property(e => e.Amount)
                      .HasPrecision(18, 2);
                entity.Property(e => e.InterestRate)
                      .HasPrecision(18, 2);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Amount)
                      .HasPrecision(18, 2);
            });

            modelBuilder.Entity<Account>()
                .Property(a => a.Balance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}