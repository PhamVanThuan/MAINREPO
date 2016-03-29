using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    [TestFixture]
    public class AccountSequence_DAOTest : TestBase
    {
        /// <summary>
        /// Tests loading and saving of AccountSequence_DAO.  AccountSequence_DAO is excluded fromt he generic 
        /// tests because deleting is not applicable.
        /// </summary>
        [Test]
        public void LoadSave()
        {
            AccountSequence_DAO accSeq1 = AccountSequence_DAO.FindFirst();
            Assert.IsNotNull(accSeq1);

            AccountSequence_DAO accSeq2 = new AccountSequence_DAO();
            accSeq2.IsUsed = false;
            accSeq2.SaveAndFlush();
        }
    }
}
