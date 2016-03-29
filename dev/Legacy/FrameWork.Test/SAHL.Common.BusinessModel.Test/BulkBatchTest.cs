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
using SAHL.Common.BusinessModel.Helpers;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class BulkBatchTest : TestBase
    {
        [Test]
        public void DeleteBatchAndTransactions()
        {
            using (new SessionScope())
            {
                string query = base.GetSQLResource(typeof(BulkBatchTest), "BulkBatchTestData.sql");
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);
                Assert.That(DT.Rows[0].ItemArray.Length == 4);

                int BulkBatchKey = Convert.ToInt32(DT.Rows[0][0]);

                DomainMessageCollection messages = new DomainMessageCollection();
                BulkBatch.DeleteBatchAndTransactions(messages, BulkBatchKey);

                query = string.Format("SELECT * FROM [2AM].[dbo].[BulkBatch] bb (nolock) "
                    + "LEFT OUTER JOIN [2AM].[dbo].[BulkBatchLog] bl (nolock) ON bl.BulkBatchKey = bb.BulkBatchKey "
                    + "LEFT OUTER JOIN [2AM].[dbo].[BulkBatchParameter] bp (nolock) ON bp.BulkBatchKey = bb.BulkBatchKey "
                    + "LEFT OUTER JOIN [2AM].[dbo].[BatchTransaction] bt (nolock) ON bt.BulkBatchKey = bb.BulkBatchKey "
                    + "WHERE bb.BulkBatchKey = {0};", BulkBatchKey);
                DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 0);
            }
        }

    }
}
