using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using Castle.ActiveRecord;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    [TestFixture]
    public class TitleDeedCheck_DAOTest : TestBase
    {
        [Test]
        public void LoadSaveLoad()
        {
            using (new SessionScope())
            {
                //TitleDeedCheck_DAO tdc = TitleDeedCheck_DAO.FindFirst();
                //tdc.TitleDeedIndicator = "On file";
                //IList<TitleDeedCheck_DAO> allRecords = TitleDeedCheck_DAO.FindAll();
                //Console.WriteLine("XXX " + allRecords[(allRecords.Count-1)].Key);
                //tdc.Key = allRecords[(allRecords.Count - 1)].Key + 10;
                //tdc.CreateAndFlush();

                //tdc.Refresh();

                //tdc.DeleteAndFlush();
            }
        }

    }
}
