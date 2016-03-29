namespace SAHL.Common.BusinessModel.Interfaces.SearchCriteria
{
    /// <summary>
    /// Defines the criteria that can be set when searching bank accounts.
    /// </summary>
    public interface IBankAccountSearchCriteria
    {
        /// <summary>
        /// The key of the <see cref="IACBBranch"/> to search against.
        /// </summary>
        string ACBBranchKey { get; set; }

        /// <summary>
        /// The bank account number to search against.
        /// </summary>
        string AccountNumber { get; set; }

        /// <summary>
        /// The ACB account type key to search against.
        /// </summary>
        int? ACBTypeKey { get; set; }

        /// <summary>
        /// The bank account name to search against.
        /// </summary>
        string AccountName { get; set; }

        /// <summary>
        /// Determines if any search criteria have been set.
        /// </summary>
        bool IsEmpty { get; }
    }
}