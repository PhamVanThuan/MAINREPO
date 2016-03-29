using ObjectMaps.Pages;
using Common.Extensions;
using WatiN.Core;
using System;
using BuildingBlocks.Services.Contracts;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace BuildingBlocks.Presenters.Origination
{
    public class UpdateAffordabilityAssessment : UpdateAffordabilityAssessmentControls
    {

        public void PopulateClientFields(double expected_income_field_value, double expected_expense_field_value)
        {
            SetValue(base.txtBasicGrossSalary_Drawings_Client, expected_income_field_value);
            SetValue(base.txtCommission_Overtime_Client, expected_income_field_value);
            SetValue(base.txtNet_Rental_Client, expected_income_field_value);
            SetValue(base.txtInvestments_Client, expected_income_field_value);
            SetValue(base.txtOtherIncome1_Client, expected_income_field_value);
            SetValue(base.txtOtherIncome2_Client, expected_income_field_value);
            SetValue(base.txtPayrollDeductions_Client, expected_expense_field_value);
            SetValue(base.txtAccomodation_Client, expected_expense_field_value);
            SetValue(base.txtTransport_Client, expected_expense_field_value);
            SetValue(base.txtFood_Client, expected_expense_field_value);
            SetValue(base.txtEducation_Client, expected_expense_field_value);
            SetValue(base.txtMedical_Client, expected_expense_field_value);
            SetValue(base.txtUtilities_Client, expected_expense_field_value);
            SetValue(base.txtChildSupport_Client, expected_expense_field_value);
            SetValue(base.txtOtherBonds_Client, expected_expense_field_value);
            SetValue(base.txtVehicle_Client, expected_expense_field_value);
            SetValue(base.txtCreditCards_Client, expected_expense_field_value);
            SetValue(base.txtPersonalLoans_Client, expected_expense_field_value);
            SetValue(base.txtRetailAccounts_Client, expected_expense_field_value);
            SetValue(base.txtOtherDebtExpenses_Client, expected_expense_field_value);
            SetValue(base.txtSAHLBond_Client, expected_expense_field_value);
            SetValue(base.txtHOC_Client, expected_expense_field_value);
            SetValue(base.txtDomesticSalary_Client, expected_expense_field_value);
            SetValue(base.txtInsurancePolicies_Client, expected_expense_field_value);
            SetValue(base.txtCommittedSavings_Client, expected_expense_field_value);
            SetValue(base.txtSecurity_Client, expected_expense_field_value);
            SetValue(base.txtTelephoneTV_Client, expected_expense_field_value);
            SetValue(base.txtOther_Client, expected_expense_field_value);
        }

        public void PopulateCreditFields(double expected_income_field_value, double expected_expense_field_value)
        {
            SetValue(base.txtBasicGrossSalary_Drawings_Credit, expected_income_field_value);
            SetValue(base.txtCommission_Overtime_Credit, expected_income_field_value);
            SetValue(base.txtNet_Rental_Credit, expected_income_field_value);
            SetValue(base.txtInvestments_Credit, expected_income_field_value);
            SetValue(base.txtOtherIncome1_Credit, expected_income_field_value);
            SetValue(base.txtOtherIncome2_Credit, expected_income_field_value);
            SetValue(base.txtPayrollDeductions_Credit, expected_expense_field_value);
            SetValue(base.txtAccomodation_Credit, expected_expense_field_value);
            SetValue(base.txtTransport_Credit, expected_expense_field_value);
            SetValue(base.txtFood_Credit, expected_expense_field_value);
            SetValue(base.txtEducation_Credit, expected_expense_field_value);
            SetValue(base.txtMedical_Credit, expected_expense_field_value);
            SetValue(base.txtUtilities_Credit, expected_expense_field_value);
            SetValue(base.txtChildSupport_Credit, expected_expense_field_value);
            SetValue(base.txtOtherBonds_Credit, expected_expense_field_value);
            SetValue(base.txtVehicle_Credit, expected_expense_field_value);
            SetValue(base.txtCreditCards_Credit, expected_expense_field_value);
            SetValue(base.txtPersonalLoans_Credit, expected_expense_field_value);
            SetValue(base.txtRetailAccounts_Credit, expected_expense_field_value);
            SetValue(base.txtOtherDebtExpenses_Credit, expected_expense_field_value);
            SetValue(base.txtOtherBonds_Consolidate, expected_expense_field_value);
            SetValue(base.txtVehicle_Consolidate, expected_expense_field_value);
            SetValue(base.txtCreditCards_Consolidate, expected_expense_field_value);
            SetValue(base.txtPersonalLoans_Consolidate, expected_expense_field_value);
            SetValue(base.txtRetailAccounts_Consolidate, expected_expense_field_value);
            SetValue(base.txtOtherDebtExpenses_Consolidate, expected_expense_field_value);
            SetValue(base.txtSAHLBond_Credit, expected_expense_field_value);
            SetValue(base.txtHOC_Credit, expected_expense_field_value);
            SetValue(base.txtDomesticSalary_Credit, expected_expense_field_value);
            SetValue(base.txtInsurancePolicies_Credit, expected_expense_field_value);
            SetValue(base.txtCommittedSavings_Credit, expected_expense_field_value);
            SetValue(base.txtSecurity_Credit, expected_expense_field_value);
            SetValue(base.txtTelephoneTV_Credit, expected_expense_field_value);
            SetValue(base.txtOther_Credit, expected_expense_field_value);
        }

        public void PopulateSingleCreditField(double expected_income_field_value)
        {
            SetValue(base.txtOtherIncome1_Credit, expected_income_field_value);
        }
        public void SelectStressFactorPercentage(int stressFactor)
        {
            base.ddlStressFactorPercentage.SelectByValue(stressFactor.ToString());
        }

        public string GetStressFactorPercentage()
        {
            return base.lblStressFactorPercentage.Text.CleanPercentageString(false);
        }

        public void SetCommentFields(string fieldToUpdate = "")
        {
            PropertyInfo[] propInfos = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic);

            if (fieldToUpdate == "")
            {
                foreach (var prop in propInfos.Where(x => x.Name.ToLower().Contains("comment")))
                {
                    SetTextFieldPropertyValue(prop);
                }
            }
            else
            {
                foreach (var prop in propInfos.Where(x => x.Name.ToLower().Contains("comment")))
                {
                    if (fieldToUpdate != "" && prop.Name.Equals(fieldToUpdate))
                    {
                        SetTextFieldPropertyValue(prop);
                    }
                }
            }
        }

        private void SetTextFieldPropertyValue(PropertyInfo prop)
        {
            var commentField = (TextField)prop.GetValue(this);
            commentField.Value = String.Format("Comment for {0}", prop.Name);
            prop.SetValue(this, commentField);
        }

        private void SetValue(TextField fieldToUpdate, double value)
        {
            fieldToUpdate.Value = value.ToString();
            fieldToUpdate.Change();
        }

        public void ClickSave()
        {
            base.btnSubmit.Click();
        }

        public void IgnoreWarningsAndContinue()
        {
            if (base.divValidationSummaryBody.Exists)
            {
                ServiceLocator.Instance.GetService<IWatiNService>().HandleConfirmationPopup(base.btnSubmit);
            }

        }

        public int GetClientMonthlyTotalExpenses()
        {
            return Convert.ToInt32(base.txtMonthlyNecessaryExpenses_Client_Total.Value.CleanCurrencyString(false));
        }

        public int GetClientNetIncome()
        {
            return Convert.ToInt32(base.txtNetIncome_Client_Total.Value.CleanCurrencyString(false));
        }

        public int GetCreditNetIncome()
        {
            return Convert.ToInt32(base.txtNetIncome_Credit_Total.Value.CleanCurrencyString(false));
        }

        public int GetCreditMonthlyTotalExpenses()
        {
            return Convert.ToInt32(base.txtMonthlyNecessaryExpenses_Credit_Total.Value.CleanCurrencyString(false));
        }

        public int GetSAHLBond()
        {
            return Convert.ToInt32(base.txtSAHLBond_Client.Value.CleanCurrencyString(false));
        }

        public int GetHOC()
        {
            return Convert.ToInt32(base.txtHOC_Client.Value.CleanCurrencyString(false));
        }

        public int GetOtherExpensesMonthlyClientTotal()
        {
            return Convert.ToInt32(base.txtOther_Client_Total.Value.CleanCurrencyString(false));
        }

        public int GetOtherExpensesMonthlyCreditTotal()
        {
            return Convert.ToInt32(base.txtOther_Credit_Total.Value.CleanCurrencyString(false));
        }

        public int GetDebtToConsolidateCreditTotal()
        {
            return Convert.ToInt32(base.txtPayment_Consolidate_Total.Value.CleanCurrencyString(false));
        }

        public int GetPaymentObligationsClientTotal()
        {
            return Convert.ToInt32(base.txtPayment_Client_Total.Value.CleanCurrencyString(false));
        }

        public int GetPaymentObligationsCreditTotal()
        {
            return Convert.ToInt32(base.txtPayment_Credit_Total.Value.CleanCurrencyString(false));
        }

        public int GetSummaryNetIncome()
        {
            return Convert.ToInt32(base.txtSummaryNetIncome.Value.CleanCurrencyString(false));
        }

        public int GetSummaryTotalExpenses()
        {
            return Convert.ToInt32(base.txtSummaryTotalExpenses.Value.CleanCurrencyString(false));
        }

        public int GetSummarySurplus_Deficit()
        {
            return Convert.ToInt32(base.txtSummarySurpusDeficit.Value.CleanCurrencyString(false));
        }

        public int GetSummaryNetHouseholdIncomePercentage()
        {
            return Convert.ToInt32(base.txtSummarySurplusPercent.Value.CleanPercentageString(false));
        }

        public int GetAppliedNCROverride()
        {
            return Convert.ToInt32(base.txtAppliedNCROverride_Credit_Total.Value.CleanCurrencyString(false));
        }

        public object GetPaymentObligationDebtToConsolidateTotal()
        {
            return Convert.ToInt32(base.txtPayment_Consolidate_Total.Value.CleanCurrencyString(false));
        }
    }
}