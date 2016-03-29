using BuildingBlocks.Services.Contracts;
using System;

namespace BuildingBlocks.Services
{
    public class CalculationsService : ICalculationsService
    {
        public float CalculateAffordabilityInstallment(float totalIncome)
        {
            var value = (totalIncome / 100) * 30;
            return float.Parse(value.ToString());
        }

        /// <summary>
        /// Calculates the interest only instalment amount.
        /// </summary>
        /// <param name="currentBalance">Current Balance</param>
        /// <param name="interestRate">Interest Rate</param>
        /// <returns></returns>
        public decimal CalculateInterestOnlyInstalment(decimal currentBalance, decimal interestRate)
        {
            decimal interestOnlyInstalment = ((currentBalance * interestRate) / 365) * 31;

            return interestOnlyInstalment;
        }

        public double CalculateLoanInstalment(double balance, double interestRate, int noOfMonths)
        {
            var payment = (balance * Math.Pow((interestRate / 12) + 1, (noOfMonths)) * interestRate / 12) / (Math.Pow(interestRate / 12 + 1, (noOfMonths)) - 1);
            return Math.Round(payment, 2, MidpointRounding.AwayFromZero);
        }
    }
}