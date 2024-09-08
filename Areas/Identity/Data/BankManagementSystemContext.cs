using Microsoft.EntityFrameworkCore;

namespace BankManagementSystem.Models
{
    public class BankManagementSystemContext : DbContext
    {
        private const string BankManagementSystemContextConnection = @"Server=.;Database=BankingManagementSystem;Integrated Security=True;Encrypt=False;";

        public BankManagementSystemContext()
        {

        }

        public BankManagementSystemContext(DbContextOptions options)
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
        }
    }
}