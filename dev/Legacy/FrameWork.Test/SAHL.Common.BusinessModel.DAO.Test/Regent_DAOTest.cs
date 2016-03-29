using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    [TestFixture]
    public class Regent_DAOTest : TestBase
    {
        
        [Test]
        public void LoadSaveLoad()
        {
            using (new SessionScope())
            {

                Regent_DAO errRep = DAODataConsistancyChecker.GetDAO<Regent_DAO>();
                errRep.CreateAndFlush();

                // now try and load it
                Regent_DAO errRep2 = Regent_DAO.Find(errRep.Key);
                Assert.IsNotNull(errRep2);

                // delete all the data
                errRep2.DeleteAndFlush();
            }
        }

    }
}
