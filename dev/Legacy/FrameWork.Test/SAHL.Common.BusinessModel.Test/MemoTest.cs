using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SAHL.Test;
using SAHL.Common;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class MemoTest : TestBase
    {
        [Test]
        public void GetByGenericKey()
        {
            using (new SessionScope())
            {
                IDomainMessageCollection messages = new DomainMessageCollection();

                ParameterCollection parameters = new ParameterCollection();
                IDbConnection con = Helper.GetSQLDBConnection();
                DataTable DT = new DataTable();

                string query = @"select top 1 GenericKey, GenericKeyTypeKey 
                        from [2AM].[dbo].[Memo] m (nolock) 
                        where GenericKey > 0
                        group by GenericKey, GenericKeyTypeKey 
                        having count(GenericKey) > 1";

                Helper.FillFromQuery(DT, query, con, parameters);

                Assert.That(DT.Rows.Count == 1, "No data found in Memo table");

                int GenericKey = (int)DT.Rows[0][0];
                int GenericKeyTypeKey = (int)DT.Rows[0][1];

                query = String.Format("select distinct MemoKey from [2AM].[dbo].[Memo] (nolock) where GenericKey = {0} and GenericKeyTypeKey = {1}", GenericKey, GenericKeyTypeKey);
                DT = new DataTable();
                Helper.FillFromQuery(DT, query, con, parameters);

                Assert.That(DT.Rows.Count > 0, "Doh!");

                IEventList<IMemo> list = Memo.GetByGenericKey(GenericKey, GenericKeyTypeKey);

                Assert.That(DT.Rows.Count == list.Count);
            }
        }

        [Test]
        public void GetByGenericKeyActiveOnly()
        {
            using (new SessionScope())
            {
                IDomainMessageCollection messages = new DomainMessageCollection();

                ParameterCollection parameters = new ParameterCollection();
                IDbConnection con = Helper.GetSQLDBConnection();
                DataTable DT = new DataTable();
                string query = @"select top 1 GenericKey, GenericKeyTypeKey 
                        from [2AM].[dbo].[Memo] m (nolock) 
                        where GenericKey > 0
                        group by GenericKey, GenericKeyTypeKey 
                        having count(GenericKey) > 1";

                Helper.FillFromQuery(DT, query, con, parameters);

                Assert.That(DT.Rows.Count == 1, "No data found in Memo table");

                int GenericKey = (int)DT.Rows[0][0];
                int GenericKeyTypeKey = (int)DT.Rows[0][1];

                query = String.Format("select distinct MemoKey from [2AM].[dbo].[Memo] (nolock) where GenericKey = {0} and GenericKeyTypeKey = {1} and GeneralStatusKey = 1", GenericKey, GenericKeyTypeKey);
                DT = new DataTable();
                Helper.FillFromQuery(DT, query, con, parameters);

                Assert.That(DT.Rows.Count > 0, "Doh!");
  
                IEventList<IMemo> list = Memo.GetByGenericKey(GenericKey, GenericKeyTypeKey, 1);

                Assert.That(DT.Rows.Count == list.Count);
            }
        }
    }
}