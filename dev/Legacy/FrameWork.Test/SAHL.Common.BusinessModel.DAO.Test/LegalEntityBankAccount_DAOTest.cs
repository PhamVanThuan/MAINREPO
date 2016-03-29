using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using SAHL.Test;
using SAHL.Common.BusinessModel.DAO;

using Castle.ActiveRecord;
using NUnit.Framework;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="LegalEntityBankAccount_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class LegalEntityBankAccount_DAOTest : TestBase
    {

        /// <summary>
        /// Tests the loading and saving of LegalEntityBankAccount_DAO objects.  This is not covered by 
        /// the generic LoadSaveLoad test as there is a unique key on the columns in the table which can randomly cause 
        /// the test to fail.
        /// </summary>
        [Test]
        public void LoadSaveLoad()
        {
            using (new SessionScope())
            {
                // get a legal entity and create a new bank account object
                LegalEntityNaturalPerson_DAO legalEntity = LegalEntityNaturalPerson_DAO.FindFirst();
                BankAccount_DAO bankAccount = DAODataConsistencyChecker.GetDAO<BankAccount_DAO>();
                bankAccount.SaveAndFlush();

                // create a new legalentity bank account and save it
                LegalEntityBankAccount_DAO leba = new LegalEntityBankAccount_DAO();
                leba.BankAccount = bankAccount;
                leba.ChangeDate = DateTime.Now;
                leba.GeneralStatus = GeneralStatus_DAO.FindFirst();
                leba.LegalEntity = legalEntity;
                leba.UserID = TestConstants.UnitTestUserID;
                leba.SaveAndFlush();
                int key = leba.Key;

                // now try and load it
                LegalEntityBankAccount_DAO leba2 = LegalEntityBankAccount_DAO.Find(key);
                Assert.IsNotNull(leba2);

                // delete all the data
                leba.DeleteAndFlush();
                bankAccount.DeleteAndFlush();
            }
        }

        /*
        #region Static Helper Methods

        /// <summary>
        /// Deletes a LegalEntityBankAccount entity from the database.
        /// </summary>
        /// <param name="legalEntityBankAccount"></param>
        public static void DeleteLegalEntityBankAccount(LegalEntityBankAccount_DAO legalEntityBankAccount)
        {
            if (legalEntityBankAccount != null && legalEntityBankAccount.Key > 0)
                TestBase.DeleteRecord("LegalEntityBankAccount", "LegalEntityBankAccountKey", legalEntityBankAccount.Key.ToString());
        }

        /// <summary>
        /// Helper method to create a new <see cref="LegalEntityBankAccount_DAO"/> entity.
        /// </summary>
        /// <returns>A new AddressBox entity (not saved to the database).</returns>
        public static LegalEntityBankAccount_DAO CreateLegalEntityBankAccount(BankAccount_DAO bankAccount, LegalEntity_DAO legalEntity)
        {
            LegalEntityBankAccount_DAO leba = new LegalEntityBankAccount_DAO();
            leba.BankAccount = bankAccount;
            leba.ChangeDate = DateTime.Now;
            leba.GeneralStatus = GeneralStatus_DAO.FindFirst();
            leba.LegalEntity = legalEntity;
            leba.UserID = TestConstants.UnitTestUserID;
            return leba;
        }

        #endregion

        */
    }
}
