using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IBankAccountRepository
    {
        IReadOnlyEventList<IACBBranch> GetACBBranchesByPrefix(int ACBBankKey, string Prefix, int maxRowCount);

        IBankAccount GetBankAccountByKey(int BankAccountKey);

        IBankAccount GetBankAccountByACBBranchCodeAndAccountNumber(string acbBranchCode, string bankAccountNumber);

        IACBBank GetACBBankByKey(int ACBBankKey);

        IACBBranch GetACBBranchByKey(string ACBBranchKey);

        IACBType GetACBTypeByKey(int ACBTypeKey);

        /// <summary>
        /// Gets an empty bank account that can later be saved to the database.
        /// </summary>
        /// <returns></returns>
        IBankAccount GetEmptyBankAccount();

        /// <summary>
        /// Saves a bank account object
        /// </summary>
        /// <param name="bankAccount"></param>
        void SaveBankAccount(IBankAccount bankAccount);

        /// <summary>
        /// Searches for Bank Accounts attached to Legal Entities.
        /// </summary>
        /// <param name="searchCriteria">The criteria for the search.</param>
        /// <param name="maxRowCount">The maximum number of records to return.  Set this to a negative number to remove all limits.</param>
        /// <returns></returns>
        IEventList<ILegalEntityBankAccount> SearchLegalEntityBankAccounts(IBankAccountSearchCriteria searchCriteria, int maxRowCount);

        /// <summary>
        /// Searches for Bank Accounts not attached to Legal Entities.
        /// </summary>
        /// <param name="searchCriteria">The criteria for the search.</param>
        /// <param name="maxRowCount">The maximum number of records to return.  Set this to a negative number to remove all limits.</param>
        /// <returns></returns>
        IEventList<IBankAccount> SearchNonLegalEntityBankAccounts(IBankAccountSearchCriteria searchCriteria, int maxRowCount);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountDebtSettlementKey"></param>
        /// <param name="bankAccount"></param>
        void SaveAccountDebtSettlementBankAccount(int accountDebtSettlementKey, IBankAccount bankAccount);

        // Commented out by MattS (20/05/2009) - isn't used anywhere and doesn't work - validation breaks it
        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="accountDebtSettlementKey"></param>
        //void DeleteAccountDebtSettlementBankAccount(int accountDebtSettlementKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationDebtSettlementKey"></param>
        void DeleteApplicationDebtSettlementBankAccount(int applicationDebtSettlementKey);
    }
}