using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    [TestFixture]
    public class ApplicationMortgageLoanDetail_DAOTest : TestBase
    {
        [Test]
        public void LoadSaveLoad()
        {
            using (new SessionScope())
            {

                ApplicationReAdvance_DAO app = DAODataConsistencyChecker.GetDAO<ApplicationReAdvance_DAO>();
                app.SaveAndFlush();

                ApplicationMortgageLoanDetail_DAO appMortLoan = DAODataConsistencyChecker.GetDAO<ApplicationMortgageLoanDetail_DAO>();

                appMortLoan.Application = app;
                appMortLoan.SaveAndFlush();

                int key = appMortLoan.Key;

                // now try and load it
                ApplicationMortgageLoanDetail_DAO appMortLoan2 = ApplicationMortgageLoanDetail_DAO.Find(key);
                Assert.IsNotNull(appMortLoan2);

				//// delete all the data
				//appMortLoan2.DeleteAndFlush();
				//app.DeleteAndFlush();
            }
        }
    }
}
