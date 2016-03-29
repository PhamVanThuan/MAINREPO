using Newtonsoft.Json;
using SAHL.Common;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.FurtherLending.DTO;
using SAHL.Web.Views.FurtherLending.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace SAHL.Web.Views.FurtherLending
{
    public partial class Calculator : SAHLCommonBaseView, IFurtherLendingCalculator
    {
        # region LocalVars
        private IControlRepository _ctrlRepo;
        private IApplicationRepository _appRepo;
        private IHOCRepository _hocRepo;
        private ILifeRepository _lifeRepo;
        private IAccountRepository _accRepo;
        private ILookupRepository _lookupRepo;
        private ILoanTransactionRepository _ltRepo;
        private IGroupExposureRepository _groupExposureRepo;
        private IRuleService _ruleServ;
        private DateTime? _raStartDate;

        private bool _hideCLV;
        private string _reportPreviewURL;

        private bool _ncaCompliant;
        private double _currBalance;
        private double _currBalanceVF;
        private double _arrearsFix;
        private double _arrearsVar;
        private double _arrearsBalance;
        private double _currInstalment;
        private double _accruedInterestVar;
        private double _accruedInterestFix;
        private double _accruedInterest;
        private double _capitalisedLife;
        private double _lifeInstalment;

        //Max values available for lending
        private double _readvanceMax;

        private double _furtherAdvanceMax;
        protected double _furtherAdvanceMaxLAA;
        private double _totalBondsRegistered;
        private double _totalLoanAgreeAmount;
        private double _furtherLoanMax;
        private double _estimatedFurtherLoanMax;

        private double _totalCashRequired;
        private double _readvanceRequired;
        private double _furtherAdvanceRequired;
        private double _furtherLoanRequired;

        private double _bondToRegister;
        private double _newIncome1;
        private double _committedLoanValue;
        private double _newCurrentBalance;
        private double _newInstalment;
        private double _newAMInstalment;
        private double _newLTV;
        private double _newLTP;
        private double _newPTI;
        private double _newRate;

        private double twentyYearInstalment;
        private double twentyYearPTI;
        private bool showTwentyYearFigures;

        private double _accCategory;
        private ISPV _newSPV;
        private IList<ISPV> _spvList;
        private int _SPVCompany;

        private double _maxLoanAmount;
        private double _existingMaxLoanAmount;
        private double _estimatedMaxLoanAmount;

        private string _productDescription;
        private bool _isInterestOnly;
        private bool _hasInvokedCap;
        private bool _hasArrears;
        private bool _readvanceInProgress;
        private bool _readvanceIsAccepted;
        private bool _furtherAdvanceInProgess;
        private bool _furtherAdvanceIsAccepted;
        private bool _furtherLoanInProgress;
        private bool _furtherLoanIsAccepted;
        private bool _showVarifix;

        //existing application stuff
        private double _readvanceInProgressAmount;

        private double _furtherAdvanceInProgressAmount;
        private double _furtherLoanInProgressAmount;
        private double _applicationValuationAmount;
        private double _applicationRate;
        private int _applicationRateKey;
        private double _applicationBondToRegister;
        private double _applicationHouseholdIncome;
        private double _accountPurchasePrice;
        private int _accAppType;
        private bool _canUpdate;

        //private bool _useTotal;
        private bool _hideAll;

        private ApprovalTypes _approvalMode;
        private OfferTypes _applicationType;

        private double _fees;
        private bool _isExceptionCreditCriteria;

        //private IBond _bond;
        private double _latestValuationAmount;

        private double _estimatedValuationAmount;
        private DateTime? _latestValuationDate;
        private double _discountV;
        private double _discountF;
        private double _baseRateV;
        private double _baseRateF;
        private double _householdIncome;
        private int _employmentTypeKeySelected;
        private int _employmentTypeKey;
        private int _marginKey;
        private int _marginKeySelected;
        private int _newOccupancyTypeKey;
        private double _marginSelected;

        //private int _remainingTerm;
        private Int32 _accountKey;

        private IAccount _account;
        private DateTime _accOpenDate;

        // ContactData
        private string _homePhoneCode;

        private string _homePhoneNumber;
        private string _workPhoneCode;
        private string _workPhoneNumber;
        private string _faxCode;
        private string _faxNumber;
        private string _cellNumber;
        private string _email;

        // CM stuff
        private int _originationSourceKey;

        private int _productKey;
        private int _mortgageLoanPurposeKey = Convert.ToInt32(MortgageLoanPurposes.Switchloan); //Further Lending is evaluated by credit as a switch loan, so use Credit Matrix for Switch
        private string NoCategory = "No Category Found";
        private string Varifix = "Varifix";

        public double CalculatedLinkRate { get; set; }

        public double CurrentLinkRate { get; set; }

        # endregion

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage)
            {
                //if !ShouldRunPage and hideall, then no page controls must be visible
                if (HideAll)
                {
                    tabsMenu.Visible = false;
                    btnbar.Visible = false;
                    return;
                }
            }

            if (ApprovalMode != ApprovalTypes.None)
            {
                if (ApprovalMode == ApprovalTypes.Approve)
                {
                    btnCalculate.Attributes.Add("style", "display: none");
                    lblNewLinkRate.Visible = true;
                }
                else
                {
                    if (!IsPostBack)
                    {
                        btnSubmit.Attributes.Add("disabled", "true");
                    }

                    ddlSPV.Visible = true;
                    lblNewSPV.Attributes.Add("style", "display: none");
                }

                ddlEmploymentType.Attributes.Add("style", "display: none");
                lEmploymentApplication.Visible = true;
                lEmploymentApplication.Text = ddlEmploymentType.SelectedItem.Text;

                lTtlFLInclFees.BorderColor = Color.Black;
                lTtlFLInclFees.BorderStyle = BorderStyle.Solid;
                lTtlFLInclFees.BorderWidth = Unit.Pixel(1);
                lblTtlFLInclFees.Visible = false;

                btnBack.Visible = false;
                btnGenerate.Visible = false;
                btnNext.Visible = false;
                btnReset.Visible = false;
                btnUpdateContact.Visible = false;

                btnSubmit.Visible = true;
                btnCancel.Visible = true;

                if (ApplicationType == OfferTypes.FurtherLoan)
                    btnQuickCash.Visible = true;

                if (ApplicationType != OfferTypes.ReAdvance)
                {
                    rowReadvance_1.Attributes.Add("style", "display: none");
                    lblReAdvance.Attributes.Add("style", "display: none");
                    tbReadvanceRequired.Attributes.Add("style", "display: none");
                    lReadvanceRequired.Attributes.Add("style", "display: none");
                }

                if (ApplicationType != OfferTypes.FurtherAdvance)
                {
                    rowFurtherAdvance_1.Attributes.Add("style", "display: none");
                    lblFurtherAdvance.Attributes.Add("style", "display: none");
                    tbFurtherAdvReq.Attributes.Add("style", "display: none");
                    lFurtherAdvReq.Attributes.Add("style", "display: none");
                }

                if (ApplicationType != OfferTypes.FurtherLoan)
                {
                    rowFurtherLoan_1.Attributes.Add("style", "display: none");
                    lblFurtherLoan.Attributes.Add("style", "display: none");
                    tbFurtherLoanReq.Attributes.Add("style", "display: none");
                    lFurtherLoanReq.Attributes.Add("style", "display: none");
                }

                //replaced rowFLClientEstimate hide with item hide style
                rowFLClientEstimate_1.Attributes.Add("style", "display: none");
                lblFurtherLoanClientEst.Attributes.Add("style", "display: none");

                //replaced rowFLTotal hide with item hide style
                rowFLTotal_1.Attributes.Add("style", "display: none");
                lblTotalCashRequired.Attributes.Add("style", "display: none");
                tbTotalCashRequired.Attributes.Add("style", "display: none");
                lTotalCashRequired.Attributes.Add("style", "display: none");

                tbEstimatedValuationAmount.Attributes.Add("style", "display: none");
                tbNewIncome1.Attributes.Add("style", "display: none");
                lblNewHouseholdIncome.Visible = true;

                if (ShouldRunPage)
                    btnCalculate_Click(null, null);
            }

            if (!string.IsNullOrEmpty(_reportPreviewURL))
            {
                reportPreviewURL.Value = _reportPreviewURL;
                btnPreview.Enabled = true;
            }

            // this is a bit of a hack till we can refactor - conditions queries are VERY slow so moved
            // this here so it is only called if we need to show it
            if (tabsMenu.ActiveTab == tpLoanCondition)
            {
                //Conditions
                IConditionsRepository conditionsRepo = RepositoryFactory.GetRepository<IConditionsRepository>();
                List<string> conditionsList = conditionsRepo.GetLastDisbursedApplicationConditions(_accountKey);
                gvConditions.Columns.Clear();
                gvConditions.AddGridBoundColumn("!", "Conditions", Unit.Percentage(100), HorizontalAlign.Left, true);
                gvConditions.DataSource = conditionsList;
                gvConditions.DataBind();
            }
        }

        public EnquiryReportParameters EnquiryReportParameters
        {
            get
            {
                double clientEstimatedValutionAmt = string.IsNullOrEmpty(this.tbEstimatedValuationAmount.Text) ? 0D : double.Parse(this.tbEstimatedValuationAmount.Text);
                return new EnquiryReportParameters((int)ReportStatements.ConfirmationOfEnquiry, (int)ReportFormats.Email, _accountKey, _readvanceMax, _furtherAdvanceMax, _furtherAdvanceMaxLAA, _furtherLoanMax, _estimatedFurtherLoanMax, _readvanceRequired, _furtherAdvanceRequired, _furtherLoanRequired, _marginSelected, clientEstimatedValutionAmt, _bondToRegister, _fees, CurrentPrincipal.Identity.Name);
            }
        }

        public void SetReportPreviewURL()
        {
            EnquiryReportParameters reportParameters = EnquiryReportParameters;

            Dictionary<string, object> reportParams = new Dictionary<string, object>();
            reportParams.Add("ReportStatementKey", reportParameters.ReportStatementKey);
            reportParams.Add("Format", reportParameters.Format);

            reportParams.Add("AccountKey", reportParameters.AccountKey);
            reportParams.Add("EstimatedValuationAmount", reportParameters.EstimatedValuationAmount);

            reportParams.Add("ReadvanceMaxAvailable", reportParameters.ReadvanceMax);
            reportParams.Add("FurtherAdvanceMaxLoanAgreementAmountAvailable", reportParameters.FurtherAdvanceMaxLAA);
            reportParams.Add("FurtherAdvanceMaxBondAmountAvailable", reportParameters.FurtherAdvanceMax);
            reportParams.Add("FurtherLoanMaxAvailable", reportParameters.FurtherLoanMax);

            reportParams.Add("ReadvanceAmountRequested", reportParameters.ReadvanceRequired);
            reportParams.Add("FurtherAdvanceLoanAgreementAmountAmountRequested", reportParameters.FurtherAdvanceLAARequired);
            reportParams.Add("FurtherAdvanceBondAmountAmountRequested", reportParameters.FurtherAdvanceRequired);
            reportParams.Add("FurtherLoanAmountRequested", reportParameters.FurtherLoanRequired);

            reportParams.Add("NewLinkRate", reportParameters.NewLinkRate);
            reportParams.Add("FurtherBondAmount", reportParameters.FurtherBondAmount);
            reportParams.Add("FurtherLoanFees", reportParameters.FurtherLoanFees);
            reportParams.Add("ADUserName", reportParameters.ADUserName);

            var buffer = new StringBuilder();
            buffer.Append("../Correspondence/CorrespondenceModalPreview.aspx?");

            int count = 0;
            bool end = false;

            foreach (var key in reportParams.Keys)
            {
                if (count == reportParams.Count - 1) end = true;

                if (end)
                    buffer.AppendFormat("{0}={1}", key, reportParams[key]);
                else
                    buffer.AppendFormat("{0}={1}&", key, reportParams[key]);

                count++;
            }

            _reportPreviewURL = buffer.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            AddTrace(this, "Page_Load start");

            if (!ShouldRunPage) return;

            if (!_canUpdate && (ReadvanceInProgress || FurtherAdvanceInProgress || FurtherLoanInProgress))
            {
                btnBack.Visible = false;
                btnCalculate.Visible = false;
                btnGenerate.Visible = false;
                btnNext.Visible = false;
                btnReset.Visible = false;
                btnCancel.Visible = false;
                btnUpdateContact.Visible = false;
                ApplicationInProgressMessage.Visible = true;
                tabsMenu.ActiveTab = tpCalculator;
                CalculationTable.Visible = false;
                return;
            }

            if (_canUpdate)
                btnCalculate_Click(null, null);

            if (!Page.IsPostBack || Request.Form["__EVENTTARGET"].Equals(btnReset.UniqueID))
            {
                if (tbProduct.Text == Varifix)
                    btnNext.Enabled = true;

                tabsMenu.ActiveTabIndex = 0;
            }
            else
            {
                if (tbProduct.Text == Varifix && (tabsMenu.ActiveTab == tpCalculator || tabsMenu.ActiveTab == tpVarifix) && tbTotalCashRequired.Text.Length == 0)
                {
                    //Dont force calculate
                }
                else
                    btnCalculate_Click(null, null);

                if (Request.Form["__EVENTTARGET"].Equals(tabsMenu.ClientID))
                {
                    int activeTabIndex = Convert.ToInt16(Request.Params.Get("__EVENTARGUMENT"));
                    tabsMenu_ActiveTabChanged(activeTabIndex);
                }
            }

            tbTotalCashRequired.Attributes.Add("style", "display: none");
            lTotalCashRequired.Attributes.Add("style", "display: inline");

            if (!ReadvanceIsAccepted && (ApprovalMode == ApprovalTypes.None || ApprovalMode == ApprovalTypes.DeclineWithOffer))
            {
                tbReadvanceRequired.Attributes.Add("style", "display: inline");
                lReadvanceRequired.Attributes.Add("style", "display: none");
            }
            else
            {
                tbReadvanceRequired.Attributes.Add("style", "display: none");
                lReadvanceRequired.Attributes.Add("style", "display: inline");
            }

            if (!FurtherAdvanceIsAccepted && (ApprovalMode == ApprovalTypes.None || ApprovalMode == ApprovalTypes.DeclineWithOffer))
            {
                tbFurtherAdvReq.Attributes.Add("style", "display: inline");
                lFurtherAdvReq.Attributes.Add("style", "display: none");
            }
            else
            {
                tbFurtherAdvReq.Attributes.Add("style", "display: none");
                lFurtherAdvReq.Attributes.Add("style", "display: inline");
            }

            if (!FurtherLoanIsAccepted && (ApprovalMode == ApprovalTypes.None || ApprovalMode == ApprovalTypes.DeclineWithOffer))
            {
                tbFurtherLoanReq.Attributes.Add("style", "display: inline");
                lFurtherLoanReq.Attributes.Add("style", "display: none");
            }
            else
            {
                tbFurtherLoanReq.Attributes.Add("style", "display: none");
                lFurtherLoanReq.Attributes.Add("style", "display: inline");
            }

            tbSendApplication.Text = SendApplicationForms().ToString();
            AddTrace(this, "Page_Load end");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            AddTrace(this, "btnCalculate_Click start");
            if (Messages.HasErrorMessages)
                return;

            SetupForCalc();

            // Auto fill bond to register, if it has not yet been done,
            // this is a user editable field and should not be owverwritten if a value already exists
            // Calc bond to register for fees calcs in the base
            if (_furtherLoanRequired > 0 && tbBondToRegister.Text.Length == 0)
                _bondToRegister = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateBondAmount(_furtherLoanRequired);

            AppRepo.CreditDisqualifications(false, 0, 0, NewIncome, NewCurrentBalance + _capitalisedLife, LatestValuationAmount, EmploymentTypeKeySelected, true, 0, readvanceOnly);

            if (!Messages.HasErrorMessages)
                ValidateCalculator();

            if (!Messages.HasErrorMessages)
            {
                OnCalculateButtonClicked(sender, e);
                ShowCalculatedValues();
                SetTabControlNavigation();
                AppRepo.CreditDisqualifications(true, _newLTV, _newPTI, NewIncome, (NewCurrentBalance + _capitalisedLife + (_fees > 0 ? _fees : 0)), LatestValuationAmount, EmploymentTypeKeySelected, true, 0, readvanceOnly);
                SetReportPreviewURL();
            }
            else if (tbEstimatedValuationAmount.Text.Length > 0)
                ShowMaxCalculatedValues();

            // Auto fill bond to register, if it has not yet been done,
            // this is a user editable field and should not be owverwritten if a value already exists
            // This has been Recalc'd in the base presenter
            if (_furtherLoanRequired > 0 && tbBondToRegister.Text.Length == 0)
            {
                tbBondToRegister.Text = _bondToRegister.ToString();
            }

            AddTrace(this, "btnCalculate_Click end");
        }

        //This could be refactored to be more efficient, maybe next time....
        protected void SetupForCalc()
        {
            InputValuesToProperties();
            GetMaximumLoanAmount();
            CalculateLendingLimits();
            InputValuesToProperties();
        }

        private void DisplayNewPTI(double pti)
        {
            lblNewPTI.Text = pti > 0 ? pti.ToString(SAHL.Common.Constants.RateFormat) : "-";
            lblConfimrNewPTI.Text = pti > 0 ? pti.ToString(SAHL.Common.Constants.RateFormat) : "-";

            lblNewPTI.ForeColor = Color.Black;
            lblConfimrNewPTI.ForeColor = Color.Black;

            if ((EmploymentTypeKeySelected == (int)EmploymentTypes.SelfEmployed) && (pti > 0.25))//PTI > 25 %
            {
                lblNewPTI.ForeColor = Color.Red;
                lblConfimrNewPTI.ForeColor = Color.Red;
            }
            else if (_newPTI > 0.30)                //PTI > 30 %
            {
                lblNewPTI.ForeColor = Color.Red;
                lblConfimrNewPTI.ForeColor = Color.Red;
            }
        }

        private void DisplayLinkRate()
        {
            if (_newCurrentBalance > _totalLoanAgreeAmount)
            {
                lblNewLinkRate.ForeColor = Color.Red;

                //dont use the style attribute when approving in credit, this is used to hide the ddl
                //if (ApprovalMode != ApprovalTypes.Approve)
            }
            else
            {
                lblNewLinkRate.ForeColor = Color.Black;
                //dont use the style attribute when approving in credit, this is used to hide the ddl
                //if (ApprovalMode != ApprovalTypes.Approve)
            }
        }

        private void DisplayNewLTV(double ltv)
        {
            lblNewLTV.Text = ltv > 0 ? ltv.ToString(SAHL.Common.Constants.RateFormat) : "-";
            lblConfirmNewLTV.Text = ltv > 0 ? ltv.ToString(SAHL.Common.Constants.RateFormat) : "-";

            if (NewCategory.Value > _accCategory)
            {
                lblNewLTV.ForeColor = Color.Red;
                lblConfirmNewLTV.ForeColor = Color.Red;
            }
            else
            {
                lblNewLTV.ForeColor = Color.Black;
                lblConfirmNewLTV.ForeColor = Color.Black;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (!SendApplicationForms() && (tabsMenu.ActiveTab == tpLoanCondition))
                tabsMenu.ActiveTab = tpConfirmation; //no forms to be sent, on conditions tab, move past contact details tab
            else
            {
                if (tbProduct.Text != Varifix && tabsMenu.ActiveTab == tpCalculator) //step over Varifix panel
                    tabsMenu.ActiveTab = tpLoanCondition;
                else
                    tabsMenu.ActiveTab = tabsMenu.Tabs[tabsMenu.ActiveTabIndex + 1]; //set the next tab to be active
            }

            SetTabControlNavigation();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (!SendApplicationForms() && (tabsMenu.ActiveTab == tpConfirmation))
                tabsMenu.ActiveTab = tpLoanCondition; //no forms to be sent, on generate tab, move past contact details tab
            else
            {
                if (tbProduct.Text != Varifix && tabsMenu.ActiveTab == tpLoanCondition) //step over Varifix panel
                    tabsMenu.ActiveTab = tpCalculator;
                else
                    tabsMenu.ActiveTab = tabsMenu.Tabs[tabsMenu.ActiveTabIndex - 1]; //Set the previous tab to be active
            }

            SetTabControlNavigation();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdateContact_Click(object sender, EventArgs e)
        {
            // Copy any possibly updated values for use by the presenter
            _homePhoneCode = tbHomePhone.Code;
            _homePhoneNumber = tbHomePhone.Number;
            _workPhoneCode = tbWorkPhone.Code;
            _workPhoneNumber = tbWorkPhone.Number;
            _faxCode = tbFax.Code;
            _faxNumber = tbFax.Number;
            _cellNumber = tbCellNumber.Text;
            _email = tbEmail.Text;

            //the step below updates the LE with ne data but fails due to warning messages and does not commit the changes.

            // save contact info for LE
            OnContactUpdateButtonClicked(sender, e);

            // only reload the LE if there are no errors
            if (this.IsValid)
            {
                IReadOnlyEventList<ILegalEntity> leList = _account.GetLegalEntitiesByRoleType(this.Messages, new int[] { 2, 3 });
                BindApplicationInformation(leList, _account.Key);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            AddTrace(this, "btnGenerate_Click start");

            SetupForCalc();

            OnGenerateButtonClicked(sender, e);
            AddTrace(this, "btnGenerate_Click end");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            AddTrace(this, "btnSubmit_Click start");
            OnSubmitButtonClicked(sender, e);
            AddTrace(this, "btnSubmit_Click end");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnQuickCash_Click(object sender, EventArgs e)
        {
            AddTrace(this, "btnQuickCash_Click start");
            OnQuickCashButtonClicked(sender, e);
            AddTrace(this, "btnQuickCash_Click end");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tab"></param>
        protected void tabsMenu_ActiveTabChanged(int tab)
        {
            tabsMenu.ActiveTabIndex = tab;
            SetTabControlNavigation();
        }

        /// <summary>
        /// Reset all user affected controls back to the values they would be when the form is first loaded
        /// </summary>
        protected void ResetForm()
        {
            Navigator.Navigate("Reset");
        }

        /// <summary>
        /// Set the display to the newly calculated values
        /// </summary>
        protected void ShowCalculatedValues()
        {
            //Add life back in for display
            lblNewLoanBalance.Text = NewCurrentBalance > 0 ? (NewCurrentBalance + _capitalisedLife).ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            lblNewInstalment.Text = _newInstalment > 0 ? (_newInstalment).ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            lblNewAMInstalment.Text = _newAMInstalment > 0 ? (_newAMInstalment).ToString(SAHL.Common.Constants.CurrencyFormat) : "-";

            lbl20yrInstalmentText.Visible = showTwentyYearFigures;
            lbl20yrInstalment.Visible = showTwentyYearFigures;
            lbl20yrPTIText.Visible = showTwentyYearFigures;
            lbl20yrPTI.Visible = showTwentyYearFigures;
            if (showTwentyYearFigures)
            {
                lbl20yrInstalment.Text = twentyYearInstalment > 0 ? (twentyYearInstalment).ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
                lbl20yrPTI.Text = twentyYearPTI > 0 ? twentyYearPTI.ToString(SAHL.Common.Constants.RateFormat) : "-";
            }

            DisplayLinkRate();
            DisplayNewLTV(_newLTV);
            DisplayNewPTI(_newPTI);

            _newOccupancyTypeKey = ddlOccupancy.SelectedIndex;
            //PTI & LTV & group exposure
            double householdIncome = this.tbNewIncome1.Text.Length == 0 ? _householdIncome : double.Parse(this.tbNewIncome1.Text);
            this.lblGroupExposurePTI.Text = Math.Round(this.GroupExposureRepository.GetAccountGroupExposurePTI(SelectedLegalEntity.Key, householdIncome), 4).ToString(SAHL.Common.Constants.RateFormat);
            double latestValuationAmount = this.tbEstimatedValuationAmount.Text.Length == 0 || ApprovalMode != ApprovalTypes.None ? _latestValuationAmount : double.Parse(this.tbEstimatedValuationAmount.Text);
            double furtherLendingAmount = _totalCashRequired + _fees;
            this.lblGroupExposureLTV.Text = Math.Round(this.GroupExposureRepository.GetAccountGroupExposureLTV(this._accountKey, latestValuationAmount, furtherLendingAmount), 4).ToString(SAHL.Common.Constants.RateFormat);

            //Loan Rates.....
            double newLinkRate = this.CalculatedLinkRate;
            lblNewLoanRate.Text = (newLinkRate + _baseRateV + DiscountVariable).ToString(SAHL.Common.Constants.RateFormat);
            lblNewLinkRate.Text = (newLinkRate).ToString(SAHL.Common.Constants.RateFormat);

            _marginSelected = newLinkRate;

            lblNewSPV.Text = _newSPV.Description;
            lblFees.Text = _fees.ToString(SAHL.Common.Constants.CurrencyFormat);

            lTtlFLInclFees.Text = (_totalCashRequired + _fees).ToString(SAHL.Common.Constants.CurrencyFormat);

            lTTLBondRegAmount.Text = (_bondToRegister + _totalBondsRegistered).ToString(SAHL.Common.Constants.CurrencyFormat);
            lTTLLoanAgreeAmount.Text = ((NewCurrentBalance + CapitalisedLife) > _totalLoanAgreeAmount ? NewCurrentBalance + CapitalisedLife : _totalLoanAgreeAmount).ToString(SAHL.Common.Constants.CurrencyFormat);

            //available values; will only need to be loaded if the user has entered a new valuation amount
            if (tbEstimatedValuationAmount.Text.Length > 0)
                ShowMaxCalculatedValues();

            if (_showVarifix)
            {
                //Rates
                lVFCalcLinkRateFix.Text = (MarginSelected + DiscountFixed).ToString(SAHL.Common.Constants.RateFormat);
                lVFCalcLoanRateFix.Text = (MarginSelected + BaseRateFixed + DiscountFixed).ToString(SAHL.Common.Constants.RateFormat);

                lVFCalcLinkRateVar.Text = (MarginSelected + DiscountVariable).ToString(SAHL.Common.Constants.RateFormat);
                lVFCalcLoanRateVar.Text = (MarginSelected + BaseRateVariable + DiscountVariable).ToString(SAHL.Common.Constants.RateFormat);

                double runningTotal = (CurrentBalance + _capitalisedLife - _accruedInterest);

                lVFCalcReadvance.Text = _readvanceRequired.ToString(SAHL.Common.Constants.CurrencyFormat);
                runningTotal += _readvanceRequired;
                lVFCalcTotalPlusReadvance.Text = runningTotal.ToString(SAHL.Common.Constants.CurrencyFormat);

                lVFCalcFurtherAdvance.Text = _furtherAdvanceRequired.ToString(SAHL.Common.Constants.CurrencyFormat);
                runningTotal += _furtherAdvanceRequired;
                lVFCalcTotalPlusFurtherAdvance.Text = runningTotal.ToString(SAHL.Common.Constants.CurrencyFormat);

                lVFCalcFurtherLoan.Text = _furtherLoanRequired.ToString(SAHL.Common.Constants.CurrencyFormat);
                runningTotal += _furtherLoanRequired;
                lVFCalcTotalPlusFurtherLoan.Text = runningTotal.ToString(SAHL.Common.Constants.CurrencyFormat);

                lVFCalcTotalPlusInterestVar.Text = (NewCurrentBalance + CapitalisedLife - _currBalanceVF).ToString(SAHL.Common.Constants.CurrencyFormat);
                lVFCalcTotalPlusInterestTotal.Text = (NewCurrentBalance + CapitalisedLife).ToString(SAHL.Common.Constants.CurrencyFormat);

                double calcInstalFix = LoanCalculator.CalculateInstallment((_currBalanceVF - _accruedInterestFix), (MarginSelected + BaseRateFixed + DiscountFixed), RemainingTerm, false);
                double calcInstalVar = (LoanCalculator.CalculateInstallment((_currBalance - _currBalanceVF - _accruedInterestVar - CapitalisedLife + TotalCashRequired), (MarginSelected + BaseRateVariable + DiscountVariable), RemainingTerm, false));// + LifeInstalment

                lVFCalcInstalmentFix.Text = calcInstalFix.ToString(SAHL.Common.Constants.CurrencyFormat);
                lVFCalcInstalmentVar.Text = calcInstalVar.ToString(SAHL.Common.Constants.CurrencyFormat);
                lVFCalcInstalmentTotal.Text = (calcInstalFix + calcInstalVar).ToString(SAHL.Common.Constants.CurrencyFormat);

                double calcInstalFixAccrued = LoanCalculator.CalculateInstallment((_currBalanceVF), (MarginSelected + BaseRateFixed + DiscountFixed), RemainingTerm, false);
                double calcInstalVarAccrued = (LoanCalculator.CalculateInstallment((_currBalance - _currBalanceVF + TotalCashRequired), (MarginSelected + BaseRateVariable + DiscountVariable), RemainingTerm, false)); //+ LifeInstalment

                lVFCalcNewInstalmentFix.Text = calcInstalFixAccrued.ToString(SAHL.Common.Constants.CurrencyFormat); ;
                lVFCalcNewInstalmentVar.Text = calcInstalVarAccrued.ToString(SAHL.Common.Constants.CurrencyFormat); ;
                lVFCalcNewInstalmentTotal.Text = (calcInstalFixAccrued + calcInstalVarAccrued).ToString(SAHL.Common.Constants.CurrencyFormat); ;

                rowConfirmationFixRate.Visible = true;
            }
        }

        private void ShowMaxCalculatedValues()
        {
            double? InitiationFeeDiscount = null;
            double initFee = 0;
            double regFee = 0;
            double cancelFee = 0;
            double interimInterest = 0;
            double bondToRegister = 0;
            double totalFees = 0;

            //available values:
            double furtherLoanAmount = _furtherLoanMax > _estimatedFurtherLoanMax ? _furtherLoanMax : _estimatedFurtherLoanMax;
            double totalCashAvailable = _readvanceMax + _furtherAdvanceMax + furtherLoanAmount;
            double bondAmount = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateBondAmount(furtherLoanAmount);

            //_account
            AppRepo.CalculateOriginationFees(furtherLoanAmount, bondAmount, OfferTypes.FurtherLoan, 0, 0, true, NCACompliant, true, false, out InitiationFeeDiscount, out initFee, out regFee, out cancelFee, out interimInterest, out bondToRegister, false, 1, 1, 1, _accountKey, _account.Details.Any(x => x.DetailType.Key == (int)DetailTypes.StaffHomeLoan), DateTime.Now, false, false);
            totalFees = (NCACompliant ? 0 : initFee) + regFee + cancelFee + interimInterest;

            lblReAdvance.Text = _readvanceMax.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblFurtherAdvance.Text = _furtherAdvanceMax.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblFurtherLoan.Text = _furtherLoanMax.ToString(SAHL.Common.Constants.CurrencyFormat);
            if (tbEstimatedValuationAmount.Text.Length > 0)
                lblFurtherLoanClientEst.Text = _estimatedFurtherLoanMax.ToString(SAHL.Common.Constants.CurrencyFormat);

            lblTotalCashRequired.Text = totalCashAvailable.ToString(SAHL.Common.Constants.CurrencyFormat);
            lBondToRegister.Text = bondAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
            lFees.Text = totalFees.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblTtlFLInclFees.Text = (totalCashAvailable + totalFees).ToString(SAHL.Common.Constants.CurrencyFormat);
            lblCLV.Text = _committedLoanValue.ToString(SAHL.Common.Constants.CurrencyFormat);
        }

        /// <summary>
        /// Copy user inputs to teh view's properties so that we can get the values in the presenter
        /// </summary>
        protected void InputValuesToProperties()
        {
            _estimatedValuationAmount = tbEstimatedValuationAmount.Text.Length > 0 ? Convert.ToDouble(tbEstimatedValuationAmount.Text) : _latestValuationAmount;
            _employmentTypeKeySelected = ddlEmploymentType.SelectedItem.Value == "-select-" ? -1 : Convert.ToInt32(ddlEmploymentType.SelectedItem.Value);

            _newIncome1 = tbNewIncome1.Text.Length > 0 ? Convert.ToDouble(tbNewIncome1.Text) : 0;
            _totalCashRequired = tbTotalCashRequired.Text.Length > 0 ? Convert.ToDouble(tbTotalCashRequired.Text) : 0;
            _readvanceRequired = tbReadvanceRequired.Text.Length > 0 ? Convert.ToDouble(tbReadvanceRequired.Text) : 0;
            _furtherAdvanceRequired = tbFurtherAdvReq.Text.Length > 0 ? Convert.ToDouble(tbFurtherAdvReq.Text) : 0;
            _furtherLoanRequired = tbFurtherLoanReq.Text.Length > 0 ? Convert.ToDouble(tbFurtherLoanReq.Text) : 0;
            _bondToRegister = tbBondToRegister.Text.Length > 0 ? Convert.ToDouble(tbBondToRegister.Text) : 0;
            _newIncome1 = tbNewIncome1.Text.Length > 0 ? Convert.ToDouble(tbNewIncome1.Text) : 0;

            tbTotalCashRequired.Text =
                ((tbReadvanceRequired.Text.Length > 0 ? Convert.ToDouble(tbReadvanceRequired.Text) : 0) +
                (tbFurtherAdvReq.Text.Length > 0 ? Convert.ToDouble(tbFurtherAdvReq.Text) : 0) +
                (tbFurtherLoanReq.Text.Length > 0 ? Convert.ToDouble(tbFurtherLoanReq.Text) : 0)).ToString();

            _newCurrentBalance = _currBalance + (tbTotalCashRequired.Text.Length > 0 ? Convert.ToDouble(tbTotalCashRequired.Text) : 0) + _fees;

            //copy tb's to labels
            lTotalCashRequired.Text = _totalCashRequired.ToString(SAHL.Common.Constants.CurrencyFormat);

            lReadvanceRequired.Text = _readvanceRequired.ToString(SAHL.Common.Constants.CurrencyFormat);
            lFurtherAdvReq.Text = _furtherAdvanceRequired.ToString(SAHL.Common.Constants.CurrencyFormat);
            lFurtherLoanReq.Text = _furtherLoanRequired.ToString(SAHL.Common.Constants.CurrencyFormat);
        }

        /// <summary>
        /// Calculate Max values for each type of further lending
        /// </summary>
        protected void CalculateLendingLimits()
        {
            //Further Loan can not be greater than the new Bond Amount.
            //Any additional further lending must be done as an Advance.
            double accruedInterestEstimate;
            double accruedInterestleft = _accruedInterest;

            //accrued interest is in the _currBalance, must subtract it
            double loanCurrentBalance = (_currBalance + _capitalisedLife - _accruedInterest);

            //readvance
            _committedLoanValue = _account.CommittedLoanValue;
            double readvance = _committedLoanValue - loanCurrentBalance;

            DateTime dt = DateTime.Now;

            IControl ctrl = CtrlRepo.GetControlByDescription("CLVReadvanceStart");
            if (ctrl != null && ctrl.ControlText.Length > 0)
                dt = Convert.ToDateTime(ctrl.ControlText);

            //if we need to use old school calc's then
            if ((ReadvanceStartDate.HasValue ? ReadvanceStartDate.Value : DateTime.Now) < dt)
            {
                readvance = (_totalLoanAgreeAmount - loanCurrentBalance);
                tblFAMessage.Visible = false;
            }

            if (readvance < 0) readvance = 0; //negative values are not valid and must be zero'd out

            if (readvance >= 0)
            {
                //we have a readvance facility but need to check accrued interest
                if (readvance > accruedInterestleft)
                {
                    _readvanceMax = readvance - accruedInterestleft;
                    accruedInterestleft = 0;
                }
                else
                {
                    _readvanceMax = 0;
                    accruedInterestleft -= readvance;
                }
            }

            //Further Advance
            //Use the lower of the valuation or total bond registration amount
            double maxtotal = LatestValuationAmount > _totalBondsRegistered ? _totalBondsRegistered : LatestValuationAmount;

            //Calc FA on the lower value
            //need to check the Further Advance against the CM also
            //using the higher of Est amount and Existing amount
            double furtherAdvanceCM = (_estimatedMaxLoanAmount > _existingMaxLoanAmount ? _estimatedMaxLoanAmount : _existingMaxLoanAmount) - loanCurrentBalance - readvance;

            double furtherAdvance = maxtotal - loanCurrentBalance - readvance;

            //Make sure that the FA amount is less than the total amount as per the Credit Matrix
            if (furtherAdvance > furtherAdvanceCM)
                furtherAdvance = furtherAdvanceCM;

            if (furtherAdvance < 0) furtherAdvance = 0; //negative values are not valid and must be zero'd out

            if (furtherAdvance >= 0)
            {
                //we have a further advance facility but need to check against accrued interest remaining
                if (furtherAdvance > accruedInterestleft)
                {
                    //when calculating available funds for FA up to LAA
                    //we need to use the LAA minus the higher of the CLV/Balance
                    double availableFAToLAA = _committedLoanValue > loanCurrentBalance ? _committedLoanValue : loanCurrentBalance;

                    //calculate the FA amount up to the CLV
                    //this is any positive difference between the LAA minus availableFAToLAA
                    _furtherAdvanceMaxLAA = _totalLoanAgreeAmount > availableFAToLAA ? _totalLoanAgreeAmount - availableFAToLAA - accruedInterestleft : 0;
                    _furtherAdvanceMaxLAA = _furtherAdvanceMaxLAA > 0 ? _furtherAdvanceMaxLAA : 0;

                    _furtherAdvanceMax = furtherAdvance - accruedInterestleft;
                    accruedInterestleft = 0;
                }
                else
                {
                    _furtherAdvanceMax = 0;
                    _furtherAdvanceMaxLAA = 0;
                    accruedInterestleft -= furtherAdvance;
                }
            }

            accruedInterestEstimate = accruedInterestleft; // estFLMax is duplication of FLMax and should use the same values

            //Further Loan
            double furtherLoan = _existingMaxLoanAmount - loanCurrentBalance - furtherAdvance - readvance;
            if (furtherLoan < 0) furtherLoan = 0; //negative values are not valid and must be zero'd out

            if (furtherLoan >= 0)
            {
                //we have a further loan facility but need to check against accrued interest remaining
                if (furtherLoan > accruedInterestleft)
                {
                    _furtherLoanMax = furtherLoan - accruedInterestleft;
                    accruedInterestleft = 0;
                }
                else
                {
                    _furtherLoanMax = 0;
                    accruedInterestleft -= furtherLoan;
                }
            }

            //Further Loan Client Estimate
            double furtherLoanClentEst = _estimatedMaxLoanAmount - loanCurrentBalance - furtherAdvance - readvance;
            if (furtherLoanClentEst < 0) furtherLoanClentEst = 0; //negative values are not valid and must be zero'd out

            if (furtherLoanClentEst > 0)
            {
                //we have a further loan facility but need to check against accrued interest remaining
                if (furtherLoanClentEst > accruedInterestleft)
                {
                    _estimatedFurtherLoanMax = furtherLoanClentEst - accruedInterestleft;
                    accruedInterestleft = 0;
                }
                else
                {
                    _estimatedFurtherLoanMax = 0;
                    accruedInterestleft = accruedInterestEstimate - furtherLoanClentEst;
                }
            }
        }

        /// <summary>
        /// Validation check for the view
        /// </summary>
        /// <returns></returns>
        private void ValidateCalculator()
        {
            if (Messages.HasErrorMessages)
                return;

            AddTrace(this, "ValidateCalculator start");
            string err;

            IControl ctrl = CtrlRepo.GetControlByDescription("Calc - maxLAA");
            double maxLAA = (ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric) : 0);

            double maxFurtherLoanEstimate = tbEstimatedValuationAmount.Text.Length > 0 ? _estimatedFurtherLoanMax : _furtherLoanMax;
            double maxLending = Math.Round((Math.Round(_readvanceMax, 2) + Math.Round(_furtherAdvanceMax, 2) + Math.Round(maxFurtherLoanEstimate, 2)), 2);

            if (_employmentTypeKeySelected < 0 || _employmentTypeKeySelected > 4)
            {
                err = "Please select a valid employment type.";
                Messages.Add(new Error(err, err));
            }

            if (_marginKeySelected < 0)
            {
                err = "Please select a rate.";
                Messages.Add(new Error(err, err));
            }

            if (_totalCashRequired <= 0)
            {
                err = "Please capture a cash amount required greater than R 0.00.";
                Messages.Add(new Error(err, err));
            }
            else
            {
                if (_totalCashRequired > maxLending + 1)
                {
                    RunRule("ExceedFurtherLendingLimit", maxLending);
                }
                if (_totalCashRequired + _currBalance > maxLAA)
                {
                    RunRule("ExceedFurtherLendingLimitLAA", (maxLAA - _currBalance));
                }
            }

            if (_readvanceRequired > _readvanceMax + 1)
            {
                RunRule("ExceedReadvanceLimit", _readvanceMax);
                //Warning //*** we dont want to stop the process we just want to show the problem...
                err = "Readvance requested is more than the allowed " + _readvanceMax.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            //if (_furtherAdvanceRequired > (_furtherAdvanceMax + _readvanceNotUsed) + 1)
            if (_furtherAdvanceRequired > (_furtherAdvanceMax + 1))
            {
                RunRule("ExceedFurtherAdvanceLimit", _furtherAdvanceMax);
                //Warning //*** we dont want to stop the process we just want to show the problem...
            }

            if (_furtherLoanRequired > (maxFurtherLoanEstimate + 1))
            {
                RunRule("ExceedFurtherLoanLimit", maxFurtherLoanEstimate);
                //Warning //*** we dont want to stop the process we just want to show the problem...
                err = "Further loan requested is more than the allowed " + maxFurtherLoanEstimate.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            if (_bondToRegister > 0 && _furtherLoanRequired <= 0)
            {
                RunRule("NoBondRequired", null);
                //Warning //*** we dont want to stop the process we just want to show the problem...
                err = "No further loan is required but a bond to register has been entered.";
            }

            if (_furtherLoanRequired > _bondToRegister)
            {
                RunRule("BondLessThanFurtherLoan", null);
                //Warning //*** we dont want to stop the process we just want to show the problem...
                err = "The Bond amount can not be less than the further loan amount.";
            }

            //CheckMaxLTP
            if (_accAppType == (int)OfferTypes.NewPurchaseLoan)
                RunRule("CheckMaxLTP", new object[] { _newLTP, _accOpenDate });

            var mortgageLoanAccount = _account as IMortgageLoanAccount;
            if (_account.Details.Any(x => x.DetailType.Key == (int)DetailTypes.AlphaHousing))
            {
                //hack to not run the rule if the _marginSelected has not been set yet, alternately just remove the rule.
                //left in for belts and braces
                if (_marginSelected != 0D)
                    RunRule("AlphaHousingLinkRateCanNotBeLowerThanExistingLinkRate", new object[] { mortgageLoanAccount.SecuredMortgageLoan.RateConfiguration.Margin.Value, _marginSelected });
            }

            AddTrace(this, "ValidateCalculator end");
        }

        /// <summary>
        /// Validation check for contact to send application to
        /// </summary>
        /// <returns></returns>
        private bool ValidateAppInformation()
        {
            int ErrorCount = 0;

            if (!SendApplicationForms())
                return true;

            // Check if correspondence hidden field contains data
            if (string.IsNullOrEmpty(tbMultiRecipientCorrespondenceData.Text))
            {
                ErrorCount++;
            }

            if (ErrorCount > 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Bind the Condition and Subsidy grid views for the account
        /// </summary>
        /// <param name="Account"></param>
        protected void BindConditionAndSubsidy(IAccount Account)
        {
            // Loan conditions check box
            LoanConditionsJavascript();

            //Detail
            IReadOnlyEventList<IDetail> listDetail = AccRepo.GetAccountDetailForFurtherLending(Account.Key);
            List<string> detailList = new List<string>();

            if (listDetail != null && listDetail.Count > 0)
            {
                foreach (IDetail det in listDetail)
                {
                    detailList.Add(det.DetailType.Description + (String.IsNullOrEmpty(det.Description) ? "" : " - " + det.Description));
                }
            }

            gvDetail.Columns.Clear();
            gvDetail.AddGridBoundColumn("!", "Details", Unit.Percentage(100), HorizontalAlign.Left, true);
            gvDetail.DataSource = detailList;
            gvDetail.DataBind();

            //Subsidy
            IEventList<ISubsidy> subsidy = Account.Subsidies;
            List<string> subProvider = new List<string>();
            if (subsidy != null && subsidy.Count > 0)
            {
                foreach (ISubsidy sub in subsidy)
                {
                    subProvider.Add(sub.SubsidyProvider.LegalEntity.DisplayName);
                }
            }

            gvSubsidy.Columns.Clear();
            gvSubsidy.AddGridBoundColumn("!", "Subsidy Provider", Unit.Percentage(100), HorizontalAlign.Left, true);
            gvSubsidy.DataSource = subProvider;
            gvSubsidy.DataBind();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LegalEntityGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            LegalEntityContactData_Populate();
        }

        protected void LegalEntityGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var legalEntityKey = e.Row.Cells[0].Text;
                var faxNumber = e.Row.Cells[(int)SAHL.Web.Controls.LegalEntityGrid.GridColumns.FaxCode].Text + e.Row.Cells[(int)SAHL.Web.Controls.LegalEntityGrid.GridColumns.FaxNumber].Text;
                CheckBox chkBoxFax = (CheckBox)e.Row.FindControl("Fax");
                var faxCorrespondenceMediumKey = (int)SAHL.Common.Globals.CorrespondenceMediums.Fax;
                chkBoxFax.Attributes.Add("onclick", "submitMultipleRecipients('" + legalEntityKey + "', '" + faxCorrespondenceMediumKey.ToString() + "', '" + faxNumber + "', this)");
                chkBoxFax.Attributes.Add("class", "checkBoxClass");
                if (String.IsNullOrEmpty(faxNumber) || faxNumber.Contains("&nbsp;"))
                {
                    chkBoxFax.Enabled = false;
                }

                var emailAddress = e.Row.Cells[(int)SAHL.Web.Controls.LegalEntityGrid.GridColumns.EmailAddress].Text;
                CheckBox chkBoxEmail = (CheckBox)e.Row.FindControl("Email");
                var emailCorrespondenceMediumKey = (int)SAHL.Common.Globals.CorrespondenceMediums.Email;
                chkBoxEmail.Attributes.Add("onclick", "submitMultipleRecipients('" + legalEntityKey + "', '" + emailCorrespondenceMediumKey.ToString() + "', '" + emailAddress + "', this)");
                if (String.IsNullOrEmpty(emailAddress) || emailAddress == "&nbsp;")
                {
                    chkBoxEmail.Enabled = false;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected void LegalEntityContactData_Populate()
        {
            ILegalEntity selectedLE = LegalEntityGrid.SelectedLegalEntity;
            tbHomePhone.Code = selectedLE.HomePhoneCode;
            tbHomePhone.Number = selectedLE.HomePhoneNumber;

            tbWorkPhone.Code = selectedLE.WorkPhoneCode;
            tbWorkPhone.Number = selectedLE.WorkPhoneNumber;

            tbFax.Code = selectedLE.FaxCode;
            tbFax.Number = selectedLE.FaxNumber;

            tbCellNumber.Text = selectedLE.CellPhoneNumber;

            tbEmail.Text = selectedLE.EmailAddress;

            tbSelectedLegalEntityKey.Text = selectedLE.Key.ToString();

            LegalEntityAlternateContactData_Populate(selectedLE);
        }

        private void LegalEntityAlternateContactData_Populate(ILegalEntity selectedLE)
        {
            if (!string.IsNullOrEmpty(tbMultiRecipientCorrespondenceData.Text))
            {
                var multiRecipeintsCorrespondenceSelection = JsonConvert.DeserializeObject<RecipientCorrespondenceSelection[]>(tbMultiRecipientCorrespondenceData.Text);
                var existingAlternateFaxNumber = multiRecipeintsCorrespondenceSelection.FirstOrDefault(x => x.LegalEntityKey == selectedLE.Key && x.IsUsingAlternateContactInfo == true && x.CorrespondenceMediumKey == (int)CorrespondenceMediums.Fax);
                tbAlternateFaxNumber.Text = existingAlternateFaxNumber != null ? existingAlternateFaxNumber.ContactInfo : string.Empty;
                var existingAlternateEmail = multiRecipeintsCorrespondenceSelection.FirstOrDefault(x => x.LegalEntityKey == selectedLE.Key && x.IsUsingAlternateContactInfo == true && x.CorrespondenceMediumKey == (int)CorrespondenceMediums.Email);
                tbAlternateEmail.Text = existingAlternateEmail != null ? existingAlternateEmail.ContactInfo : string.Empty;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="lstLegalEntities"></param>
        /// <param name="accountKey"></param>
        protected void BindApplicationInformation(IReadOnlyEventList<ILegalEntity> lstLegalEntities, int accountKey)
        {
            AddTrace(this, "BindApplicationInformation start");

            // Setup the LegalEntityGrid
            LegalEntityGrid.HeaderCaption = "Applicant Details";

            //LegalEntityGrid.GridHeight = 100;
            LegalEntityGrid.ColumnIDPassportVisible = true;
            LegalEntityGrid.ColumnIDPassportHeadingType = SAHL.Web.Controls.LegalEntityGrid.GridColumnIDPassportHeadingType.IDAndPassportNumber;
            LegalEntityGrid.ColumnRoleVisible = true;
            LegalEntityGrid.ColumnLegalEntityStatusVisible = true;
            LegalEntityGrid.ColumnEmailAddressVisible = true;
            LegalEntityGrid.ColumnFaxCodeVisible = true;
            LegalEntityGrid.ColumnFaxNumberVisible = true;
            LegalEntityGrid.ColumnCheckBoxEmailVisible = true;
            LegalEntityGrid.ColumnCheckboxFaxVisible = true;
            // Set the Data related properties
            LegalEntityGrid.AccountKey = accountKey;

            // Bind the grid
            LegalEntityGrid.BindLegalEntities(lstLegalEntities);

            // Populate contact data for editing
            LegalEntityContactData_Populate();
            AddTrace(this, "BindApplicationInformation end");
        }

        /// <summary>
        /// Register client side script for the Loan Conditions panel
        /// </summary>
        protected void LoanConditionsJavascript()
        {
            chkCondition.Attributes.Add("onclick", "loanConditionsOnClick();");
        }

        /// <summary>
        ///
        /// </summary>
        protected void SetTabControlNavigation()
        {
            if (!IsPostBack)
                tabsMenu.ActiveTabIndex = 0;

            //if an rcs user then dont activate other tabs
            if (OriginationSourceHelper.PrimaryOriginationSourceKey(this.CurrentPrincipal) == (int)SAHL.Common.Globals.OriginationSources.RCS)
                return;

            if (!Messages.HasErrorMessages)
            {
                if (ApprovalMode == ApprovalTypes.None)
                {
                    if (tabsMenu.ActiveTabIndex == 0)
                    {
                        // Calculate and Reset buttons display on the first tab
                        btnCalculate.Attributes.Add("style", "display: inline");
                        btnReset.Attributes.Add("style", "display: inline");

                        //First tab, so no back button
                        btnBack.Enabled = false;
                        btnBack.Attributes.Add("style", "display: none");
                    }

                    if (tabsMenu.ActiveTabIndex > 0)
                    {
                        // Hide Calculate and Reset buttons if not on the first tab
                        btnCalculate.Attributes.Add("style", "display: none");
                        btnReset.Attributes.Add("style", "display: none");

                        // Any tab but the first one can always have a back button
                        btnBack.Enabled = true;
                        btnBack.Attributes.Add("style", "display: inline");
                    }

                    if (tabsMenu.ActiveTabIndex == tabsMenu.Tabs.Count - 1) //Last tab, no next button
                        btnNext.Attributes.Add("style", "display: none");
                    else
                        btnNext.Attributes.Add("style", "display: inline"); //All other tabs have a next button

                    // Set to disabled here as default and set to enabled if applicable in the code below
                    if (tabsMenu.ActiveTabIndex == 0 && tbProduct.Text == Varifix) // Always allow to switch to Varifix tab if varifix
                        btnNext.Enabled = true;
                    else
                        btnNext.Enabled = false;

                    if (tbProduct.Text != Varifix)
                        tpVarifix.Enabled = false;

                    btnUpdateContact.Attributes.Add("style", "display: none");
                    btnGenerate.Attributes.Add("style", "display: none");

                    tpLoanCondition.Enabled = false;
                    tpInformation.Enabled = false;
                    tpConfirmation.Enabled = false;

                    if (lblNewInstalment.Text.Length > 1)   //Calculations have been done, allow move to Details and Conditions
                    {
                        if ((tabsMenu.ActiveTab == tpCalculator) || tabsMenu.ActiveTab == tpVarifix)
                            btnNext.Enabled = true;

                        tpLoanCondition.Enabled = true;

                        if (chkCondition.Checked)           //Conditions checked, allow to move to App Info
                        {
                            if (SendApplicationForms())
                            {
                                if (tabsMenu.ActiveTab == tpLoanCondition)
                                    btnNext.Enabled = true;
                                if (tabsMenu.ActiveTab == tpInformation)
                                    btnUpdateContact.Attributes.Add("style", "display: inline");

                                tpInformation.Enabled = true;
                            }

                            if (ValidateAppInformation())// At least one of the send to options have been selected and have data captured
                            {
                                if (tabsMenu.ActiveTab == tpInformation)
                                    btnNext.Enabled = true;

                                tpConfirmation.Enabled = true;

                                if (tabsMenu.ActiveTab == tpConfirmation) // populate generate application panel
                                {
                                    PopulateConfirmationDetail();
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool SendApplicationForms()
        {
            chkIncludeNaedoForm.Enabled = false;

            if (_canUpdate)
            {
                tpConfirmation.HeaderText = "Update Application";
                btnGenerate.Text = "Update Application";
            }

            if (SendReadvanceApplicationForm || SendFurtherAdvanceApplicationForm || SendFurtherLoanApplicationForm)
            {
                chkIncludeNaedoForm.Enabled = true;
                return true;
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        private void GetMaximumLoanAmount()
        {
            IFinancialsService finsS = ServiceFactory.GetService<IFinancialsService>();

            if (_employmentTypeKeySelected > 0 && _employmentTypeKeySelected != (int)EmploymentTypes.Unknown)
                _existingMaxLoanAmount = finsS.GetMaxLoanAmountForFurtherLending(this.Messages, _originationSourceKey, _productKey, _mortgageLoanPurposeKey, _employmentTypeKeySelected, _latestValuationAmount);
            else //temporarily use salaried
                _existingMaxLoanAmount = finsS.GetMaxLoanAmountForFurtherLending(this.Messages, _originationSourceKey, _productKey, _mortgageLoanPurposeKey, (int)EmploymentTypes.Salaried, _latestValuationAmount);

            _maxLoanAmount = _existingMaxLoanAmount;
            // if there is a new Valuation Amount then get the Max Loan Amount for the client estimate
            if (_estimatedValuationAmount > 0)
            {
                _estimatedMaxLoanAmount = finsS.GetMaxLoanAmountForFurtherLending(this.Messages, _originationSourceKey, _productKey, _mortgageLoanPurposeKey, _employmentTypeKeySelected, _estimatedValuationAmount);
                _maxLoanAmount = _estimatedMaxLoanAmount;
            }
            else
            {
                _estimatedMaxLoanAmount = _maxLoanAmount;
            }
        }

        /// <summary>
        /// Populate the confirmation TabPanel with relevant detail
        /// </summary>
        private void PopulateConfirmationDetail()
        {
            btnGenerate.Attributes.Add("style", "display: inline");

            //Create Application
            lblAccountNumber.Text = _accountKey.ToString();

            string products = _productDescription;
            if (_hasInvokedCap) products += ", CAP";
            if (_isInterestOnly) products += ", Interest Only";
            lblAccountProduct.Text = products;

            lblCreatedDate.Text = DateTime.Now.ToString();
            lblInitiatedBy.Text = LegalEntityGrid.SelectedLegalEntity.DisplayName;
            lblCreatedUser.Text = CurrentPrincipal.Identity.Name;
            lblTotalAmountRequired.Text = _totalCashRequired.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblRapidRequired.Text = _readvanceRequired.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblFurtherAdvanceRequired.Text = _furtherAdvanceRequired.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblFurtherLoanRequired.Text = _furtherLoanRequired.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblLegalFeeEstimate.Text = _fees.ToString(SAHL.Common.Constants.CurrencyFormat);

            // Convert the multiRecipientData into object
            if (!string.IsNullOrEmpty(tbMultiRecipientCorrespondenceData.Text))
            {
                var multiRecipeintsCorrespondenceSelection = JsonConvert.DeserializeObject<RecipientCorrespondenceSelection[]>(tbMultiRecipientCorrespondenceData.Text);
                rptSendingInformation.DataSource = multiRecipeintsCorrespondenceSelection;
                rptSendingInformation.DataBind();
            }

            // Calculated Amounts
            lblConfimNewLoanBalance.Text = NewCurrentBalance > 0 ? (NewCurrentBalance + CapitalisedLife).ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            lblConfirmNewInstalment.Text = _newInstalment > 0 ? (_newInstalment).ToString(SAHL.Common.Constants.CurrencyFormat) : "-";

            DisplayNewLTV(_newLTV);
            DisplayNewPTI(_newPTI);

            //lblConfirmNewLoanRate.Text = "TODO";
            lblConfirmNewLoanRateV.Text = (_marginSelected + _baseRateV + _discountV).ToString(SAHL.Common.Constants.RateFormat); ;
            lblConfirmNewLoanRateF.Text = (_marginSelected + _baseRateF + _discountF).ToString(SAHL.Common.Constants.RateFormat);
            lblConfirmNewLinkRate.Text = (_marginSelected).ToString(SAHL.Common.Constants.RateFormat);
        }

        protected void rptSendingInformation_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var sendingInformationRow = (RecipientCorrespondenceSelection)e.Item.DataItem;
                if (sendingInformationRow.LegalEntityKey > 0)
                {
                    ILegalEntityRepository legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                    var lblContactName = (SAHLLabel)e.Item.FindControl("SAHLlblDisplayName");
                    lblContactName.Text = legalEntityRepo.GetLegalEntityByKey(sendingInformationRow.LegalEntityKey).DisplayName;
                }

                var lblCorrespondenceMedium = (SAHLLabel)e.Item.FindControl("SAHLlblCorrespondenceMedium");
                lblCorrespondenceMedium.Text = ((CorrespondenceMediums)sendingInformationRow.CorrespondenceMediumKey).ToString();

                var lblContactInfo = (SAHLLabel)e.Item.FindControl("SAHLlblContactInfo");
                lblContactInfo.Text = sendingInformationRow.ContactInfo;
            }
        }

        private IControlRepository CtrlRepo
        {
            get
            {
                if (_ctrlRepo == null)
                    _ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();

                return _ctrlRepo;
            }
        }

        private IApplicationRepository AppRepo
        {
            get
            {
                if (_appRepo == null)
                    _appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                return _appRepo;
            }
        }

        private IAccountRepository AccRepo
        {
            get
            {
                if (_accRepo == null)
                    _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                return _accRepo;
            }
        }

        private ILookupRepository LookupRepo
        {
            get
            {
                if (_lookupRepo == null)
                    _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lookupRepo;
            }
        }

        private ILoanTransactionRepository LTRepo
        {
            get
            {
                if (_ltRepo == null)
                    _ltRepo = RepositoryFactory.GetRepository<ILoanTransactionRepository>();

                return _ltRepo;
            }
        }

        private IRuleService RuleServ
        {
            get
            {
                if (_ruleServ == null)
                    _ruleServ = ServiceFactory.GetService<IRuleService>();

                return _ruleServ;
            }
        }

        private IHOCRepository HOCRepository
        {
            get
            {
                if (_hocRepo == null)
                    _hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
                return _hocRepo;
            }
        }

        private ILifeRepository LifeRepository
        {
            get
            {
                if (_lifeRepo == null)
                    _lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
                return _lifeRepo;
            }
        }

        private IGroupExposureRepository GroupExposureRepository
        {
            get
            {
                if (_groupExposureRepo == null)
                    _groupExposureRepo = RepositoryFactory.GetRepository<IGroupExposureRepository>();
                return _groupExposureRepo;
            }
        }

        private void RunRule(string Rule, params object[] prms)
        {
            RuleServ.ExecuteRule(Messages, Rule, prms);
        }

        private bool readvanceOnly
        {
            get
            {
                if (_furtherAdvanceRequired > 0 || _furtherLoanRequired > 0)
                    return false;

                return true;
            }
        }

        #region IFurtherLoanCalculator Members

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnContactUpdateButtonClicked;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnGenerateButtonClicked;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnCalculateButtonClicked;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnQuickCashButtonClicked;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        ///
        /// </summary>
        /// <param name="vML"></param>
        /// <param name="fML"></param>
        /// <param name="Account"></param>
        /// <param name="app"></param>
        /// <param name="Reset"></param>
        public void BindDisplay(IMortgageLoan vML, IMortgageLoan fML, IAccount Account, IApplication app, bool Reset)
        {
            AddTrace(this, "BindDisplay start");

            _account = Account;
            _accountKey = Account.Key;
            _accOpenDate = Account.OpenDate.HasValue ? Account.OpenDate.Value : DateTime.Now;
            lblAdvancesThisYear.Text = LTRepo.GetAdvancesDisbursedThisYearByAccountKey(_accountKey).ToString();

            _productDescription = Account.Product.Description;
            //Loan Subsidies and Conditions
            BindConditionAndSubsidy(Account);

            lblProduct.Text = Account.Product.Description;

            foreach (IAccount accRelated in Account.RelatedChildAccounts.Where(x => x.AccountStatus.Key == (int)AccountStatuses.Open))
            {
                string premium = "";
                if (accRelated.Product.Key == (int)Products.HomeOwnersCover)
                {
                    premium = this.HOCRepository.GetMonthlyPremium(accRelated.Key).ToString(SAHL.Common.Constants.CurrencyFormat);
                }

                if (accRelated.Product.Key == (int)Products.LifePolicy)
                {
                    premium = this.LifeRepository.GetMonthlyPremium(accRelated.Key).ToString(SAHL.Common.Constants.CurrencyFormat);
                }
                HtmlTableRow htmlRow = new HtmlTableRow();
                htmlRow.Attributes.Add("class", "rowSmall");
                HtmlTableCell htmlLabel = new HtmlTableCell();
                htmlLabel.Attributes.Add("class", "TitleText");
                htmlLabel.InnerText = accRelated.Product.Description;
                HtmlTableCell htmlValue = new HtmlTableCell();
                htmlValue.Style.Add("text-align", "right");
                htmlValue.InnerText = premium;
                htmlRow.Cells.Add(htmlLabel);
                htmlRow.Cells.Add(htmlValue);
                relatedChildAccounts.Controls.Add(htmlRow);
            }

            HtmlTableRow ohtmlRow = new HtmlTableRow();
            ohtmlRow.Attributes.Add("class", "rowSmall");
            HtmlTableCell ohtmlLabel = new HtmlTableCell();
            ohtmlLabel.Attributes.Add("class", "TitleText");
            ohtmlLabel.InnerText = "Service Fee";
            HtmlTableCell ohtmlValue = new HtmlTableCell();
            ohtmlValue.Style.Add("text-align", "right");
            ohtmlValue.InnerText = Account.InstallmentSummary.MonthlyServiceFee.ToString(SAHL.Common.Constants.CurrencyFormat); ;
            ohtmlRow.Cells.Add(ohtmlLabel);
            ohtmlRow.Cells.Add(ohtmlValue);
            relatedChildAccounts.Controls.Add(ohtmlRow);

            if (Account.FinancialServices[0].Category != null)
            {
                lCategoryAccount.Text = Account.FinancialServices[0].Category.Description;
                _accCategory = Account.FinancialServices[0].Category.Value;
            }
            else
            {
                lCategoryAccount.Text = "No Category";
            }

            //ApplicationInformation
            IReadOnlyEventList<ILegalEntity> leList = Account.GetLegalEntitiesByRoleType(this.Messages, new int[] { 2, 3 });
            BindApplicationInformation(leList, Account.Key);

            //Calculator
            if (fML != null)
            {
                _showVarifix = true;
                _currBalance = Convert.ToDouble(vML.CurrentBalance + (vML.AccruedInterestMTD.HasValue ? vML.AccruedInterestMTD : 0) + fML.CurrentBalance + (fML.AccruedInterestMTD.HasValue ? fML.AccruedInterestMTD : 0));
                _currBalanceVF = Convert.ToDouble(fML.CurrentBalance + (fML.AccruedInterestMTD.HasValue ? fML.AccruedInterestMTD : 0));
                _arrearsFix = fML.ArrearBalance;
                _arrearsVar = vML.ArrearBalance;
                _arrearsBalance = _arrearsVar + _arrearsFix;

                _accruedInterestVar = (vML.AccruedInterestMTD.HasValue ? vML.AccruedInterestMTD.Value : 0);
                _accruedInterestFix = (fML.AccruedInterestMTD.HasValue ? fML.AccruedInterestMTD.Value : 0);

                _accruedInterest = _accruedInterestVar + _accruedInterestFix;
                _currInstalment = vML.Payment + fML.Payment - LifeInstalment;
                _baseRateF = fML.ActiveMarketRate;
                _discountF = fML.RateAdjustment;

                _totalBondsRegistered = vML.SumBondRegistrationAmounts() + fML.SumBondRegistrationAmounts();
                _totalLoanAgreeAmount = vML.SumBondLoanAgreementAmounts() + fML.SumBondLoanAgreementAmounts();
            }
            else
            {
                _currBalance = Convert.ToDouble(vML.CurrentBalance + (vML.AccruedInterestMTD.HasValue ? vML.AccruedInterestMTD : 0));
                _arrearsBalance = vML.ArrearBalance;
                _accruedInterest = vML.AccruedInterestMTD.HasValue ? vML.AccruedInterestMTD.Value : 0;
                _currInstalment = vML.Payment - LifeInstalment;
                _totalBondsRegistered = vML.SumBondRegistrationAmounts();
                _totalLoanAgreeAmount = vML.SumBondLoanAgreementAmounts();
            }

            //Subtract any capitalised life before doing any calcs, and add the life instalment back to the monthly instalment
            IMortgageLoanAccount mortgageLoanAccount = Account as IMortgageLoanAccount;
            if (mortgageLoanAccount != null)
            {
                if (mortgageLoanAccount.LifePolicyAccount != null)
                {
                    foreach (IFinancialService fs in mortgageLoanAccount.LifePolicyAccount.FinancialServices)
                    {
                        if (fs.AccountStatus.Key == (int)AccountStatuses.Open || fs.AccountStatus.Key == (int)AccountStatuses.Dormant)
                        {
                            _lifeInstalment = fs.Payment;
                            break;
                        }
                    }
                }

                _capitalisedLife = mortgageLoanAccount.CapitalisedLife;
                _currBalance -= _capitalisedLife;
            }

            // Get Data for calcs
            _discountV = vML.RateAdjustment;
            _baseRateV = vML.ActiveMarketRate;

            lMarketRateAccount.Text = _baseRateV.ToString(SAHL.Common.Constants.RateFormat);
            lMarketRateApplication.Text = _baseRateV.ToString(SAHL.Common.Constants.RateFormat);

            _latestValuationAmount = vML.GetActiveValuationAmount();
            _latestValuationDate = vML.GetActiveValuationDate();
            _householdIncome = Account.GetHouseholdIncome();
            _SPVCompany = vML.Account.SPV.SPVCompany.Key;
            _originationSourceKey = Account.OriginationSource.Key;
            _productKey = Account.Product.Key;

            lblSPV.Text = vML.Account.SPV.Description;

            ISupportsVariableLoanApplicationInformation aivl = null;
            if (app != null)
            {
                aivl = app.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                if (aivl != null)
                {
                    ddlSPV.SelectedValue = aivl.VariableLoanInformation.SPV.Key.ToString();
                    double appIncome = (aivl.VariableLoanInformation.HouseholdIncome.HasValue ? aivl.VariableLoanInformation.HouseholdIncome.Value : 0);
                    tbNewIncome1.Text = appIncome.ToString();
                    lblNewHouseholdIncome.Text = appIncome.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
            }

            lblBondRegAmount.Text = _totalBondsRegistered.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblLoanAgreeAmount.Text = _totalLoanAgreeAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblRemainingTerm.Text = vML.RemainingInstallments.ToString();
            // Add capitalised life back to show the real current balance
            lblCurrentBalance.Text = (_currBalance + _capitalisedLife).ToString(SAHL.Common.Constants.CurrencyFormat);
            lblCurrentInstalment.Text = (_currInstalment).ToString(SAHL.Common.Constants.CurrencyFormat);
            //Show Amortising instalment row if InterestOnly
            rowAmortisingInstalment.Visible = IsInterestOnly;
            lblCurrentLTV.Text = ((_currBalance + _capitalisedLife) / _latestValuationAmount).ToString(SAHL.Common.Constants.RateFormat);
            lblCurrentPTI.Text = ((_currInstalment) / _householdIncome).ToString(SAHL.Common.Constants.RateFormat);
            lblCurrentLoanRate.Text = vML.InterestRate.ToString(SAHL.Common.Constants.RateFormat);

            lblCurrentLinkRate.Text = vML.RateConfiguration.Margin.Value.ToString(SAHL.Common.Constants.RateFormat);

            //      Valuation Information
            lblValuationAmount.Text = _latestValuationAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblLastValuationDate.Text = _latestValuationDate.HasValue ? _latestValuationDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "";

            //      Arrears Information
            lblArrearsLast6Mth.Text = (_hasArrears ? "Yes" : "No");
            lblArrearsBalance.Text = _arrearsBalance > 0 ? _arrearsBalance.ToString(SAHL.Common.Constants.CurrencyFormat) : "R 0";

            //      Income Information
            lblHouseholdIncome.Text = _householdIncome.ToString(SAHL.Common.Constants.CurrencyFormat);
            lAccruedInterest.Text = _accruedInterest.ToString(SAHL.Common.Constants.CurrencyFormat);

            if (aivl != null)
                _employmentTypeKey = aivl.VariableLoanInformation.EmploymentType.Key;
            else
                _employmentTypeKey = Account.GetEmploymentTypeKey();

            ddlEmploymentType.SelectedIndex = _employmentTypeKey;
            //need the employment type key set to get the max loan amount
            _employmentTypeKeySelected = (ddlEmploymentType.SelectedItem.Value == "-select-" ? -1 : Convert.ToInt32(ddlEmploymentType.SelectedItem.Value));

            //      Rate
            _marginKeySelected = vML.RateConfiguration.Margin.Key;
            lblDiscount.Text = _discountV.ToString(SAHL.Common.Constants.RateFormat);
            lDiscount.Text = _discountV.ToString(SAHL.Common.Constants.RateFormat);

            //Group exposure items

            this.lblGroupExposurePTI.Text = Math.Round(this.GroupExposureRepository.GetAccountGroupExposurePTI(SelectedLegalEntity.Key, _householdIncome), 4).ToString(SAHL.Common.Constants.RateFormat);

            this.lblGroupExposureLTV.Text = Math.Round(this.GroupExposureRepository.GetAccountGroupExposureLTV(this._accountKey, _latestValuationAmount, 0), 4).ToString(SAHL.Common.Constants.RateFormat);

            //Further Lending Limits
            GetMaximumLoanAmount();
            //NEED MAX LOAN AMOUNT BY HERE
            CalculateLendingLimits();

            // In progress applications
            if (_readvanceInProgress)
                tbReadvanceRequired.Text = _readvanceInProgressAmount.ToString();
            if (_furtherAdvanceInProgess)
                tbFurtherAdvReq.Text = _furtherAdvanceInProgressAmount.ToString();
            if (_furtherLoanInProgress)
                tbFurtherLoanReq.Text = _furtherLoanInProgressAmount.ToString();

            if (_readvanceInProgress || _furtherAdvanceInProgess || _furtherLoanInProgress)
            {
                tbBondToRegister.Text = _applicationBondToRegister.ToString();

                tbTotalCashRequired.Text = (_readvanceInProgressAmount + _furtherAdvanceInProgressAmount + _furtherLoanInProgressAmount).ToString();

                if (_latestValuationAmount != _applicationValuationAmount && _applicationValuationAmount > 0)
                    tbEstimatedValuationAmount.Text = _applicationValuationAmount.ToString();

                tbNewIncome1.Text = _applicationHouseholdIncome.ToString();
                lblNewHouseholdIncome.Text = _applicationHouseholdIncome.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            lAccountOpenDate.Text = Account.OpenDate.HasValue ? Account.OpenDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "No open date";

            ShowMaxCalculatedValues();

            lblAreaClass.Text = vML.Property.AreaClassification.Description;

            if (vML.Property.OccupancyType != null)
            {
                lOccupancy.Text = vML.Property.OccupancyType.Description;
                ddlOccupancy.SelectedIndex = vML.Property.OccupancyType.Key;
            }

            //NCA Compliance
            lNCACompliant.Text = NCACompliant ? "Yes" : "No";

            //Populate the Varifix tab
            if (_showVarifix)
            {
                tpVarifix.Enabled = true;
                tbProduct.Text = Varifix;

                //Market Rate
                lVFCurrMarketRateFix.Text =
                    lVFCalcMarketRateFix.Text = _baseRateF.ToString(SAHL.Common.Constants.RateFormat);
                lVFCurrMarketRateVar.Text =
                    lVFCalcMarketRateVar.Text = _baseRateV.ToString(SAHL.Common.Constants.RateFormat);
                //Link Rate
                lVFCurrLinkRateFix.Text = fML.RateConfiguration.Margin.Value.ToString(SAHL.Common.Constants.RateFormat);
                lVFCurrLinkRateVar.Text = vML.RateConfiguration.Margin.Value.ToString(SAHL.Common.Constants.RateFormat);
                //Loan Rate
                lVFCurrLoanRateFix.Text = fML.InterestRate.ToString(SAHL.Common.Constants.RateFormat);
                lVFCurrLoanRateVar.Text = vML.InterestRate.ToString(SAHL.Common.Constants.RateFormat);
                //Current balance
                lVFCurrCurrentBalanceFix.Text =
                    lVFCalcCurrentBalanceFix.Text = fML.CurrentBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
                lVFCurrCurrentBalanceVar.Text =
                    lVFCalcCurrentBalanceVar.Text = vML.CurrentBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
                lVFCurrCurrentBalanceTotal.Text =
                    lVFCalcCurrentBalanceTotal.Text = (vML.CurrentBalance + fML.CurrentBalance).ToString(SAHL.Common.Constants.CurrencyFormat);
                //Readvance Amount

                //Further Advance Amount

                //Further Loan Amount

                //Current Instalment
                double fixInstal = 0;
                if (fML.CurrentBalance > 0)
                    fixInstal = LoanCalculator.CalculateInstallment(fML.CurrentBalance, fML.InterestRate, fML.RemainingInstallments, false);
                lVFCurrInstalmentFix.Text = fixInstal.ToString(SAHL.Common.Constants.CurrencyFormat);
                double varInstal = 0;
                if (vML.CurrentBalance > 0)
                    varInstal = LoanCalculator.CalculateInstallment(vML.CurrentBalance - _capitalisedLife, vML.InterestRate, vML.RemainingInstallments, false);
                lVFCurrInstalmentVar.Text = varInstal.ToString(SAHL.Common.Constants.CurrencyFormat);
                lVFCurrInstalmentTotal.Text = (fixInstal + varInstal).ToString(SAHL.Common.Constants.CurrencyFormat);
                //Interest Accrued
                lVFCurrInterestAccruedFix.Text =
                    lVFCalcInterestAccruedFix.Text = _accruedInterestFix.ToString(SAHL.Common.Constants.CurrencyFormat);
                lVFCurrInterestAccruedVar.Text =
                    lVFCalcInterestAccruedVar.Text = _accruedInterestVar.ToString(SAHL.Common.Constants.CurrencyFormat);
                lVFCurrInterestAccruedTotal.Text =
                    lVFCalcInterestAccruedTotal.Text = (_accruedInterest).ToString(SAHL.Common.Constants.CurrencyFormat);
                //Current Balance + Interest
                lVFCurrTotalPlusInterestFix.Text =
                    lVFCalcTotalPlusInterestFix.Text = (fML.CurrentBalance + _accruedInterestFix).ToString(SAHL.Common.Constants.CurrencyFormat);
                lVFCurrTotalPlusInterestVar.Text = (vML.CurrentBalance + _accruedInterestVar).ToString(SAHL.Common.Constants.CurrencyFormat);
                lVFCurrTotalPlusInterestTotal.Text = (fML.CurrentBalance + _accruedInterestFix + vML.CurrentBalance + _accruedInterestVar).ToString(SAHL.Common.Constants.CurrencyFormat);

                //New Instalment
                double fixInstalAcc = 0;
                if ((fML.CurrentBalance + _accruedInterestFix) > 0)
                    fixInstalAcc = LoanCalculator.CalculateInstallment((fML.CurrentBalance + _accruedInterestFix), fML.InterestRate, fML.RemainingInstallments, false);
                double varInstalAcc = 0;
                if ((vML.CurrentBalance + _accruedInterestVar) > 0)
                    varInstalAcc = LoanCalculator.CalculateInstallment((vML.CurrentBalance - _capitalisedLife + _accruedInterestVar), vML.InterestRate, vML.RemainingInstallments, false);
                lVFCurrNewInstalmentFix.Text = fixInstalAcc.ToString(SAHL.Common.Constants.CurrencyFormat);
                lVFCurrNewInstalmentVar.Text = varInstalAcc.ToString(SAHL.Common.Constants.CurrencyFormat);
                lVFCurrNewInstalmentTotal.Text = (fixInstalAcc + varInstalAcc).ToString(SAHL.Common.Constants.CurrencyFormat);
                //Arrear Balance
                lVFCurrArrearFix.Text =
                    lVFCalcArrearFix.Text = fML.ArrearBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
                lVFCurrArrearVar.Text =
                    lVFCalcArrearVar.Text = vML.ArrearBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
                lVFCurrArrearTotal.Text =
                    lVFCalcArrearTotal.Text = (fML.ArrearBalance + vML.ArrearBalance).ToString(SAHL.Common.Constants.CurrencyFormat);
            }
            AddTrace(this, "BindDisplay end");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="EmploymentTypes"></param>
        /// <param name="AccountEmploymentTypeKey"></param>
        public void BindEmploymentTypes(IEventList<IEmploymentType> EmploymentTypes, int AccountEmploymentTypeKey)
        {
            AddTrace(this, "BindEmploymentTypes start");
            if (AccountEmploymentTypeKey == 0)
                AccountEmploymentTypeKey = 5;

            ddlEmploymentType.DataSource = EmploymentTypes.BindableDictionary;
            ddlEmploymentType.DataBind();

            lEmploymentAccount.Text = EmploymentTypes.ObjectDictionary[AccountEmploymentTypeKey.ToString()].Description;
            AddTrace(this, "BindEmploymentTypes end");
        }

        public void BindOccupancyTypes(IEventList<IOccupancyType> occupancyTypes, int AccountOccupancyTypeKey)
        {
            AddTrace(this, "BindOccupancyTypes start");

            ddlOccupancy.DataSource = occupancyTypes.BindableDictionary;
            ddlOccupancy.DataBind();
            lOccupancy.Text = occupancyTypes.ObjectDictionary[AccountOccupancyTypeKey.ToString()].Description;
            AddTrace(this, "BindOccupancyTypes end");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="SPVs"></param>
        public void BindSPVs(IList<ISPV> SPVs)
        {
            AddTrace(this, "BindSPVs start");
            _spvList = SPVs;

            ddlSPV.PleaseSelectItem = false;
            ddlSPV.DataTextField = "Description";
            ddlSPV.DataValueField = "Key";
            ddlSPV.DataSource = _spvList;
            ddlSPV.DataBind();
            AddTrace(this, "BindSPVs end");
        }

        #region Properties

        //16486Remove start
        /// <summary>
        /// check the start date of the readvance to determine what calc to use in the view
        /// this property and all related code can be removed a month after go live:
        /// select * from control where controlDescription = 'CLVReadvanceStart'
        /// </summary>
        public DateTime? ReadvanceStartDate
        {
            get { return _raStartDate; }
            set { _raStartDate = value; }
        }

        public bool HideCLV
        {
            get { return _hideCLV; }
            set
            {
                _hideCLV = value;
                tblFAMessage.Visible = false;
            }
        }

        //16486Remove end

        /// <summary>
        /// Indicates if the Account is NCA Compliant, if not add initiation fee for FLoans
        /// </summary>
        public bool NCACompliant
        {
            get { return _ncaCompliant; }
            set { _ncaCompliant = value; }
        }

        public double LatestValuationAmount
        {
            get
            {
                if (ApprovalMode == ApprovalTypes.None)
                    return tbEstimatedValuationAmount.Text.Length > 0 ? Convert.ToDouble(tbEstimatedValuationAmount.Text) : _latestValuationAmount;
                else
                    return _latestValuationAmount;
            }
        }

        public int EmploymentTypeKeySelected
        {
            get { return _employmentTypeKeySelected; }
        }

        public double MarginSelected
        {
            get { return _marginSelected; }
        }

        public int MarginKeySelected
        {
            get { return _marginKeySelected; }
        }

        public double TotalCashRequired
        {
            get { return _totalCashRequired; }
        }

        public double ReadvanceRequired
        {
            get { return _readvanceRequired; }
        }

        public double FurtherAdvRequired
        {
            get { return _furtherAdvanceRequired; }
        }

        public double FurtherLoanRequired
        {
            get { return _furtherLoanRequired; }
        }

        public double BondToRegister
        {
            get { return _bondToRegister; }
            set { _bondToRegister = value; }
        }

        public double CurrentBalance
        {
            get { return _currBalance; }
        }

        public double CurrentBalanceVF
        {
            get { return _currBalanceVF; }
        }

        public double NewCurrentBalance
        {
            get { return _newCurrentBalance; }
            set { _newCurrentBalance = value; }
        }

        public bool HasArrears
        {
            set { _hasArrears = value; }
        }

        public int RemainingTerm
        {
            get { return Convert.ToInt32(lblRemainingTerm.Text); }
        }

        public double NewInstalment
        {
            get { return _newInstalment; }
            set { _newInstalment = value; }
        }

        public double NewAMInstalment
        {
            get { return _newAMInstalment; }
            set { _newAMInstalment = value; }
        }

        public double BaseRateVariable
        {
            get { return _baseRateV; }
        }

        public double BaseRateFixed
        {
            get { return _baseRateF; }
        }

        public bool IsInterestOnly
        {
            get { return _isInterestOnly; }
            set { _isInterestOnly = value; }
        }

        public double TwentyYearInstalment
        {
            set { twentyYearInstalment = value; }
        }

        public double TwentyYearPTI
        {
            set { twentyYearPTI = value; }
        }

        public bool ShowTwentyYearFigures
        {
            get
            {
                return showTwentyYearFigures;
            }
            set
            {
                showTwentyYearFigures = value;
            }
        }

        public double NewPTI
        {
            set { _newPTI = value; }
        }

        public double NewRate
        {
            set { _newRate = value; }
        }

        public double NewLTV
        {
            set { _newLTV = value; }
        }

        public double NewLTP
        {
            set { _newLTP = value; }
        }

        public double CapitalisedLife
        {
            get { return _capitalisedLife; }
        }

        public double LifeInstalment
        {
            get { return _lifeInstalment; }
        }

        public double NewIncome
        {
            get
            {
                return (_newIncome1 > 0 ? _newIncome1 : _householdIncome);
            }
        }

        public int NewOccupancyTypeKey
        {
            get
            {
                return this._newOccupancyTypeKey;
            }
        }

        public double DiscountVariable
        {
            get { return _discountV; }
        }

        public double DiscountFixed
        {
            get { return _discountF; }
            //set { _discount = value; }
        }

        public double ReadvanceMax
        {
            get { return _readvanceMax; }
            //set { _readvanceMax = value; }
        }

        public double FurtherAdvanceMax
        {
            get { return _furtherAdvanceMax; }
            //set { _furtherAdvanceMax = value; }
        }

        public double FurtherLoanMax
        {
            get { return _furtherLoanMax; }
            //set { _furtherLoanMax = value; }
        }

        public ISPV NewSPV
        {
            get
            {
                if (ApprovalMode == ApprovalTypes.None || ApprovalMode == ApprovalTypes.Approve)
                    return _newSPV;
                else
                {
                    foreach (ISPV spv in _spvList)
                    {
                        if (spv.Key == Convert.ToInt32(ddlSPV.SelectedValue))
                        {
                            return spv;
                        }
                    }

                    return _newSPV;
                }
            }
            set { _newSPV = value; }
        }

        public int SPVCompany
        {
            get { return _SPVCompany; }
            //set { _SPVCompany = value; }
        }

        public bool ReadvanceInProgress
        {
            get { return _readvanceInProgress; }
            set { _readvanceInProgress = value; }
        }

        /// <summary>
        /// If the advance has been accepted it can not be edited
        /// </summary>
        public bool ReadvanceIsAccepted
        {
            get { return _readvanceIsAccepted; }
            set
            {
                tbReadvanceAccepted.Text = value.ToString();
                _readvanceIsAccepted = value;
            }
        }

        public bool FurtherAdvanceInProgress
        {
            get { return _furtherAdvanceInProgess; }
            set { _furtherAdvanceInProgess = value; }
        }

        /// <summary>
        /// If the advance has been accepted it can not be edited
        /// </summary>
        public bool FurtherAdvanceIsAccepted
        {
            get { return _furtherAdvanceIsAccepted; }
            set
            {
                tbFurtherAdvanceAccepted.Text = value.ToString();
                _furtherAdvanceIsAccepted = value;
            }
        }

        public bool FurtherLoanInProgress
        {
            get { return _furtherLoanInProgress; }
            set { _furtherLoanInProgress = value; }
        }

        /// <summary>
        /// If the advance has been accepted it can not be edited
        /// </summary>
        public bool FurtherLoanIsAccepted
        {
            get { return _furtherLoanIsAccepted; }
            set
            {
                tbFurtherLoanAccepted.Text = value.ToString();
                _furtherLoanIsAccepted = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public double ReadvanceInProgressAmount
        {
            get
            {
                return _readvanceInProgressAmount;
            }
            set
            {
                _readvanceInProgressAmount = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public double FurtherAdvanceInProgressAmount
        {
            get
            {
                return _furtherAdvanceInProgressAmount;
            }
            set
            {
                _furtherAdvanceInProgressAmount = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public double FurtherLoanInProgressAmount
        {
            get
            {
                return _furtherLoanInProgressAmount;
            }
            set
            {
                _furtherLoanInProgressAmount = value;
            }
        }

        public ICategory NewCategory
        {
            get
            {
                if (tbNewCategory.Text == NoCategory)
                    return LookupRepo.Categories.ObjectDictionary["5"]; //return cat 5
                else
                    return LookupRepo.Categories.ObjectDictionary[tbNewCategory.Text];
            }
            set
            {
                if (value != null)
                {
                    lCategoryApplication.Text = value.Description;
                    tbNewCategory.Text = value.Key.ToString();
                }
                else
                {
                    lCategoryApplication.ForeColor = Color.Red;
                    lCategoryApplication.Text = NoCategory;
                    tbNewCategory.ForeColor = Color.Red;
                    tbNewCategory.Text = NoCategory;
                }
            }
        }

        public ILegalEntity SelectedLegalEntity
        {
            get { return LegalEntityGrid.SelectedLegalEntity; }
        }

        public bool SendReadvanceApplicationForm
        {
            get
            {
                if (ReadvanceRequired > 0 && (HelpDeskRework || !ReadvanceInProgress))
                    return true;

                return false;
            }
        }

        public bool SendFurtherAdvanceApplicationForm
        {
            get
            {
                if (FurtherAdvRequired > 0 && (HelpDeskRework || !FurtherAdvanceInProgress))
                    return true;

                return false;
            }
        }

        public bool SendFurtherLoanApplicationForm
        {
            get
            {
                if (FurtherLoanRequired > 0 && (HelpDeskRework || !FurtherLoanInProgress))
                    return true;

                return false;
            }
        }

        public bool HelpDeskRework { get; set; }

        #region ContactData

        public string HomePhoneCode
        {
            get { return _homePhoneCode; }
            set { _homePhoneCode = value; }
        }

        public string HomePhoneNumber
        {
            get { return _homePhoneNumber; }
            set { _homePhoneNumber = value; }
        }

        public string WorkPhoneCode
        {
            get { return _workPhoneCode; }
            set { _workPhoneCode = value; }
        }

        public string WorkPhoneNumber
        {
            get { return _workPhoneNumber; }
            set { _workPhoneNumber = value; }
        }

        public string FaxCode
        {
            get { return _faxCode; }
            set { _faxCode = value; }
        }

        public string FaxNumber
        {
            get { return _faxNumber; }
            set { _faxNumber = value; }
        }

        public string CellNumber
        {
            get { return _cellNumber; }
            set { _cellNumber = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        #endregion ContactData

        public double Fees
        {
            get { return _fees; }
            set { _fees = value; }
        }

        public bool IsExceptionCreditCriteria
        {
            get { return _isExceptionCreditCriteria; }
            set
            {
                _isExceptionCreditCriteria = value;
                tblException.Visible = value;
            }
        }

        public double ApplicationValuationAmount
        {
            get
            {
                return _applicationValuationAmount;
            }
            set
            {
                _applicationValuationAmount = value;
            }
        }

        public double ApplicationRate
        {
            get
            {
                return _applicationRate;
            }
            set
            {
                _applicationRate = value;
            }
        }

        public int ApplicationRateKey
        {
            get
            {
                return _applicationRateKey;
            }
            set
            {
                _applicationRateKey = value;
            }
        }

        public double ApplicationBondToRegister
        {
            get
            {
                return _applicationBondToRegister;
            }
            set
            {
                _applicationBondToRegister = value;
            }
        }

        public double ApplicationHouseholdIncome
        {
            get { return _applicationHouseholdIncome; }
            set { _applicationHouseholdIncome = value; }
        }

        public double AccountPurchasePrice
        {
            get { return _accountPurchasePrice; }
            set
            {
                _accountPurchasePrice = value;
            }
        }

        public int AccountApplicationType
        {
            get { return _accAppType; }
            set { _accAppType = value; }
        }

        public bool CanUpdate
        {
            set { _canUpdate = value; }
        }

        /// <summary>
        /// Set whether the view is being used to do credit approvals
        /// </summary>
        public ApprovalTypes ApprovalMode
        {
            get { return _approvalMode; }
            set
            {
                _approvalMode = value;
                tbApprovalMode.Text = value.ToString();
            }
        }

        /// <summary>
        /// The selected application type
        /// This is currently only used to show/hide rows
        /// in the approval mode.
        /// </summary>
        public OfferTypes ApplicationType
        {
            get { return _applicationType; }
            set { _applicationType = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string ExistingApplicationMessage
        {
            get { return lblApplicationInProgress.Text; }
            set { lblApplicationInProgress.Text = value; }
        }

        /// <summary>
        /// Text to display on the submit button for Credit screens
        /// </summary>
        public string BtnSubmitText
        {
            get { return btnSubmit.Text; }
            set { btnSubmit.Text = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool BtnSubmitEnabled
        {
            get { return btnSubmit.Enabled; }
            set { btnSubmit.Enabled = value; }
        }

        #endregion Properties

        /// <summary>
        /// If a non new business application is loaded, hide all controls on the page
        /// </summary>
        public bool HideAll
        {
            get { return _hideAll; }
            set { _hideAll = value; }
        }

        #endregion IFurtherLoanCalculator Members

        public List<RecipientCorrespondenceSelection> SendingInformation
        {
            get
            {
                var multiRecipeintsCorrespondenceSelection = new List<RecipientCorrespondenceSelection>();
                if (!string.IsNullOrEmpty(tbMultiRecipientCorrespondenceData.Text))
                {
                    multiRecipeintsCorrespondenceSelection = JsonConvert.DeserializeObject<List<RecipientCorrespondenceSelection>>(tbMultiRecipientCorrespondenceData.Text);
                }
                return multiRecipeintsCorrespondenceSelection;
            }
        }

        public bool IncludeNaedoForm
        {
            get
            {
                return chkIncludeNaedoForm.Checked;
            }
        }
    }
}