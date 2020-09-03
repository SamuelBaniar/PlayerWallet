using Microsoft.EntityFrameworkCore;
using PlayerWallet.Data.Models;
using System;

namespace PlayerWallet.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletTransaction> Transactions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Wallet>().ToTable("Wallets");
            builder.Entity<Wallet>().HasKey(w => w.Guid);
            builder.Entity<Wallet>().Property(w => w.Guid).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Wallet>().HasMany(w => w.Transactions).WithOne(t => t.Wallet).HasForeignKey(t => t.WalletGuid);

            builder.Entity<WalletTransaction>().ToTable("Transactions");
            builder.Entity<WalletTransaction>().HasKey(t => t.Guid);
            builder.Entity<WalletTransaction>().Property(t => t.Guid).IsRequired();
            builder.Entity<WalletTransaction>().Property(t => t.TransactionType).IsRequired(); 
            builder.Entity<WalletTransaction>().Property(t => t.Amount).IsRequired();

            SeedFakeData(builder);
        }

        private void SeedFakeData(ModelBuilder builder)
        {
            var fakeWallets = new Wallet[]
            {
                new Wallet { Guid = Guid.Parse("00000000-0000-0000-0000-000000000001"), Balance = 0},
                new Wallet { Guid = Guid.Parse("00000000-0000-0000-0000-000000000002"), Balance = 100 }
            };

            builder.Entity<Wallet>().HasData(fakeWallets);
        }
    }
}
