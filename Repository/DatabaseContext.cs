using Microsoft.EntityFrameworkCore;
using SimpLedger.Repository.Models.Account;
using SimpLedger.Repository.Models.Auth;
using SimpLedger.Repository.Models.Enterprise;
using SimpLedger.Repository.Models.Inventory;
using SimpLedger.Repository.Models.Sales;
using SimpLedger.Repository.Models.Verification;

namespace SimpLedger.Repository
{
    public class DatabaseContext(DbContextOptions<DatabaseContext> context): DbContext(context)
    {

        public DbSet<Sale> Sales { get; set; }
        public DbSet<SalesItem> SalesItem { get; set; }

        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Product> Product { get; set; }

        public DbSet<Branch> Branch { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Employee> Employee { get; set; }

        public DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<AccountType> AccountType { get; set; }
        public DbSet<ExpiredToken> ExpiredTokens { get; set; }

        public DbSet<VerificationCode> VerificationCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Setting Primary Key
            modelBuilder.Entity<Sale>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<SalesItem>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<Inventory>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<Product>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<Branch>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<Company>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<Employee>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<UserAccount>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<AccountType>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<ExpiredToken>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<VerificationCode>()
                .HasKey(k => k.Id);

            #endregion

            #region Setting RelationShip

            modelBuilder.Entity<Sale>()
                .HasOne(b => b.Branch)
                .WithMany(s => s.Sales)
                .HasForeignKey(f => f.Branch_Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SalesItem>()
                .HasOne(s => s.Sales)
                .WithMany(s => s.SalesItems)
                .HasForeignKey(s => s.Sales_Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SalesItem>()
                .HasOne(i => i.Inventory)
                .WithMany(s => s.SalesItems)
                .HasForeignKey(i => i.Inventory_Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Inventory>()
                .HasOne(b => b.Branch)
                .WithMany(i => i.Inventories)
                .HasForeignKey(b => b.Branch_Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Inventory>()
                .HasOne(p => p.Product)
                .WithMany(i => i.Inventories)
                .HasForeignKey(p => p.Product_Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Branch>()
                .HasOne(c => c.Company)
                .WithMany(b => b.Branches)
                .HasForeignKey(c => c.Company_Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Company>()
                .HasOne(u => u.UserAccount)
                .WithMany(c => c.Company)
                .HasForeignKey(u => u.UserAccount_Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.UserAccount)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.UserAccount_Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Branch)
                .WithMany(b => b.Employees)
                .HasForeignKey(e => e.Branch_Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserAccount>()
                .HasOne(u => u.AccountType)
                .WithMany(a => a.UserAccounts)
                .HasForeignKey(u => u.AccountType_Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<VerificationCode>()
                .HasOne(u => u.UserAccount)
                .WithMany(v => v.VerificationCodes)
                .HasForeignKey(u => u.UserAccount_Id)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion
        }

    }
}
