using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SAHL.Common;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Utils;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Life.Interfaces;

namespace SAHL.Web.Views.Life
{
    public partial class Policy : SAHLCommonBaseView, IPolicy
    {
        private IApplicationLife _applicationLife;
        private ILifePolicy _lifePolicy;
        private string _loanPipelineStatus;
        private bool _confirmMode;
        private bool _showReassuranceFields;
        private bool _showClaimStatusInformation;

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.ShouldRunPage)
                return;

            // Set Control Attributes
            lblPolicyStatusKey.Style.Add("display", "none");
            btnRemove.Attributes.Add("onclick", "return ConfirmDelete()");

            // Register Javascript
            RegisterClientJavascript();
        }

        #region IPolicy Members

        /// <summary>
        /// Sets whether to show or hide the workflow header
        /// When the view is used in worflow this should be set to TRUE
        /// othewise it should be set to false
        /// </summary>
        public bool ShowWorkFlowHeader
        {
            set { WorkFlowHeader1.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the workflow related buttons
        /// </summary>
        public bool WorkFlowButtonsVisible
        {
            set { pnlPlanButtons.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'Add Life' button
        /// </summary>
        public bool ShowAddLifeButton
        {
            set { btnAdd.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'Remove Life' button
        /// </summary>
        public bool ShowRemoveLifeButton
        {
            set { btnRemove.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'Recalculate Premiums' button
        /// </summary>
        public bool ShowRecalculatePremiumsButton
        {
            set { btnRecalculatePremiums.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'Premium Calculator' button
        /// </summary>
        public bool ShowPremiumCalculatorButton
        {
            set { btnPremiumQuote.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the Claim Status information
        /// </summary>
        public bool ShowClaimStatusInformation
        {
            set { _showClaimStatusInformation = value; }
        }

        /// <summary>
        /// Bind the Assured Lives Grid
        /// </summary>
        /// <param name="assuredLives"></param>
        public void BindAssuredLivesGrid(IReadOnlyEventList<ILegalEntity> assuredLives)
        {
            AddTrace(this, "_view_BindAssuredLivesGrid : Start");

            // Setup the grid
            LegalEntityGrid.HeaderCaption = "Assured Lives";
            LegalEntityGrid.GridHeight = 100;
            LegalEntityGrid.PostBackType = SAHL.Common.Web.UI.Controls.GridPostBackType.NoneWithClientSelect;
            LegalEntityGrid.ColumnIDPassportVisible = true;
            LegalEntityGrid.ColumnIDPassportHeadingType = SAHL.Web.Controls.LegalEntityGrid.GridColumnIDPassportHeadingType.IDAndPassportNumber;
            LegalEntityGrid.ColumnInsurableInterestVisible = true;
            LegalEntityGrid.ColumnBenefitsVisible = true;
            LegalEntityGrid.ColumnLifeStatusVisible = true;

            // Set the Data related properties
            LegalEntityGrid.AccountKey = _lifePolicy != null ? _lifePolicy.FinancialService.Account.Key : _applicationLife.Account.Key;
            if (_lifePolicy != null)
                LegalEntityGrid.DateOfAcceptance = _lifePolicy.DateOfAcceptance;

            // Bind the grid
            LegalEntityGrid.BindLegalEntities(assuredLives);

            AddTrace(this, "_view_BindAssuredLivesGrid : End");
        }

        /// <summary>
        /// Binds and sets the Contact Name dropdown list
        /// </summary>
        /// <param name="assuredLives"></param>
        /// <param name="contactPersonKey"></param>
        public void BindContactPersons(IReadOnlyEventList<ILegalEntity> assuredLives, int contactPersonKey)
        {
            AddTrace(this, "_view_BindContactPersons : Start");

            // The Policy Holder (Contact Name) can only be a person who plays a role on the loan and on the policy
            foreach (ILegalEntityNaturalPerson np in assuredLives)
            {
                ddlPolicyHolder.Items.Add(new ListItem(np.GetLegalName(LegalNameFormat.Full), np.Key.ToString()));
            }

            ddlPolicyHolder.DataBind();

            // Select the LE which is the PolicyHoldeyLEKey on the Policy Record
            // If there is no PolicyHoldeyLEKey set then select the first LE in the list
            int iSelIdx = 0, i = 0;
            if (contactPersonKey > -1)
            {
                foreach (ListItem li in ddlPolicyHolder.Items)
                {
                    if (li.Value == contactPersonKey.ToString())
                    {
                        iSelIdx = i;
                        break;
                    }
                    i++;
                }
            }
            ddlPolicyHolder.SelectedIndex = iSelIdx;
            if (ddlPolicyHolder.SelectedIndex > -1)
                lblPolicyHolder.Text = ddlPolicyHolder.SelectedItem.Text;

            AddTrace(this, "_view_BindContactPersons : End");
        }

        /// <summary>
        /// Private method to handle the above 2 overloaded functions
        /// </summary>
        /// <param name="accountLifePolicy"></param>
        /// <param name="loanAccount"></param>
        /// <param name="mortgageLoanVariableFS"></param>
        /// <param name="mortgageLoanFixedFS"></param>
        /// <param name="hocFS"></param>
        /// <param name="lifeIsConditionOfLoan"></param>
        /// <param name="contactNumber"></param>
        public void BindPolicyDetails(IAccountLifePolicy accountLifePolicy, SAHL.Common.BusinessModel.Interfaces.IAccount loanAccount, IMortgageLoan mortgageLoanVariableFS, IMortgageLoan mortgageLoanFixedFS, IHOC hocFS, bool lifeIsConditionOfLoan, string contactNumber)
        {
            AddTrace(this, "_view_BindPolicyDetails : Start");
            _applicationLife = accountLifePolicy.CurrentLifeApplication;
            _lifePolicy = accountLifePolicy.LifePolicy;
            _showClaimStatusInformation = false;

            double dInitialSumAssured = 0, dCurrentSumAssured = 0, dIPBenefitPremium = 0, dDeathBenefitPremium = 0,
                dAnnualPremium = 0, dMonthlyInstalment = 0, dTotalInstalment = 0, dRegistrationAmount = 0, dBondInstalment = 0, dHOCInstalment = 0,
                dDeathBenefitAmount = 0, dIPBAmount = 0, dIPBenefitRetention = 0, dDeathBenefitRetention = 0, dRetainedDBSumAssured = 0,
                dReassuredDBAmount = 0, dRetainedIPBSumAssured = 0, dReassuredIPBAmount = 0, dMonthlyServiceFee = 0;

            int remainingTerm = 0;

            remainingTerm = mortgageLoanVariableFS == null ? 0 : Convert.ToInt32(mortgageLoanVariableFS.RemainingInstallments);

            lblPolicyNumber.Text = accountLifePolicy.Key.ToString();
            int lifePolicyTypeKey = _lifePolicy != null ? _lifePolicy.LifePolicyType.Key : _applicationLife.LifePolicyType.Key;
            if (lifePolicyTypeKey == (int)LifePolicyTypes.AccidentOnlyCover)
                benefitType.InnerText = "Disability Premium";
            else
                benefitType.InnerText = "IP Benefit Premium";

            lblPolicyStatus.Text = _lifePolicy != null ? _lifePolicy.LifePolicyStatus.Description : "Prospect";
            lblPolicyStatusKey.Text = _lifePolicy != null ? _lifePolicy.LifePolicyStatus.Key.ToString() : "1";
            lblApplicationStatus.Text = _applicationLife.ApplicationStatus.Description;
            lblConsultant.Text = _applicationLife.Consultant != null ? _applicationLife.Consultant.LegalEntity.GetLegalName(LegalNameFormat.FullNoSalutation) : "";

            IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();

            if (_lifePolicy != null) // Get the data from the LifePolicy table
            {
                lblPolicyType.Text = _lifePolicy.LifePolicyType.Description;
                lblDateOfAcceptance.Text = _lifePolicy.DateOfAcceptance.HasValue ? _lifePolicy.DateOfAcceptance.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
                lblDateOfCommencement.Text = _lifePolicy.DateOfCommencement.HasValue ? _lifePolicy.DateOfCommencement.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
                lblCancellationDate.Text = _lifePolicy.DateOfCancellation.HasValue ? _lifePolicy.DateOfCancellation.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
                // Instalment Data
                dInitialSumAssured = _lifePolicy.SumAssured;
                dCurrentSumAssured = _lifePolicy.CurrentSumAssured.HasValue ? _lifePolicy.CurrentSumAssured.Value : 0;
                dIPBenefitPremium = _lifePolicy.InstallmentProtectionPremium;
                dDeathBenefitPremium = _lifePolicy.DeathBenefitPremium;
                dAnnualPremium = _lifePolicy.YearlyPremium;
                dMonthlyInstalment = _lifePolicy.MonthlyPremium;

                // Reassurance Data
                dDeathBenefitAmount = dCurrentSumAssured;
                dDeathBenefitRetention = _lifePolicy.DeathReassuranceRetention.HasValue ? _lifePolicy.DeathReassuranceRetention.Value : 0;
                dRetainedDBSumAssured = dDeathBenefitAmount > 0 && dDeathBenefitAmount < dDeathBenefitRetention ? dDeathBenefitAmount : dDeathBenefitRetention;
                dReassuredDBAmount = dDeathBenefitAmount - dRetainedDBSumAssured;

                dIPBAmount = dBondInstalment;
                dIPBenefitRetention = _lifePolicy.IPBReassuranceRetention.HasValue ? _lifePolicy.IPBReassuranceRetention.Value : 0;
                dRetainedIPBSumAssured = dIPBAmount > 0 && dIPBAmount < dIPBenefitRetention ? dIPBAmount : dIPBenefitRetention;
                dReassuredIPBAmount = dIPBAmount - dRetainedIPBSumAssured;

                //Claim Status
                if (_lifePolicy.ClaimStatus != null && _lifePolicy.ClaimType != null)
                {
                    lblClaimStatus.Text = _lifePolicy.ClaimType.Description + " " + _lifePolicy.ClaimStatus.Description;
                    lblClaimStatusDate.Text = _lifePolicy.ClaimStatusDate.HasValue ? _lifePolicy.ClaimStatusDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
                }
            }
            else // Get the data from the applicationLife table
            {
                lblPolicyType.Text = _applicationLife.LifePolicyType.Description;

                lblDateOfAcceptance.Text = "-";
                lblDateOfCommencement.Text = "-";
                lblCancellationDate.Text = "-";

                // Instalment Data
                dInitialSumAssured = _applicationLife.SumAssured;
                dCurrentSumAssured = _applicationLife.CurrentSumAssured.HasValue ? _applicationLife.CurrentSumAssured.Value : 0;
                dIPBenefitPremium = _applicationLife.InstallmentProtectionPremium;
                dDeathBenefitPremium = _applicationLife.DeathBenefitPremium;
                dAnnualPremium = _applicationLife.YearlyPremium;
                dMonthlyInstalment = _applicationLife.MonthlyPremium;
            }

            // Loan Data
            lblLoanNumber.Text = loanAccount.Key.ToString();

            // Get the Loan Application Object
            IMortgageLoanAccount mortgageLoanAccount = loanAccount as IMortgageLoanAccount;
            IApplicationMortgageLoan applicationMortgageLoan = mortgageLoanAccount.CurrentMortgageLoanApplication;
            IApplicationInformationVariableLoan applicationInformationVariableLoan = null;
            IApplicationProductMortgageLoan applicationProductMortgageLoan = null;
            IApplicationProductVariFixLoan applicationProductVariFixLoan = null;
            if (applicationMortgageLoan != null)
            {
                applicationProductMortgageLoan = applicationMortgageLoan.CurrentProduct as IApplicationProductMortgageLoan;
                applicationProductVariFixLoan = applicationMortgageLoan.CurrentProduct as IApplicationProductVariFixLoan;
                ISupportsVariableLoanApplicationInformation VLI = applicationMortgageLoan.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                if (VLI != null)
                    applicationInformationVariableLoan = VLI.VariableLoanInformation;
            }

            if (mortgageLoanVariableFS != null && mortgageLoanVariableFS.Account.AccountStatus.Key == (int)AccountStatuses.Open)
            {
                dBondInstalment = mortgageLoanFixedFS != null && mortgageLoanFixedFS.Payment > 0 ? mortgageLoanFixedFS.Payment + mortgageLoanVariableFS.Payment : mortgageLoanVariableFS.Payment;
            }
            else
            {
                if (applicationProductMortgageLoan != null)
                {
					var pricingAdjustments = applicationMortgageLoan.GetRateAdjustments();
					double dVariablePercent = (applicationProductMortgageLoan.EffectiveRate.HasValue ? applicationProductMortgageLoan.EffectiveRate.Value + pricingAdjustments: 0);
					double dFixedPercent = applicationProductVariFixLoan == null ? 0 : (applicationProductVariFixLoan.FixedEffectiveRate.HasValue ? applicationProductVariFixLoan.FixedEffectiveRate.Value + pricingAdjustments : 0);
                    double dVariableAmount = 0, dFixedAmount = 0;

                    // check for interest only
                    bool interestOnly = applicationMortgageLoan.HasFinancialAdjustment(FinancialAdjustmentTypeSources.InterestOnly);
                    // check for edge
                    if (interestOnly == false)
                        interestOnly = applicationMortgageLoan.HasFinancialAdjustment(FinancialAdjustmentTypeSources.Edge);

                    if (applicationProductVariFixLoan != null)
                    {
                        // TRAC #13636 - If the loan is an application always recalc the loan instalment
                        // get the variable instalmentHalo.pUpdateHOCPremium
                        dVariableAmount = applicationProductVariFixLoan.VariableRandValue.HasValue ? applicationProductVariFixLoan.VariableRandValue.Value : 0;
                        dBondInstalment = LoanCalculator.CalculateInstallment(dVariableAmount, dVariablePercent, remainingTerm, interestOnly);
                        // add the fixed instalment
                        dFixedAmount = applicationProductVariFixLoan.FixedRandValue.HasValue ? applicationProductVariFixLoan.FixedRandValue.Value : 0;
                        dBondInstalment += LoanCalculator.CalculateInstallment(dFixedAmount, dFixedPercent, remainingTerm, interestOnly);
                    }
                    else
                    {
                        // get the variable istalment
                        dVariableAmount = applicationProductMortgageLoan.LoanAgreementAmount.HasValue ? applicationProductMortgageLoan.LoanAgreementAmount.Value : 0;
                        dBondInstalment = LoanCalculator.CalculateInstallment(dVariableAmount, dVariablePercent, remainingTerm, interestOnly);
                    }
                }
            }

            lblBondInstallment.Text = dBondInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);

            lblLoanStatus.Text = loanAccount.AccountStatus.Description;
            lblLoanTerm.Text = mortgageLoanVariableFS.InitialInstallments + " months";

            dRegistrationAmount = applicationInformationVariableLoan != null && applicationInformationVariableLoan.BondToRegister.HasValue ? Convert.ToDouble(applicationInformationVariableLoan.BondToRegister) : 0;
            lblLoanAmount.Text = dRegistrationAmount.ToString(SAHL.Common.Constants.CurrencyFormat);

            int _debitOrderDay = 0;
            // Get first active FinancialServiceBankAccount record
            if (mortgageLoanVariableFS != null)
            {
                foreach (IFinancialServiceBankAccount bac in mortgageLoanVariableFS.FinancialServiceBankAccounts)
                {
                    if (bac.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
                    {
                        _debitOrderDay = bac.DebitOrderDay;
                        break;
                    }
                }
            }

            lblLoanDebitOrderDay.Text = _debitOrderDay == 0 ? "-" : _debitOrderDay.ToString();
            lblPipelineStatus.Text = _loanPipelineStatus == null || _loanPipelineStatus.Length < 1 ? "-" : _loanPipelineStatus;

            // Life is conditon of loan ?
            lblLifeCondition.Text = lifeIsConditionOfLoan == true ? "Yes" : "No";

            // Populate HOC Details - do not include the prorata premium in the HOC instalment TRAC#11851
            if (hocFS != null)
            {
                dHOCInstalment = hocFS.HOCMonthlyPremium.HasValue ? hocFS.HOCMonthlyPremium.Value : 0;
                lblHOCInstallment.Text = dHOCInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            // TRAC #13636  - Monthly Service Fee  - add to total premium
            //              - If the policy is inforce then get the discounted monthly service fee
            if (_lifePolicy != null && _lifePolicy.LifePolicyStatus.Key == (int)SAHL.Common.Globals.LifePolicyStatuses.Inforce)
                dMonthlyServiceFee = Convert.ToDouble(ctrlRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.SAHLMonthlyFeeDiscounted).ControlNumeric);
            else
                dMonthlyServiceFee = Convert.ToDouble(ctrlRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.SAHLMonthlyFee).ControlNumeric);

            // Instalment Data
            lblInitialSumAssured.Text = dInitialSumAssured.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblCurrentSumAssured.Text = dCurrentSumAssured.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblIPBenefitPremium.Text = dIPBenefitPremium.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblDeathBenefitPremium.Text = dDeathBenefitPremium.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblAnnualPremium.Text = dAnnualPremium.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblMonthlyInstalment.Text = dMonthlyInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblMonthlyServiceFee.Text = dMonthlyServiceFee.ToString(SAHL.Common.Constants.CurrencyFormat);

            dTotalInstalment = dMonthlyInstalment + dBondInstalment + dHOCInstalment + dMonthlyServiceFee;
            lblTotalAmountDue.Text = dTotalInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);

            // Reassurance Data
            lblRetainedDBSumAssured.Text = dRetainedDBSumAssured.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblReassuredDBAmount.Text = dReassuredDBAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblRetainedIPBSumAssured.Text = dRetainedIPBSumAssured.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblReassuredIPBAmount.Text = dReassuredIPBAmount.ToString(SAHL.Common.Constants.CurrencyFormat);

            btnAdd.Attributes.Remove("onclick");
            btnRecalculatePremiums.Attributes.Remove("onclick");
            if (_lifePolicy != null && _lifePolicy.LifePolicyStatus.Key == (int)SAHL.Common.Globals.LifePolicyStatuses.Inforce)
            {
                btnAdd.Attributes.Add("onclick", "return ConfirmAdd()");
                btnRecalculatePremiums.Attributes.Add("onclick", "return ConfirmPremiumRecalc()");
            }

            // if we are in Confirm mode then get the X2 data and set the alternate contact number
            if (this.ConfirmMode == true)
                lblAlternateContact.Text = String.IsNullOrEmpty(contactNumber) ? "-" : contactNumber.ToString();

            AddTrace(this, "_view_BindPolicyDetails : End");
        }

        /// <summary>
        /// Property to enable/disable the Contact Person dropdown box
        /// </summary>
        public bool ContactPersonEnabled
        {
            set
            {
                ddlPolicyHolder.Visible = value;
                lblPolicyHolder.Visible = !value;
            }
        }

        /// <summary>
        /// Gets/Sets whether we are in Confirm Mode
        /// </summary>
        public bool ConfirmMode
        {
            get { return _confirmMode; }
            set { _confirmMode = value; }
        }

        /// <summary>
        /// Gets/Sets whether to show/hide the Reassurance fields
        /// </summary>
        public bool ShowReassuranceFields
        {
            get { return _showReassuranceFields; }
            set { _showReassuranceFields = value; }
        }

        /// <summary>
        /// Property to get the Contact Person key
        /// </summary>
        public int ContactPersonKey
        {
            get { return Convert.ToInt32(ddlPolicyHolder.SelectedValue); }
        }

        /// <summary>
        /// Property to set the Loan Pipeline Status
        /// </summary>
        public string LoanPipelineStatus
        {
            set { _loanPipelineStatus = value; }
        }

        public bool ManualLifePolicyPaymentVisible
        {
            set { rowManualLifePolicyPayment.Visible = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnAcceptPlanButtonClicked;
        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnAddLifeButtonClicked;
        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnConsideringButtonClicked;
        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnDeclinePlanButtonClicked;
        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnPremiumCalculatorButtonClicked;
        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnQuoteRequiredButtonClicked;
        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnRecalculatePremiumsButtonClicked;
        /// <summary>
        ///
        /// </summary>
        public event KeyChangedEventHandler OnRemoveLifeButtonClicked;
        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnSendEndorsementButtonClicked;
        /// <summary>
        ///
        /// </summary>
        public event KeyChangedEventHandler OnContactPersonSelectedIndexChanged;

        #endregion IPolicy Members

        /// <summary>
        ///
        /// </summary>
        private void RegisterClientJavascript()
        {
            // string sFormName = "window." + this.Form.Name;

            StringBuilder sbJavascript = new StringBuilder();

            sbJavascript.AppendLine("function ConfirmDelete ()");
            sbJavascript.AppendLine("{");
            sbJavascript.AppendLine("var legalentitygrid = document.getElementById('" + this.LegalEntityGrid.ClientID + "');");
            sbJavascript.AppendLine("var statuskey = document.getElementById('" + this.lblPolicyStatusKey.ClientID + "');");
            sbJavascript.AppendLine("var legalentitycount = legalentitygrid.rows.length;");
            //sbJavascript.AppendLine("if (legalentitycount<=2)");
            //sbJavascript.AppendLine("{");
            //sbJavascript.AppendLine("alert('Assured Life cannot be Removed. At least one Assured Life must exist on the Policy.');");
            //sbJavascript.AppendLine("return false");
            //sbJavascript.AppendLine("}");
            //sbJavascript.AppendLine("else");
            //sbJavascript.AppendLine("{");
            sbJavascript.AppendLine("if (statuskey.innerText=='" + (int)SAHL.Common.Globals.LifePolicyStatuses.Inforce + "')");
            sbJavascript.AppendLine("return confirm('This Policy is Inforce, are you sure you want to Remove the Assured Life ?\\n\\nRemoving an Assured Life will perform a Premium Recalculation and generate Financial Transactions.');");
            sbJavascript.AppendLine("else");
            sbJavascript.AppendLine("return confirm('Are you sure you want to Remove the Assured Life from this Policy ?');");
            //sbJavascript.AppendLine("}");
            sbJavascript.AppendLine("}");
            if (!Page.ClientScript.IsClientScriptBlockRegistered("ConfirmDelete"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ConfirmDelete", sbJavascript.ToString(), true);

            sbJavascript = new StringBuilder();
            sbJavascript.AppendLine("function ConfirmAdd ()");
            sbJavascript.AppendLine("{");
            sbJavascript.AppendLine("return confirm('This Policy is Inforce, are you sure you want to Add an Assured Life ?\\n\\nAdding an Assured Life will perform a Premium Recalculation and generate Financial Transactions.');");
            sbJavascript.AppendLine("}");
            if (!Page.ClientScript.IsClientScriptBlockRegistered("ConfirmAdd"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ConfirmAdd", sbJavascript.ToString(), true);

            sbJavascript = new StringBuilder();
            sbJavascript.AppendLine("function ConfirmPolicyHolder ()");
            sbJavascript.AppendLine("{");
            sbJavascript.AppendLine("return confirm('This Policy is Inforce, are you sure you want to Change the Contact Name ?');");
            sbJavascript.AppendLine("}");
            if (!Page.ClientScript.IsClientScriptBlockRegistered("ConfirmPolicyHolder"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ConfirmPolicyHolder", sbJavascript.ToString(), true);

            sbJavascript = new StringBuilder();
            sbJavascript.AppendLine("function ConfirmPremiumRecalc ()");
            sbJavascript.AppendLine("{");
            sbJavascript.AppendLine("return confirm('This Policy is Inforce, are you sure you want to Recalculate the Premiums ?\\n\\nRecalculation will generate Financial Transactions.');");
            sbJavascript.AppendLine("}");
            if (!Page.ClientScript.IsClientScriptBlockRegistered("ConfirmPremiumRecalc"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ConfirmPremiumRecalc", sbJavascript.ToString(), true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!this.ShouldRunPage)
                return;

            if (this.ConfirmMode == true)
            {
                pnlPlanButtons.Visible = true;
                btnAccept.Text = "Confirm Quote";
                lblAlternateContact.Visible = true;
                lblAlternateContactText.Visible = true;
            }
            else
            {
                btnAccept.Text = "Accept Plan";
                lblAlternateContact.Visible = false;
                lblAlternateContactText.Visible = false;
            }

            if (_showReassuranceFields)
            {
                trRetainedDBSumAssured.Visible = true;
                trReassuredDBAmount.Visible = true;
                trSpacer1.Visible = false;
                trSpacer2.Visible = true;
                /* The IP benefit does not apply to ‘Accident only cover’ life policies
                 * hence no IP benefit reassurance premiums are required
                 */
                int lifePolicyTypeKey = _lifePolicy != null ? _lifePolicy.LifePolicyType.Key : _applicationLife.LifePolicyType.Key;

                if (lifePolicyTypeKey == (int)LifePolicyTypes.StandardCover)
                {
                    trRetainedIPBSumAssured.Visible = true;
                    trReassuredIPBAmount.Visible = true;
                }
                else
                {
                    trRetainedIPBSumAssured.Visible = false;
                    trReassuredIPBAmount.Visible = false;
                }
            }
            else
            {
                trRetainedDBSumAssured.Visible = false;
                trReassuredDBAmount.Visible = false;
                trRetainedIPBSumAssured.Visible = false;
                trReassuredIPBAmount.Visible = false;
                trSpacer1.Visible = true;
                trSpacer2.Visible = true;
            }

            if (_showClaimStatusInformation == false)
            {
                trClaimStatus.Visible = false;
                trClaimStatusDate.Visible = false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPolicyHolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnContactPersonSelectedIndexChanged(sender, new KeyChangedEventArgs(ddlPolicyHolder.SelectedValue));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            OnAddLifeButtonClicked(sender, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            OnRemoveLifeButtonClicked(sender, new KeyChangedEventArgs(LegalEntityGrid.SelectedLegalEntity.Key));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSendEndorsement_Click(object sender, EventArgs e)
        {
            OnSendEndorsementButtonClicked(sender, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRecalculatePremiums_Click(object sender, EventArgs e)
        {
            OnRecalculatePremiumsButtonClicked(sender, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPremiumQuote_Click(object sender, EventArgs e)
        {
            OnPremiumCalculatorButtonClicked(sender, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            OnAcceptPlanButtonClicked(sender, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDecline_Click(object sender, EventArgs e)
        {
            OnDeclinePlanButtonClicked(sender, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnQuote_Click(object sender, EventArgs e)
        {
            OnQuoteRequiredButtonClicked(sender, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConsidering_Click(object sender, EventArgs e)
        {
            OnConsideringButtonClicked(sender, e);
        }
    }
}