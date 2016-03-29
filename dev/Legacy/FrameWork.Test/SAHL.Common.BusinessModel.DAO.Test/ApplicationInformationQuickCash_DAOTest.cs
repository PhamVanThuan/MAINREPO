using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    [TestFixture]
    public class ApplicationInformationQuickCash_DAOTest : TestBase
    {
        [Test]
        public void LoadSaveLoad()
        {
            using (new SessionScope())
            {

                ApplicationInformation_DAO appInf = DAODataConsistancyChecker.GetDAO<ApplicationInformation_DAO>();
                appInf.SaveAndFlush();

                ApplicationInformationQuickCash_DAO appInfQC = DAODataConsistancyChecker.GetDAO<ApplicationInformationQuickCash_DAO>();

                appInfQC.ApplicationInformation = appInf;
                appInfQC.SaveAndFlush();

                int key = appInfQC.Key;

                // now try and load it
                ApplicationInformationQuickCash_DAO appInfQC2 = ApplicationInformationQuickCash_DAO.Find(key);
                Assert.IsNotNull(appInfQC2);

                // delete all the data
                appInfQC2.DeleteAndFlush();
                appInf.DeleteAndFlush();
            }
        }
    }
}
