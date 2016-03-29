namespace SAHL.Common.BusinessModel.Interfaces
{
    public interface IApplicationProductVariFixLoan : IApplicationProductMortgageLoan, ISupportsVariFixApplicationInformation, ISupportsVariableLoanApplicationInformation
    {
        /// <summary>
        /// Percentage of the Total Loan amount that is fixed.
        /// </summary>
        double? FixedPercentage
        {
            get;
            set;
        }

        /// <summary>
        /// Percentage of the Total Loan amount that is variable.
        /// </summary>
        double? VariablePercentage
        {
            get;
        }

        /// <summary>
        /// Rand value based on the FixedPercentage.
        /// </summary>
        double? FixedRandValue
        {
            get;
        }

        /// <summary>
        /// Rand value based on the VariablePercentage.
        /// </summary>
        double? VariableRandValue
        {
            get;
        }

        /// <summary>
        /// Market rate (base rate) for the fixed leg.
        /// </summary>
        double? FixedMarketRate
        {
            get;
        }

        /// <summary>
        /// Market rate (base rate) for the variable leg.
        /// </summary>
        double? VariableMarketRate
        {
            get;
        }

        /// <summary>
        /// Link rate for the mortgage loan.
        /// </summary>
        double? LinkRate
        {
            get;
        }

        /// <summary>
        /// Total interest rate (base + link) for the fixed leg.
        /// </summary>
        double? FixedEffectiveRate
        {
            get;
        }

        /// <summary>
        /// Total interest rate (base + link) for the variable leg.
        /// </summary>
        double? VariableEffectiveRate
        {
            get;
        }

        /// <summary>
        /// Calculated. Instalment for the fixed leg.
        /// </summary>
        double? FixedInstalment
        {
            get;
        }

        /// <summary>
        /// Calculated. Instalment for the variable leg.
        /// </summary>
        double? VariableInstalment
        {
            get;
        }
    }
}