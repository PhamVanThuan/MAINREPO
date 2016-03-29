using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class AffordabilityAssessmentSAHLPaymentObligationDetailModel
    {
        public AffordabilityAssessmentSAHLPaymentObligationDetailModel()
        {
            SAHLBond = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.SAHLBond);
            HOC = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.HOC);
        }

        public AffordabilityAssessmentItemModel SAHLBond { get; set; }

        public AffordabilityAssessmentItemModel HOC { get; set; }

        public int MonthlyTotal_Client
        {
            get
            {
                return (SAHLBond.ClientValue ?? 0)
                    + (HOC.ClientValue ?? 0);
            }
        }

        public int MonthlyTotal_Credit
        {
            get
            {
                return (SAHLBond.CreditValue ?? 0)
                    + (HOC.CreditValue ?? 0);
            }
        }
    }
}