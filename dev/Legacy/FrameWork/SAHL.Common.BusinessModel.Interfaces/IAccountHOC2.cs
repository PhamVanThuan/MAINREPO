namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IAccountHOC
    {
        /// <summary>
        /// Returns the HOC object that is linked to this acount. This property will throw an exception if there are more that 1 Financial Service and Null if there are none.
        /// </summary>
        IHOC HOC
        {
            get;
        }

        /// <summary>
        /// Returns the parent MortgageLoan account. Will throw an exception if there is more than 1 parent account.
        /// </summary>
        IMortgageLoanAccount MortgageLoanAccount { get; }

		double MonthlyPremium { get; }
    }
}