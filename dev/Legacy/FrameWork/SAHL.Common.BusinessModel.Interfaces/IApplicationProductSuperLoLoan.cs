namespace SAHL.Common.BusinessModel.Interfaces
{
    public interface IApplicationProductSuperLoLoan : IApplicationProductMortgageLoan, ISupportsInterestOnlyApplicationInformation, ISupportsSuperLoApplicationInformation, ISupportsVariableLoanApplicationInformation
    {
        double LoyaltyBonusY2 { get; }

        double LoyaltyBonusY3 { get; }

        double LoyaltyBonusY4 { get; }

        double LoyaltyBonusY5 { get; }
    }
}