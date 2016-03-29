using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    [TestFixture]
    public class PropertyAccessDetails_DAOTest : TestBase
    {
        [Test]
        public void LoadSaveLoad()
        {
            using (new SessionScope())
            {
                Property_DAO p = DAODataConsistancyChecker.GetDAO<Property_DAO>();
                p.SaveAndFlush();


                PropertyAccessDetails_DAO pad = DAODataConsistancyChecker.GetDAO<PropertyAccessDetails_DAO>();
                pad.Property = p;
                pad.CreateAndFlush();

                // now try and load it
                PropertyAccessDetails_DAO pad2 = PropertyAccessDetails_DAO.Find(pad.Key);
                Assert.IsNotNull(pad2);

                // delete all the data
                pad2.DeleteAndFlush();
                p.DeleteAndFlush();
            }
        }
    }
}
