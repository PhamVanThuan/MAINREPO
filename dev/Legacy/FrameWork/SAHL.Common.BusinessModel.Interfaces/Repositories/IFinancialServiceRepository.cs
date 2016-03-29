using System;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    /// <summary>
    /// A repository responsible for functionality surrounding financial services.
    /// </summary>
    public interface IFinancialServiceRepository
    {
        /// <summary>
        /// Gets a financial service when the financial service key is known.
        /// </summary>
        /// <param name="Key">The integer financial service Key.</param>
        /// <returns>The <see cref="IFinancialService">financial service found using the supplied financial service key, returns null if no financial service is found.</see></returns>
        IFinancialService GetFinancialServiceByKey(int Key);

        /// <summary>
        /// Gets a <see cref="IFinancialServiceBankAccount"></see> by key.
        /// </summary>
        /// <param name="key">The primary key.</param>
        /// <returns>The <see cref="IFinancialServiceBankAccount"> object. This will return null if the object doesn't exist.</see></returns>
        IFinancialServiceBankAccount GetFinancialServiceBankAccountByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="FinancialServiceToSave">A modified <see cref="IFinancialService">financial service that will be saved to the db.</see></param>
        void SaveFinancialService(IFinancialService FinancialServiceToSave);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IFinancialServiceBankAccount GetEmptyFinancialServiceBankAccount();

        /// <summary>
        ///
        /// </summary>
        /// <param name="bankAccount"></param>
        void SaveFinancialServiceBankAccount(IFinancialServiceBankAccount bankAccount);

        /// <summary>
        /// Get the suspended interest amount and
        /// date that interest was suspended from
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="suspendedDate"></param>
        /// <returns></returns>
        Decimal GetSuspendedInterest(int accountKey, out DateTime? suspendedDate);

        /// <summary>
        /// returns whether this is a non-performing loan
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        bool IsLoanNonPerforming(int accountKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="FinancialServiceGroupKey"></param>
        /// <returns></returns>
        IFinancialServiceGroup GetFinancialServiceGroup(int FinancialServiceGroupKey);
    }
}