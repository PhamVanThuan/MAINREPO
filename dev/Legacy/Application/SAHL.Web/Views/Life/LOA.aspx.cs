using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common;
using SAHL.Common.Authentication;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Life.Interfaces;

namespace SAHL.Web.Views.Life
{
    public partial class LOA : SAHLCommonBaseView, ILOA
    {
        private string _emailBody = "";

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            _emailBody = txtMailBody.Text;

            OnNextButtonClicked(sender, e);
        }

        #region ILOA Members

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnNextButtonClicked;

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationLife"></param>
        /// <param name="applicationMortgageLoan"></param>
        /// <param name="applicationInformationVariableLoan"></param>
        /// <param name="applicationInformationVarifixLoan"></param>
        /// <param name="mortgageLoanVariable"></param>
        /// <param name="mortgageLoanFixed"></param>
        /// <param name="accountHOC"></param>
        public void BindLOADetails(IApplication applicationLife, IApplicationMortgageLoan applicationMortgageLoan, IApplicationInformationVariableLoan applicationInformationVariableLoan, IApplicationInformationVarifixLoan applicationInformationVarifixLoan, IMortgageLoan mortgageLoanVariable, IMortgageLoan mortgageLoanFixed, IAccountHOC accountHOC)
        {
            double dCashPortion = 0, dInterimInterest = 0, dLoanAgreementAmount = 0, dTotalMonthlyInstalment = 0, dVariableAmount = 0, dFixedAmount = 0,
                   dMonthlyInstalment = 0, dRegistrationAmount = 0, dHOCSumAssured = 0, dHOCInstalment = 0,
                   dVariablePercent = 0, dFixedPercent = 0, dMonthlyServiceFee = 0;
            int remainingTerm = 0;

            IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();

            // Populate Loan Details
            remainingTerm = mortgageLoanVariable == null ? 0 : Convert.ToInt32(mortgageLoanVariable.RemainingInstallments);

            txtBondTerm.Text = remainingTerm.ToString() + " months";

            IApplicationProductMortgageLoan applicationProductMortgageLoan = applicationMortgageLoan.CurrentProduct as IApplicationProductMortgageLoan;

            IApplicationProductVariFixLoan applicationProductVariFixLoan = applicationMortgageLoan.CurrentProduct as IApplicationProductVariFixLoan;

            // TODO : Check the values for RegistrationAmount,LoanAgreementAmount,CashPortion adn InterimInterest

            // The following used to come off prospect but we must now get them from OfferInformationVariableLoan
            // For Varifix we add the Variable and the Fixed amounts
            if (applicationInformationVariableLoan != null)
            {
                dRegistrationAmount = applicationInformationVariableLoan.BondToRegister.HasValue ? Convert.ToDouble(applicationInformationVariableLoan.BondToRegister) : 0;
                dLoanAgreementAmount = applicationProductMortgageLoan.LoanAgreementAmount.HasValue ? Convert.ToDouble(applicationProductMortgageLoan.LoanAgreementAmount) : 0;
                IApplicationMortgageLoanWithCashOut AppCashOut = applicationMortgageLoan as IApplicationMortgageLoanWithCashOut;
                if (null != AppCashOut)
                    dCashPortion = AppCashOut.RequestedCashAmount.HasValue ? Convert.ToDouble(AppCashOut.RequestedCashAmount) : 0;
                dInterimInterest = applicationInformationVariableLoan.InterimInterest.HasValue ? Convert.ToDouble(applicationInformationVariableLoan.InterimInterest) : 0;

                txtRegistrationAmount.Text = dRegistrationAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                txtLoanAgreementAmount.Text = dLoanAgreementAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                txtCashPortion.Text = dCashPortion.ToString(SAHL.Common.Constants.CurrencyFormat);
                txtInterimInterest.Text = dInterimInterest.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

            // If there is a mortgage loan then get the values from there otherwise get the values from the offer
            bool interestOnly = false;
            if (mortgageLoanVariable != null && mortgageLoanVariable.Account.AccountStatus.Key == (int)AccountStatuses.Open)
            {
                dMonthlyInstalment = (mortgageLoanFixed != null && mortgageLoanFixed.Payment > 0) ? mortgageLoanFixed.Payment + mortgageLoanVariable.Payment : mortgageLoanVariable.Payment;
                dVariablePercent = mortgageLoanVariable == null ? 0 : mortgageLoanVariable.InterestRate;
                dFixedPercent = mortgageLoanFixed == null ? 0 : mortgageLoanFixed.InterestRate;

                // check for interest only
                interestOnly = mortgageLoanVariable.HasInterestOnly();

                txtVariFix.Text = mortgageLoanFixed == null ? "No" : "Yes";
            }
            else
            {
                if (applicationProductMortgageLoan != null)
                {
                    double pricingAdjustment = applicationProductMortgageLoan.Application.GetRateAdjustments();
                    dVariablePercent = (applicationProductMortgageLoan.EffectiveRate.HasValue ? applicationProductMortgageLoan.EffectiveRate.Value : 0) + pricingAdjustment;
                    dFixedPercent = applicationProductVariFixLoan == null ? 0 : ((applicationProductVariFixLoan.FixedEffectiveRate.HasValue ? applicationProductVariFixLoan.FixedEffectiveRate.Value : 0) + pricingAdjustment);

                    // check for interest only
                    interestOnly = applicationMortgageLoan.HasFinancialAdjustment(FinancialAdjustmentTypeSources.InterestOnly);
                    // check for edge
                    if (interestOnly == false)
                        interestOnly = applicationMortgageLoan.HasFinancialAdjustment(FinancialAdjustmentTypeSources.Edge);

                    if (applicationProductVariFixLoan != null)
                    {
                        // TRAC #13636 - If the loan is an application always recalc the loan instalment
                        // get the variable instalment
                        dVariableAmount = applicationProductVariFixLoan.VariableRandValue.HasValue ? applicationProductVariFixLoan.VariableRandValue.Value : 0;
                        dMonthlyInstalment = LoanCalculator.CalculateInstallment(dVariableAmount, dVariablePercent, remainingTerm, interestOnly);
                        // add the fixed instalment
                        dFixedAmount = applicationProductVariFixLoan.FixedRandValue.HasValue ? applicationProductVariFixLoan.FixedRandValue.Value : 0;
                        dMonthlyInstalment += LoanCalculator.CalculateInstallment(dFixedAmount, dFixedPercent, remainingTerm, interestOnly);
                    }
                    else
                    {
                        // get the variable istalment
                        dVariableAmount = applicationProductMortgageLoan.LoanAgreementAmount.HasValue ? applicationProductMortgageLoan.LoanAgreementAmount.Value : 0;
                        dMonthlyInstalment = LoanCalculator.CalculateInstallment(dVariableAmount, dVariablePercent, remainingTerm, interestOnly);
                    }

                    txtVariFix.Text = applicationProductVariFixLoan == null ? "No" : "Yes";
                }
            }

            txtInterestOnly.Text = interestOnly ? "Yes" : "No";

            txtMonthlyInstalment.Text = dMonthlyInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);

            txtVariable.Text = dVariablePercent == 0 ? "" : (dVariablePercent * 100).ToString();
            txtFixed.Text = dFixedPercent == 0 ? "" : (dFixedPercent * 100).ToString();

            // Populate HOC Details - do not include the prorata premium in the HOC instalment TRAC#11851
            if (accountHOC != null)
            {
                dHOCSumAssured = accountHOC.HOC.HOCTotalSumInsured;
                dHOCInstalment = accountHOC.HOC.HOCMonthlyPremium.HasValue ? accountHOC.HOC.HOCMonthlyPremium.Value : 0;
            }

            txtHOCSumAssured.Text = dHOCSumAssured.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtHOCPremium.Text = dHOCInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);

            // TRAC #13636 - Get the SAHL Monthly Service Fee to add to total monthly instalment
            dMonthlyServiceFee = Convert.ToDouble(ctrlRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.SAHLMonthlyFee).ControlNumeric);
            lblMonthlyServiceFee.Text = "( includes " + dMonthlyServiceFee.ToString(SAHL.Common.Constants.CurrencyFormat) + " monthly service fee )";

            dTotalMonthlyInstalment = dHOCInstalment + dMonthlyInstalment + dMonthlyServiceFee;
            txtTotalMonthlyInstalment.Text = dTotalMonthlyInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);

            lblLoanConsultant.Text = "-"; lblAttorneyName.Text = "-";

            // Populate Attorney Name
            //http://sahls31:8181/trac/SAHL.db/ticket/12791
            //IReadOnlyEventList<IApplicationRole> rolesAttorney = applicationMortgageLoan.GetApplicationRolesByType(OfferRoleTypes.ConveyanceAttorney);
            //lblAttorneyName.Text = rolesAttorney != null && rolesAttorney.Count > 0 ? rolesAttorney[0].LegalEntity.DisplayName : "Unknown";
            // Populate Attorney Name
            IApplicationRole roleAttorney = applicationMortgageLoan.GetLatestApplicationRoleByType(OfferRoleTypes.ConveyanceAttorney);
            lblAttorneyName.Text = roleAttorney != null ? roleAttorney.LegalEntity.GetLegalName(LegalNameFormat.FullNoSalutation) : "Unknown";
            // Populate Branch Consultant Name
            IApplicationRole roleConsultant = applicationMortgageLoan.GetLatestApplicationRoleByType(OfferRoleTypes.BranchConsultantD);
            lblLoanConsultant.Text = roleConsultant != null ? roleConsultant.LegalEntity.GetLegalName(LegalNameFormat.FullNoSalutation) : "Unknown";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="loanConditions"></param>
        public void BindLoanConditions(IList<string> loanConditions)
        {
            // Setup Loan Conditions Grid
            LoanConditionsGrid.Columns.Clear();
            //LoanConditionsGrid.HeaderCaption = "Loan Conditions";
            //LoanConditionsGrid.HeaderRow.Visible = false;
            LoanConditionsGrid.AddGridBoundColumn("", "", Unit.Percentage(100), HorizontalAlign.Left, true);

            LoanConditionsGrid.DataSource = loanConditions;
            LoanConditionsGrid.DataBind();
        }

        #endregion ILOA Members

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoanConditionsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                e.Row.Cells[0].Text = e.Row.DataItem.ToString();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string EmailBody
        {
            get { return _emailBody; }
            set { _emailBody = value; }
        }
    }
}