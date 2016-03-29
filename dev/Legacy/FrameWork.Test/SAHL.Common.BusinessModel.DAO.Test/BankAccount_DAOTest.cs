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
    /// Class for testing the <see cref="BankAccount_DAO"/> entity.
    /// </summary>
//    [TestFixture]
    public class BankAccount_DAOTest : TestBase
    {
        #region Static Helper Methods

        /// <summary>
        /// Deletes a bank account from the database.
        /// </summary>
        /// <param name="bankAccount"></param>
        public static void DeleteBankAccount(BankAccount_DAO bankAccount)
        {
            if (bankAccount != null && bankAccount.Key > 0)
                TestBase.DeleteRecord("BankAccount", "BankAccountKey", bankAccount.Key.ToString());
        }

        /// <summary>
        /// Helper method to create a new <see cref="BankAccount_DAO"/> entity.
        /// </summary>
        /// <returns>A new BankAccount entity (not saved to the database).</returns>
        public static BankAccount_DAO CreateBankAccount()
        {
            BankAccount_DAO bankAccount = new BankAccount_DAO();
            bankAccount.ACBBranch = ACBBranch_DAO.FindFirst();
            bankAccount.ACBType = ACBType_DAO.Find(1);
            bankAccount.AccountName = "Unit Test";
            bankAccount.AccountNumber = "000000000";
            bankAccount.ChangeDate = DateTime.Now;
            bankAccount.UserID = TestConstants.UnitTestUserID;
            return bankAccount;
        }

        #endregion
    }
}
