namespace SAHL.Common.BusinessModel.Interfaces.DataTransferObjects
{
    public interface ICalculated20YearLoanDetailsFor30YearTermLoan
    {
        double PricingAdjustment { get; set; }

        double EffectiveRate { get; set; }

        double Instalment { get; set; }

        double PTI { get; set; }
    }
}