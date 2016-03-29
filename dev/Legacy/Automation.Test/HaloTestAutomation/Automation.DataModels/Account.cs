using Common.Enums;
using System;
using System.Collections.Generic;

namespace Automation.DataModels
{
    public sealed class Account : Record, IDataModel
    {
        public int AccountKey { get; set; }

        public double FixedPayment { get; set; }

        public AccountStatusEnum AccountStatusKey { get; set; }

        public ProductEnum ProductKey { get; set; }

        public ProductEnum RRR_ProductKey { get; set; }

        public double Instalment { get; set; }

        public decimal CurrentBalance { get; set; }

        public decimal ArrearBalance { get; set; }

        public double SubsidyAmount { get; set; }

        public double TotalInstalment { get; set; }

        public int SPVKey { get; set; }

        public int ChildAccountKey { get; set; }

        public List<LoanFinancialService> FinancialServices { get; set; }

        public Role Role { get; set; }

        public int LifeAccountKey { get; set; }

        public int HOCAccountKey { get; set; }

        public int SubsidyClient { get; set; }

        public LoanDetail LoanDetail { get; set; }

        public LoanFinancialService FinancialService { get; set; }

        public string AccountLegalName { get; set; }

        public DateTime OpenDate { get; set; }
    }

    public sealed class LoanFinancialService
    {
        public int FinancialServiceKey { get; set; }

        public double Payment { get; set; }

        public AccountStatusEnum FinancialServiceStatusKey { get; set; }

        public int FinancialServiceTypeKey { get; set; }

        public int AccountStatusKey { get; set; }

        public int Term { get; set; }

        public decimal InitialBalance { get; set; }

        public int RemainingInstalments { get; set; }

        public decimal InterestRate { get; set; }

        public decimal ActiveMarketRate { get; set; }

        public decimal RateAdjustment { get; set; }

        public decimal MTDInterest { get; set; }

        public int CreditMatrixKey { get; set; }

        public double LinkRate { get; set; }

        public decimal MarketRate { get; set; }

        public int MortgageLoanPurpose { get; set; }

        public Balance SuspendedInterest { get; set; }

        public bool IsSurchargeRateAdjustment { get; set; }
    }
}