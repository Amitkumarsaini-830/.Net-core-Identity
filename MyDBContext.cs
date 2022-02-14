using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace com.aadviktech.IMS.DB
{
    public class MyDBContext : IdentityDbContext<MyAppUser, AppRoles, int>
    {
        //private HttpContext ctx;

        //public MyDBContext() { }
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {

        }

        #region DataSets
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<City> CityMaster { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<TaxRate> TaxRates { get; set; }
        public virtual DbSet<TaxGroup> TaxGroups { get; set; }
        public virtual DbSet<UnitName> UnitNames { get; set; }
        public virtual DbSet<UCRate> UCRates { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionLog> TransactionLogs { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<ExpenseCategory> ExpenseCategorys { get; set; }
       // public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
       // public virtual DbSet<IncomeCategory> IncomeCategories { get; set; }
       // public virtual DbSet<IncomeSubCategory> IncomeSubCategories { get; set; }
       // public virtual DbSet<IncomeItem> IncomeItems { get; set; }
      //  public virtual DbSet<IncomeTransaction> IncomeTransactions { get; set; }
        public virtual DbSet<ItemCategory> ItemCategories { get; set; }
        public virtual DbSet<ItemSubCategory> ItemSubCategories { get; set; }
        public virtual DbSet<TransactionItemDetail> TransactionItemsDetails { get; set; }
        public virtual DbSet<LedgerGroup> LedgerGroups { get; set; }
        public virtual DbSet<LedgerSubGroup> LedgerSubGroups { get; set; }
        public virtual DbSet<Invoice_no> Invoice_Nos { get; set; }
        public virtual DbSet<FinanceInfo> FinanceInfos { get; set; }
        public virtual DbSet<Shopes> Shopes { get; set; }
        public virtual DbSet<WalletInfo> WalletInfos { get; set; }
        public virtual DbSet<BankTrnxInfo> BankTrnxInfos { get; set; }
        public virtual DbSet<CashTransaction> CashTransactions{get;set;}
        public virtual DbSet<WalletTransactions> WalletTransactions {get;set;}
        public virtual DbSet<Cash> Cashes { get; set; }
        public virtual DbSet<PayemtModeOption> PayemtModeOptions { get; set; }
        public virtual DbSet<Description> Descriptions { get; set; }
        public virtual DbSet<ExtraCharge> ExtraCharges { get; set; }
        public virtual DbSet<Ledger> Ledgers { get; set; }

        #endregion

        #region Relationships
        //Seeding
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppRoles>().HasData(
                new AppRoles() { Id = 1, ConcurrencyStamp = "840b47ec-7015-42ce-8a0b-554eae466334", Name = "admin", NormalizedName = "ADMIN" },
                new AppRoles() { Id = 2, ConcurrencyStamp = "4776a1b2-dbe4-4056-82ec-8bed211d1454", Name = "accountant", NormalizedName = "ACCOUNTANT" },
                new AppRoles() { Id = 3, ConcurrencyStamp = "7dd6a2df-3a49-4b8d-bbbd-0de2146b92ba", Name = "client", NormalizedName = "CLIENT" },
                new AppRoles() { Id = 4, ConcurrencyStamp = "7dd6a2df-3a49-4b8d-bbbd-0de2146b92bb", Name = "party", NormalizedName = "PARTY" }
            );

            builder.Entity<MyAppUser>().HasData(
                new MyAppUser() { Id = 1, ConcurrencyStamp = "2f2fd975-299c-4739-87fe-e7eeabf73677", Active = true, Deleted = false, Email = "ims.system@aadviktech.com", EmailConfirmed = true, FullName = "Admin", NormalizedEmail = "IMS.SYSTEM@AADVIKTECH.COM", NormalizedUserName = "ADMIN", UserName = "admin", PasswordHash = "test@123", SecurityStamp = "6Y5B4K4NQAEHHKEWUD5C5M5Z6B6L75XZ" }
                );

            builder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int>() { UserId = 1, RoleId = 1 }
                );

        }

        #endregion
    }
}
