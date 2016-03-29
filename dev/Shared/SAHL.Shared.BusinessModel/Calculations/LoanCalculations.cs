
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Shared.BusinessModel.Calculations
{
    public class LoanCalculations : ILoanCalculations
    {
        public decimal CalculateLTV(decimal totalLoanAmount, decimal propertyValue)
        {
            decimal propValue = propertyValue <= 0 ? 1 : propertyValue;
            var ltv = totalLoanAmount / propValue;

            return ltv;
        }

        public decimal CalculatePTI(decimal installment, decimal householdIncome)
        {
            var pti = 1m;
            if (householdIncome > 0)
            {
                pti = installment / householdIncome;
            }

            return pti;
        }

        public decimal CalculateInstalment(decimal totalLoanValue, decimal annualInterestRate, decimal remainingTerm, bool interestOnly)
        {
            if (interestOnly)
            {
                return CalculateInstalmentInterestOnly(annualInterestRate, totalLoanValue);
            }
            else
            {
                return CalculateInstalment(totalLoanValue, annualInterestRate, remainingTerm);
            }
        }

        private decimal CalculateInstalmentInterestOnly(decimal annualInterestRate, decimal totalLoanValue)
        {
            if (totalLoanValue < 0.01m)
            {
                return 0;
            }

            if (annualInterestRate <= 0)
            {
                throw new Exception("Annual Interest Rate must be greater than zero.");
            }

            // IO instalment always calculated at 31 days to make sure we always collect at a minimum the interest accrued in the month
            return ((totalLoanValue * (annualInterestRate / 365)) * 31); 
        }

        private decimal CalculateInstalment(decimal totalLoanValue, decimal annualInterestRate, decimal remainingTerm)
        {
            if (totalLoanValue < 0.01m)
            {
                return 0;
            }

            if (remainingTerm <= 0)
            {
                return totalLoanValue;
            }

            if (annualInterestRate <= 0)
            {
                throw new Exception("Annual Interest Rate must be greater than zero.");
            }

            // Adjust to monthly rate
            decimal monthlyInterestRate = annualInterestRate / 12;

            var installment = (monthlyInterestRate + (monthlyInterestRate / (decimal)(Math.Pow((1 + (double)monthlyInterestRate), (double)remainingTerm) - 1))) * totalLoanValue;

            return installment;
        }
    }
}
