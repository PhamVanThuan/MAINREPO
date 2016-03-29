using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Origination.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILoanCalculator : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listProducts"></param>
        void BindProductDropdown(ReadOnlyEventList<IProduct> listProducts);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listPurpose"></param>
        void BindPurposeDropdown(ReadOnlyEventList<IMortgageLoanPurpose> listPurpose);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listEmploymentType"></param>
        void BindEmploymentType(IEventList<IEmploymentType> listEmploymentType);

        /// <summary>
        /// What it says on the tin
        /// </summary>
        /// <param name="applicationSource"></param>
        void PopulateMarketingSource(IEventList<IApplicationSource> applicationSource);

        /// <summary>
        /// Raised when the calculate button is clicked.
        /// </summary>
        event EventHandler OnCalculateButtonClicked;

        /// <summary>
        /// Raised when the cancel button is clicked.
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// Raised when the create application button is clicked.
        /// </summary>
        event EventHandler OnCreateApplicationButtonClicked;

        #region Properties

        #region inputs

        #region LegalEntity

        /// <summary>
        /// 
        /// </summary>
        int ExistingLegalEntityKey
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
      
       
       
      
        string MarketingSource
        {
            get;
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        int ProductKey
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        SAHL.Common.Globals.MortgageLoanPurposes MortgageLoanPurpose
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        int EmploymentTypeKey
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        double EstimatedPropertyValue
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        double Deposit
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        double CurrentLoan
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        double CashOut
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        Int16 Term
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        bool CapitaliseFees
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        bool InterestOnly
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        double HouseholdIncome
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        double LoanAmountRequired
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        double PurchasePrice
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        bool DisableCreateApplication
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        bool IsEstateAgentApplication
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        bool IsOldMutualDeveloperLoan
        {
            get;
        }

        bool HideEstateAgent
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        int VarifixMarketRateKey
        {
            get;
        }

        #endregion

        #region results

        /// <summary>
        /// 
        /// </summary>
        double LTV
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double PTI
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double PTIFix
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double LoanAmountTotal
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double InterestRate
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        int MarginKey
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double ActiveMarketRate
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        double LinkRate
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        double InterestRateFix
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double InstalmentTotal
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double InstalmentIOTotal
        {
            get; 
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double InstalmentEHLAM
        {
            get; 
            set;
        }

        
        /// <summary>
        /// 
        /// </summary>
        double InstalmentFix
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double InstalmentVar
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double FinanceChargesIOTotal
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double FinanceChargesTotal
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double FinanceChargesFix
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double FinanceChargesVar
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double CancellationFee
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double RegistrationFee
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double InitiationFee
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double TotalFee
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double InterimInterest
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        bool IncomeSufficient
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        bool ApplicationQualifies
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double LoanAmountFix
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double LoanAmountVar
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double FixPercent
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        int CreditMatrixKey
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        int CategoryKey
        {
            get;
            set;
        }

        #endregion

        #region Other

        /// <summary>
        /// 
        /// </summary>
        string CreateApplicationButtonText
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        int EdgeTerm
        {
            get;
            set;
        }

        #endregion

        #endregion
    }
}