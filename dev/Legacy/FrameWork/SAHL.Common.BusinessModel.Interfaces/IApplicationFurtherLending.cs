namespace SAHL.Common.BusinessModel.Interfaces
{
    public interface IApplicationFurtherLending : IApplication, IApplicationMortgageLoanWithCashOut
    {
        /// <summary>
        /// Calculate and return the LTV
        /// based on the Account current balance plus application amount
        /// and latest valuation amount
        /// </summary>
        double EstimatedDisbursedLTV { get; }
    }
}