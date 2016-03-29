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
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.Common.Web.UI;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Web.AJAX;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Origination
{
    public partial class LoanCalculator : SAHLCommonBaseView, ILoanCalculator
    {
        private double _bondRequired;
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage)
                return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage) 
                return;
            else
                base.RegisterWebService(ServiceConstants.LegalEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;

            if (IsPostBack)
            {
                switch (MortgageLoanPurpose)
                {
                    case MortgageLoanPurposes.Switchloan: //2: Switch loan
                        //rMarketValue.style.display = "inline";
                        //rCurrentLoan.style.display = "inline";
                        //rCashOut.style.display = "inline";
                        //rCapitaliseFees.style.display = "inline";
                        rowPurchasePrice.Attributes.Add("style", "display: none");
                        rowCashDeposit.Attributes.Add("style", "display: none");
                        rowCashRequired.Attributes.Add("style", "display: none");
                        tbCashOut.CssClass = "";

                        break;
                    case MortgageLoanPurposes.Newpurchase: //3: New purchase
                        //rPurchasePrice.style.display = "inline";
                        //rCashDeposit.style.display = "inline";
                        rowMarketValue.Attributes.Add("style", "display: none");
                        rowCurrentLoan.Attributes.Add("style", "display: none");
                        rowCashOut.Attributes.Add("style", "display: none");
                        rowCapitaliseFees.Attributes.Add("style", "display: none");
                        rowCashRequired.Attributes.Add("style", "display: none");
                        rowInterimInterest.Visible = false;

                        break;
                    case MortgageLoanPurposes.Refinance: //4: Refinance
                        //rMarketValue.style.display = "inline";
                        //rCapitaliseFees.style.display = "inline";
                        //rCashRequired.style.display = "inline";
                        rowCurrentLoan.Attributes.Add("style", "display: none");
                        rowCashOut.Attributes.Add("style", "display: none");
                        rowPurchasePrice.Attributes.Add("style", "display: none");
                        rowCashDeposit.Attributes.Add("style", "display: none");
                        rowInterimInterest.Visible = false;
                        tbCashOut.CssClass = "mandatory";

                        break;
                    default:
                        rowMarketValue.Attributes.Add("style", "display: none");
                        rowCurrentLoan.Attributes.Add("style", "display: none");
                        rowCashOut.Attributes.Add("style", "display: none");
                        rowCapitaliseFees.Attributes.Add("style", "display: none");
                        rowPurchasePrice.Attributes.Add("style", "display: none");
                        rowCashDeposit.Attributes.Add("style", "display: none");
                        rowCashRequired.Attributes.Add("style", "display: none");
                        rowInterimInterest.Visible = false;

                        break;
                }

                switch (ProductKey)
                {
                    case (int)Products.VariFixLoan: //"2": //VariFix
                        rowVarifix.Attributes.Add("style", "display: inline");
                        rowVariFixReset.Attributes.Add("style", "display: inline");
                        //rowInterestOnly.Attributes.Add("style", "display: none");
                        tblStandard.Visible = false;
                        rowPTIStandard.Visible = false;
                        break;
                    case (int)Products.Edge: //"11": //Edge
                        rowVarifix.Attributes.Add("style", "display: none");
                        rowVariFixReset.Attributes.Add("style", "display: none");
                        //rowInterestOnly.Attributes.Add("style", "display: none");
                        rowEHLIOInstal.Attributes.Add("style", "display: inline");
                        rowEHLAMInstal.Attributes.Add("style", "display: inline");
                        rowEHLAMInstalFull.Attributes.Add("style", "display: inline");
                        rowVarInstal.Attributes.Add("style", "display: none");
                        tbLoanTerm.Text = EdgeTerm.ToString();
                        tbLoanTerm.Enabled = false;
                        tblVariFix.Visible = false;
                        rowPTIStdForFix.Visible = false;
                        rowPTIFix.Visible = false;
                        break;
                    default: // 9 NewVariable : 5 Super Lo
                        //rowInterestOnly.Attributes.Add("style", "display: inline");
                        rowVarifix.Attributes.Add("style", "display: none");
                        rowVariFixReset.Attributes.Add("style", "display: none");
                        rowEHLIOInstal.Attributes.Add("style", "display: none");
                        rowEHLAMInstal.Attributes.Add("style", "display: none");
                        rowEHLAMInstalFull.Attributes.Add("style", "display: none");
                        rowVarInstal.Attributes.Add("style", "display: inline");
                        tblVariFix.Visible = false;
                        rowPTIStdForFix.Visible = false;
                        rowPTIFix.Visible = false;
                        break;
                }

                if (chkCapitaliseFees.Checked)
                {
                    lblFeeInfo.Text = "** Total Fees included because you elected to have them capitalised.";
                    lblFeeInfoFix.Text = "** Total Fees included because you elected to have them capitalised.";
                    lblFeeInfoInd.Text = "**";
                    lblFeeInfoIndFix.Text = "**";
                }
                else
                {
                    lblFeeInfo.Text = "";
                    lblFeeInfoFix.Text = "";
                    lblFeeInfoInd.Text = "";
                    lblFeeInfoIndFix.Text = "";
                }

                //if (!chkInterestOnly.Checked)
                //{
                    lblIOSAHLMonthlyInst.Visible = false;
                    lblIOSAHLIntOverTerm.Visible = false;
                    lblColumnHeader2.Visible = false;
                    tdInterestOnlyColumnHeader.BgColor = System.Drawing.Color.White.Name;
                    tdInterestOnlyColumnHeader.Width = Unit.Percentage(0).ToString();
                //}
            }
            else
            {
                //Product
                rowVarifix.Attributes.Add("style", "display: none");
                rowVariFixReset.Attributes.Add("style", "display: none");
                //rowInterestOnly.Attributes.Add("style", "display: inline");
                //Loan Purpose
                rowPurchasePrice.Attributes.Add("style", "display: none");
                rowCashDeposit.Attributes.Add("style", "display: none");
                rowMarketValue.Attributes.Add("style", "display: none");
                rowCurrentLoan.Attributes.Add("style", "display: none");
                rowCashOut.Attributes.Add("style", "display: none");
                rowCapitaliseFees.Attributes.Add("style", "display: none");
                rowCashRequired.Attributes.Add("style", "display: none");
            }

            tbPurchasePrice.Attributes.Add("onblur", "updateVal('" + tbPurchasePrice.ClientID + "')");
            tbMarketValue.Attributes.Add("onblur", "updateVal('" + tbMarketValue.ClientID + "')");
            tbCashOut.Attributes.Add("onblur", "updateVal('" + tbCashOut.ClientID + "')");
            tbCashRequired.Attributes.Add("onblur", "updateVal('" + tbCashRequired.ClientID + "')");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnCreateApplication(object sender, EventArgs e)
        {
            OnCreateApplicationButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void acNatAddIDNumber_ItemSelected(object sender, KeyChangedEventArgs e)
        {
           //check whther the recalculate must be fired for display purposes
            if (String.Compare(tbValidCalc.Text, true.ToString(), true) == 0)
                OnCalculateButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leKey"></param>
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnCalculate_Click(object sender, EventArgs e)
        {
           OnCalculateButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
            //Navigator.Navigate("ClientSuperSearch");
        }

        #region ILoanCalculator Members

        #region methods and events

        /// <summary>
        /// Bind the Product Dropdown
        /// </summary>
        /// <param name="listProducts"></param>
        public void BindProductDropdown(ReadOnlyEventList<IProduct> listProducts)
        {
            ddlProduct.DataValueField = "Key";
            ddlProduct.DataTextField = "Description";
            ddlProduct.DataSource = listProducts;
            ddlProduct.DataBind();
        }

        /// <summary>
        /// Bind the Product Dropdown
        /// </summary>
        /// <param name="listPurpose"></param>
        public void BindPurposeDropdown(ReadOnlyEventList<IMortgageLoanPurpose> listPurpose)
        {
            ddlPurpose.DataValueField = "Key";
            ddlPurpose.DataTextField = "Description";
            ddlPurpose.DataSource = listPurpose;
            ddlPurpose.DataBind();
        }

        /// <summary>
        /// Bind the Employment type dropdown
        /// </summary>
        /// <param name="listEmploymentType"></param>
        public void BindEmploymentType(IEventList<IEmploymentType> listEmploymentType)
        {
            ddlEmploymentType.DataSource = listEmploymentType.BindableDictionary;
            ddlEmploymentType.DataBind();

            // remove the 'Unknown' employment type
            ListItem li = ddlEmploymentType.Items.FindByValue(Convert.ToString((int)SAHL.Common.Globals.EmploymentTypes.Unknown));
            if (li != null)
                ddlEmploymentType.Items.Remove(li);
        }

        /// <summary>
        /// What it says on the tin
        /// </summary>
        /// <param name="applicationSource"></param>
        public void PopulateMarketingSource(IEventList<IApplicationSource> applicationSource)
        {

			ddlMarketingSource.DataTextField = "Description";
			ddlMarketingSource.DataValueField = "Key";
			ddlMarketingSource.DataSource = applicationSource;
			ddlMarketingSource.DataBind();


        }

        /// <summary>
        /// Handles the calculate button click
        /// </summary>
        public event EventHandler OnCalculateButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// Handles the create application button click
        /// </summary>
        public event EventHandler OnCreateApplicationButtonClicked;

        #endregion

        #region Properties

        #region user inputs

        #region LegalEntity

        /// <summary>
        /// 
        /// </summary>
        public int ExistingLegalEntityKey
        {
            get
            {
                if (tbLegalEntityKey.Text.Length > 0)
                    return Convert.ToInt32(tbLegalEntityKey.Text);

                return 0;
            }
        }

       

        /// <summary>
        /// 
        /// </summary>
       

        

        /// <summary>
        /// 
        /// </summary>
        public string MarketingSource
        {
            get 
            { 
                if (ddlMarketingSource.SelectedValue == "-select-")
                    return String.Empty;

                return ddlMarketingSource.SelectedValue; 
            }
        }

        #endregion


        /// <summary>
        /// The selected product key
        /// </summary>
        public int ProductKey
        {
            get { return Convert.ToInt16(ddlProduct.SelectedValue == "-select-" ? "0" : ddlProduct.SelectedValue); }
        }

        /// <summary>
        /// The Loan Purpose Key 
        /// MortgageLoanPurposeKey	Description
        /// 2	Switch loan
        /// 3	New purchase
        /// 4	Refinance
        /// </summary>
        public MortgageLoanPurposes MortgageLoanPurpose
        {
            get { return (MortgageLoanPurposes)Enum.ToObject(typeof(MortgageLoanPurposes), Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "1" : ddlPurpose.SelectedValue)); }
        }

        /// <summary>
        /// The employment type key
        /// </summary>
        public int EmploymentTypeKey
        {
            get { return Convert.ToInt16(ddlEmploymentType.SelectedValue == "-select-" ? "0" : ddlEmploymentType.SelectedValue); }
        }

        /// <summary>
        /// Market value of the property
        /// </summary>
        public double EstimatedPropertyValue
        {
            get
            {
                if (Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue) == (int)MortgageLoanPurposes.Newpurchase)
                    return tbPurchasePrice.Text.Length > 0 ? Convert.ToDouble(tbPurchasePrice.Text) : 0;
                else
                    return tbMarketValue.Text.Length > 0 ? Convert.ToDouble(tbMarketValue.Text) : 0;
            }
        }

        /// <summary>
        /// Deposit to pay for a new purchase
        /// </summary>
        public double Deposit
        {
            get
            {
                if (Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue) == (int)MortgageLoanPurposes.Newpurchase && tbCashDeposit.Text.Length > 0)
                    return Convert.ToDouble(tbCashDeposit.Text);
                else
                    return 0;
            }
        }

        /// <summary>
        /// Existing loan amount with another provider for switch loans
        /// </summary>
        public double CurrentLoan
        {
            get
            {
                if (Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue) == (int)MortgageLoanPurposes.Switchloan && tbCurrentLoan.Text.Length > 0)
                    return Convert.ToDouble(tbCurrentLoan.Text);
                else
                    return 0;
            }
        }

        /// <summary>
        /// Cash value required by the client for switch and refinance
        /// </summary>
        public double CashOut
        {
            get
            {
                if (Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue) == (int)MortgageLoanPurposes.Switchloan && tbCashOut.Text.Length > 0)
                    return Convert.ToDouble(tbCashOut.Text);

                if (Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue) == (int)MortgageLoanPurposes.Refinance && tbCashRequired.Text.Length > 0)
                    return Convert.ToDouble(tbCashRequired.Text);

                return 0;
            }
        }

        /// <summary>
        /// The term of the Loan in months
        /// </summary>
        public Int16 Term
        {
            get { return tbLoanTerm.Text.Length > 0 ? Convert.ToInt16(tbLoanTerm.Text) : (Int16)0; }
        }

        /// <summary>
        /// Should fees be capitalised to the Loan amount for switch and refinance
        /// </summary>
        public bool CapitaliseFees
        {
            get
            {
                if (Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue) != (int)MortgageLoanPurposes.Newpurchase)
                    return chkCapitaliseFees.Checked;

                return false;
            }
        }

        /// <summary>
        /// Indicates interest only mortgage loan application
        /// </summary>
        public bool InterestOnly
        {
            get
            {
                //if (Convert.ToInt16(ddlProduct.SelectedValue == "-select-" ? "0" : ddlProduct.SelectedValue) != (int)Products.VariFixLoan)
                //    return chkInterestOnly.Checked;

                return false;

            }
        }

        /// <summary>
        /// Total income of the household occupants
        /// </summary>
        public double HouseholdIncome
        {
            get { return tbHouseholdIncome.Text.Length > 0 ? Convert.ToDouble(tbHouseholdIncome.Text) : 0; }
        }

        /// <summary>
        /// Loan amount required
        /// </summary>
        public double LoanAmountRequired
        {
            get
            {
                switch (Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue))
                {
                    case (int)MortgageLoanPurposes.Newpurchase:
                        _bondRequired = (tbPurchasePrice.Text.Length > 0 ? Convert.ToDouble(tbPurchasePrice.Text) : 0) - (tbCashDeposit.Text.Length > 0 ? Convert.ToDouble(tbCashDeposit.Text) : 0);
                        break;
                    case (int)MortgageLoanPurposes.Switchloan:
                        _bondRequired = (tbCurrentLoan.Text.Length > 0 ? Convert.ToDouble(tbCurrentLoan.Text) : 0) + (tbCashOut.Text.Length > 0 ? Convert.ToDouble(tbCashOut.Text) : 0) + (InterimInterest);
                        break;
                    case (int)MortgageLoanPurposes.Refinance:
                        _bondRequired = tbCashRequired.Text.Length > 0 ? Convert.ToDouble(tbCashRequired.Text) : 0;
                        break;
                    default:
                        _bondRequired = 0;
                        break;
                }

                if (chkCapitaliseFees.Checked)
                    _bondRequired += TotalFee;

                return _bondRequired;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double PurchasePrice
        {
            get { return tbPurchasePrice.Text.Length > 0 ? Convert.ToDouble(tbPurchasePrice.Text) : 0; ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int VarifixMarketRateKey
        {
            get 
            {
                if (chkVFReset5Year.Checked)
                    return (int)MarketRates.FiveYearResetFixedMortgageRate;

                return (int)MarketRates.TwentyYearFixedMortgageRate;
            }
        }

        #endregion

        #region results

        /// <summary>
        /// 
        /// </summary>
        public double LTV
        {
            get { return Convert.ToDouble(tbLTV.Text); }
            set
            {
                lblLTV.Text = value.ToString(SAHL.Common.Constants.RateFormat);
                tbLTV.Text = value.ToString(); //for getter
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double PTI
        {
            get { return Convert.ToDouble(tbPTI.Text); }
            set
            {
                lblPTI.Text = value.ToString(SAHL.Common.Constants.RateFormat);
                lblVarPTI.Text = value.ToString(SAHL.Common.Constants.RateFormat);
                tbPTI.Text = value.ToString(); //to get again
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double PTIFix
        {
            set { lblFixPTI.Text = value.ToString(SAHL.Common.Constants.RateFormat); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double LoanAmountTotal
        {
            set
            {
                lblSAHLTotLoan.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
                lblTotalFixLoan.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        public int MarginKey
        {
            get { return Convert.ToInt16(tbMarginKey.Text); }
            set { tbMarginKey.Text = value.ToString(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double ActiveMarketRate
        {
            get { return Convert.ToDouble(tbActiveMarketRate.Text); }
            set { tbActiveMarketRate.Text = value.ToString(); }
        }

        public double LinkRate
        {
            get { return Convert.ToDouble(tbLinkRate.Text); }
            set { tbLinkRate.Text = value.ToString(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double InterestRate
        {
            set
            {
                lblSAHLIntRate.Text = value.ToString(SAHL.Common.Constants.RateFormat);
                lblVarRate.Text = value.ToString(SAHL.Common.Constants.RateFormat);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double InterestRateFix
        {
            set { lblFixRate.Text = value.ToString(SAHL.Common.Constants.RateFormat); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double InstalmentTotal
        {
            get { return Convert.ToDouble(tbInstalmentTotal.Text); }
            set
            {
                lblSAHLMonthlyInst.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
                lblTotFixMonthlyInst.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
                lblAMEHLInstFull.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
                tbInstalmentTotal.Text = value.ToString(); // for getter
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double InstalmentIOTotal
        {
            get { return Convert.ToDouble(lblIOSAHLMonthlyInst.Text); }
            set
            {
                lblIOSAHLMonthlyInst.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
                lblIOEHLInst.Text = lblIOSAHLMonthlyInst.Text;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double InstalmentEHLAM
        {
            get { return Convert.ToDouble(lblAMEHLInst.Text); }
            set
            {
                lblAMEHLInst.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double InstalmentFix
        {
            get { return Convert.ToDouble(tbInstalmentFix.Text); }
            set 
            { 
                lblFixMonthlyInst.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
                tbInstalmentFix.Text = value.ToString();
            
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double InstalmentVar
        {
            set { lblVarMonthlyInst.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double FinanceChargesTotal
        {
            set
            {
                lblSAHLIntOverTerm.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
                lblTotFixIntPaidTerm.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double FinanceChargesIOTotal
        {
            set
            {
                lblIOSAHLIntOverTerm.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double FinanceChargesFix
        {
            set { lblIntPaidTermFix.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double FinanceChargesVar
        {
            set { lblIntPaidTermVar.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double CancellationFee
        {
            get { return Convert.ToDouble(tbCancellationFee.Text); }
            set 
            {
                tbCancellationFee.Text = value.ToString();
                lblCancellationFee.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double RegistrationFee
        {
            get { return Convert.ToDouble(tbRegistrationFee.Text); }
            set 
            {
                tbRegistrationFee.Text = value.ToString(); 
                lblRegFee.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double InitiationFee
        {
            get { return Convert.ToDouble(tbInitiationFee.Text); }
            set 
            {
                tbInitiationFee.Text = value.ToString();
                lblBondPrepFee.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double TotalFee
        {
            get
            {
                if (string.IsNullOrEmpty(tbTotalFees.Text))
                    return 0;
                else
                    return Convert.ToDouble(tbTotalFees.Text);
            }
            set
            {
                lblTotalFees.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
                tbTotalFees.Text = value.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double InterimInterest
        {
            get 
            { 
                if (MortgageLoanPurpose == MortgageLoanPurposes.Switchloan)
                    return Convert.ToDouble(tbInterimInterest.Text.Length > 0 ? tbInterimInterest.Text : "0");

                return 0;
            }
            set
            {
                lblInterimIntProv.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
                tbInterimInterest.Text = value.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IncomeSufficient
        {
            set
            {
                if (value == false)
                    lblNotQualifyMsg.Visible = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ApplicationQualifies
        {
            set
            {
                lblQualifyMsg.Visible = value;
                btnCreateApplication.Enabled = value;
                pnlResults.Visible = value;
                tbValidCalc.Text = value.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double LoanAmountFix
        {
            set { lblFixLoanAmount.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double LoanAmountVar
        {
            set { lblVarLoanAmount.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat); }
        }

        #endregion

        #region other

        /// <summary>
        /// The percentage of the Loan amount to fix for VariFix product
        /// </summary>
        public double FixPercent
        {
            get
            {
                if (Convert.ToInt16(ddlProduct.SelectedValue == "-select-" ? "0" : ddlProduct.SelectedValue) == (int)Products.VariFixLoan && tbFixPercentage.Text.Length > 0)
                    return Convert.ToDouble(tbFixPercentage.Text) / 100; // convert user integer to percentage

                return 0;
            }
            set
            {
                lblFixPercent.Text = value.ToString(SAHL.Common.Constants.RateFormat);
                lblFixedPercent.Text = value.ToString(SAHL.Common.Constants.RateFormat);
                lblVariablePercent.Text = (1 - value).ToString(SAHL.Common.Constants.RateFormat);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CreditMatrixKey
        {
            get { return Convert.ToInt16(tbCreditMatrixKey.Text); }
            set { tbCreditMatrixKey.Text = value.ToString(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CategoryKey
        {
            get { return Convert.ToInt16(tbCategoryKey.Text); }
            set { tbCategoryKey.Text = value.ToString(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool DisableCreateApplication
        {
            set { btnCreateApplication.Visible = false; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEstateAgentApplication
        {
            get {return chkEstateAgent.Checked; }
        }

        public bool IsOldMutualDeveloperLoan
        {
            get { return chkOMLDeveloperLoan.Checked; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool HideEstateAgent
        {
            set { rowEstateAgentDeal.Visible = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CreateApplicationButtonText
        {
            set { btnCreateApplication.Text = value;}
        }

        
        /// <summary>
        /// 
        /// </summary>
        public int EdgeTerm
        {
            get { return _edgeTerm; }
            set { _edgeTerm = value; }
        }
        private int _edgeTerm;
        
        #endregion

        #endregion
        #endregion
    }
}
