using System.Data;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    /// <summary>
    /// A repository responsible for functionality surrounding accounts.
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        int GetDetailTypeKeyByDescription(string description);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        double GetCurrentMonthsInArrears(int AccountKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        void RemoveNonNCACompliantAccountInformation(int accountKey);

        /// <summary>
        /// X2 Worklist Helper Method
        /// </summary>
        /// <param name="instanceID"></param>
        /// <returns></returns>
        IAccount GetAccountByCap2InstanceID(long instanceID);

        /// <summary>
        /// Updates the Financial Service Debit Order
        /// </summary>
        /// <param name="iAppdo"></param>
        void UpdateAccountDebitOrderFromApplicationDebitOrder(IApplicationDebitOrder iAppdo);

        /// <summary>
        /// Returns the PD score for an account
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        DataSet GetBehaviouralScore(int AccountKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ProductKey"></param>
        /// <returns></returns>
        IAccount CreateDiscriminatedAccountByProduct(int ProductKey);

        /// <summary>
        /// Gets an account by the account key.
        /// </summary>
        /// <param name="Key">The integer account Key.</param>
        /// <returns>The <see cref="IAccount">account found using the supplied account key, returns null if no account is found.</see></returns>
        IAccount GetAccountByKey(int Key);

        /// <summary>
        /// When a FurtherLoan is created the FL application STILL needs a reserved account key.
        /// As the Account has been created already there will already be a reserved account from
        /// the initial offer so we go look it up.
        /// </summary>
        /// <param name="Key">Account Key of EXISTING Mortgage Loan Account, well could be any account in fact.</param>
        /// <returns></returns>
        IAccountSequence GetAccountSequenceByKey(int Key);

        /// <summary>
        /// Gets an account status object by key.
        /// </summary>
        /// <param name="key">Account Status key</param>
        /// <returns></returns>
        IAccountStatus GetAccountStatusByKey(AccountStatuses key);

        //// <summary>
        //// Gets an account by the FinancialServiceKey.
        //// </summary>
        //// <param name="Messages">Collection of Domain Messages that will be added to if the method encounters errors.</param>
        //// <param name="Key">The integer FinancialServiceKey.</param>
        //// <returns>The <see cref="IAccount">account found using the supplied FinancialServiceKey, returns null if no account is found.</see></returns>
        //IAccount GetAccountByFinancialServiceKey(int Key);

        /// <summary>
        /// Gets an account by the offer key.
        /// </summary>
        /// <param name="applicationKey">The integer application Key.</param>
        /// <returns>The <see cref="IAccount">account found using the supplied application key, returns null if no account is found.</see></returns>
        IAccount GetAccountByApplicationKey(int applicationKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        bool TitleDeedsOnFile(int accountKey);

        IReadOnlyEventList<IDetail> GetDetailByAccountKey(int AccountKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <param name="DetailTypeKey"></param>
        /// <returns></returns>
        IReadOnlyEventList<IDetail> GetDetailByAccountKeyAndDetailType(int AccountKey, int DetailTypeKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <param name="DetailTypeKey"></param>
        /// <returns></returns>
        IDetail GetLatestDetailByAccountKeyAndDetailType(int AccountKey, int DetailTypeKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        IReadOnlyEventList<IDetail> GetAccountDetailForFurtherLending(int AccountKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <param name="DetailTypeKey"></param>
        void RemoveDetailByAccountKeyAndDetailTypeKey(int AccountKey, int DetailTypeKey);

        void RemoveDetailByKey(int DetailKey);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IDetail CreateEmptyDetail();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IAccountLifePolicy CreateEmptyLifePolicyAccount();

        void DeleteDetail(IDetail detail);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Account"></param>
        void SaveAccount(IAccount Account);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ads"></param>
        void SaveAccountDebtSettlement(IAccountDebtSettlement ads);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Account"></param>
        void CreateAccount(IAccount Account);

        /// <summary>
        ///
        /// </summary>
        /// <param name="detail"></param>
        void SaveDetail(IDetail detail);

        /// <summary>
        /// Creates and Empty Role object
        /// </summary>
        /// <returns>IRole</returns>
        IRole CreateEmptyRole();

        /// <summary>
        /// Gets a Role object by key
        /// </summary>
        /// <param name="key">Role Key.</param>
        /// <returns>IRole</returns>
        IRole GetRoleByKey(int key);

        /// <summary>
        /// Save a Role record.
        /// </summary>
        /// <param name="role">The Role entity.</param>
        void SaveRole(IRole role);

        /// <summary>
        /// Used to perform a search against accounts.
        /// </summary>
        /// <param name="searchCriteria">The search criteria used to perform the search.</param>
        /// <param name="maxRowCount">The maximum number of records to return.</param>
        /// <returns></returns>
        IEventList<IAccount> SearchAccounts(IAccountSearchCriteria searchCriteria, int maxRowCount);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IExpenseType GetExpenseTypeByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="userID"></param>
        void ConvertStaffLoan(int accountKey, string userID);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="userID"></param>
        /// <param name="cancellationReason"></param>
        void UnConvertStaffLoan(int accountKey, string userID, CancellationReasons cancellationReason);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <param name="RegentStatusKey"></param>
        /// <returns></returns>
        IRegent GetRegent(int AccountKey, int RegentStatusKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="application"></param>
        void CreateOfferRolesNotInAccount(IApplication application);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="accountStatusKey"></param>
        /// <param name="fixedPayment"></param>
        /// <param name="adUserName"></param>
        void UpdateAccount(int accountKey, int? accountStatusKey, float? fixedPayment, string adUserName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="accountStatusKey"></param>
        void UpdateAccountStatusWithNoValidation(int applicationKey, int accountStatusKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="fixedPayment"></param>
        /// <param name="userid"></param>
        void UpdateFixedPayment(int accountkey, double fixedPayment, string userid);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="userID"></param>
        /// <param name="CancellationReasonKey"></param>
        void OptOutNonPerforming(int accountKey, string userID, int CancellationReasonKey);

        void OptOutNonPerforming(int accountKey);

        void OptInNonPerforming(int accountKey, string userID);

        void ReOpenAccount(int accountKey, string userID);

        bool HasApplicationBeenInCompany2(int accountKey);

        IReadOnlyEventList<IDetail> GetDetailByAccountKeyAndDetailClassKey(int accountKey, int detailClassKey);

        #region AccountAttorneyInvoice

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IAccountAttorneyInvoice GetAccountAttorneyInvoiceByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        IEventList<IAccountAttorneyInvoice> GetAccountAttorneyInvoiceListByAccountKey(int accountKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accAttInv"></param>
        void SaveAccountAttorneyInvoice(IAccountAttorneyInvoice accAttInv);

        /// <summary>
        /// Get an empty AccountAttorneyInvoice
        /// </summary>
        /// <returns></returns>
        IAccountAttorneyInvoice CreateEmptyAccountAttorneyInvoice();

        /// <summary>
        ///
        /// </summary>
        /// <param name="accAttInv"></param>
        void DeleteAccountAttorneyInvoice(IAccountAttorneyInvoice accAttInv);

        #endregion AccountAttorneyInvoice

        void CloseLoanAccount(int accountKey, string userid);

        void ClosePersonalLoanAccount(int accountKey, string userid);

    }
}