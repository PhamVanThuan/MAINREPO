using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class AffordabilityAssessmentIncomeDeductionsDetailModel
    {
        public AffordabilityAssessmentIncomeDeductionsDetailModel()
        {
            PayrollDeductions = new AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType.PayrollDeductions);
        }

        public AffordabilityAssessmentItemModel PayrollDeductions { get; set; }

        public int GrossIncomeDeductions_Client
        {
            get
            {
                return (PayrollDeductions.ClientValue ?? 0);
            }
        }

        public int GrossIncomeDeductions_Credit
        {
            get
            {
                return (PayrollDeductions.CreditValue ?? 0);
            }
        }
    }
}