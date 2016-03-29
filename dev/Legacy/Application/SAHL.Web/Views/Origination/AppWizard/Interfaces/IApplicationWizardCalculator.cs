using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using System.Collections;

namespace SAHL.Web.Views.Origination.Interfaces
{
    public interface IApplicationWizardCalculator : IViewBase
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
        /// 
        /// </summary>
        /// <param name="DictDefs"></param>
        void BindNeedsIdentificationDropdown(IDictionary DictDefs);

        /// <summary>
        /// Bind the controls (for update mode)
        /// </summary>
        /// <param name="application"></param>
        void BindControls(IApplication application);
        
        /// <summary>
        /// Raised when the calculate button is clicked.
        /// </summary>
        event EventHandler OnCalculateButtonClicked;

        event EventHandler OnBackButtonClicked;

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

        /// <summary>
        /// 
        /// </summary>
        int ProductKey
        {
            get;          
            set;
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
        int NeedsIdentificationKey { get;}


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
        int VarifixMarketRateKey
        {
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        bool CreateApplicationReadyOnly 
        { 
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        bool IsEstateAgentDeal
        {
            set;
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

        #region Legal Entity

        /// <summary>
        /// 
        /// </summary>
        ILegalEntity legalEntity { set;}

        /// <summary>
        /// 
        /// </summary>
		IApplication MortgageLoanApplication { get; set; }
		string ApplicationSource { get; set; }

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
        bool CallStartupScript { set;}


        /// <summary>
        /// 
        /// </summary>
        bool ShowBackButton { set;}

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
