using System.Collections.Generic;
using System.Data;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IFinancialService : IEntityValidation
    {
        DataTable GetTransactions(IDomainMessageCollection Messages, List<int> TransactionTypeKeys);

        DataTable GetTransactions(IDomainMessageCollection Messages, int GeneralStatusKey, List<int> TransactionTypeKeys);

        /// <summary>
        ///
        /// </summary>
        IFinancialServiceType FinancialServiceType
        {
            get;
        }

        /// <summary>
        /// Gets the currently active <see cref="IFinancialServiceBankAccount"/> associated with the financial service.
        /// </summary>
        IFinancialServiceBankAccount CurrentBankAccount { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fats"></param>
        /// <returns></returns>
        IFinancialAdjustment GetPendingFinancialAdjustmentByTypeSource(FinancialAdjustmentTypeSources fats);

        /// <summary>
        /// Gets all LifePolicyClaim objects for the given FinancialService
        /// </summary>
        /// <returns></returns>
        IList<ILifePolicyClaim> GetLifePolicyClaims();

        /// <summary>
        /// Gets the pending LifePolicyClaim object for the given FinancialService
        /// </summary>
        /// <returns></returns>
        ILifePolicyClaim GetLifePolicyClaimPending();
    }
}