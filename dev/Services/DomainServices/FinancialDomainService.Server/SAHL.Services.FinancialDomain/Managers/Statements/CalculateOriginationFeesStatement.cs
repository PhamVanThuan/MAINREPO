using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using System;

namespace SAHL.Services.FinancialDomain.Managers.Statements
{
    public class CalculateOriginationFeesStatement : ISqlStatement<OriginationFeesModel>
    {
        public decimal LoanAmount { get; protected set; }
        public decimal BondRequired { get; protected set; }
        public int LoanType { get; protected set; }
        public decimal CashOut { get; protected set; }
        public decimal OverRideCancelFee { get; protected set; }
        public bool CapitaliseFees { get; protected set; }
        public bool NCACompliant { get; protected set; }
        public bool IsBondExceptionAction { get; protected set; }
        public bool QuickPay { get; protected set; }
        public decimal HouseholdIncome { get; protected set; }
        public int EmploymentTypeKey { get; protected set; }
        public decimal LTV { get; protected set; }
        public int ApplicationParentAccountKey { get; protected set; }
        public bool IsStaffLoan { get; protected set; }
        public bool IsDiscountedInitiationFee { get; protected set; }
        public DateTime OfferStartDate { get; protected set; }
        public bool CapitaliseInitiationFee { get; protected set; }
        public bool IsGEPF { get; protected set; }

        public CalculateOriginationFeesStatement(decimal loanAmount, decimal bondRequired, OfferType loanType, decimal cashOut, decimal overRideCancelFee, bool capitaliseFees, bool ncaCompliant,
                                             bool isBondExceptionAction, bool quickPay, decimal householdIncome, EmploymentType employmentTypeKey, decimal ltv, int applicationParentAccountKey,
                                             bool isStaffLoan, bool isDiscountedInitiationFee, DateTime offerStartDate, bool capitaliseInitiationFee, bool isGEPF)
        {
            this.LoanAmount = loanAmount;
            this.BondRequired = bondRequired;
            this.LoanType = (int)loanType;
            this.CashOut = cashOut;
            this.OverRideCancelFee = overRideCancelFee;
            this.CapitaliseFees = capitaliseFees;
            this.NCACompliant = ncaCompliant;
            this.IsBondExceptionAction = isBondExceptionAction;
            this.QuickPay = quickPay;
            this.HouseholdIncome = householdIncome;
            this.EmploymentTypeKey = (int)employmentTypeKey;
            this.LTV = ltv;
            this.ApplicationParentAccountKey = applicationParentAccountKey;
            this.IsStaffLoan = isStaffLoan;
            this.IsDiscountedInitiationFee = isDiscountedInitiationFee;
            this.OfferStartDate = offerStartDate;
            this.CapitaliseFees = capitaliseFees;
            this.IsGEPF = isGEPF;
        }

        public string GetStatement()
        {
            return @"
                EXECUTE [2AM].[dbo].[GetFees]
                @LoanAmount
                ,@BondRequired
                ,@LoanType
                ,@CashOut
                ,@OverRideCancelFee
                ,@CapitaliseFees
                ,@NCACompliant
                ,@IsBondExceptionAction
                ,@QuickPay
                ,@HouseholdIncome
                ,@EmploymentTypeKey
                ,@LTV
                ,@ApplicationParentAccountKey
                ,@IsStaffLoan
                ,@IsDiscountedInitiationFee
                ,@OfferStartDate
                ,@CapitaliseInitiationFee
                ,@isGEPF";
        }
    }
}