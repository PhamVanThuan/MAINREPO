namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// This interface exposes all properties required to display Varifix loan information. To be implemented by the both the Account and Application.
    /// </summary>
    public interface IVarifixLoanInfo
    {
        /// <summary>
        /// Key pointing to the current link rate.
        /// </summary>
        int LinkRateKey
        {
            get;
            set;
        }

        /// <summary>
        /// Percentage of the Total Loan amount that is fixed.
        /// </summary>
        double FixedPercentage
        {
            get;
            set;
        }

        /// <summary>
        /// Percentage of the Total Loan amount that is variable.
        /// </summary>
        double VariablePercentage
        {
            get;
        }

        /// <summary>
        /// Boolean indicating that the percentage split is to be 'suggested' by the system.
        /// </summary>
        bool UseMaximum
        {
            set;
        }

        /// <summary>
        /// Rand value based on the FixedPercentage.
        /// </summary>
        double FixedRandValue
        {
            get;
        }

        /// <summary>
        /// Rand value based on the VariablePercentage.
        /// </summary>
        double VariableRandValue
        {
            get;
        }

        /// <summary>
        /// Market rate (base rate) for the fixed leg.
        /// </summary>
        double FixedMarketRate
        {
            get;
        }

        /// <summary>
        /// Market rate (base rate) for the variable leg.
        /// </summary>
        double VariableMarketRate
        {
            get;
        }

        /// <summary>
        /// Link rate for the fixed leg.
        /// </summary>
        double FixedLinkRate
        {
            get;
        }

        /// <summary>
        /// Link rate for the variable leg.
        /// </summary>
        double VariableLinkRate
        {
            get;
        }

        /// <summary>
        /// Total interest rate (base + link) for the fixed leg.
        /// </summary>
        double FixedEffectiveRate
        {
            get;
        }

        /// <summary>
        /// Total interest rate (base + link) for the variable leg.
        /// </summary>
        double VariableEffectiveRate
        {
            get;
        }

        /// <summary>
        /// Calculated. Instalment for the fixed leg.
        /// </summary>
        double FixedInstalment
        {
            get;
        }

        /// <summary>
        /// Calculated. Instalment for the variable leg.
        /// </summary>
        double VariableInstalment
        {
            get;
        }

        /// <summary>
        /// This method calculates the varifix details using the current parameters.
        /// </summary>
        void RecalculateVarifixDetails();
    }
}