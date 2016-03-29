using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Shared.BusinessModel.Calculations
{
    public interface ILoanCalculations
    {
        decimal CalculateLTV(decimal totalLoanAmount, decimal propertyValue);

        decimal CalculatePTI(decimal installment, decimal householdIncome);

        decimal CalculateInstalment(decimal totalLoanValue, decimal annualInterestRate, decimal remainingTerm, bool interstOnly);
    }
}
