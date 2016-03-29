using System;

namespace SAHL.Web.Objects
{
    /// <summary>
    /// This is the base class for the PreProspect Object 
    /// </summary>
    [Serializable]
    public class PreProspectBase
    {

        // New Origination Calculator Variables
        //------------------------------------------

        public int marketrateKey;
        public int CreditMatrixKey;
        public int EmploymentType;
        public int ProductKey;  // Default to variable loan
        public double LinkRate = 0.021;
        public double InterestRate = 12;
        public double MaximumInstallment;
        // PurposeNumber = 1 - Unknown , 2 - Switch Loan, 3 - New Purchase, 4 - Refinance, 5 - Further Loan 
        public int PurposeNumber = 1;
        public int MaturityMonths = 6;
        public int IncomeType = 1;
        public int NumberOfApplicants = 1;
        public int CategoryKey;


        //------------------------------------------
        // Marketing Information
        public string AdvertisingCampaignID = "";
        public string UserURL = "";
        public string ReferringServerURL = "";
        public string UserAddress = "";

        //------------------------------------------

        // Application form contact Information 
        public string PreProspectFirstNames = "";
        public string PreProspectSurname = "";
        public string PreProspectIDNumber = "";
        public string PreProspectHomePhoneCode = "";
        public string PreProspectHomePhoneNumber = "";
        public string PreProspectEmailAddress = "";
        //------------------------------------------

        public int VarifixMarketRateKey;
        public int MarketRateTypeNumber = 1;
        public string ReferenceNumber = "";

        public int rateConfigKey;

        public int MortgageLoanPurpose;
        public bool InterestOnly;
        public int Product = 3; // Default to variable loan - old 


        public double InitiationFee;
        public double RegistrationFee;
        public bool CapitaliseFees;
        public double CancellationFee;
        public double InterimInterest;
        public double ValuationFee;
        public double TransferFee;
        public double Deposit;
        public double CurrentLoan;
        public double TotalFee;
        public double HouseholdIncome;
        public double LTV;
        public double PTI;
        public double ActiveMarketRate;
        public double InstalmentTotal;
        public double EstimatedPropertyValue;
        public int MarginKey;
        public int Term = 240;
        public double CashOut;
        public double InstalmentFix;
        public double FixPercent;
        public double PurchasePrice;
        public double TotalPrice;
        public double LoanAmountRequired;

        //VariFix Variables 
        public bool FixLoan;
        public double ElectedFixedPercentage;
        public double ElectedVariablePercentage;
        public double ElectedFixedRate;
    }

    /// <summary>
    /// This is the base class for the PreProspect Object 
    /// </summary>
    [Serializable]
    public class OriginationFees
    {
        public double loanamount;
        public double bondrequired;
        public int appType;
        public double cashout;
        public double OverrideCancelFeeamount;
        public bool capitalisefees;
        public double initiationfee;
        public double registrationfee;
        public double cancellationfee;
        public double interiminterest;
        public double bondtoregister;
    }

    /// <summary>
    /// This is the base class for the credit Criteria object for passing to and from the web service
    /// </summary>
    [Serializable]
    public class CreditCriteria
    {
        public int CreditCriteriaKey;
        public double CreditCriteriaPTI;
        public double CreditCriteriaPTIValue;
        public double CreditCriteriaMarginValue;
        public int CreditCriteriaMarginKey;
        public int CreditCriteriaCreditMatrixKey;
        public int CreditCriteriaCategoryKey;
    }


}
