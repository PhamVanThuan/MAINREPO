using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class AffordabilityAssessmentIncomeDetailModel
    {
        public AffordabilityAssessmentIncomeDetailModel()
        {
            BasicGrossSalary_Drawings = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.BasicGrossSalary_Drawings);
            Commission_Overtime = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.Commission_Overtime);
            Net_Rental = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.NetRental);
            Investments = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.Investments);
            OtherIncome1 = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.OtherIncome1);
            OtherIncome2 = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.OtherIncome2);
        }

        public AffordabilityAssessmentItemModel BasicGrossSalary_Drawings { get; set; }

        public AffordabilityAssessmentItemModel Commission_Overtime { get; set; }

        public AffordabilityAssessmentItemModel Net_Rental { get; set; }

        public AffordabilityAssessmentItemModel Investments { get; set; }

        public AffordabilityAssessmentItemModel OtherIncome1 { get; set; }

        public AffordabilityAssessmentItemModel OtherIncome2 { get; set; }

        public int GrossIncome_Client
        {
            get
            {
                return (BasicGrossSalary_Drawings.ClientValue ?? 0)
                    + (Commission_Overtime.ClientValue ?? 0)
                    + (Net_Rental.ClientValue ?? 0)
                    + (Investments.ClientValue ?? 0)
                    + (OtherIncome1.ClientValue ?? 0)
                    + (OtherIncome2.ClientValue ?? 0);
            }
        }

        public int GrossIncome_Credit
        {
            get
            {
                return (BasicGrossSalary_Drawings.CreditValue ?? 0)
                    + (Commission_Overtime.CreditValue ?? 0)
                    + (Net_Rental.CreditValue ?? 0)
                    + (Investments.CreditValue ?? 0)
                    + (OtherIncome1.CreditValue ?? 0)
                    + (OtherIncome2.CreditValue ?? 0);
            }
        }
    }
}