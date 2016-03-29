using ObjectMaps.Pages;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class UpdateAffordabilityAssessmentControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtBasicGrossSalary_Drawings_Client")]
        protected TextField txtBasicGrossSalary_Drawings_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtCommission_Overtime_Client")]
        
        protected TextField txtCommission_Overtime_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtNet_Rental_Client")]
        protected TextField txtNet_Rental_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtInvestments_Client")]
        protected TextField txtInvestments_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtOtherIncome1_Client")]
        protected TextField txtOtherIncome1_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtOtherIncome2_Client")]
        protected TextField txtOtherIncome2_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtGrossIncome_Client_Total")]
        protected TextField txtGrossIncome_Client_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtPayrollDeductions_Client")]
        protected TextField txtPayrollDeductions_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtNetIncome_Client_Total")]
        protected TextField txtNetIncome_Client_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtAccomodation_Client")]
        protected TextField txtAccomodation_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtTransport_Client")]
        protected TextField txtTransport_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtFood_Client")]
        protected TextField txtFood_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtEducation_Client")]
        protected TextField txtEducation_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtMedical_Client")]
        protected TextField txtMedical_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtUtilities_Client")]
        protected TextField txtUtilities_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtChildSupport_Client")]
        protected TextField txtChildSupport_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtMonthlyNecessaryExpenses_Client_Total")]
        protected TextField txtMonthlyNecessaryExpenses_Client_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtAllocableIncome_Client_Total")]
        protected TextField txtAllocableIncome_Client_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtMinMonthlyFixedExpenses")]
        protected TextField txtMinMonthlyFixedExpenses { get; set; }

        [FindBy(Id = "ctl00_Main_txtBasicGrossSalary_Drawings_Credit")]
        protected TextField txtBasicGrossSalary_Drawings_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtCommission_Overtime_Credit")]
        protected TextField txtCommission_Overtime_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtNet_Rental_Credit")]
        protected TextField txtNet_Rental_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtInvestments_Credit")]
        protected TextField txtInvestments_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtOtherIncome1_Credit")]
        protected TextField txtOtherIncome1_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtOtherIncome2_Credit")]
        protected TextField txtOtherIncome2_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtGrossIncome_Credit_Total")]
        protected TextField txtGrossIncome_Credit_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtPayrollDeductions_Credit")]
        protected TextField txtPayrollDeductions_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtNetIncome_Credit_Total")]
        protected TextField txtNetIncome_Credit_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtAccomodation_Credit")]
        protected TextField txtAccomodation_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtTransport_Credit")]
        protected TextField txtTransport_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtFood_Credit")]
        protected TextField txtFood_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtEducation_Credit")]
        protected TextField txtEducation_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtMedical_Credit")]
        protected TextField txtMedical_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtUtilities_Credit")]
        protected TextField txtUtilities_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtChildSupport_Credit")]
        protected TextField txtChildSupport_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtMonthlyNecessaryExpenses_Credit_Total")]
        protected TextField txtMonthlyNecessaryExpenses_Credit_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtAppliedNCROverride_Credit_Total")]
        protected TextField txtAppliedNCROverride_Credit_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtAllocableIncome_Credit_Total")]
        protected TextField txtAllocableIncome_Credit_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtOtherBonds_Client")]
        protected TextField txtOtherBonds_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtVehicle_Client")]
        protected TextField txtVehicle_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtCreditCards_Client")]
        protected TextField txtCreditCards_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtPersonalLoans_Client")]
        protected TextField txtPersonalLoans_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtRetailAccounts_Client")]
        protected TextField txtRetailAccounts_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtOtherDebtExpenses_Client")]
        protected TextField txtOtherDebtExpenses_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtPayment_Client_Total")]
        protected TextField txtPayment_Client_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtDiscretionaryIncome_Client_Total")]
        protected TextField txtDiscretionaryIncome_Client_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtSAHLBond_Client")]
        protected TextField txtSAHLBond_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtHOC_Client")]
        protected TextField txtHOC_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtDomesticSalary_Client")]
        protected TextField txtDomesticSalary_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtInsurancePolicies_Client")]
        protected TextField txtInsurancePolicies_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtCommittedSavings_Client")]
        protected TextField txtCommittedSavings_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtSecurity_Client")]
        protected TextField txtSecurity_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtTelephoneTV_Client")]
        protected TextField txtTelephoneTV_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtOther_Client")]
        protected TextField txtOther_Client { get; set; }

        [FindBy(Id = "ctl00_Main_txtOther_Client_Total")]
        protected TextField txtOther_Client_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtSurplusDeficit_Client_Total")]
        protected TextField txtSurplusDeficit_Client_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtStressFactorValue")]
        protected TextField txtStressFactorValue { get; set; }

        [FindBy(Id = "ctl00_Main_txtAfterStressFactorApplied")]
        protected TextField txtAfterStressFactorApplied { get; set; }

        [FindBy(Id = "ctl00_Main_txtOtherBonds_Consolidate")]
        protected TextField txtOtherBonds_Consolidate { get; set; }

        [FindBy(Id = "ctl00_Main_txtVehicle_Consolidate")]
        protected TextField txtVehicle_Consolidate { get; set; }

        [FindBy(Id = "ctl00_Main_txtCreditCards_Consolidate")]
        protected TextField txtCreditCards_Consolidate { get; set; }

        [FindBy(Id = "ctl00_Main_txtPersonalLoans_Consolidate")]
        protected TextField txtPersonalLoans_Consolidate { get; set; }

        [FindBy(Id = "ctl00_Main_txtRetailAccounts_Consolidate")]
        protected TextField txtRetailAccounts_Consolidate { get; set; }

        [FindBy(Id = "ctl00_Main_txtOtherDebtExpenses_Consolidate")]
        protected TextField txtOtherDebtExpenses_Consolidate { get; set; }

        [FindBy(Id = "ctl00_Main_txtPayment_Consolidate_Total")]
        protected TextField txtPayment_Consolidate_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtOtherBonds_Credit")]
        protected TextField txtOtherBonds_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtVehicle_Credit")]
        protected TextField txtVehicle_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtCreditCards_Credit")]
        protected TextField txtCreditCards_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtPersonalLoans_Credit")]
        protected TextField txtPersonalLoans_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtRetailAccounts_Credit")]
        protected TextField txtRetailAccounts_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtOtherDebtExpenses_Credit")]
        protected TextField txtOtherDebtExpenses_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtPayment_Credit_Total")]
        protected TextField txtPayment_Credit_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtDiscretionaryIncome_Credit_Total")]
        protected TextField txtDiscretionaryIncome_Credit_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtSAHLBond_Credit")]
        protected TextField txtSAHLBond_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtHOC_Credit")]
        protected TextField txtHOC_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtDomesticSalary_Credit")]
        protected TextField txtDomesticSalary_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtInsurancePolicies_Credit")]
        protected TextField txtInsurancePolicies_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtCommittedSavings_Credit")]
        protected TextField txtCommittedSavings_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtSecurity_Credit")]
        protected TextField txtSecurity_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtTelephoneTV_Credit")]
        protected TextField txtTelephoneTV_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtOther_Credit")]
        protected TextField txtOther_Credit { get; set; }

        [FindBy(Id = "ctl00_Main_txtOther_Credit_Total")]
        protected TextField txtOther_Credit_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtSurplusDeficit_Credit_Total")]
        protected TextField txtSurplusDeficit_Credit_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtOtherBonds_ToBe")]
        protected TextField txtOtherBonds_ToBe { get; set; }

        [FindBy(Id = "ctl00_Main_txtVehicle_ToBe")]
        protected TextField txtVehicle_ToBe { get; set; }

        [FindBy(Id = "ctl00_Main_txtCreditCards_ToBe")]
        protected TextField txtCreditCards_ToBe { get; set; }

        [FindBy(Id = "ctl00_Main_txtPersonalLoans_ToBe")]
        protected TextField txtPersonalLoans_ToBe { get; set; }

        [FindBy(Id = "ctl00_Main_txtRetailAccounts_ToBe")]
        protected TextField txtRetailAccounts_ToBe { get; set; }

        [FindBy(Id = "ctl00_Main_txtOtherDebtExpenses_ToBe")]
        protected TextField txtOtherDebtExpenses_ToBe { get; set; }

        [FindBy(Id = "ctl00_Main_txtPayment_ToBe_Total")]
        protected TextField txtPayment_ToBe_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtDiscretionaryIncome_ToBe_Total")]
        protected TextField txtDiscretionaryIncome_ToBe_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtSAHLBond_ToBe")]
        protected TextField txtSAHLBond_ToBe { get; set; }

        [FindBy(Id = "ctl00_Main_txtHOC_ToBe")]
        protected TextField txtHOC_ToBe { get; set; }

        [FindBy(Id = "ctl00_Main_txtDomesticSalary_ToBe")]
        protected TextField txtDomesticSalary_ToBe { get; set; }

        [FindBy(Id = "ctl00_Main_txtInsurancePolicies_ToBe")]
        protected TextField txtInsurancePolicies_ToBe { get; set; }

        [FindBy(Id = "ctl00_Main_txtCommittedSavings_ToBe")]
        protected TextField txtCommittedSavings_ToBe { get; set; }

        [FindBy(Id = "ctl00_Main_txtSecurity_ToBe")]
        protected TextField txtSecurity_ToBe { get; set; }

        [FindBy(Id = "ctl00_Main_txtTelephoneTV_ToBe")]
        protected TextField txtTelephoneTV_ToBe { get; set; }

        [FindBy(Id = "ctl00_Main_txtOther_ToBe")]
        protected TextField txtOther_ToBe { get; set; }

        [FindBy(Id = "ctl00_Main_txtOther_ToBe_Total")]
        protected TextField txtOther_ToBe_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtSurplusDeficit_ToBe_Total")]
        protected TextField txtSurplusDeficit_ToBe_Total { get; set; }

        [FindBy(Id = "ctl00_Main_txtSummaryDebtToConsolidate")]
        protected TextField txtSummaryDebtToConsolidate { get; set; }

        [FindBy(Id = "ctl00_Main_txtSummaryNetIncome")]
        protected TextField txtSummaryNetIncome { get; set; }

        [FindBy(Id = "ctl00_Main_txtSummaryTotalExpenses")]
        protected TextField txtSummaryTotalExpenses { get; set; }

        [FindBy(Id = "ctl00_Main_txtSummarySurpusDeficit")]
        protected TextField txtSummarySurpusDeficit { get; set; }

        [FindBy(Id = "ctl00_Main_txtSummarySurplusPercent")]
        protected TextField txtSummarySurplusPercent { get; set; }

        [FindBy(Id = "ctl00_Main_hidOtherBonds_Comments")]
        protected TextField txtOtherBonds_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidCommission_Overtime_Comments")]
        protected TextField txtCommission_Overtime_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidVehicle_Comments")]
        protected TextField txtVehicle_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidNet_Rental_Comments")]
        protected TextField txtnNet_Rental_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidCreditCards_Comments")]
        protected TextField txtCreditCards_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidInvestments_Comments")]
        protected TextField txtInvestments_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidPersonalLoans_Comments")]
        protected TextField txtPersonalLoans_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidOtherIncome1_Comments")]
        protected TextField txtOtherIncome1_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidRetailAccounts_Comments")]
        protected TextField txtRetailAccounts_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidOtherIncome2_Comments")]
        protected TextField txtOtherIncome2_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidOtherDebtExpenses_Comments")]
        protected TextField txtOtherDebtExpenses_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidPayrollDeductions_Comments")]
        protected TextField txtPayrollDeductions_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidSAHLBond_Comments")]
        protected TextField txtSAHLBond_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidHOC_Comments")]
        protected TextField txtHOC_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidAccomodation_Comments")]
        protected TextField txtAccomodation_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidTransport_Comments")]
        protected TextField txtTransport_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidDomesticSalary_Comments")]
        protected TextField txtDomesticSalary_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidInsurancePolicies_Comments")]
        protected TextField txtInsurancePolicies_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidFood_Comments")]
        protected TextField txtFood_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidEducation_Comments")]
        protected TextField txtEducation_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidCommittedSavings_Comments")]
        protected TextField txtCommittedSavings_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidMedical_Comments")]
        protected TextField txtMedical_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidSecurity_Comments")]
        protected TextField txtSecurity_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidUtilities_Comments")]
        protected TextField txtUtilities_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidTelephoneTV_Comments")]
        protected TextField txtTelephoneTV_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidChildSupport_Comments")]
        protected TextField txtChildSupport_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidOther_Comments")]
        protected TextField txtOther_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_hidBasicGrossSalary_Drawings_Comments")]
        protected TextField txtBasicGrossSalary_Drawings_Comments { get; set; }

        [FindBy(Id = "ctl00_Main_ddlStressFactorPercentage")]
        protected SelectList ddlStressFactorPercentage { get; set; }

        [FindBy(Id = "ctl00_Main_lblStressFactorPercentage")]
        protected Span spanStressFactorPercentage { get; set; }

        [FindBy(Id = "ctl00_Main_lblContributingApplicants")]
        protected Span spanContributingApplicants { get; set; }

        [FindBy(Id = "ctl00_Main_lblHouseholdDependants")]
        protected Span spanHouseholdDependants { get; set; }

        [FindBy(Id = "ctl00_Main_lblStressFactorPercentage")]
        protected Span lblStressFactorPercentage { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button btnSubmit { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }
        
    }
}