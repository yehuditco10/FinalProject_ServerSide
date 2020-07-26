using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Transaction.Data
{
   public class TransactionContext:DbContext
    {
        public DbSet<Entities.Transaction> Transactions { get; set; }
        public TransactionContext(DbContextOptions<TransactionContext> options)
: base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.AppSettings["TransactionConnection"]);
                base.OnConfiguring(optionsBuilder);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
       
            modelBuilder.Entity<Entities.Transaction>().ToTable("Transaction");
            //Transaction
            modelBuilder.Entity<Entities.Transaction>()
                              .HasIndex(transaction => transaction.Id)
                  .IsUnique();
            modelBuilder.Entity<Entities.Transaction>()
                          .Property(transaction => transaction.FromAccountId)
                          .IsRequired(); ; 
            modelBuilder.Entity<Entities.Transaction>()
                          .Property(transaction => transaction.ToAccountId)
                          .IsRequired(); ;
            modelBuilder.Entity<Entities.Transaction>()
                          .Property(transaction => transaction.Amount);
            modelBuilder.Entity<Entities.Transaction>()
                  .Property(transaction => transaction.Date);
            modelBuilder.Entity<Entities.Transaction>()
                  .Property(transaction => transaction.Status);
            modelBuilder.Entity<Entities.Transaction>()
                 .Property(transaction => transaction.FailureReason);
        }
    }
}
