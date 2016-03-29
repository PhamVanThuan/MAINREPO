using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.TestWTF.DAOHelpers
{
    /// <summary>
    /// Provides data access methods for testing the <see cref="LifePolicy_DAO"/> domain entity.
    /// </summary>
    public class LifePolicyHelper : BaseHelper<LifePolicy_DAO>
    {
        /// <summary>
        /// Creates a new <see cref="LifePolicy_DAO"/> entity.
        /// </summary>
        /// <returns>A new LifePolicy_DAO entity (not yet persisted).</returns>
        public LifePolicy_DAO CreateLifePolicy()
        {
            // get an account
            LifePolicy_DAO lifePolicy = new LifePolicy_DAO();
            lifePolicy.Key = 11223344;
            lifePolicy.Account = Account_DAO.FindFirst();
            lifePolicy.AccountStatus = AccountStatus_DAO.FindFirst();
            lifePolicy.LifePolicyStatus = LifePolicyStatus_DAO.FindFirst();

            lifePolicy.DeathBenefit = 0;
            lifePolicy.InstallmentProtectionBenefit = 0;
            lifePolicy.DeathBenefitPremium = 0;
            lifePolicy.InstallmentProtectionPremium = 0;
            lifePolicy.DateOfExpiry = System.DateTime.Now;
            lifePolicy.DeathRetentionLimit = 0;
            lifePolicy.InstallmentProtectionRetentionLimit = 0;
            lifePolicy.UpliftFactor = 0;
            lifePolicy.JointDiscountFactor = 0;

            CreatedEntities.Add(lifePolicy);

            return lifePolicy;
        }

        /// <summary>
        /// Ensures that all life policies created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (LifePolicy_DAO lifePolicy in CreatedEntities)
            {
                if (lifePolicy.Key > 0)
                    TestBase.DeleteRecord("LifePolicy", "FinancialServiceKey", lifePolicy.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }
}
