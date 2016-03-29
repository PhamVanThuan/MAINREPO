using Common.Extensions;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination
{
    /// <summary>
    ///
    /// </summary>
    public class AffordabilityDetailsUpdate : AffordabilityDetailsControls
    {
        /// <summary>
        /// Populates the Income fields provided
        /// </summary>
        /// <param name="basicIncome"></param>
        /// <param name="commissionAndOvertime"></param>
        /// <param name="rentalFromInvestmentProperties"></param>
        /// <param name="incomeFromOtherInvestments"></param>
        /// <param name="incomeFromOtherInvestmentsDescription"></param>
        /// <param name="otherIncome1"></param>
        /// <param name="otherIncome1Description"></param>
        /// <param name="otherIncome2"></param>
        /// <param name="otherIncome2Description"></param>
        public void PopulateIncomeFields(int basicIncome, int commissionAndOvertime, int rentalFromInvestmentProperties,
            int incomeFromOtherInvestments, string incomeFromOtherInvestmentsDescription, int otherIncome1, string otherIncome1Description,
            int otherIncome2, string otherIncome2Description)
        {
            base.txtBasic.SetNewValueIfNotTheSameAsCurrent(basicIncome.ToString());
            base.txtCommission.SetNewValueIfNotTheSameAsCurrent(commissionAndOvertime.ToString());
            base.txtRentalIncome.SetNewValueIfNotTheSameAsCurrent(rentalFromInvestmentProperties.ToString());
            base.txtIncomeOtherInv.SetNewValueIfNotTheSameAsCurrent(incomeFromOtherInvestments.ToString());
            base.txtIncomeOtherInvestmentsDesc.SetNewValueIfNotTheSameAsCurrent(incomeFromOtherInvestmentsDescription);
            base.txtOtherIncome1.SetNewValueIfNotTheSameAsCurrent(otherIncome1.ToString());
            base.txtOtherIncome1Desc.SetNewValueIfNotTheSameAsCurrent(otherIncome1Description);
            base.txtOtherIncome2.SetNewValueIfNotTheSameAsCurrent(otherIncome2.ToString());
            base.txtOtherIncome2Desc.SetNewValueIfNotTheSameAsCurrent(otherIncome2Description);
        }

        /// <summary>
        /// Populates the expense fields provided.
        /// </summary>
        /// <param name="salaryDeductions"></param>
        /// <param name="livingExpenses"></param>
        /// <param name="bondRepayments"></param>
        /// <param name="carRepayments"></param>
        /// <param name="creditCards"></param>
        /// <param name="overdraft"></param>
        /// <param name="retailAccounts"></param>
        /// <param name="creditAccounts"></param>
        /// <param name="plannedSavings"></param>
        /// <param name="otherInstalments"></param>
        /// <param name="otherInstalmentsDescription"></param>
        /// <param name="otherExpenses"></param>
        /// <param name="otherExpensesDescription"></param>
        /// <param name="rentalPayment"></param>
        /// <param name="personalLoans"></param>
        /// <param name="otherDebtPayment"></param>
        /// <param name="otherDebtPaymentDescription"></param>
        /// <param name="medical"></param>
        /// <param name="clothing"></param>
        /// <param name="utilities"></param>
        /// <param name="rateAndTaxes"></param>
        /// <param name="transport"></param>
        /// <param name="insurance"></param>
        /// <param name="domestic"></param>
        /// <param name="telephone"></param>
        /// <param name="education"></param>
        /// <param name="childsupport"></param>
        public void PopulateExpenseFields(int salaryDeductions, int livingExpenses, int bondRepayments, int carRepayments,
            int creditCards, int overdraft, int retailAccounts, int creditAccounts, int plannedSavings, int otherInstalments,
            string otherInstalmentsDescription, int otherExpenses, string otherExpensesDescription, int rentalPayment,
            int personalLoans, int otherDebtPayment, string otherDebtPaymentDescription, int medical, int clothing, int utilities,
            int rateAndTaxes, int transport, int insurance, int domestic, int telephone, int education, int childsupport)
        {
            base.txtSalaryDeductions.SetNewValueIfNotTheSameAsCurrent(salaryDeductions.ToString());
            base.txtLivingExpenses.SetNewValueIfNotTheSameAsCurrent(livingExpenses.ToString());
            base.txtOtherExpenses.SetNewValueIfNotTheSameAsCurrent(otherExpenses.ToString());
            base.txtOtherExpensesDesc.SetNewValueIfNotTheSameAsCurrent(otherExpensesDescription);
            base.txtMedicalExpenses.SetNewValueIfNotTheSameAsCurrent(medical.ToString());
            base.txtClothing.SetNewValueIfNotTheSameAsCurrent(clothing.ToString());
            base.txtUtilities.SetNewValueIfNotTheSameAsCurrent(utilities.ToString());
            base.txtRatesAndTaxs.SetNewValueIfNotTheSameAsCurrent(rateAndTaxes.ToString());
            base.txtTransport.SetNewValueIfNotTheSameAsCurrent(transport.ToString());
            base.txtInsurance.SetNewValueIfNotTheSameAsCurrent(insurance.ToString());
            base.txtDomestic.SetNewValueIfNotTheSameAsCurrent(domestic.ToString());
            base.txtTelephone.SetNewValueIfNotTheSameAsCurrent(telephone.ToString());
            base.txtEducation.SetNewValueIfNotTheSameAsCurrent(education.ToString());
            base.txtChildSupport.SetNewValueIfNotTheSameAsCurrent(childsupport.ToString());

            base.txtBondRepayments.SetNewValueIfNotTheSameAsCurrent(bondRepayments.ToString());
            base.txtCarRepayments.SetNewValueIfNotTheSameAsCurrent(carRepayments.ToString());
            base.txtCreditCards.SetNewValueIfNotTheSameAsCurrent(creditCards.ToString());
            base.txtOverdraft.SetNewValueIfNotTheSameAsCurrent(overdraft.ToString());
            base.txtRetailAccounts.SetNewValueIfNotTheSameAsCurrent(retailAccounts.ToString());
            base.txtCreditAccounts.SetNewValueIfNotTheSameAsCurrent(creditAccounts.ToString());
            base.txtPlannedSavings.SetNewValueIfNotTheSameAsCurrent(plannedSavings.ToString());
            base.txtOtherInstalments.SetNewValueIfNotTheSameAsCurrent(otherInstalments.ToString());
            base.txtOtherInstalmentsDesc.SetNewValueIfNotTheSameAsCurrent(otherInstalmentsDescription);

            base.txtRentalPayments.SetNewValueIfNotTheSameAsCurrent(rentalPayment.ToString());
            base.txtPersonalLoans.SetNewValueIfNotTheSameAsCurrent(personalLoans.ToString());
            base.txtOtherDebtPayment.SetNewValueIfNotTheSameAsCurrent(otherDebtPayment.ToString());
            base.txtOtherDebtPaymentDesc.SetNewValueIfNotTheSameAsCurrent(otherDebtPaymentDescription);
        }

        /// <summary>
        /// Populates the dependents fields
        /// </summary>
        /// <param name="dependantsInHousehold"></param>
        /// <param name="contributingDependants"></param>
        public void PopulateDependantFields(int dependantsInHousehold, int contributingDependants)
        {
            base.txtDependantsInHousehold.SetNewValueIfNotTheSameAsCurrent(dependantsInHousehold.ToString());
            base.txtContributingDependants.SetNewValueIfNotTheSameAsCurrent(contributingDependants.ToString());
        }

        /// <summary>
        /// Retrieves the Income Fields from the Screen
        /// </summary>
        /// <param name="basicIncome"></param>
        /// <param name="commissionAndOvertime"></param>
        /// <param name="rentalFromInvestmentProperties"></param>
        /// <param name="incomeFromOtherInvestments"></param>
        /// <param name="incomeFromOtherInvestmentsDescription"></param>
        /// <param name="otherIncome1"></param>
        /// <param name="otherIncome1Description"></param>
        /// <param name="otherIncome2"></param>
        /// <param name="otherIncome2Description"></param>
        public void GetIncomeFieldValues(out int basicIncome, out int commissionAndOvertime, out int rentalFromInvestmentProperties, out int incomeFromOtherInvestments,
            out string incomeFromOtherInvestmentsDescription, out int otherIncome1, out string otherIncome1Description, out int otherIncome2,
            out string otherIncome2Description)
        {
            basicIncome = int.Parse(base.txtBasic.Value);
            commissionAndOvertime = int.Parse(base.txtCommission.Value);
            rentalFromInvestmentProperties = int.Parse(base.txtRentalIncome.Value);
            incomeFromOtherInvestments = int.Parse(base.txtIncomeOtherInv.Value);
            incomeFromOtherInvestmentsDescription = base.txtIncomeOtherInvestmentsDesc.Value;
            otherIncome1 = int.Parse(base.txtOtherIncome1.Value);
            otherIncome1Description = base.txtOtherIncome1Desc.Value;
            otherIncome2 = int.Parse(base.txtOtherIncome2.Value);
            otherIncome2Description = base.txtOtherIncome2Desc.Value;
        }

        /// <summary>
        /// Retrieves the Expense Fields from the Screen
        /// </summary>
        /// <param name="salaryDeductions"></param>
        /// <param name="livingExpenses"></param>
        /// <param name="bondRepayments"></param>
        /// <param name="carRepayments"></param>
        /// <param name="creditCards"></param>
        /// <param name="overdraft"></param>
        /// <param name="retailAccounts"></param>
        /// <param name="creditAccounts"></param>
        /// <param name="plannedSavings"></param>
        /// <param name="otherInstalments"></param>
        /// <param name="otherInstalmentsDescription"></param>
        /// <param name="otherExpenses"></param>
        /// <param name="otherExpensesDescription"></param>
        public void GetExpenseFieldValues(out int salaryDeductions, out int livingExpenses, out int bondRepayments, out int carRepayments, out int creditCards,
            out int overdraft, out int retailAccounts, out int creditAccounts, out int plannedSavings, out int otherInstalments, out string otherInstalmentsDescription,
            out int otherExpenses, out string otherExpensesDescription)
        {
            salaryDeductions = int.Parse(base.txtSalaryDeductions.Value);
            livingExpenses = int.Parse(base.txtLivingExpenses.Value);
            bondRepayments = int.Parse(base.txtBondRepayments.Value);
            carRepayments = int.Parse(base.txtCarRepayments.Value);
            creditCards = int.Parse(base.txtCreditCards.Value);
            overdraft = int.Parse(base.txtOverdraft.Value);
            retailAccounts = int.Parse(base.txtRetailAccounts.Value);
            creditAccounts = int.Parse(base.txtCreditAccounts.Value);
            plannedSavings = int.Parse(base.txtPlannedSavings.Value);
            otherInstalments = int.Parse(base.txtOtherInstalments.Value);
            otherInstalmentsDescription = base.txtOtherInstalmentsDesc.Value;
            otherExpenses = int.Parse(base.txtOtherExpenses.Value);
            otherExpensesDescription = base.txtOtherExpensesDesc.Value;
        }

        public void GetAffordabilityTotals(out int incomeTotal, out int monthlyExpenseTotal, out int debtRepaymentTotal, out int totalIncome, out int totalExpense, out int affordability)
        {
            incomeTotal = int.Parse(lblIncomeTotal.Text.CleanCurrencyString(true));
            monthlyExpenseTotal = int.Parse(base.lblMonthlyExpenseTotal.Text.CleanCurrencyString(true));
            debtRepaymentTotal = int.Parse(base.lblDebtRepaymentTotal.Text.CleanCurrencyString(true));
            totalIncome = int.Parse(base.lblTotalIncome.Text.CleanCurrencyString(true));
            totalExpense = int.Parse(base.lblTotalExpenses.Text.CleanCurrencyString(true));
            affordability = int.Parse(base.lblAffordability.Text.CleanCurrencyString(true));
        }

        /// <summary>
        /// Enters the fields for the dependents
        /// </summary>
        /// <param name="dependantsInHousehold"></param>
        /// <param name="contributingDependants"></param>
        public void GetDependantFields(out string dependantsInHousehold, out string contributingDependants)
        {
            dependantsInHousehold = base.txtDependantsInHousehold.Value;
            contributingDependants = base.txtContributingDependants.Value;
        }

        /// <summary>
        /// Clicks the Submit Button
        /// </summary>
        public void ClickUpdateButton()
        {
            base.btnSubmit.Click();
        }

        /// <summary>
        /// Clicks the Cancel Button
        /// </summary>
        public void ClickCancelButton()
        {
            base.btnCancel.Click();
        }
    }
}