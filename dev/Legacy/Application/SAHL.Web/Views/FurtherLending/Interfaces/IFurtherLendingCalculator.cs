using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.FurtherLending.DTO;

namespace SAHL.Web.Views.FurtherLending.Interfaces
{
    public interface IFurtherLendingCalculator : IViewBase
    {
        /// <summary>
        /// Raised when the contact update button is clicked.
        /// </summary>
        event EventHandler OnContactUpdateButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnGenerateButtonClicked;

        /// <summary>
        /// Event raised when Calculate button is clicked
        /// </summary>
        event EventHandler OnCalculateButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnQuickCashButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vML"></param>
        /// <param name="fML"></param>
        /// <param name="Account"></param>
        /// <param name="app"></param>
        /// <param name="Reset"></param>
        void BindDisplay(IMortgageLoan vML, IMortgageLoan fML, IAccount Account, IApplication app, bool Reset);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmploymentTypes"></param>
        /// <param name="AccountEmploymentTypeKey"></param>
        void BindEmploymentTypes(IEventList<IEmploymentType> EmploymentTypes, int AccountEmploymentTypeKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="occupancyTypes"></param>
        /// <param name="AccountOccupancyTypeKey"></param>
        void BindOccupancyTypes(IEventList<IOccupancyType> occupancyTypes, int AccountOccupancyTypeKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SPVs"></param>
        void BindSPVs(IList<ISPV> SPVs);

        #region Properties

        //16486Remove start
        /// <summary>
        /// check the start date of the readvance to determine what calc to use in the view
        /// this property and all related code can be removed a month after go live: 
        /// select * from control where controlDescription = 'CLVReadvanceStart'
        /// </summary>
        DateTime? ReadvanceStartDate
        {
            get;
            set;
        }
        /// <summary>
        /// Hack fix for FA created before the 'CLVReadvanceStart' date where no Re-Advance exists
        /// </summary>
        bool HideCLV
        {
            get;
            set;
        }
        //16486Remove end

        /// <summary>
        /// Indicates if the Account is NCA Compliant, if not add initiation fee for FLoans
        /// </summary>
        bool NCACompliant
        {
            get;
            set;
        }

        /// <summary>
        /// Get the value of the Estimated Valuation Amount or Latest Valuation
        /// </summary>
        double LatestValuationAmount
        {
            get;
            //set;
        }

        ///// <summary>
        ///// Get the key of the Employment Type for the Account to reset the form
        ///// </summary>
        //int EmploymentTypeKey
        //{
        //    get;
        //    //set;
        //}

        /// <summary>
        /// Get the key of the Employment Type selected in the drop down list
        /// </summary>
        int EmploymentTypeKeySelected
        {
            get;
            //set;
        }

        /// <summary>
        /// Get the value of the Rate selected in the drop down list
        /// </summary>
        double MarginSelected
        {
            get;
            //set;
        }

        /// <summary>
        /// Get the Margin key of the Rate currently selected
        /// </summary>
        int MarginKeySelected
        {
            get;
            //set;
        }

        /// <summary>
        /// Get the value of the Total cash required textbox
        /// </summary>
        double TotalCashRequired
        {
            get;
            //set;
        }

        /// <summary>
        /// Get the value of the Rapid Cash required textbox
        /// </summary>
        double ReadvanceRequired
        {
            get;
            //set;
        }

        /// <summary>
        /// Get the value of the Further advance Cash required textbox
        /// </summary>
        double FurtherAdvRequired
        {
            get;
            //set;
        }

        /// <summary>
        /// Get the value of the Further Loan Cash required textbox
        /// </summary>
        double FurtherLoanRequired
        {
            get;
            //set;
        }

        /// <summary>
        /// Get the value of the Bond to register textbox
        /// </summary>
        double BondToRegister
        {
            get;
            set;
        }


        /// <summary>
        /// Get the Current Balance for the disbursed Account
        /// </summary>
        double CurrentBalance
        {
            get;
            //set ; 
        }

        /// <summary>
        /// Get the Current Balance for the Fixed leg of the Account
        /// </summary>
        double CurrentBalanceVF
        {
            get;
            //set ; 
        }

        /// <summary>
        /// The Current Balance for the disbursed Account plus any Further Lending request
        /// </summary>
        double NewCurrentBalance
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        bool HasArrears
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        int RemainingTerm
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        double NewInstalment
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double NewAMInstalment
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        double BaseRateVariable
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        double BaseRateFixed
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        bool IsInterestOnly
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double NewLTV
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double NewLTP
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double NewPTI
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double NewRate
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double TwentyYearInstalment
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double TwentyYearPTI
        {
            set;
        }

        bool ShowTwentyYearFigures 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// 
        /// </summary>
        double CapitalisedLife
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        double LifeInstalment
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        double NewIncome
        {
            get;
        }

        int NewOccupancyTypeKey
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        double DiscountVariable
        {
            get;
            //set;
        }

        /// <summary>
        /// 
        /// </summary>
        double DiscountFixed
        {
            get;
            //set;
        }

        /// <summary>
        /// 
        /// </summary>
        double ReadvanceMax
        {
            get;
            //set;
        }

        /// <summary>
        /// 
        /// </summary>
        double FurtherAdvanceMax
        {
            get;
            //set;
        }

        /// <summary>
        /// 
        /// </summary>
        double FurtherLoanMax
        {
            get;
            //set;
        }

        /// <summary>
        /// 
        /// </summary>
        ISPV NewSPV
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        int SPVCompany
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        ILegalEntity SelectedLegalEntity
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        ICategory NewCategory
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        string HomePhoneCode
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        string HomePhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        string WorkPhoneCode
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        string WorkPhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        string FaxCode
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        string FaxNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        string CellNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        string Email
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double Fees
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        bool IsExceptionCreditCriteria
        {
            get;
            set;
        }

        #region existing application values

        /// <summary>
        /// 
        /// </summary>
        bool ReadvanceInProgress
        {
            get;
            set;
        }

        /// <summary>
        /// If the advance has been accepted it can not be edited
        /// </summary>
        bool ReadvanceIsAccepted
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double ReadvanceInProgressAmount
        {
            get;
            set;
        }



        /// <summary>
        /// 
        /// </summary>
        bool FurtherAdvanceInProgress
        {
            get;
            set;
        }

        /// <summary>
        /// If the advance has been accepted it can not be edited
        /// </summary>
        bool FurtherAdvanceIsAccepted
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double FurtherAdvanceInProgressAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        bool FurtherLoanInProgress
        {
            get;
            set;
        }

        /// <summary>
        /// If the advance has been accepted it can not be edited
        /// </summary>
        bool FurtherLoanIsAccepted
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double FurtherLoanInProgressAmount
        {
            get;
            set;
        }

        /// <summary>
        /// The latest valuation amount for the applications
        /// </summary>
        double ApplicationValuationAmount
        {
            get;
            set;
        }

        /// <summary>
        /// Application margin rate
        /// </summary>
        double ApplicationRate
        {
            get;
            set;
        }

        /// <summary>
        /// Application margin rate key
        /// </summary>
        int ApplicationRateKey
        {
            get;
            set;
        }

        /// <summary>
        /// Bond to register for all open applications
        /// </summary>
        double ApplicationBondToRegister
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double ApplicationHouseholdIncome
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        double AccountPurchasePrice
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        int AccountApplicationType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        bool CanUpdate
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        //bool UseTotal
        //{ 
        //    get;
        //    set;
        //}


        /// <summary>
        /// Set whether the view is being used to do credit approvals
        /// </summary>
        SAHL.Common.Globals.ApprovalTypes ApprovalMode
        {
            get;
            set;
        }

        /// <summary>
        /// The selected application type
        /// This is currently only used to show/hide rows
        /// in the approval mode.
        /// </summary>
        SAHL.Common.Globals.OfferTypes ApplicationType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        string ExistingApplicationMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Text to display on the submit button for Credit screens
        /// </summary>
        string BtnSubmitText
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        bool BtnSubmitEnabled
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// If a non new business application is loaded, hide all controls on the page
        /// </summary>
        bool HideAll { get; set; }

        double CalculatedLinkRate { get; set; }

        double CurrentLinkRate { get; set; }

        List<RecipientCorrespondenceSelection> SendingInformation { get; }

        bool IncludeNaedoForm { get; }

        EnquiryReportParameters EnquiryReportParameters { get; }

        bool SendReadvanceApplicationForm { get; }

        bool SendFurtherAdvanceApplicationForm { get; }

        bool SendFurtherLoanApplicationForm { get; }

        bool HelpDeskRework { get; set; }

        #endregion
    }

    public class EnquiryReportParameters
    {

        public EnquiryReportParameters(int reportStatementKey, int format, int accountKey, double readvanceMax, double furtherAdvanceMax, double furtherAdvanceMaxLAA, double furtherLoanMax,
            double estimatedFurtherLoanMax, double readvanceRequired, double furtherAdvanceRequired, double furtherLoanRequired, double newLinkRate,
            double estimatedValuationAmount, double furtherBondAmount, double furtherLoanFees, string aduserName)
        {
            this.ReportStatementKey = reportStatementKey;
            this.Format = format;
            this.AccountKey = accountKey;

            this.ReadvanceRequired = readvanceRequired;
            this.ReadvanceMax = readvanceMax;

            this.FurtherAdvanceMaxLAA = furtherAdvanceMaxLAA;
            this.FurtherAdvanceMax = furtherAdvanceMax;

            this.FurtherLoanRequired = furtherLoanRequired;
            this.FurtherLoanMax = furtherLoanMax;
            this.EstimatedFurtherLoanMax = estimatedFurtherLoanMax;

            this.NewLinkRate = newLinkRate;
            this.EstimatedValuationAmount = estimatedValuationAmount;
            this.FurtherBondAmount = furtherBondAmount;
            this.FurtherLoanFees = furtherLoanFees;
            this.ADUserName = aduserName;

            if ((this.FurtherAdvanceMaxLAA > 0 && furtherAdvanceRequired > 0)
                && this.FurtherAdvanceMaxLAA > furtherAdvanceRequired)
            {
                this.FurtherAdvanceLAARequired = furtherAdvanceRequired;
            }
            else if ((this.FurtherAdvanceMaxLAA > 0 && furtherAdvanceRequired > 0)
                    && this.FurtherAdvanceMaxLAA < furtherAdvanceRequired)
            {
                this.FurtherAdvanceLAARequired = this.FurtherAdvanceMaxLAA;
                this.FurtherAdvanceRequired = furtherAdvanceRequired;
            }
            else
            {
                this.FurtherAdvanceRequired = furtherAdvanceRequired;
            }
        }

        public int ReportStatementKey { get; protected set; }

        public int Format { get; protected set; }

        public int AccountKey { get; protected set; }

        public double ReadvanceMax { get; protected set; }

        public double FurtherAdvanceMax { get; protected set; }

        public double FurtherAdvanceMaxLAA { get; protected set; }

        public double FurtherLoanMax { get; protected set; }

        public double EstimatedFurtherLoanMax { get; protected set; }

        public double ReadvanceRequired { get; protected set; }

        public double FurtherAdvanceRequired { get; protected set; }

        public double FurtherAdvanceLAARequired { get; protected set; }

        public double FurtherLoanRequired { get; protected set; }

        public double EstimatedValuationAmount { get; protected set; }

        public double FurtherBondAmount { get; protected set; }

        public double FurtherLoanFees { get; protected set; }

        public double NewLinkRate { get; protected set; }

        public string ADUserName { get; protected set; }


    }
}


