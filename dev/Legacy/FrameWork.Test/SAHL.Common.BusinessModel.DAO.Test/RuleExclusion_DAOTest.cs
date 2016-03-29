using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using SAHL.Test;
using SAHL.Common.BusinessModel.DAO;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="RuleExclusion_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class RuleExclusion_DAOTest : TestBase
    {

        /// <summary>
        /// Tests the retrieval of applications off an Account.
        /// </summary>
        [Test]
        public void LoadSaveLoad()
        {
            int key;

            // try and load any random item
            using (new SessionScope())
            {
                RuleExclusion_DAO re = RuleExclusion_DAO.FindFirst();
                if (re == null)
                    Assert.Ignore("No data to run LoadSaveLoad test");
            }

            // create a new object, ensuring RuleItemKey FK is not broken
            using (new SessionScope())
            {
                RuleExclusion_DAO re = DAODataConsistancyChecker.GetDAO<RuleExclusion_DAO>();
                re.RuleItemKey = RuleItem_DAO.FindFirst().Key;
                re.SaveAndFlush();
                key = re.Key;
            }

            // load the saved object in a new session, and then delete it
            using (new SessionScope())
            {
                // now try and load it
                RuleExclusion_DAO re = RuleExclusion_DAO.Find(key);
                Assert.IsNotNull(re);
                re.DeleteAndFlush();
            }

        }

       
	
	}
}
