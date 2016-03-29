using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Utils;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Life.Interfaces;

namespace SAHL.Web.Views.Life
{
    public partial class PremiumQuote : SAHLCommonBaseView, IPremiumQuote
    {
        private int _accountKey;
        private IMortgageLoan _mortgageLoanVariableFS;
        private IMortgageLoan _mortgageLoanFixedFS;
        private IList<ILegalEntity> _lstLegalEntities;
        private string _selectedAgeList;
        private string _legalEntityName;
        private int _ipBenefitMaxAge;
        private DateTime? _dateOfBirth;
        private ILifeRepository _lifeRepo;

        private enum GridColumnPositions
        {
            Select = 0,
            LegalEntityKey = 1,
            Name = 2,
            DateOfBirth = 3,
            AgeNextBirthday = 4,
            AgeForPremiumCalc = 5,
            Benefits = 6
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.ShouldRunPage)
                return;
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

            if (PolicyTypeSelectedValue == (int)LifePolicyTypes.AccidentOnlyCover)
                benefitType.InnerText = "Disability Premium";
            else
                benefitType.InnerText = "IP Benefit Premium";
        }

        #region IPremiumQuote Members

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnCalculateButtonClicked;
        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnAddButtonClicked;
        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        ///
        /// </summary>
        /// <param name="lifePolicy"></param>
        /// <param name="loanAccount"></param>
        /// <param name="hocFS"></param>
        public void BindPremiumDetails(ILifePolicy lifePolicy, double monthlyInstalment, SAHL.Common.BusinessModel.Interfaces.IAccount loanAccount, SAHL.Common.BusinessModel.Interfaces.IHOC hocFS)
        {
            BindPremiumDetails(lifePolicy, null, loanAccount, hocFS, monthlyInstalment);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationLife"></param>
        /// <param name="loanAccount"></param>
        /// <param name="hocFS"></param>
        public void BindPremiumDetails(IApplicationLife applicationLife, SAHL.Common.BusinessModel.Interfaces.IAccount loanAccount, SAHL.Common.BusinessModel.Interfaces.IHOC hocFS)
        {
            BindPremiumDetails(null, applicationLife, loanAccount, hocFS, 0D);
        }

        /// <summary>
        ///
        /// </summary>
        public void BindAssuredLivesGrid()
        {
            // Setup the grid
            LegalEntityGrid.PostBackType = SAHL.Common.Web.UI.Controls.GridPostBackType.None;

            LegalEntityGrid.Columns.Clear();

            LegalEntityGrid.AddCheckBoxColumn("chkSelect", "", true, Unit.Percentage(1), HorizontalAlign.Center, true);
            LegalEntityGrid.AddGridBoundColumn("Key", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            LegalEntityGrid.AddGridBoundColumn("", "Legal Entity Name", Unit.Empty, HorizontalAlign.Left, true);
            LegalEntityGrid.AddGridBoundColumn("", "Date Of Birth", new Unit(100, UnitType.Pixel), HorizontalAlign.Center, true);
            LegalEntityGrid.AddGridBoundColumn("", "Next Age", new Unit(50, UnitType.Pixel), HorizontalAlign.Center, true);
            LegalEntityGrid.AddGridBoundColumn("", "Premium Age", new Unit(50, UnitType.Pixel), HorizontalAlign.Center, false);
            LegalEntityGrid.AddGridBoundColumn("", "Benefits", new Unit(150, UnitType.Pixel), HorizontalAlign.Left, true);

            LegalEntityGrid.Columns[(int)GridColumnPositions.DateOfBirth].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            LegalEntityGrid.Columns[(int)GridColumnPositions.AgeForPremiumCalc].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

            LegalEntityGrid.DataSource = lstLegalEntities;
            LegalEntityGrid.DataBind();
        }

        public void BindLifePolicyTypes(IEventList<ILifePolicyType> lifePolicyTypes)
        {
            ddlPolicyType.DataSource = lifePolicyTypes;
            ddlPolicyType.DataValueField = "Key";
            ddlPolicyType.DataTextField = "Description";
            ddlPolicyType.DataBind();
        }

        protected void ddlPolicyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCalculate_Click(sender, e);
        }

        public int PolicyTypeSelectedValue
        {
            set
            {
                ddlPolicyType.SelectedValue = value.ToString();
            }
            get
            {
                return Convert.ToInt32(ddlPolicyType.SelectedValue);
            }
        }

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
        ///
        /// </summary>
        public string LegalEntityName
        {
            get { return _legalEntityName; }
            set { _legalEntityName = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime? DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public IList<ILegalEntity> lstLegalEntities
        {
            get { return _lstLegalEntities; }
            set { _lstLegalEntities = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string SelectedAgeList
        {
            get { return _selectedAgeList; }
            set { _selectedAgeList = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public int IPBenefitMaxAge
        {
            get { return _ipBenefitMaxAge; }
            set { _ipBenefitMaxAge = value; }
        }

        #endregion IPremiumQuote Members

        /// <summary>
        ///
        /// </summary>
        /// <param name="lifePolicy"></param>
        /// <param name="applicationLife"></param>
        /// <param name="loanAccount"></param>
        /// <param name="hocFS"></param>
        private void BindPremiumDetails(ILifePolicy lifePolicy, IApplicationLife applicationLife, SAHL.Common.BusinessModel.Interfaces.IAccount loanAccount, SAHL.Common.BusinessModel.Interfaces.IHOC hocFS, double monthlyInstalment)
        {
            double dInitialSumAssured = 0, dCurrentSumAssured = 0, dIPBenefitPremium = 0, dDeathBenefitPremium = 0,
                dAnnualPremium = 0, dMonthlyInstalment = 0, dTotalInstalment = 0, dBondInstalment = 0,
                dHOCInstalment = 0, dHOCAdminFee = 0, dMonthlyServiceFee = 0;

            int remainingTerm = 0;

            IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();

            if (lifePolicy != null) // Get the data from the LifePolicy table
            {
                _accountKey = lifePolicy.FinancialService.Account.Key;

                dInitialSumAssured = lifePolicy.SumAssured;
                dCurrentSumAssured = lifePolicy.CurrentSumAssured.HasValue ? lifePolicy.CurrentSumAssured.Value : dInitialSumAssured;
                dIPBenefitPremium = lifePolicy.InstallmentProtectionPremium;
                dDeathBenefitPremium = lifePolicy.DeathBenefitPremium;
                dAnnualPremium = lifePolicy.YearlyPremium;
                dMonthlyInstalment = monthlyInstalment;
            }
            else // Get the data from the Offer Life table
            {
                _accountKey = applicationLife.ReservedAccount.Key;
                dInitialSumAssured = applicationLife.SumAssured;
                dCurrentSumAssured = applicationLife.CurrentSumAssured.HasValue ? applicationLife.CurrentSumAssured.Value : dInitialSumAssured;
                dIPBenefitPremium = applicationLife.InstallmentProtectionPremium;
                dDeathBenefitPremium = applicationLife.DeathBenefitPremium;
                dAnnualPremium = applicationLife.YearlyPremium;
                dMonthlyInstalment = applicationLife.MonthlyPremium;
            }

            // Loan data
            IMortgageLoanAccount mortgageLoanAccount = loanAccount as IMortgageLoanAccount;
            // get the variable portion, we will always have this around takeon which happens before a life policy is created
            _mortgageLoanVariableFS = mortgageLoanAccount.SecuredMortgageLoan;
            // see if we have a fixed portion
            IAccountVariFixLoan varifixLoanAccount = loanAccount as IAccountVariFixLoan;
            if (varifixLoanAccount != null)
                _mortgageLoanFixedFS = varifixLoanAccount.FixedSecuredMortgageLoan;

            // Get the Loan Application Object
            IApplicationMortgageLoan applicationMortgageLoan = mortgageLoanAccount.CurrentMortgageLoanApplication;
            IApplicationProductMortgageLoan applicationProductMortgageLoan = null;
            IApplicationProductVariFixLoan applicationProductVariFixLoan = null;

            remainingTerm = _mortgageLoanVariableFS == null ? 0 : Convert.ToInt32(_mortgageLoanVariableFS.RemainingInstallments);

            if (applicationMortgageLoan != null)
            {
                applicationProductMortgageLoan = applicationMortgageLoan.CurrentProduct as IApplicationProductMortgageLoan;
                applicationProductVariFixLoan = applicationMortgageLoan.CurrentProduct as IApplicationProductVariFixLoan;
            }

            if (_mortgageLoanVariableFS != null && _mortgageLoanVariableFS.Account.AccountStatus.Key == (int)AccountStatuses.Open)
            {
                dBondInstalment = _mortgageLoanFixedFS != null && _mortgageLoanFixedFS.Payment > 0 ? _mortgageLoanFixedFS.Payment + _mortgageLoanVariableFS.Payment : _mortgageLoanVariableFS.Payment;
            }
            else
            {
                if (applicationProductMortgageLoan != null)
                {
                    double dVariablePercent = applicationProductMortgageLoan.EffectiveRate.HasValue ? applicationProductMortgageLoan.EffectiveRate.Value : 0;
                    double dFixedPercent = applicationProductVariFixLoan == null ? 0 : (applicationProductVariFixLoan.FixedEffectiveRate.HasValue ? applicationProductVariFixLoan.FixedEffectiveRate.Value : 0);
                    double dVariableAmount = 0, dFixedAmount = 0;

                    // check for interest only
                    bool interestOnly = applicationMortgageLoan.HasFinancialAdjustment(FinancialAdjustmentTypeSources.InterestOnly);
                    // check for edge
                    if (interestOnly == false)
                        interestOnly = applicationMortgageLoan.HasFinancialAdjustment(FinancialAdjustmentTypeSources.Edge);

                    if (applicationProductVariFixLoan != null)
                    {
                        // TRAC #13636 - If the loan is an application always recalc the loan instalment
                        // get the variable instalment
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

                    // TRAC #13636 - If the loan is an application then add the HOC admin fee to the premium
                    dHOCAdminFee = Convert.ToDouble(ctrlRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.HOC.HOCAdministrationFee).ControlNumeric);
                }
            }

            // Populate HOC Details - do not include the prorata premium in the HOC instalment TRAC#11851
            if (hocFS != null)
                dHOCInstalment = hocFS.HOCMonthlyPremium.HasValue ? hocFS.HOCMonthlyPremium.Value : 0;

            // TRAC #13636  - Monthly Service Fee  - add to total premium
            //              - If the policy is inforce then get the discounted monthly service fee
            if (lifePolicy != null && lifePolicy.LifePolicyStatus.Key == (int)SAHL.Common.Globals.LifePolicyStatuses.Inforce)
                dMonthlyServiceFee = Convert.ToDouble(ctrlRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.SAHLMonthlyFeeDiscounted).ControlNumeric);
            else
                dMonthlyServiceFee = Convert.ToDouble(ctrlRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.SAHLMonthlyFee).ControlNumeric);

            lblCurrentSumAssured.Text = dCurrentSumAssured.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblIPBenefitPremium.Text = dIPBenefitPremium.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblDeathBenefitPremium.Text = dDeathBenefitPremium.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblAnnualPremium.Text = dAnnualPremium.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblMonthlyInstalment.Text = dMonthlyInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblBondInstallment.Text = dBondInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblHOCInstallment.Text = dHOCInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblMonthlyServiceFee.Text = dMonthlyServiceFee.ToString(SAHL.Common.Constants.CurrencyFormat);

            dTotalInstalment = dMonthlyInstalment + dBondInstalment + dHOCInstalment + dMonthlyServiceFee;
            lblTotalAmountDue.Text = dTotalInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LegalEntityGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Get the LegalEntity Row
                ILegalEntityNaturalPerson le = e.Row.DataItem as ILegalEntityNaturalPerson;

                // Name
                cells[(int)GridColumnPositions.Name].Text = le.GetLegalName(LegalNameFormat.Full);

                // Date Of Birth
                cells[(int)GridColumnPositions.DateOfBirth].Text = le.DateOfBirth.HasValue ? le.DateOfBirth.Value.ToString(SAHL.Common.Constants.DateFormat) : "";

                // Age to use in premium calc
                // if the policy has not been accepted, then this will be the legalentities age next birthday
                // otherwise it will be the age since the assured life was added to the policy
                if (_lifeRepo == null)
                    _lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();

                // get the date the assured life was added to the life policy
                DateTime? dteAddedToPolicy = _lifeRepo.GetDateAssuredLifeAddedToPolicy(_accountKey, le.Key);

                int ageForCalc = _lifeRepo.GetAssuredLifeAgeForPremiumCalc(le, dteAddedToPolicy.Value);
                cells[(int)GridColumnPositions.AgeForPremiumCalc].Text = ageForCalc.ToString();

                // Age at next birthday - if today is not the 1st then get the 1st of next month
                DateTime dteToUseInAgeCalc = DateTime.Now.Day != 1 ? SAHL.Common.Utils.DateUtils.FirstDayOfNextMonth(DateTime.Now) : DateTime.Now;
                cells[(int)GridColumnPositions.AgeNextBirthday].Text = le.DateOfBirth.HasValue ? DateUtils.CalculateAgeNextBirthday(le.DateOfBirth.Value, dteToUseInAgeCalc).ToString() : "-";
                if (ageForCalc > 0)
                    cells[(int)GridColumnPositions.AgeNextBirthday].ToolTip = "Age to use in premium calc : " + ageForCalc.ToString();

                if (PolicyTypeSelectedValue == (int)SAHL.Common.Globals.LifePolicyTypes.AccidentOnlyCover)
                    cells[(int)GridColumnPositions.Benefits].Text = "Accident Benefit";
                else
                {
                    // work out benefits based on age as at date the assured life was added to the life policy
                    DateTime dteFirstOfNextMonth = SAHL.Common.Utils.DateUtils.FirstDayOfNextMonth(dteAddedToPolicy.Value);
                    int ageAtDateAddedToPolicy = SAHL.Common.Utils.DateUtils.CalculateAgeNextBirthday(le.DateOfBirth.HasValue ? le.DateOfBirth.Value : DateTime.Now, dteFirstOfNextMonth);

                    cells[(int)GridColumnPositions.Benefits].Text = ageAtDateAddedToPolicy < _ipBenefitMaxAge ? "Death and IP Benefit" : "Death Benefit";
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddLife_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (dteDOB.Date != null)
                _dateOfBirth = dteDOB.Date.Value;
            _legalEntityName = txtName.Text.Trim();

            // add the entered legalentity to the list
            OnAddButtonClicked(sender, e);

            txtName.Text = "";
            dteDOB.Date = null;

            // rebind the legal entities
            BindAssuredLivesGrid();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            // Get the selected rows via the checkboxes from the GridView control
            bool firstSelected = true; ;
            SelectedAgeList = "";
            for (int i = 0; i < LegalEntityGrid.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)LegalEntityGrid.Rows[i].FindControl("chkSelect");
                object o = Page.Request.Form[cb.UniqueID];
                bool isChecked = (o == null) ? false : true;
                if (isChecked)
                {
                    if (firstSelected == true)
                    {
                        SelectedAgeList = LegalEntityGrid.Rows[i].Cells[(int)GridColumnPositions.AgeForPremiumCalc].Text;
                        firstSelected = false;
                    }
                    else
                        SelectedAgeList += ", " + LegalEntityGrid.Rows[i].Cells[(int)GridColumnPositions.AgeForPremiumCalc].Text;
                }
            }

            // perform the Premium Calculations
            OnCalculateButtonClicked(sender, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }
    }
}