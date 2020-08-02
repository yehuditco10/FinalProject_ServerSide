using Account.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Account.Data
{
    public class AccountContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Entities.Account> Accounts { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<EmailVerification> EmailVerificationS { get; set; }
        public AccountContext(DbContextOptions<AccountContext> options)
  : base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.AppSettings["AccountConnection"]);
                base.OnConfiguring(optionsBuilder);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Entities.Account>().ToTable("Account");
            modelBuilder.Entity<Operation>().ToTable("Operation"); 
            modelBuilder.Entity<EmailVerification>().ToTable("EmailVerification");
            //Customer
            modelBuilder.Entity<Customer>()
                               .Property(customer => customer.Id);
            modelBuilder.Entity<Customer>()
                              .Property(customer => customer.FirstName);
            modelBuilder.Entity<Customer>()
                .Property(customer => customer.LastName);
            modelBuilder.Entity<Customer>()
                  .HasIndex(customer => customer.Email)
                  .IsUnique();
            modelBuilder.Entity<Customer>()
              .Property(customer => customer.PasswordHash);
            modelBuilder.Entity<Customer>()
              .Property(customer => customer.PasswordSalt);
            //Account
            modelBuilder.Entity<Entities.Account>()
                .Property(account => account.Id)
           .IsRequired(); ;
            modelBuilder.Entity<Entities.Account>()
               .Property(account => account.CustomerId)
           .IsRequired(); ;
            modelBuilder.Entity<Entities.Account>()
                  .Property(account => account.Opendate)
                  .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Entities.Account>()
           .Property(account => account.Balance)
           .HasDefaultValue(1000);

            //Operation
            modelBuilder.Entity<Operation>()
                               .Property(operation => operation.Id);
            //EmailVerification
            modelBuilder.Entity<EmailVerification>()
                            .Property(emailVerification => emailVerification.Id);
            modelBuilder.Entity<EmailVerification>()
                  .HasIndex(emailVerification => emailVerification.Email)
                  .IsUnique();
            modelBuilder.Entity<EmailVerification>()
           .Property(emailVerification => emailVerification.VerificationCode)
       .IsRequired(); ;
            modelBuilder.Entity<EmailVerification>()
                  .Property(emailVerification => emailVerification.ExpirationTime)
                  .HasDefaultValueSql("getdate()")
                  .IsRequired();

        }
    }
}
