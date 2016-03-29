using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Origination.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Origination
{
    public partial class ApplicationWizardSummary : SAHLCommonBaseView,IApplicationWizardSummary
    {
        #region Private Members

        //private List<ILegalEntity> _legalEntities;
      
        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        #region IApplicationWizardSummary Members              
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstLegalEntities"></param>
        /// <param name="application"></param>
        public void BindLegalEntities(IList<ILegalEntity> lstLegalEntities, IApplication application)
        {
            grdLegalEntity.AddGridBoundColumn("LegalEntityKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            grdLegalEntity.AddGridBoundColumn("LegalEntityName", "Legal Entity", Unit.Percentage(16), HorizontalAlign.Left, true);
            grdLegalEntity.AddGridBoundColumn("ResidentialAddress", "Residential Address", Unit.Percentage(42), HorizontalAlign.Left, true);
            grdLegalEntity.AddGridBoundColumn("PostalAddress", "Postal Address", Unit.Percentage(42), HorizontalAlign.Left, true);

            List<LegalEntityGridRowItem> lstGridItems = new List<LegalEntityGridRowItem>();
            foreach (ILegalEntity le in lstLegalEntities)
            {
                LegalEntityGridRowItem itm = new LegalEntityGridRowItem();
                itm.LegalEntityKey = le.Key;
                itm.LegalEntityName = le.DisplayName;
                bool foundResidential = false;
                bool foundPostal = false;
                foreach (ILegalEntityAddress leAddress in le.LegalEntityAddresses)
                {
                    if (!foundResidential)
                    {
                        if (leAddress.AddressType.Key == (int)SAHL.Common.Globals.AddressTypes.Residential)
                        {
                            itm.ResidentialAddress = leAddress.Address.GetFormattedDescription(AddressDelimiters.Comma);
                            foundResidential = true;
                        }
                    }

                    if (!foundPostal)
                    {
                        if (leAddress.AddressType.Key == (int)SAHL.Common.Globals.AddressTypes.Postal)
                        {
                            itm.PostalAddress = leAddress.Address.GetFormattedDescription(AddressDelimiters.Comma);
                            foundPostal = true;
                        }
                    }

                    if (foundResidential && foundPostal)
                        break;
                }
                lstGridItems.Add(itm);          
            }
            grdLegalEntity.DataSource = lstGridItems;
            grdLegalEntity.DataBind();
        }
   
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCalculateButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnFinishedButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnAddButtonClicked;
        
        /// <summary>
        /// 
        /// </summary>
        public event SAHL.Common.Web.UI.Events.KeyChangedEventHandler OnUpdateButtonClicked;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (grdLegalEntity.Rows.Count > 0 && grdLegalEntity.SelectedIndex > -1)
            {
                KeyChangedEventArgs args = new KeyChangedEventArgs(grdLegalEntity.Rows[grdLegalEntity.SelectedIndex].Cells[0].Text);
                OnUpdateButtonClicked(sender, args);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            OnAddButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalculator_Click(object sender, EventArgs e)
        {
            OnCalculateButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFinish_Click(object sender, EventArgs e)
        {
            OnFinishedButtonClicked(sender, e);
        }

        #region IApplicationWizardSummary Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        public void BindLoanGrid(IApplication application)
        {
            List<LoanGridRowItem> lstItems = new List<LoanGridRowItem>();

            LoanGridRowItem itm = new LoanGridRowItem();
          
            switch (application.ApplicationType.Key)
            {
                case (int)OfferTypes.FurtherAdvance:
                    itm.Purpose = MortgageLoanPurposes.Switchloan.ToString();
                    break;
                case (int)OfferTypes.FurtherLoan:
                    itm.Purpose = MortgageLoanPurposes.Switchloan.ToString();
                    break;
                case (int)OfferTypes.Life:
                    throw new NotSupportedException("This is not a mortgage loan.");
                case (int)OfferTypes.NewPurchaseLoan:
                    itm.Purpose = MortgageLoanPurposes.Newpurchase.ToString();
                    break;
                case (int)OfferTypes.ReAdvance:
                    itm.Purpose = MortgageLoanPurposes.ReAdvance.ToString();
                    break;
                case (int)OfferTypes.RefinanceLoan:
                    itm.Purpose = MortgageLoanPurposes.Refinance.ToString();
                    break;
                case (int)OfferTypes.SwitchLoan:
                    itm.Purpose = MortgageLoanPurposes.Switchloan.ToString();
                    break;
                default:
                    itm.Purpose = "Unknown";
                    break;
            }

            if (application.ApplicationType.Key != (int)SAHL.Common.Globals.OfferTypes.Unknown)
            {
                itm.Product = application.GetLatestApplicationInformation().Product.Description;


                ISupportsVariableLoanApplicationInformation supports = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                if (supports != null)
                {
                    itm.EmploymentType = supports.VariableLoanInformation.EmploymentType.Description;
                }

                itm.CashOutAmount = "N/A";
                IApplicationInformationVariableLoanForSwitchAndRefinance info = supports.VariableLoanInformation as IApplicationInformationVariableLoanForSwitchAndRefinance;
                if (info != null)
                {
                    if (info.RequestedCashAmount != null)
                    {

                        itm.CashOutAmount = info.RequestedCashAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                    }
                }
                itm.FixedPercentage = "N/A";
                ISupportsVariFixApplicationInformation supportsVarifix = application.CurrentProduct as ISupportsVariFixApplicationInformation;
                if (supportsVarifix != null)
                {
                    itm.FixedPercentage = (supportsVarifix.VariFixInformation.FixedPercent * 100).ToString() + " %";
                }

                itm.InterestOnly = "No";
                foreach (IApplicationInformationFinancialAdjustment fa in application.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments)
                {
                    if (fa.FinancialAdjustmentTypeSource.Key == (int)SAHL.Common.Globals.FinancialAdjustmentTypeSources.InterestOnly)
                    {
                        itm.InterestOnly = "Yes";
                        break;
                    }
                }


                itm.HouseholdIncome = application.GetHouseHoldIncome().ToString(SAHL.Common.Constants.CurrencyFormat);
                itm.CurrentLoanAmount = supports.VariableLoanInformation.ExistingLoan.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                IApplicationMortgageLoanNewPurchase mlNP = application as IApplicationMortgageLoanNewPurchase;
                if (mlNP != null)
                {
                    itm.PurchasePrice = mlNP.PurchasePrice.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
                itm.CashDeposit = supports.VariableLoanInformation.CashDeposit.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                itm.MarketValue = supports.VariableLoanInformation.PropertyValuation.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                itm.Term = supports.VariableLoanInformation.Term.Value.ToString();

                IApplicationMortgageLoanSwitch mlSwitch = application as IApplicationMortgageLoanSwitch;
                if (mlSwitch != null)
                {
                    if (mlSwitch.CapitaliseFees)
                    {
                        itm.CapitaliseFees = "Yes";
                    }
                    else
                    {
                        itm.CapitaliseFees = "No";
                    }
                }

                IApplicationMortgageLoanRefinance mlRefinance = application as IApplicationMortgageLoanRefinance;
                if (mlRefinance != null)
                {
                    if (mlRefinance.CapitaliseFees)
                    {
                        itm.CapitaliseFees = "Yes";
                    }
                    else
                    {
                        itm.CapitaliseFees = "No";
                    }
                }

            }
            else
            {
                itm.Product = "Unknown";
            }

            lstItems.Add(itm);

            grdLoan.DataSource = lstItems;

            switch (application.ApplicationType.Key)
            {
                case (int)SAHL.Common.Globals.OfferTypes.SwitchLoan:
                    {
                        ISupportsVariableLoanApplicationInformation vlInfo = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                        if (vlInfo != null)
                        {
                            grdLoan.AddGridBoundColumn("Purpose", "Loan Purpose", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("Product", "Product", Unit.Percentage(13), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("MarketValue", "Market Value", Unit.Percentage(12), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("CurrentLoanAmount", "Current Loan Amount", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("CashOutAmount", "Cashout Amount", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("EmploymentType", "Employment Type", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("Term", "Loan Term", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("CapitaliseFees", "Capitalise Fees", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("InterestOnly", "Interest Only", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("HouseholdIncome", "Household Income", Unit.Percentage(12), HorizontalAlign.Left, true);
                        }
                        
                        ISupportsVariFixApplicationInformation vfInfo = application.CurrentProduct as ISupportsVariFixApplicationInformation;
                        if (vfInfo != null)
                        {
                            grdLoan.Columns.Clear();
                            grdLoan.AddGridBoundColumn("Purpose", "Loan Purpose", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("Product", "Product", Unit.Percentage(13), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("MarketValue", "Market Value", Unit.Percentage(12), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("CurrentLoanAmount", "Current Loan Amount", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("CashOutAmount", "Cashout Amount", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("EmploymentType", "Employment Type", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("Term", "Loan Term", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("CapitaliseFees", "Capitalise Fees", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("FixedPercentage", "Fixed Percentage", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("HouseholdIncome", "Household Income", Unit.Percentage(12), HorizontalAlign.Left, true);
                        }
                        break;
                    }

                case (int)SAHL.Common.Globals.OfferTypes.NewPurchaseLoan:              
                    {
                        ISupportsVariableLoanApplicationInformation vlInfo = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                        if (vlInfo != null)
                        {
                            grdLoan.AddGridBoundColumn("Purpose", "Loan Purpose", Unit.Percentage(13), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("Product", "Product", Unit.Percentage(13), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("PurchasePrice", "Purchase Price", Unit.Percentage(12), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("CashDeposit", "Cash Deposit", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("EmploymentType", "Employment Type", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("Term", "Loan Term", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("InterestOnly", "Interest Only", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("HouseholdIncome", "Household Income", Unit.Percentage(12), HorizontalAlign.Left, true);
                        }

                       
                        ISupportsVariFixApplicationInformation vfInfo = application.CurrentProduct as ISupportsVariFixApplicationInformation;
                        if (vfInfo != null)
                        {
                            grdLoan.Columns.Clear();
                            grdLoan.AddGridBoundColumn("Purpose", "Loan Purpose", Unit.Percentage(13), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("Product", "Product", Unit.Percentage(13), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("PurchasePrice", "Purchase Price", Unit.Percentage(12), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("CashDeposit", "Cash Deposit", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("EmploymentType", "Employment Type", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("Term", "Loan Term", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("FixedPercentage", "Fixed Percentage", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("HouseholdIncome", "Household Income", Unit.Percentage(12), HorizontalAlign.Left, true);
                        }                       

                        ISupportsSuperLoApplicationInformation slInfo = application.CurrentProduct as ISupportsSuperLoApplicationInformation;
                        if (slInfo != null)
                        {
                            grdLoan.Columns.Clear();
                            grdLoan.AddGridBoundColumn("Purpose", "Loan Purpose", Unit.Percentage(13), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("Product", "Product", Unit.Percentage(13), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("PurchasePrice", "Purchase Price", Unit.Percentage(12), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("CashDeposit", "Cash Deposit", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("EmploymentType", "Employment Type", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("Term", "Loan Term", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("InterestOnly", "Interest Only", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("HouseholdIncome", "Household Income", Unit.Percentage(12), HorizontalAlign.Left, true);
                        }
                        break;
                    }

                case (int)SAHL.Common.Globals.OfferTypes.RefinanceLoan:               
                    {
                        ISupportsVariableLoanApplicationInformation vlInfo = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                        if (vlInfo != null)
                        {
                            grdLoan.AddGridBoundColumn("Purpose", "Loan Purpose", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("Product", "Product", Unit.Percentage(13), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("MarketValue", "Market Value", Unit.Percentage(12), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("CashOutAmount", "Cash Required", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("EmploymentType", "Employment Type", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("Term", "Loan Term", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("CapitaliseFees", "Capitalise Fees", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("InterestOnly", "Interest Only", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("HouseholdIncome", "Household Income", Unit.Percentage(12), HorizontalAlign.Left, true);
                        }

                        ISupportsVariFixApplicationInformation vfInfo = application.CurrentProduct as ISupportsVariFixApplicationInformation;
                        if (vfInfo != null)
                        {
                            grdLoan.Columns.Clear();
                            grdLoan.AddGridBoundColumn("Purpose", "Loan Purpose", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("Product", "Product", Unit.Percentage(13), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("MarketValue", "Market Value", Unit.Percentage(12), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("CashOutAmount", "Cash Required", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("EmploymentType", "Employment Type", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("Term", "Loan Term", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("CapitaliseFees", "Capitalise Fees", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("FixedPercentage", "Fixed Percentage", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("HouseholdIncome", "Household Income", Unit.Percentage(12), HorizontalAlign.Left, true);

                        }

                        ISupportsSuperLoApplicationInformation slInfo = application.CurrentProduct as ISupportsSuperLoApplicationInformation;
                        if (slInfo != null)
                        {
                            grdLoan.Columns.Clear();
                            grdLoan.AddGridBoundColumn("Purpose", "Loan Purpose", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("Product", "Product", Unit.Percentage(13), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("MarketValue", "Market Value", Unit.Percentage(12), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("CashOutAmount", "Cash Required", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("EmploymentType", "Employment Type", Unit.Percentage(10), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("Term", "Loan Term", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("CapitaliseFees", "Capitalise Fees", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("InterestOnly", "Interest Only", Unit.Percentage(6), HorizontalAlign.Left, true);
                            grdLoan.AddGridBoundColumn("HouseholdIncome", "Household Income", Unit.Percentage(12), HorizontalAlign.Left, true);
                        }
                        break;
                    }
                default: //Application type Unknown
                    {
                        grdLoan.AddGridBoundColumn("Purpose", "Loan Purpose", Unit.Percentage(15), HorizontalAlign.Left, true);
                        grdLoan.AddGridBoundColumn("Product", "Product", Unit.Percentage(15), HorizontalAlign.Left, true);
                        break;
                    }
            }
            
            grdLoan.DataBind();
        }

        #endregion
    }

    #region LegalEntityGridRowItem

    public class LegalEntityGridRowItem
    {
        private int _legalEntityKey;
        private string _legalEntityName;
        private string _residentialAddress;
        private string _postalAddress;

        public int LegalEntityKey
        {
            get
            {
                return _legalEntityKey;
            }
            set
            {
                _legalEntityKey = value;
            }
        }

        public  string LegalEntityName
        {
            get
            {
                return _legalEntityName;
            }
            set
            {
                _legalEntityName = value;
            }
        }
        public string ResidentialAddress
        {
            get
            {
                return  _residentialAddress;
            }
            set
            {
                _residentialAddress = value;
            }
        }

        public string PostalAddress
        {
            get
            {
                return _postalAddress;
            }
            set
            {
                _postalAddress = value;
            }
        }

    }

    #endregion

    #region LoanGridRowItem

    public class LoanGridRowItem
    {        
        private string _purpose;
        private string _product;
        private string _purchasePrice;
        private string _cashDeposit;
        private string _cashOutAmount;
        private string _fixedPercentage;
        private string _employmentType;
        private string _householdIncome;
        private string _capitaliseFees;
        private string _term;
        private string _currentLoanAmount;
        private string _marketValue;
        private string _interestOnly;


        public string InterestOnly
        {
            get
            {
                return _interestOnly;
            }
            set
            {
                _interestOnly = value;
            }
        }

        public string MarketValue
        {
            get
            {
                return _marketValue;
            }
            set
            {
                _marketValue = value;
            }
        }


        public string Term
        {
            get
            {
                return _term;
            }
            set
            {
                _term = value;
            }
        }

        public string CurrentLoanAmount
        {
            get
            {
                return _currentLoanAmount;
            }
            set
            {
                _currentLoanAmount = value;
            }
        }

        public string CapitaliseFees
        {
            get
            {
                return _capitaliseFees;
            }
            set
            {
                _capitaliseFees = value;
            }
        }

        public string HouseholdIncome
        {
            get
            {
                return _householdIncome;
            }
            set
            {
                _householdIncome = value;
            }
        }

        public string Purpose
        {
            get
            {
                return _purpose;
            }
            set
            {
                _purpose = value;
            }
        }

        public string Product
        {
            get
            {
                return _product;
            }
            set
            {
                _product = value;
            }
        }

        public string PurchasePrice
        {
            get
            {
                return _purchasePrice;
            }
            set
            {
                _purchasePrice = value;
            }
        }

        public string CashOutAmount
        {
            get
            {
                return _cashOutAmount;
            }
            set
            {
                _cashOutAmount = value;
            }
        }

        public string CashDeposit
        {
            get
            {
                return _cashDeposit;
            }
            set
            {
                _cashDeposit = value;
            }
        }



        public string FixedPercentage
        {
            get
            {
                return _fixedPercentage;
            }
            set
            {
                _fixedPercentage = value;
            }
        }

        public string EmploymentType
        {
            get
            {
                return _employmentType;
            }
            set
            {
                _employmentType = value;
            }
        }

        
    }

    #endregion
}
