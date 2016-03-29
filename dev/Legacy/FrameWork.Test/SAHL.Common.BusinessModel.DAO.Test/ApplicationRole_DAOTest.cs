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
    /// Class for testing the <see cref="ApplicationRole_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class ApplicationRole_DAOTest : TestBase
    {


        /// <summary>
        /// Tests the loading and saving of StageDefinitionStageDefinitionGroup_DAO objects.  This is not covered by 
        /// the generic LoadSaveLoad test as there is a unique key on the columns in the table which can randomly cause 
        /// the test to fail.
        /// </summary>
        [Test]
        public void LoadSaveLoad()
        {
            int appRoleKey = 0;

            // load one up
            using (new SessionScope())
            {
                ApplicationRole_DAO appRole1 = ApplicationRole_DAO.FindFirst();
                Assert.IsNotNull(appRole1);
            }

            // create items for the test and try and save a new object
            using (new SessionScope())
            {
                // create a new one and save it
                ApplicationRole_DAO appRole = new ApplicationRole_DAO();
                appRole.ApplicationKey = Application_DAO.FindFirst().Key;
                appRole.ApplicationRoleType = ApplicationRoleType_DAO.FindFirst();
                appRole.GeneralStatus = GeneralStatus_DAO.FindFirst();
                appRole.LegalEntityKey = LegalEntity_DAO.FindFirst().Key;
                appRole.StatusChangeDate = DateTime.Now;
                appRole.SaveAndFlush();
                appRoleKey = appRole.Key;
            }

            using (new SessionScope())
            {

                // now try and load it in a new session
                ApplicationRole_DAO appRole2 = ApplicationRole_DAO.Find(appRoleKey);
                Assert.IsNotNull(appRole2);

            }

            // delete all the data
            using (new SessionScope())
            {
                DeleteRecord("OfferRole", "OfferRoleKey", appRoleKey.ToString());
            }

        }
    }
}
