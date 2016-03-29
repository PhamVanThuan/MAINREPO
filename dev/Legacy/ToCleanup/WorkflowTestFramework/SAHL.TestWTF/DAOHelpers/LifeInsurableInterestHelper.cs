using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;

namespace SAHL.TestWTF.DAOHelpers
{
    /// <summary>
    /// Provides data access methods for testing the <see cref="LifeInsurableInterest_DAO"/> domain entity.
    /// </summary>
    public class LifeInsurableInterestHelper : BaseHelper<LifeInsurableInterest_DAO>
    {
        /// <summary>
        /// Creates a new <see cref="LifeInsurableInterest_DAO"/> entity.
        /// </summary>
        /// <returns>A new LifeInsurableInterest_DAO entity (not yet persisted).</returns>
        public LifeInsurableInterest_DAO CreateLifeInsurableInterest()
        {
            LifeInsurableInterest_DAO lifeInsurableInterest = new LifeInsurableInterest_DAO();
            lifeInsurableInterest.Account = Account_DAO.FindFirst() as Account_DAO;
            lifeInsurableInterest.LegalEntity = LegalEntity_DAO.FindFirst() as LegalEntity_DAO;
            lifeInsurableInterest.LifeInsurableInterestType = LifeInsurableInterestType_DAO.FindFirst() as LifeInsurableInterestType_DAO;

            CreatedEntities.Add(lifeInsurableInterest);

            return lifeInsurableInterest;
        }

        /// <summary>
        /// Ensures that all addresses created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (LifeInsurableInterest_DAO lifeInsurableInterest in CreatedEntities)
            {
                if (lifeInsurableInterest.Key > 0)
                    TestBase.DeleteRecord("LifeInsurableInterest", "LifeInsurableInterestKey", lifeInsurableInterest.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }
}
