using System.Collections.Generic;
using SAHL.Test;
using Castle.ActiveRecord;
using NUnit.Framework;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="LegalEntity_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class LegalEntity_DAOTest : TestBase
    {
        /// <summary>
        /// Tests the searching of legal entities by ID number.
        /// </summary>
        [Test]
        public void FindByIDNumber()
        {
            using (new SessionScope())
            {
                string prefix = "76";
                IList<LegalEntityNaturalPerson_DAO> legalEntities = LegalEntityNaturalPerson_DAO.FindByIDNumber(prefix, 5);


                foreach (LegalEntityNaturalPerson_DAO legalEntity in legalEntities)
                {
                  //  CitizenType_DAO CitType = legalEntity.GetPreviousValue((object)"CitizenType") as CitizenType_DAO; 
                    Assert.IsTrue(legalEntity.IDNumber.StartsWith(prefix), legalEntity.IDNumber + " does not start with " + prefix + " as expected.");
                }
            }
        }

        /// <summary>
        /// Tests the searching of companies by registration number.
        /// </summary>
        [Test]
        public void FindCompaniesByRegistrationNumber()
        {
            string prefix = "10";
            IList<LegalEntityCompany_DAO> legalEntities = LegalEntityCompany_DAO.FindByRegistrationNumber(prefix, 5);
            foreach (LegalEntityCompany_DAO legalEntity in legalEntities)
            {
                Assert.IsTrue(legalEntity.RegistrationNumber.StartsWith(prefix), legalEntity.RegistrationNumber + " does not start with " + prefix + " as expected.");
            }
        }

        /// <summary>
        /// Tests the searching of trusts by registration number.
        /// </summary>
        [Test]
        public void FindTrustsByRegistrationNumber()
        {
            string prefix = "10";
            IList<LegalEntityTrust_DAO> legalEntities = LegalEntityTrust_DAO.FindByRegistrationNumber(prefix, 5);
            foreach (LegalEntityTrust_DAO legalEntity in legalEntities)
            {
                Assert.IsTrue(legalEntity.RegistrationNumber.StartsWith(prefix), legalEntity.RegistrationNumber + " does not start with " + prefix + " as expected.");
            }
        }

        /// <summary>
        /// Tests the searching of closed corporations by registration number.
        /// </summary>
        [Test]
        public void FindClosedCorporationsByRegistrationNumber()
        {
            string prefix = "20";
            IList<LegalEntityCloseCorporation_DAO> legalEntities = LegalEntityCloseCorporation_DAO.FindByRegistrationNumber(prefix, 5);
            foreach (LegalEntityCloseCorporation_DAO legalEntity in legalEntities)
            {
                Assert.IsTrue(legalEntity.RegistrationNumber.StartsWith(prefix), legalEntity.RegistrationNumber + " does not start with " + prefix + " as expected.");
            }
        }


        #region Static Helper Methods

        /// <summary>
        /// Helper method to delete a legal entity object from the database.
        /// </summary>
        /// <param name="legalEntity"></param>
        public static void DeleteLegalEntity(LegalEntity_DAO legalEntity)
        {
            if (legalEntity != null && legalEntity.Key > 0)
                TestBase.DeleteRecord("LegalEntity", "LegalEntityKey", legalEntity.Key.ToString());
        }

        #endregion


    }
}
