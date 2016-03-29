using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class AffordabilityAssessmentNecessaryExpensesDetailModel
    {
        public AffordabilityAssessmentNecessaryExpensesDetailModel()
        {
            AccommodationExpenses_Rental = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.Accommodationexp_Rental);
            Transport = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.Transport);
            Food = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.Food);
            Education = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.Education);
            Medical = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.Medical);
            Utilities = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.Utilities);
            ChildSupport = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.ChildSupport);
        }

        public AffordabilityAssessmentItemModel AccommodationExpenses_Rental { get; set; }

        public AffordabilityAssessmentItemModel Transport { get; set; }

        public AffordabilityAssessmentItemModel Food { get; set; }

        public AffordabilityAssessmentItemModel Education { get; set; }

        public AffordabilityAssessmentItemModel Medical { get; set; }

        public AffordabilityAssessmentItemModel Utilities { get; set; }

        public AffordabilityAssessmentItemModel ChildSupport { get; set; }

        public int MonthlyTotal_Client
        {
            get
            {
                return (AccommodationExpenses_Rental.ClientValue ?? 0)
                    + (Transport.ClientValue ?? 0)
                    + (Food.ClientValue ?? 0)
                    + (Education.ClientValue ?? 0)
                    + (Medical.ClientValue ?? 0)
                    + (Utilities.ClientValue ?? 0)
                    + (ChildSupport.ClientValue ?? 0);
            }
        }

        public int MonthlyTotal_Credit
        {
            get
            {
                return (AccommodationExpenses_Rental.CreditValue ?? 0)
                    + (Transport.CreditValue ?? 0)
                    + (Food.CreditValue ?? 0)
                    + (Education.CreditValue ?? 0)
                    + (Medical.CreditValue ?? 0)
                    + (Utilities.CreditValue ?? 0)
                    + (ChildSupport.CreditValue ?? 0);
            }
        }
    }
}