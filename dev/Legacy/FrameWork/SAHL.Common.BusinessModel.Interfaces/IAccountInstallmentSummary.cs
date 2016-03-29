namespace SAHL.Common.BusinessModel.Interfaces
{
    public interface IAccountInstallmentSummary
    {
        double AmortisingInstallment { get; }

        bool IsInterestOnly { get; }

        double MonthsInArrears { get; }

        double MonthlyServiceFee { get; }

        double TotalAmountDue { get; }

        double TotalLoanInstallment { get; }

        double TotalPremium { get; }

        double TotalShortTermLoanInstallment { get; }

        double TotalArrearsBalance { get; }

        double TotalRegentPremium { get; }

        double CurrentBalance { get; }
    }
}