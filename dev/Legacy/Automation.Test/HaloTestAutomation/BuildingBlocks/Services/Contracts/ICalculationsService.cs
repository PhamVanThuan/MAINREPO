namespace BuildingBlocks.Services.Contracts
{
    public interface ICalculationsService
    {
        float CalculateAffordabilityInstallment(float totalIncome);

        decimal CalculateInterestOnlyInstalment(decimal currentBalance, decimal interestRate);

        double CalculateLoanInstalment(double balance, double interestRate, int term);
    }
}