using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Test.DAOHelpers
{
    /// <summary>
    /// Provides data access methods for testing the <see cref="AccountLifePolicy_DAO"/> domain entity.
    /// </summary>
    public class LifePolicyAccountHelper : BaseHelper<AccountLifePolicy_DAO>
    {
        /// <summary>
        /// Creates a new <see cref="AccountLifePolicy_DAO"/> entity.
        /// </summary>
        /// <returns>A new AccountLifePolicy_DAO entity (not yet persisted).</returns>
        public AccountLifePolicy_DAO CreateLifePolicyAccount()
        {
            return new AccountLifePolicy_DAO();
        }

        /// <summary>
        /// Ensures that all entities created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (AccountLifePolicy_DAO lifePolicyAccount in CreatedEntities)
            {
                if (lifePolicyAccount.Key > 0)
                    TestBase.DeleteRecord("Account", "AccountKey", lifePolicyAccount.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }
}
