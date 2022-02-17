using System;
using System.Collections.Generic;
using System.Text;

namespace com.aadviktech.IMS.Constant.AllEnum
{
    class AllEnums
    {



    }
    public enum LedgerType
        {
        Party=1,
        Bank=2,
        CreatedLedger=3
        }

    public enum Taxability
    {
          NilRated=0,
           Exempt=1,
           Taxable=2

    }
    public enum UserRole
    {
        admin = 1,
        accountant = 2,
        client = 3,
        party = 4
    }

    public enum GstType
    {
        UnRegistered = 1,
        Regular = 2,
        Composition = 3,
        Customer = 4
    }
    public enum BalanceType
    {
        ToPay=0,
        ToReceive=1
    }
    public enum BusinessType
    {
        None = 1,
        Retail = 2,
        Wholesale = 3,
        Distributor = 4,
        Service = 5,
        Manufacturing = 6,
        Others = 7
    }

    public enum GstCategory
    {
        Other = 1,
        SGST = 2,
        CGST = 3,
        IGST = 4,
        CESS = 5,
        FloodCESS = 6
    }
    public enum TaxType
    {
        WithoutTax = 0,
        WithTax = 1
    }
    public enum PaymentType
    {
        Cash = 1,
        Wallet = 2,
        Finance = 3,
        Bank=4,
    }
    public enum BankTrasfer
    {
        CashToBank = 1,
        BankToCash = 2,
        BankToBank = 3,
        AdjustBankBalance = 4,

    }
    public enum ExpensesCategory
    {
        IndirectExpenses = 1,
        DirectExpenses = 2
    }

    public enum TrsnsacctionsType
    {
        Sales = 1,
        Purchase = 2,
        SalesReturn = 3,
        PurchaseReturn = 4,
        Bank = 5,
        Expenses = 6,
        PaymentIn=7,
        PaymentOut=8,
        CashDeposit=9,
        CashWithdrow=10,
        settlement=11,
        Income=12,
        PaymentInIncome=13,
        PaymentOutExpenses=14,
    }
    public enum ItemType
    {
        Goods = 2,
        Expenses = 1,
        Service = 3
    }
    public enum WhenRepeatSerialNo
    {
        Never=0,
        Yearly=1,
        Monthly=2
    }
    public enum ExtraChargeCategory
    {
        Add=1,
        Less=2,
    }
    public enum AseDesc
    {
        Ascending = 1,
        Descending =2
       
    }
}
