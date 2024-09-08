using Microsoft.EntityFrameworkCore;

namespace BankManagementSystem.Models
{
    public class BankingSystemContext : DbContext
    {
        private const string connectionString = @"Server=.;Database=BankingManagementSystem;Integrated Security=True;Encrypt=False;";

        public BankingSystemContext()
        {
            
        }

        public BankingSystemContext(DbContextOptions options)
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
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
