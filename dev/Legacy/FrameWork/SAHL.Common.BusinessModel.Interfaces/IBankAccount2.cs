using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public enum BankAccountNameFormat
    {
        Full,
        Short
    };

    public partial interface IBankAccount : IEntityValidation
    {
        string GetDisplayName(BankAccountNameFormat Format);

        /// <summary>
        /// Gets a list of <see cref="IFinancialServiceBankAccount"/> objects attached to the bank account.  A maximum
        /// of 50 items will be returned.
        /// </summary>
        /// <returns>A list of <see cref="IFinancialServiceBankAccount"/> objects.</returns>
        IReadOnlyEventList<IFinancialServiceBankAccount> GetFinancialServiceBankAccounts();

        /// <summary>
        /// Gets a list of <see cref="IFinancialServiceBankAccount"/> objects attached to the bank account.  Be careful when
        /// using this method as some bank accounts have up to 5,000 related records (e.g. the SA Home Loans trust fund).
        /// </summary>
        /// <param name="maxRecords">The maximum number of records to return.</param>
        /// <returns>A list of <see cref="IFinancialServiceBankAccount"/> objects.</returns>
        IReadOnlyEventList<IFinancialServiceBankAccount> GetFinancialServiceBankAccounts(int maxRecords);

        /// <summary>
        /// Gets a shallow copy of the object when it was first loaded.  For new bank accounts, this will
        /// be null.  Collections and methods are not implemented.
        /// </summary>
        IBankAccount Original { get; }
    }
}