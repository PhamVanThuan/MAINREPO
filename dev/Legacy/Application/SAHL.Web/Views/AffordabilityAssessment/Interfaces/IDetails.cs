using SAHL.Common.Web.UI;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Web.Views.AffordabilityAssessment.Interfaces
{
    public enum AffordabilityAssessmentMode
    {
        Display,
        Update_Credit,
        Update_NonCredit
    }

    public interface IDetails : IViewBase
    {
        /// <summary>
        ///
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        /// <summary>
        ///
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        ///
        /// </summary>
        AffordabilityAssessmentMode AffordabilityAssessmentMode { get; set; }

        /// <summary>
        ///
        /// </summary>
        bool ButtonRowVisible { set; }

        int MinimumMonthlyFixedExpenses { get; }

        int AssessmentStressFactorKey { get; }

        int? BasicGrossSalary_Drawings_Client { get; }

        int? BasicGrossSalary_Drawings_Credit { get; }

        int? Commission_Overtime_Client { get; }

        int? Commission_Overtime_Credit { get; }

        int? Net_Rental_Client { get; }

        int? Net_Rental_Credit { get; }

        int? Investments_Client { get; }

        int? Investments_Credit { get; }

        int? OtherIncome1_Client { get; }

        int? OtherIncome1_Credit { get; }

        int? OtherIncome2_Client { get; }

        int? OtherIncome2_Credit { get; }

        int? GrossMonthlyIncome_Client { get; }

        int? PayrollDeductions_Client { get; }

        int? PayrollDeductions_Credit { get; }

        int? Accomodation_Client { get; }

        int? Accomodation_Credit { get; }

        int? Transport_Client { get; }

        int? Transport_Credit { get; }

        int? Food_Client { get; }

        int? Food_Credit { get; }

        int? Education_Client { get; }

        int? Education_Credit { get; }

        int? Medical_Client { get; }

        int? Medical_Credit { get; }

        int? Utilities_Client { get; }

        int? Utilities_Credit { get; }

        int? ChildSupport_Client { get; }

        int? ChildSupport_Credit { get; }

        int? OtherBonds_Client { get; }

        int? OtherBonds_Credit { get; }

        int? OtherBonds_Consolidate { get; }

        int? Vehicle_Client { get; }

        int? Vehicle_Credit { get; }

        int? Vehicle_Consolidate { get; }

        int? CreditCards_Client { get; }

        int? CreditCards_Credit { get; }

        int? CreditCards_Consolidate { get; }

        int? PersonalLoans_Client { get; }

        int? PersonalLoans_Credit { get; }

        int? PersonalLoans_Consolidate { get; }

        int? RetailAccounts_Client { get; }

        int? RetailAccounts_Credit { get; }

        int? RetailAccounts_Consolidate { get; }

        int? OtherDebtExpenses_Client { get; }

        int? OtherDebtExpenses_Credit { get; }

        int? OtherDebtExpenses_Consolidate { get; }

        int? SAHLBond_Client { get; set; }

        int? SAHLBond_Credit { get; set; }

        int? SAHLBond_ToBe { set; }

        int? HOC_Client { get; set; }

        int? HOC_Credit { get; set; }

        int? HOC_ToBe { set; }

        int? DomesticSalary_Client { get; }

        int? DomesticSalary_Credit { get; }

        int? InsurancePolicies_Client { get; }

        int? InsurancePolicies_Credit { get; }

        int? CommittedSavings_Client { get; }

        int? CommittedSavings_Credit { get; }

        int? Security_Client { get; }

        int? Security_Credit { get; }

        int? TelephoneTV_Client { get; }

        int? TelephoneTV_Credit { get; }

        int? Other_Client { get; }

        int? Other_Credit { get; }

        string BasicGrossSalary_Drawings_Comments { get; }

        string Commission_Overtime_Comments { get; }

        string Net_Rental_Comments { get; }

        string Investments_Comments { get; }

        string OtherIncome1_Comments { get; }

        string OtherIncome2_Comments { get; }

        string PayrollDeductions_Comments { get; }

        string Accomodation_Comments { get; }

        string Transport_Comments { get; }

        string Food_Comments { get; }

        string Education_Comments { get; }

        string Medical_Comments { get; }

        string Utilities_Comments { get; }

        string ChildSupport_Comments { get; }

        string OtherBonds_Comments { get; }

        string Vehicle_Comments { get; }

        string CreditCards_Comments { get; }

        string PersonalLoans_Comments { get; }

        string RetailAccounts_Comments { get; }

        string OtherDebtExpenses_Comments { get; }

        string SAHLBond_Comments { get; }

        string HOC_Comments { get; }

        string DomesticSalary_Comments { get; }

        string InsurancePolicies_Comments { get; }

        string CommittedSavings_Comments { get; }

        string Security_Comments { get; }

        string TelephoneTV_Comments { get; }

        string Other_Comments { get; }

        AffordabilityAssessmentModel AffordabilityAssessment { get; set; }

        IList<string> InvalidCommentsList { get; }

        bool CommentsValid { get; }

        bool SAHLBondValueRetrieved { get; set; }

        bool SAHLHocValueRetrieved { get; set; }

        bool FurtherLendingUser { get; set; }

        /// <summary>
        ///
        /// </summary>
        void BindAffordabilityAssessmentDetail();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="affordabilityAssessmentStressFactors"></param>
        /// <param name="assessmentStressFactorKey"></param>
        void BindAssessmentStressFactors(IEnumerable<AffordabilityAssessmentStressFactorModel> affordabilityAssessmentStressFactors, int assessmentStressFactorKey);
    }
}