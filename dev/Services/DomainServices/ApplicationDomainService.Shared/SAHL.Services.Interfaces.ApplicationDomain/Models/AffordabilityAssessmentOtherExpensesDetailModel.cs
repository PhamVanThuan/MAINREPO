using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class AffordabilityAssessmentOtherExpensesDetailModel
    {
        public AffordabilityAssessmentOtherExpensesDetailModel()
        {
            DomesticSalary = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.DomesticSalary);
            InsurancePolicies = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.InsurancePolicy_ies);
            Security = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.Security);
            Telephone_TV = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.Telephone_TV);
            Other = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.Other);
            CommittedSavings = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.CommittedSavings);
        }

        public AffordabilityAssessmentItemModel DomesticSalary { get; set; }

        public AffordabilityAssessmentItemModel InsurancePolicies { get; set; }

        public AffordabilityAssessmentItemModel Security { get; set; }

        public AffordabilityAssessmentItemModel Telephone_TV { get; set; }

        public AffordabilityAssessmentItemModel Other { get; set; }

        public AffordabilityAssessmentItemModel CommittedSavings { get; set; }

        public int MonthlyTotal_Client
        {
            get
            {
                return (DomesticSalary.ClientValue ?? 0)
                    + (InsurancePolicies.ClientValue ?? 0)
                    + (Security.ClientValue ?? 0)
                    + (Telephone_TV.ClientValue ?? 0)
                    + (Other.ClientValue ?? 0)
                    + (CommittedSavings.ClientValue ?? 0);
            }
        }

        public int MonthlyTotal_Credit
        {
            get
            {
                return (DomesticSalary.CreditValue ?? 0)
                    + (InsurancePolicies.CreditValue ?? 0)
                    + (Security.CreditValue ?? 0)
                    + (Telephone_TV.CreditValue ?? 0)
                    + (Other.CreditValue ?? 0)
                    + (CommittedSavings.CreditValue ?? 0);
            }
        }
    }
}