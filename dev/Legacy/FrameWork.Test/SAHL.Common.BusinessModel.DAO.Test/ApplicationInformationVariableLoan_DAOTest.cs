using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    [TestFixture]
    public class ApplicationInformationVariableLoan_DAOTest : TestBase
    {
        [Test]
        public void LoadSaveLoad()
        {
            using (new SessionScope())
            {

                ApplicationInformation_DAO appInf = DAODataConsistencyChecker.GetDAO<ApplicationInformation_DAO>();
                appInf.SaveAndFlush();

                ApplicationInformationVariableLoan_DAO appInfVL = DAODataConsistencyChecker.GetDAO<ApplicationInformationVariableLoan_DAO>();
                //appInfVL.LoanAgreementAmount = 200.00;
                appInfVL.ApplicationInformation = appInf;
                appInfVL.SaveAndFlush();

                int key = appInfVL.Key;

                //// now try and load it
                ApplicationInformationVariableLoan_DAO appInfVL2 = ApplicationInformationVariableLoan_DAO.Find(key);
                Assert.IsNotNull(appInfVL2);

                //// delete all the data
                appInfVL2.DeleteAndFlush();
                appInf.DeleteAndFlush();
            }
        }
    }
}
