using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;

namespace SAHL.Common.BusinessModel.DataTransferObjects
{
    public class Calculated20YearLoanDetailsFor30YearTermLoan : ICalculated20YearLoanDetailsFor30YearTermLoan
    {
        public double PricingAdjustment { get; set; }

        public double EffectiveRate { get; set; }

        public double Instalment { get; set; }

        public double PTI { get; set; }
    }
}