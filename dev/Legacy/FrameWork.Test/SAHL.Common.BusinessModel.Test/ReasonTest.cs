using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SAHL.Test;
using SAHL.Common;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using System.Configuration;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class ReasonTest : TestBase
    {
        
        [Test]
        public void GetByReasonTypeGroup()
        {
            string query = "SELECT TOP 3 rtg.ReasonTypeGroupKey FROM [2AM].[dbo].[ReasonTypeGroup] rtg (nolock) "
               + "JOIN [2AM].[dbo].[ReasonType] rt (nolock) on rt.ReasonTypeGroupKey = rtg.ReasonTypeGroupKey "
               + "WHERE rtg.ParentKey is null";
            DataTable DT = base.GetQueryResults(query);

            Assert.That(DT.Rows.Count > 0);

            int[] keys = new int[DT.Rows.Count];
            string skeys = "";
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                keys[i] = Convert.ToInt32(DT.Rows[i][0]);
                skeys += "," + keys[i].ToString();
            }

            skeys = skeys.Remove(0, 1);

            IReasonRepository reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();
            IReadOnlyEventList<IReasonType> list  = reasonRepo.GetReasonTypeByReasonTypeGroup(keys);

            query = "SELECT rt.ReasonTypeKey FROM [2AM].[dbo].[ReasonTypeGroup] rtg (nolock) "
               + "JOIN [2AM].[dbo].[ReasonType] rt (nolock) on rt.ReasonTypeGroupKey = rtg.ReasonTypeGroupKey "
               + "WHERE rtg.ReasonTypeGroupKey in ({0})";
            query = String.Format(query, skeys);
            DT = base.GetQueryResults(query);

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Assert.That(list[i].Key == Convert.ToInt32(DT.Rows[i][0]));
            }
        }

    }
}
