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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Authentication;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Helpers;

namespace SAHL.Web.Views.Life
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Account : SAHLCommonBaseView, SAHL.Web.Views.Life.Interfaces.IAccount
    {
        #region IAccount Members

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnLegalEntityGridSelectedIndexChanged;

        /// <summary>
        /// implements <see cref="SAHL.Web.Views.Life.Interfaces.IAccount.BindApplicantGrid"/>
        /// </summary>
        /// <param name="lstLegalEntities"></param>
        /// <param name="accountKey"></param>
        public void BindApplicantGrid(IReadOnlyEventList<ILegalEntity> lstLegalEntities, int accountKey)
        {
            // Setup the grid
            LegalEntityGrid.HeaderCaption = "Loan Applicants";
            LegalEntityGrid.GridHeight = 150;
            LegalEntityGrid.PostBackType = SAHL.Common.Web.UI.Controls.GridPostBackType.None;
            LegalEntityGrid.ColumnIDPassportVisible = true;
            LegalEntityGrid.ColumnIDPassportHeadingType = SAHL.Web.Controls.LegalEntityGrid.GridColumnIDPassportHeadingType.IDAndPassportNumber;
            LegalEntityGrid.ColumnRoleVisible = true;

            // Set the Data related properties
            LegalEntityGrid.AccountKey = accountKey;

            // Bind the grid
            LegalEntityGrid.BindLegalEntities(lstLegalEntities);

        }

        /// <summary>
        /// implements <see cref="SAHL.Web.Views.Life.Interfaces.IAccount.BindLoanSummaryControls"/>
        /// </summary>
        /// <param name="loanAccount"></param>
        /// <param name="applicationMortgageLoan"></param>
        /// <param name="applicationInformationVariableLoan"></param>
        /// <param name="applicationInformationVarifixLoan"></param>
        /// <param name="mortgageLoanVariable"></param>
        /// <param name="mortgageLoanFixed"></param>
        public void BindLoanSummaryControls(SAHL.Common.BusinessModel.Interfaces.IAccount loanAccount, IApplicationMortgageLoan applicationMortgageLoan, IApplicationInformationVariableLoan applicationInformationVariableLoan, IApplicationInformationVarifixLoan applicationInformationVarifixLoan, IMortgageLoan mortgageLoanVariable, IMortgageLoan mortgageLoanFixed)
        {
            IApplicationProductMortgageLoan applicationProductMortgageLoan = applicationMortgageLoan.CurrentProduct as IApplicationProductMortgageLoan;
            IApplicationProductVariFixLoan applicationProductVariFixLoan = applicationMortgageLoan.CurrentProduct as IApplicationProductVariFixLoan;

            int remainingTerm = applicationProductMortgageLoan != null && applicationProductMortgageLoan.Term.HasValue ? applicationProductMortgageLoan.Term.Value : 0;

            lblLoanNumber.Text = loanAccount.Key.ToString();
            lblStatus.Text = loanAccount.AccountStatus.Description;
            lblInitialTerm.Text = remainingTerm > 0 ? remainingTerm.ToString() + " months" : "-";
            lblBondAmount.Text = applicationInformationVariableLoan != null && applicationInformationVariableLoan.BondToRegister.HasValue ? applicationInformationVariableLoan.BondToRegister.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";

            double dPropertyValuation = mortgageLoanVariable.GetActiveValuationAmount();
            lblPropertyValuation.Text = dPropertyValuation.ToString(SAHL.Common.Constants.CurrencyFormat);
           
            lblNextResetDate.Text = mortgageLoanVariable.NextResetDate.HasValue ? mortgageLoanVariable.NextResetDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
            lblPurpose.Text = mortgageLoanVariable.MortgageLoanPurpose.Description.ToString();
            lblCloseDate.Text = mortgageLoanVariable.CloseDate.HasValue ? mortgageLoanVariable.CloseDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
            lblRemainingTerm.Text = mortgageLoanVariable.RemainingInstallments.ToString() + " months";

            lblArrearBalance.Text = mortgageLoanVariable.ArrearBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblCurrentBalance.Text = mortgageLoanVariable.CurrentBalance.ToString(SAHL.Common.Constants.CurrencyFormat);

            int _debitOrderDay = 0;
            // Get first active FinancialServiceBankAccount record 
            if (mortgageLoanVariable != null)
            {
                foreach (IFinancialServiceBankAccount bac in mortgageLoanVariable.FinancialServiceBankAccounts)
                {
                    if (bac.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
                    {
                        _debitOrderDay = bac.DebitOrderDay;
                        break;
                    }
                }
            }
            lblDebitOrderDay.Text = _debitOrderDay.ToString();
         
            bool openAccount=false;
            bool interestOnly = false;
            double dMonthlyInstalment = 0;
            if (mortgageLoanVariable != null && mortgageLoanVariable.Account.AccountStatus.Key == (int)AccountStatuses.Open)
            {
                openAccount = true;
                lblProduct.Text = mortgageLoanVariable.Account.Product.Description;
                lblOpenDate.Text = mortgageLoanVariable.OpenDate.Value.ToString(SAHL.Common.Constants.DateFormat);
                lblMarketRate.Text = mortgageLoanVariable.ActiveMarketRate.ToString(SAHL.Common.Constants.RateFormat);
                lblLinkRate.Text = mortgageLoanVariable.RateConfiguration.Margin.Value.ToString(SAHL.Common.Constants.RateFormat);
                lblMarketRateDescription.Text = mortgageLoanVariable.RateConfiguration.MarketRate.Description;
                lblEffectiveRate.Text = mortgageLoanVariable.InterestRate.ToString(SAHL.Common.Constants.RateFormat);

                // check for interest only
                interestOnly = mortgageLoanVariable.HasInterestOnly();
                dMonthlyInstalment = (mortgageLoanFixed != null && mortgageLoanFixed.Payment > 0) ? mortgageLoanFixed.Payment + mortgageLoanVariable.Payment : mortgageLoanVariable.Payment;
            }

            if (mortgageLoanFixed != null && mortgageLoanFixed.ActiveMarketRate > 0) // Varifix Loan
            {
                lblOpenDate.Text = lblOpenDate.Text + mortgageLoanFixed.OpenDate != null ? " / " + mortgageLoanFixed.OpenDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
                lblMarketRate.Text = lblMarketRate.Text + " / " + mortgageLoanFixed.ActiveMarketRate.ToString(SAHL.Common.Constants.RateFormat);
                lblLinkRate.Text = lblLinkRate.Text + " / " + mortgageLoanFixed.RateConfiguration.Margin.Value.ToString(SAHL.Common.Constants.RateFormat);
                lblMarketRateDescription.Text = lblMarketRateDescription.Text + " / " + mortgageLoanFixed.RateConfiguration.MarketRate.Description;
                lblEffectiveRate.Text = lblEffectiveRate.Text + " / " + mortgageLoanFixed.InterestRate.ToString(SAHL.Common.Constants.RateFormat);
            }

            if (openAccount==false)
            {
                double currentBalance = 0;
                double marketRateVariable = 0, marketRateFixed = 0;
                double linkRateVariable = 0, linkRateFixed = 0;
                double effectiveRateVariable = 0, effectiveRateFixed = 0;
                double variableAmount = 0, fixedAmount = 0;

                ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                lblMarketRateDescription.Text = lookupRepo.MarketRates.ObjectDictionary[((int)MarketRates.ThreeMonthJIBARRounded).ToString()].Description;

                if (applicationProductMortgageLoan != null)
                {
                    // check for interest only
                    interestOnly = applicationMortgageLoan.HasFinancialAdjustment(FinancialAdjustmentTypeSources.InterestOnly);
                    // check for edge
                    if (interestOnly == false)
                        interestOnly = applicationMortgageLoan.HasFinancialAdjustment(FinancialAdjustmentTypeSources.Edge);

                    lblProduct.Text = applicationMortgageLoan.GetLatestApplicationInformation().Product.Description;
                    
                    currentBalance = applicationProductMortgageLoan.LoanAgreementAmount.HasValue ? applicationProductMortgageLoan.LoanAgreementAmount.Value : 0;
                    if (applicationProductVariFixLoan != null)
                    {
                        marketRateVariable = applicationProductVariFixLoan.VariableMarketRate.HasValue ? applicationProductVariFixLoan.VariableMarketRate.Value : 0;
                        marketRateFixed = applicationProductVariFixLoan.FixedMarketRate.HasValue ? applicationProductVariFixLoan.FixedMarketRate.Value : 0;
                        linkRateVariable = applicationProductMortgageLoan.DiscountedLinkRate.HasValue ? applicationProductMortgageLoan.DiscountedLinkRate.Value : 0;
                        linkRateFixed = applicationProductVariFixLoan.DiscountedLinkRate.HasValue ? applicationProductVariFixLoan.DiscountedLinkRate.Value : 0;
                        effectiveRateVariable = applicationProductVariFixLoan.VariableEffectiveRate.HasValue ? applicationProductVariFixLoan.VariableEffectiveRate.Value : 0;
                        effectiveRateFixed = applicationProductVariFixLoan.FixedEffectiveRate.HasValue ? applicationProductVariFixLoan.FixedEffectiveRate.Value : 0;
                        // TRAC #13636 - If the loan is an application always recalc the loan installment
                        // get the variable instalment
                        variableAmount = applicationProductVariFixLoan.VariableRandValue.HasValue ? applicationProductVariFixLoan.VariableRandValue.Value : 0;
                        dMonthlyInstalment = LoanCalculator.CalculateInstallment(variableAmount, effectiveRateVariable, remainingTerm, interestOnly);
                        // add the fixed instalment
                        fixedAmount = applicationProductVariFixLoan.FixedRandValue.HasValue ? applicationProductVariFixLoan.FixedRandValue.Value : 0;
                        dMonthlyInstalment += LoanCalculator.CalculateInstallment(fixedAmount, effectiveRateFixed, remainingTerm, interestOnly);

                    }
                    else
                    {
                        marketRateVariable = applicationProductMortgageLoan.MarketRate.HasValue ? applicationProductMortgageLoan.MarketRate.Value : 0;
                        linkRateVariable = applicationProductMortgageLoan.DiscountedLinkRate.HasValue ? applicationProductMortgageLoan.DiscountedLinkRate.Value : 0;
                        effectiveRateVariable = applicationProductMortgageLoan.EffectiveRate.HasValue ? applicationProductMortgageLoan.EffectiveRate.Value : 0;
                        // TRAC #13636 - If the loan is an application always recalc the loan installment
                        dMonthlyInstalment = LoanCalculator.CalculateInstallment(currentBalance, effectiveRateVariable, remainingTerm, interestOnly);
                    }

                    lblOpenDate.Text = "-";
                    lblCurrentBalance.Text = currentBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
                    if (applicationProductVariFixLoan != null)
                    {
                        lblMarketRate.Text = marketRateVariable.ToString(SAHL.Common.Constants.RateFormat) + " / " + marketRateFixed.ToString(SAHL.Common.Constants.RateFormat);
                        lblLinkRate.Text = linkRateVariable.ToString(SAHL.Common.Constants.RateFormat) + " / " + linkRateFixed.ToString(SAHL.Common.Constants.RateFormat);
                        lblEffectiveRate.Text = effectiveRateVariable.ToString(SAHL.Common.Constants.RateFormat) + " / " + effectiveRateFixed.ToString(SAHL.Common.Constants.RateFormat);
                    }
                    else
                    {
                        lblMarketRate.Text = marketRateVariable.ToString(SAHL.Common.Constants.RateFormat);
                        lblLinkRate.Text = linkRateVariable.ToString(SAHL.Common.Constants.RateFormat);
                        lblEffectiveRate.Text = effectiveRateVariable.ToString(SAHL.Common.Constants.RateFormat);
                    }
                }
            }

            lblInstallment.Text = dMonthlyInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);   
            if (interestOnly)
                lblProduct.Text += " - Interest Only";

            // Get the Loan Pipeline Status
            ILifeRepository _lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
            string sPipelineStatus = _lifeRepo.GetLoanPipelineStatus(loanAccount.Key);
            lblPipelineStatus.Text = String.IsNullOrEmpty(sPipelineStatus) ? "-" : sPipelineStatus;
        }

        #endregion
    }
}