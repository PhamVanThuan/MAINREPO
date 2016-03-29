using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    [TestFixture]
    public class RegentStatus_DAOTest : TestBase
    {
        [Test]
        public void LoadSaveLoad()
        {
            using (new SessionScope())
            {
                RegentStatus_DAO rs = DAODataConsistancyChecker.GetDAO<RegentStatus_DAO>();
                rs.Key = 100;
                rs.CreateAndFlush();

                // now try and load it
                RegentStatus_DAO rs2 = RegentStatus_DAO.Find(rs.Key);
                Assert.IsNotNull(rs2);

                // delete all the data
                rs2.DeleteAndFlush();
            }
        }
    }
}
