using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class AffordabilityAssessmentPaymentObligationDetailModel
    {
        public AffordabilityAssessmentPaymentObligationDetailModel()
        {
            OtherBonds = new AffordabilityAssessmentConsolidatableItemModel(AffordabilityAssessmentItemType.OtherBond_s);
            Vehicle = new AffordabilityAssessmentConsolidatableItemModel(AffordabilityAssessmentItemType.Vehicle);
            CreditCards = new AffordabilityAssessmentConsolidatableItemModel(AffordabilityAssessmentItemType.CreditCard_s);
            PersonalLoans = new AffordabilityAssessmentConsolidatableItemModel(AffordabilityAssessmentItemType.PersonalLoan_s);
            RetailAccounts = new AffordabilityAssessmentConsolidatableItemModel(AffordabilityAssessmentItemType.RetailAccounts);
            OtherDebtExpenses = new AffordabilityAssessmentConsolidatableItemModel(AffordabilityAssessmentItemType.OtherDebtExpenses);
        }

        public AffordabilityAssessmentConsolidatableItemModel OtherBonds { get; set; }

        public AffordabilityAssessmentConsolidatableItemModel Vehicle { get; set; }

        public AffordabilityAssessmentConsolidatableItemModel CreditCards { get; set; }

        public AffordabilityAssessmentConsolidatableItemModel PersonalLoans { get; set; }

        public AffordabilityAssessmentConsolidatableItemModel RetailAccounts { get; set; }

        public AffordabilityAssessmentConsolidatableItemModel OtherDebtExpenses { get; set; }

        public int MonthlyTotal_Client
        {
            get
            {
                return (OtherBonds.ClientValue ?? 0)
                    + (Vehicle.ClientValue ?? 0)
                    + (CreditCards.ClientValue ?? 0)
                    + (PersonalLoans.ClientValue ?? 0)
                    + (RetailAccounts.ClientValue ?? 0)
                    + (OtherDebtExpenses.ClientValue ?? 0);
            }
        }

        public int MonthlyTotal_DebtToConsolidate
        {
            get
            {
                return (OtherBonds.ConsolidationValue ?? 0)
                    + (Vehicle.ConsolidationValue ?? 0)
                    + (CreditCards.ConsolidationValue ?? 0)
                    + (PersonalLoans.ConsolidationValue ?? 0)
                    + (RetailAccounts.ConsolidationValue ?? 0)
                    + (OtherDebtExpenses.ConsolidationValue ?? 0);
            }
        }

        public int MonthlyTotal_Credit
        {
            get
            {
                return (OtherBonds.CreditValue ?? 0)
                    + (Vehicle.CreditValue ?? 0)
                    + (CreditCards.CreditValue ?? 0)
                    + (PersonalLoans.CreditValue ?? 0)
                    + (RetailAccounts.CreditValue ?? 0)
                    + (OtherDebtExpenses.CreditValue ?? 0);
            }
        }

        public int MonthlyTotal_ToBe
        {
            get
            {
                return OtherBonds.ToBeValue + Vehicle.ToBeValue + CreditCards.ToBeValue + PersonalLoans.ToBeValue
                    + RetailAccounts.ToBeValue + OtherDebtExpenses.ToBeValue;
            }
        }
    }
}