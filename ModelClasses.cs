using com.aadviktech.IMS.Constant.AllEnum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace com.aadviktech.IMS.DB
{
    #region Identity Classes
    public partial class MyAppUser : IdentityUser<int>
    {
        public string FullName { get; set; }
        public bool Active { get; set; } = true;
        public bool Deleted { get; set; } = false;
        public string CompanyName { get; set; }
        public decimal Balance { get; set; }
        public decimal ClosingBalance { get; set; }
        public int ToPayToRecive { get; set; }
        public string TINNO { get; set; }
        public string GSTINNO { get; set; }
        public int GSTType { get; set; }
        public string Address { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime DateIngoreTill { get; set; }
        public int StateId { get; set; }
        public int GroupId { get; set; }
        public int AccountantId { get; set; }
        public int ClientId { get; set; }
        public int RoleId { get; set; }
        public int BusinessType { get; set; }
        public string BusinessCategory { get; set; }
        public string Descriptions { get; set; }
        public int CityId { get; set; }
        public int PinCode { get; set; }
        public int LedegerGruop { get; set; }
        public int LedgerSubGroup { get; set; }
        public string ProfilePicture { get; set; }
        [NotMapped]
        public IFormFile ProfileImage { get; set; }
        public decimal IGST { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal RoundOff { get; set; }

    }

    public class AppRoles : IdentityRole<int>
    {
        public AppRoles()
        {
        }
        public AppRoles(string roleName) : base(roleName)
        {
        }
    }

    public class CustomPasswordHasher : IPasswordHasher<MyAppUser>      //new remove password hashing
    {
        public string HashPassword(MyAppUser user, string password)
        {
            return password;
        }

        public PasswordVerificationResult VerifyHashedPassword(MyAppUser user, string hashedPassword, string providedPassword)
        {
            return hashedPassword.Equals(providedPassword) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }

    //public class RoleInitializer
    //{
    //    public static async Task Initialize(RoleManager<AppRoles> roleManager)
    //    {
    //        if (!await roleManager.RoleExistsAsync("Admin"))
    //        {
    //            var role = new AppRoles("Admin");
    //            await roleManager.CreateAsync(role);
    //        }
    //        if (!await roleManager.RoleExistsAsync("User"))
    //        {
    //            AppRoles role = new AppRoles("User");
    //            await roleManager.CreateAsync(role);
    //        }
    //    }
    //}
    #endregion

    public partial class MyAppUser
    {
        [NotMapped]
        public string CnfPassword { get; set; }
        [NotMapped]
        public string accountantName { get; set; }
        [NotMapped]
        public string ClientName { get; set; }
        [NotMapped]
        public int TatalAmount { get; set; }
    }
    public class LedgerGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }
        public int CreatedBy { get; set; }
    }
    public class LedgerSubGroup
    {
        public int Id { get; set; }
        public int ledgerCategoryId { get; set; }
        public string LedgerNameCat { get; set; }
        public string NameSubCat { get; set; }
        public int CreatedBy { get; set; }
        public bool Deleted { get; set; }
    }
    public class Sp_ClientTransaction
    {
        public int Userid { get; set; }
        public decimal Balance { get; set; }
        public decimal Sales { get; set; }
        public decimal puchase { get; set; }
        public string CompanyName { get; set; }

    }

    public class Sp_ProfitLoss
    {
        public decimal SALE { get; set; }
        public decimal SaleReturn { get; set; }
        public decimal PURCHASE { get; set; }
        public decimal PurchaseReturn { get; set; }
        public decimal DisIn { get; set; }
        public decimal DisOut { get; set; }
        public decimal Expenses { get; set; }
        public decimal Income { get; set; }
        public decimal TaxPayble { get; set; }
        public decimal TaxRecive { get; set; }
        public decimal OPeningStock { get; set; }
        public decimal ClosingStock { get; set; }
        public decimal GrossProfit { get; set; }
        public decimal NetProfit { get; set; }
    }

    public class SP_BalanceSheet
    {
        public decimal CashIn { get; set; }
        public decimal BankBal { get; set; }
        public decimal TaxRecieve { get; set; }
        public decimal TaxPayble { get; set; }
        public decimal SundryDebt { get; set; }
        public decimal SundryCredit { get; set; }
        public decimal ClosingStock { get; set; }
        public decimal NetProfit { get; set; }
        public decimal TatalAssets { get; set; }
        public decimal Tataliability { get; set; }
        public decimal OpeningBankBal { get; set; }
    }

    public class Sp_TrialBalance
    {

        public decimal CGSTcr { get; set; }
        public decimal CGSTdr { get; set; }
        public decimal ClosingCGST { get; set; }
        public decimal SGSTcr { get; set; }
        public decimal SGSTdr { get; set; }
        public decimal IGSTcr { get; set; }
        public decimal IGSTdr { get; set; }
        public decimal ClosingSGST { get; set; }
        public decimal ClosnigIGST { get; set; }
        public decimal Debtors { get; set; }
        public decimal Creditors { get; set; }
        public decimal Sales { get; set; }
        public decimal Purchase { get; set; }
        public decimal BankBalance { get; set; }
        public decimal Wallet { get; set; }

    }
    public class SP_LedgerList 
    {
       public int Id { get; set; }
        public string Names { get; set; }
        public int IdTypes { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }
        public int CreatedBy { get; set; }

    }

    public class State
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool IsActive { get; set; }
        public int StateCode { get; set; }

    }
    public class City
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }


    public class ItemCategory
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public string Name { get; set; }
        public int ItemType { get; set; }
        public int LedgerGroup { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
    }
    public class ItemSubCategory
    {
        public int Id { get; set; }
        public int Categoryid { get; set; }
        public string CategoryName { get; set; }
        public int CreatedBy { get; set; }
        public string SubCatName { get; set; }
        public int ItemType { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
    }


    public class Item
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public string Name { get; set; }
        public string HSN_SAC { get; set; }
        public decimal SalesPrice { get; set; }
        public bool IsTaxableSP { get; set; }
        public decimal PurchasePrice { get; set; }
        public bool IsTaxablePP { get; set; }
        public int TaxGroup { get; set; }
        public int OpeningStock { get; set; }
        public int ClosingStock { get; set; }
        public decimal PerchasePerUnit { get; set; }
        public int MinStock { get; set; }
        public string ItemLocation { get; set; }
        public int UCRatesId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CategoryId { get; set; }
        public bool Deleted { get; set; }
        public int ItemCategotyId { get; set; }
        public string ItemCAtegoryName { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public bool IsActive { get; set; }
        public bool IsSerialNo { get; set; }
        //  public int Taxability { get; set; }
        public int Taxabilities { get; set; }
        public DateTime gstApplicableFrom { get; set; }
        public DateTime gstApplicableTo { get; set; }
        public bool GSTApplicableDate { get; set; }
        [NotMapped]
        public int IsGSTApplicableDate { get; set; }
        public bool IsSetStandardRate { get; set; }
        public decimal StockValue { get; set; }
        public string ItemPics { get; set; }
        [NotMapped]
        public IFormFile ItemPic { get; set; }

    }

    public class TaxRate
    {
        public int Id { get; set; }
        public string TaxType { get; set; }
        public string TaxName { get; set; }
        public decimal Rate { get; set; }
        public bool Deleted { get; set; }
        public int CreatedBy { get; set; }

    }
    public class TaxGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TaxRateIds { get; set; }
        public bool Deleted { get; set; }
        public int CreatedBy { get; set; }
        [NotMapped]
        public string TaxRatName { get; set; }
    }
    public class UnitName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool Deleted { get; set; }
        public int CreatedBy { get; set; }
    }
    public class UCRate
    {
        public int Id { get; set; }
        public int BaseUnitId { get; set; }
        public int SecondaryUnitId { get; set; }
        public decimal SUQuantity { get; set; }
        public bool Deleted { get; set; }
        public int CreatedBy { get; set; }
        //public bool AlternateUnitIsApplicable { get; set; }
        public int AlternateUnitIsApplicables { get; set; }

    }

    public partial class Transaction
    {
        public long Id { get; set; }
        [NotMapped]
        public string PartyName { get; set; }
        public string Order_Invoice_NoPrifix { get; set; }
        public string OriginalInvoiceNo { get; set; }
        public string Shope_Name { get; set; }
        public string Order_Invoice_No { get; set; }
        public int CreatedBy { get; set; }
        public int TransactionType { get; set; }
        public DateTime Invoice_Date { get; set; }
        public string ReferneceNo { get; set; }
        public DateTime ReferenceData { get; set; }
        public string PO_No { get; set; }
        public DateTime PO_Date { get; set; }
        public string VehicleNo { get; set; }
        public string Code_No { get; set; }
        public int PartyId { get; set; }
        public string ShipongPartyName { get; set; }
        public string ShipongPartyMobile { get; set; }
        public string ShipongPartyEmail { get; set; }
        public string ShipongPartyAddress { get; set; }
        public string ShipongPartyGStNo { get; set; }
        public int ShipongPartyStateId { get; set; }
        public int ShipongPartyCityId { get; set; }
        [NotMapped]
        public int ItemId { get; set; }
        [NotMapped]
        public int Quantity { get; set; }
        [NotMapped]
        public int UnitId { get; set; }
        [NotMapped]
        public decimal PricePerUnit { get; set; }
        [NotMapped]
        public decimal Discount { get; set; }
        [NotMapped]
        public decimal DisAmount { get; set; }
        [NotMapped]
        public decimal TaxRate { get; set; }
        [NotMapped]
        public decimal TaxAmount { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal TaxbleAmt { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        [NotMapped]
        public string PaymentMode { get; set; }
        public string Description { get; set; }
        public bool IsRoundOff { get; set; }
        public decimal RoundOffAmt { get; set; }
        public decimal RecievedAmt { get; set; }
        public decimal RemainingAmt { get; set; }
        public decimal RemainingAmt2 { get; set; }
        public string RefNo { get; set; }
        public int ReturnNo { get; set; }
        public bool Cancelled { get; set; }
        public bool Deleted { get; set; }
        public bool IsCash { get; set; }
        public string CustomerName { get; set; }
        [NotMapped]
        public string PaymentInOutInvoiceNo { get; set; }
        public decimal ExtraChargeAmt { get; set; }
        public decimal ExtraChargeTax { get; set; }
        public string Attachment { get; set; }
        [NotMapped]
        public IFormFile Attachments { get; set; }
        public virtual ICollection<TransactionItemDetail> TransactionItemsDetails { get; set; }
        public virtual ICollection<CashTransaction> CashTransactions { get; set; }
        public virtual ICollection<WalletInfo> WalletInfos { get; set; }
        public virtual ICollection<BankTrnxInfo> BankTrnxInfos { get; set; }
        public virtual ICollection<FinanceInfo> FinanceInfos { get; set; }
        public virtual ICollection<ExtraChargeData> ExtraChargesDada { get; set; }
        [NotMapped]
        public List<DueTrnxInfo> dueTrnxInfos { get; set; }
        public decimal Balance { get; set; }
        public decimal IGSTBalance { get; set; }
        public decimal SGstBalance { get; set; }
        public decimal CGstBalance { get; set; }
        public decimal RoundOffBalance { get; set; }

    }

    public partial class Transaction
    {
        [NotMapped]
        public decimal cashAmt { get; set; }
        [NotMapped]
        public decimal BankAmount { get; set; }
        [NotMapped]
        public string BankName { get; set; }
        [NotMapped]
        public string BankTnxType { get; set; }
        [NotMapped]
        public string Wallet_Name { get; set; }
        [NotMapped]
        public string UPI_Id { get; set; }
        [NotMapped]
        public decimal WaletAmount { get; set; }
        [NotMapped]
        public decimal DownPayment { get; set; }
        [NotMapped]
        public decimal ExtrachargeBalance { get; set; }
        [NotMapped]
        public decimal ExtrachargeTaxableVal { get; set; }
    }

    public class ExtraChargeData
    {
        public long Id { get; set; }
        public long TxnId { get; set; }
        public int TrnxType { get; set; }
        [NotMapped]
        public int IncomeExpenses { get; set; }
        [NotMapped]
        public string Name { get; set; }
        public int ExtraChargeId { get; set; }
        public int ExtrachrgeType { get; set; }
        public string HNSCode { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public decimal TaxableVal { get; set; }
        public decimal Balance { get; set; }
        public bool IsTaxIncluded { get; set; }
        public bool IsTaxable { get; set; }
        [ForeignKey("TxnId")]
        public Transaction Transaction { get; set; }

    }
    public class DueTrnxInfo
    {
        public long Id { get; set; }
        public decimal ReceivedAmt { get; set; }
        public bool IsCheck { get; set; }
    }
    public class TransactionItemDetail
    {
        public long Id { get; set; }
        public long TxnId { get; set; }
        public int ItemId { get; set; }
        public string Serial_No { get; set; }
        public string HNS_Code { get; set; }
        public int Quantity { get; set; }
        public int UnitId { get; set; }
        public decimal PricePerUnit { get; set; }
        public int IsTaxable_SP { get; set; }
        public decimal Discount { get; set; }
        public decimal DisAmount { get; set; }
        public decimal SGSTRate { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTRate { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTRate { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal TaxableValue { get; set; }
        public decimal TotalAmount { get; set; }
        public bool Deleted { get; set; }
        public bool Cancelled { get; set; }
        public decimal TotalTax { get; set; }

        [ForeignKey("TxnId")]
        public Transaction Transaction { get; set; }
        [NotMapped]
        public int TaxRate { get; set; }
        [NotMapped]
        public string Names { get; set; }
    }

    public class TransactionLog
    {
        public long Id { get; set; }
        public long TxnId { get; set; }
        public int PartyId { get; set; }
        public int CreatedBy { get; set; }
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        public int UnitId { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal Discount { get; set; }
        public decimal DisAmount { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime DueOn { get; set; }
        public int StateId { get; set; }
        public string PaymentMode { get; set; }
        public string Description { get; set; }
        public bool IsRoundOff { get; set; }
        public decimal RecievedAmt { get; set; }
        public decimal RemainingAmt { get; set; }
        public string RefNo { get; set; }
        public int ReturnNo { get; set; }
        public int Order_Invoice_No { get; set; }
        public int TransactionType { get; set; }
        public bool Deleted { get; set; }


    }

    public class Payment
    {
        public int Id { get; set; }
        public int PartyId { get; set; }
        public int CreatedBy { get; set; }
        public string PaymentMode { get; set; }
        public string Description { get; set; }
        public string RefNo { get; set; }
        public int Order_Reciept_No { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int PaymentType { get; set; }
        public bool Deleted { get; set; }
    }

    public class ExpenseCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }
        public int CreatedBy { get; set; }
    }

    //public class Expense
    //{
    //    public int Id { get; set; }
    //    public int CreatedBy { get; set; }
    //    public int ExpenseCategoryId { get; set; }
    //    public string Item { get; set; }
    //    [NotMapped]
    //    public string ItemName { get; set; }
    //    public decimal Quantity { get; set; }
    //    public decimal PricePerUnit { get; set; }
    //    public decimal Amount { get; set; }
    //    public string PaymentMode { get; set; }
    //    public string RefNo { get; set; }
    //    public string Description { get; set; }
    //    public DateTime CreatedOn { get; set; }
    //    public DateTime ModifiedOn { get; set; }
    //    public bool IsRoundOff { get; set; }
    //    public bool Deleted { get; set; }
    //}
    //public class IncomeCategory
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public bool Deleted { get; set; }
    //    public int CreatedBy { get; set; }
    //    public int LedgerGroup { get; set; }
    //}
    //public class IncomeSubCategory
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public int CategoryId { get; set; }
    //    public string CategoryName { get; set; }
    //    public bool Deleted { get; set; }
    //    public int CreatedBy { get; set; }
    //}
    //public class IncomeItem
    //{
    //    public int Id { get; set; }
    //    public int CreatedBy { get; set; }
    //    public string Name { get; set; }
    //    public string HSN_SAC { get; set; }
    //    public decimal SalesPrice { get; set; }
    //    public bool IsTaxableSP { get; set; }
    //    public decimal PurchasePrice { get; set; }
    //    public bool IsTaxablePP { get; set; }
    //    public int TaxGroup { get; set; }
    //    public int OpeningStock { get; set; }
    //    public int ClosingStock { get; set; }
    //    public decimal PerchasePerUnit { get; set; }
    //    public int MinStock { get; set; }
    //    public string ItemLocation { get; set; }
    //    public int UCRatesId { get; set; }
    //    public DateTime CreatedOn { get; set; }
    //    public int CategoryId { get; set; }
    //    public bool Deleted { get; set; }
    //    public int ItemCategotyId { get; set; }
    //    public string ItemCategoryName { get; set; }
    //    public int SubCategoryId { get; set; }
    //    public String SubCategoryName { get; set; }
    //    public bool IsActive {get; set;}
    //    public bool IsSerialNo { get; set; }
    //    //  public int Taxability { get; set; }
    //    public int Taxabilities { get; set; }
    //    public DateTime gstApplicableFrom { get; set; }
    //    public DateTime gstApplicableTo { get; set; }
    //    public bool GSTApplicableDate { get; set; }
    //    [NotMapped]
    //    public int IsGSTApplicableDate { get; set; }
    //    public bool IsSetStandardRate { get; set; }
    //    public decimal StockValue { get; set; }
    //    public string ItemPics { get; set; }
    //    [NotMapped]
    //    public IFormFile ItemPic { get; set; }
    //}

    //public class IncomeTransaction
    //{
    //    public int Id { get; set; }
    //    public int CreatedBy { get; set; }
    //    public int InComeCategoryId { get; set; }
    //    public string Item { get; set; }
    //    [NotMapped]
    //    public string ItemName { get; set; }
    //    public decimal Quantity { get; set; }
    //    public decimal PricePerUnit { get; set; }
    //    public decimal Amount { get; set; }
    //    public string PaymentMode { get; set; }
    //    public string RefNo { get; set; }
    //    public string Description { get; set; }
    //    public DateTime CreatedOn { get; set; }
    //    public DateTime ModifiedOn { get; set; }
    //    public bool IsRoundOff { get; set; }
    //    public bool Deleted { get; set; }
    //}

    [Table("ErrorLog")]
    public partial class ErrorLog
    {
        [Key]
        public int ErrorId { get; set; }

        public string Input { get; set; }

        public string Error { get; set; }

        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }

    }

    public class BankAccount
    {
        public int Id { get; set; }
        public int LedgerSubGroup { get; set; }
        public int CreatedBy { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string HolderName { get; set; }
        public string AccountNo { get; set; }
        public string IFSC { get; set; }
        public string Branch { get; set; }
        public string StateId { get; set; }
        public string CityId { get; set; }
        public int Country { get; set; }
        public string Address { get; set; }
        public string Pin { get; set; }
        public bool IsGSTApplicable { get; set; }
        public int GSTType { get; set; }
      //  public string UPID_QR_Code { get; set; }
        public decimal OpeningBal { get; set; }
        public decimal ClosingBal { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        //public bool PrintQR { get; set; }
        //public bool PrintDetails { get; set; }
        public bool Deleted { get; set; }
    }

    public class Invoice_no
    {
        public int Id { get; set; }
        public int TransactionType { get; set; }
        public bool InOutWord { get; set; }
        public bool AutoManula { get; set; }
        public bool IsIncludeYear { get; set; }
        public bool IsIncludeMonth { get; set; }
        public string Prefix { get; set; }
        public string StringSerialNo { get; set; }
        public int WhenRepeatSerialNo { get; set; }
        public bool IsMultipleShope { get; set; }
        public int CreatedBy { get; set; }
        public bool Deleted { get; set; }
    }
    public class Shopes
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public string ShopName { get; set; }
        public string LableForInvoice { get; set; }
        public bool Deleted { get; set; }
    }
    public class CashTransaction
    {
        public int id { get; set; }
        [NotMapped]
        public string PartyName { get; set; }
        [NotMapped]
        public int TransactionType { get; set; }
        public long TrnxId { get; set; }
        public int TranxType { get; set; }
        public decimal cashAmt { get; set; }
        public int CreatedBy { get; set; }
        public bool Deleted { get; set; }
        [ForeignKey("TrnxId")]
        public Transaction Transaction { get; set; }
        DateTime Date { get; set; }
        public string Discription { get; set; }
    }
    public class FinanceInfo
    {
        [ForeignKey("TrnxId")]
        public Transaction Transaction { get; set; }
        public int Id { get; set; }
        public long TrnxId { get; set; }
        public int TrnxType { get; set; }
        public int financeCompany { get; set; }
        public string FinanceCompanyName { get; set; }
        public string DownPaymentVia { get; set; }
        public decimal AssetsCost { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal MarginMoney { get; set; }
        public int No_Of_EMI { get; set; }
        public decimal EMI_Amount { get; set; }
        public int AdvanceEMI { get; set; }
        public decimal BundleAdvanceEMIAmt { get; set; }
        public decimal FileCharge { get; set; }
        public decimal DownPayment { get; set; }
        public decimal NetLoanAmt { get; set; }
        public decimal DBDPercentage { get; set; }
        public decimal DBDAmt { get; set; }
        public decimal GSTAmt { get; set; }
        public decimal NetDisbursalAmt { get; set; }
        public int CreatedBy { get; set; }
        public bool Deleted { get; set; }
    }
    public class WalletInfo
    {
        [ForeignKey("TrnxId")]
        public Transaction Transaction { get; set; }
        public int Id { get; set; }
        [NotMapped]
        public string PartyName { get; set; }
        [NotMapped]
        public int TransactionType { get; set; }
        public long TrnxId { get; set; }
        public int TrnxType { get; set; }
        public int WalletId { get; set; }
        public string Wallet_Name { get; set; }
        public string UPI_Id { get; set; }
        public string UPItanxId { get; set; }
        public decimal Amount { get; set; }
        public int CreatedBy { get; set; }
        public bool Deleted { get; set; }
    }
    public class BankTrnxInfo
    {
        [ForeignKey("TrnxId")]
        public Transaction Transaction { get; set; }
        [NotMapped]
        public string PartyName { get; set; }
        [NotMapped]
        public int TransactionType { get; set; }
        public int Id { get; set; }
        public long TrnxId { get; set; }
        public int TrnxType { get; set; }
        public string BankTnxType { get; set; }
        public string ChequeNo { get; set; }
        public DateTime chequeDate { get; set; }
        public string DepositTo { get; set; }
        public string BankName { get; set; }
        public string IFCcode { get; set; }
        public decimal DBD_Amt { get; set; }
        public decimal Amount { get; set; }
        public bool IsChequeDishonerd { get; set; }
        public int CreatedBy { get; set; }
        public bool Deleted { get; set; }
        public string Discription { get; set; }
        [NotMapped]
        public int BankTrasfer { get; set; }
        [NotMapped]
        public string SecondBank { get; set; }
        [NotMapped]
        public string IncreaseReduse { get; set; }

    }

    public class WalletTransactions
    {
        public int Id { get; set; }
        public decimal OpeningBal { get; set; }
        public decimal ClosingBal { get; set; }
        public string WalletName { get; set; }
        public string UPIid { get; set; }
        public int createdby { get; set; }
        public bool Deleted { get; set; }
        public string QRCode { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ChangedOn { get; set; }

    }
    public class Cash
    {
        public int Id { get; set; }
        public decimal OpeningBal { get; set; }
        public decimal ClosingBal { get; set; }
        public int CreatdeBy { get; set; }
        public bool Deleted { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ChangedOn { get; set; }
    }
    public class PayemtModeOption
    {
        public int Id { get; set; }
        public int createdBy { get; set; }
        public bool IsBank { get; set; }
        public bool IsCash { get; set; }
        public bool IsWallet { get; set; }
        public bool IsFinance { get; set; }
    }
    public class Description
    {
        public int Id { get; set; }
        public int CreateBy { get; set; }
        public bool Deleted { get; set; }
        public string Descriptions { get; set; }
        public int TransactionType { get; set; }
    }

    public class ExtraCharge
    {
        public int Id { get; set; }
        public int IncomeExpenses { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }
        public int CreateBy { get; set; }
        public bool IsHNSApplicable { get; set; }
        public bool IsGstApplicable { get; set; }
        public int GST { get; set; }
        public string HNS_Code { get; set; }
        [NotMapped]
        public string Rates { get; set; }
        public int LedgerGroup { get; set; }
        public decimal ClosingBalance { get; set; }

    }


    public class Ledger
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public int LedgerGroup { get; set; }
        public int LedgerSubGroup { get; set; }
        public string Name { get; set; }
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string IFCCode { get; set; }
        public string Branch { get; set; }
        public string Email { get; set; }
        public string PAN { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Pin { get; set; }
        public int Registration_Type { get; set; }
        public string GSTN { get; set; }
        public bool IsGSTApplicable { get; set; }
        //  public string Opening_Balance { get; set; }
        public bool MailingDetailsRequeued { get; set; }
        //  public string RegistrationType { get; set; }
        public decimal OpeningBal { get; set; }
        //  public int Registration_types { get; set; }
        public string ODLimit { get; set; }
        //Branch/Division
        public int BusinessType { get; set; }
        public int Good_Service { get; set; }
        public decimal Depreciation { get; set; }
        public decimal Rate { get; set; }
        public string HSNCode { get; set; }
        public decimal Amount { get; set; }

    }

}
