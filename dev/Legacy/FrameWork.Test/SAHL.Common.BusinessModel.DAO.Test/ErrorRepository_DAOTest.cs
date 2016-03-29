using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    [TestFixture]
    public class ErrorRepository_DAOTest : TestBase
    {
        [Test]
        public void LoadSaveLoad()
        {
            using (new SessionScope())
            {

                ErrorRepository_DAO errRep = DAODataConsistancyChecker.GetDAO<ErrorRepository_DAO>();

                errRep.CreateAndFlush();

                // now try and load it
                ErrorRepository_DAO errRep2 = ErrorRepository_DAO.Find(errRep.Key);
                Assert.IsNotNull(errRep2);

                // delete all the data
                errRep2.DeleteAndFlush();
            }
        }
    }
}
