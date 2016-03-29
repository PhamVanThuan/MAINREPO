using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Models.Client.FinancialProfile
{
    public class FinancialProfileChildModel : IHaloTileModel
    {
        public string AgeOfOldestConfirmationOfIncome { get; set; }

        public decimal TotalConfirmedIncome { get; set; }

        public decimal CurrentTotalExpensesProvidedOnMostRecentSAHLLoanApplication { get; set; }

        public decimal CurrentTotalMonthlyPaymentExposureToSAHL { get; set; }

        public decimal CurrentTotalOutstandingBalanceLoanExposureToSAHL { get; set; }

        public decimal CurrentTotalArrearBalanceExposureToSAHL { get; set; }
    }
}