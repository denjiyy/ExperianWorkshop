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

            //modelBuilder.HasSequence<int>("UserSequence")
            //    .StartsAt(1)
            //    .IncrementsBy(1);

            //modelBuilder.Entity<User>()
            //    .Property(u => u.Id)
            //    .HasDefaultValueSql("NEXT VALUE FOR UserSequence");

            //modelBuilder.HasSequence<int>("AccountSequence")
            //    .StartsAt(1)
            //    .IncrementsBy(1);

            //modelBuilder.Entity<Account>()
            //    .Property(a => a.Id)
            //    .HasDefaultValueSql("NEXT VALUE FOR AccountSequence");

            //modelBuilder.HasSequence<int>("CardSequence")
            //    .StartsAt(1)
            //    .IncrementsBy(1);

            //modelBuilder.Entity<Card>()
            //    .Property(c => c.Id)
            //    .HasDefaultValueSql("NEXT VALUE FOR CardSequence");

            //modelBuilder.HasSequence<int>("LoanSequence")
            //    .StartsAt(1)
            //    .IncrementsBy(1);

            //modelBuilder.Entity<Loan>()
            //    .Property(l => l.Id)
            //    .HasDefaultValueSql("NEXT VALUE FOR LoanSequence");

            //modelBuilder.HasSequence<int>("TransactionSequence")
            //    .StartsAt(1)
            //    .IncrementsBy(1);

            //modelBuilder.Entity<Transaction>()
            //    .Property(t => t.Id)
            //    .HasDefaultValueSql("NEXT VALUE FOR TransactionSequence");

            //modelBuilder.HasSequence<int>("PaymentSequence")
            //    .StartsAt(1)
            //    .IncrementsBy(1);

            //modelBuilder.Entity<Payment>()
            //    .Property(p => p.Id)
            //    .HasDefaultValueSql("NEXT VALUE FOR PaymentSequence");

            base.OnModelCreating(modelBuilder);
        }
    }
}