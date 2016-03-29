using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using Castle.ActiveRecord;
using System.Data;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    [TestFixture]
    public class ApplicationAttribute_DAOTest : TestBase
    {
        /// <summary>
        /// Tests the loading and saving of ApplicationAttribute_DAO objects.  This is not covered by 
        /// the generic LoadSaveLoad test as there is a unique key on the columns in the table which can 
        /// randomly cause the test to fail.
        /// </summary>
        [Test]
        public void LoadSaveLoad()
        {
            int aaKey = 0;
            int offerKey;
            
            try
            {
                // find an offer key that doesn't exit in the target table
                string sql = @"select top 1 offerkey from offer (nolock)
                    where offerkey not in (select offerkey from offerattribute)";
                DataTable dt = base.GetQueryResults(sql);
                offerKey = Convert.ToInt32(dt.Rows[0][0]);

                // do a load of an offer attribute item
                using (new SessionScope())
                {
                    // try and load any random item
                    ApplicationAttribute_DAO aa = ApplicationAttribute_DAO.FindFirst();
                    Assert.IsNotNull(aa);
                }

                // test saving
                using (new SessionScope())
                {
                    // create a new one and save it
                    ApplicationAttribute_DAO aa = new ApplicationAttribute_DAO();
                    aa.Application = Application_DAO.Find(offerKey);
                    aa.ApplicationAttributeType = ApplicationAttributeType_DAO.FindFirst();
                    aa.SaveAndFlush();
                    aaKey = aa.Key;
                }

                // reload the object
                using (new SessionScope())
                {
                    ApplicationAttribute_DAO aa = ApplicationAttribute_DAO.Find(aaKey);
                    Assert.IsNotNull(aa);
                }
            }
            finally
            {
                // clean up the data - use SQL here otherwise you get cascade issues
                if (aaKey > 0)
                    base.GetQueryResults("delete from OfferAttribute where OfferAttributeKey = " + aaKey.ToString());

            }
        }
    }
}
