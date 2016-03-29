using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Data;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    [TestFixture]
    public class ApplicationAccountRelationship_DAOTest : TestBase
    {
        /// <summary>
        /// Explicitly written otherwise you get unique key constraint errors.
        /// </summary>
        [Test]
        public void LoadSaveLoad()
        {
            // initial load
            ApplicationAccountRelationship_DAO aar = ApplicationAccountRelationship_DAO.FindFirst();
            Assert.IsNotNull(aar);

            // save - although we have to find an account that does not exist in the table
            string sql = "SELECT TOP 1 A.AccountKey FROM [2AM].[dbo].[Account] A (nolock) "
                + "WHERE a.AccountKey not in (SELECT AccountKey FROM OfferAccountRelationship)";
            DataTable dt = base.GetQueryResults(sql);
            int accountKey = Convert.ToInt32(dt.Rows[0][0]);

            Application_DAO application = Application_DAO.FindFirst();
            Account_DAO account = Account_DAO.Find(accountKey);
            ApplicationAccountRelationship_DAO aarSave = new ApplicationAccountRelationship_DAO();
            aarSave.Account = account;
            aarSave.Application = application;
            aarSave.SaveAndFlush();
            int key = aarSave.Key;

            // load again
            ApplicationAccountRelationship_DAO load2 = ApplicationAccountRelationship_DAO.Find(key);
            Assert.IsNotNull(load2);

            // delete
            aarSave.DeleteAndFlush();

        }

    }
}
