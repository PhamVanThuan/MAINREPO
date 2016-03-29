using NUnit.Framework;
using SAHL.Test;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.DAO;
using System;
using Castle.ActiveRecord;
using Rhino.Mocks;
using System.Data;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
	[TestFixture]
	public class HelpDeskRepositoryTest : TestBase
	{
        private IHelpDeskRepository _repo = RepositoryFactory.GetRepository<IHelpDeskRepository>();

        [Test]
        public void GetHelpDeskCallSummaryByLegalEntityKey()
        {
            string sql = @"select top 1 GenericKey
                from Memo m (nolock)
                inner join HelpDeskQuery hdq (nolock) on hdq.MemoKey = m.MemoKey
                where m.GenericKeyTypeKey = 3 -- LegalEntity
                ";
            DataTable dt = GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data");
            int leKey = Convert.ToInt32(dt.Rows[0]["GenericKey"]);
            dt.Dispose();

            using (new SessionScope())
            {
                IReadOnlyEventList<IHelpDeskQuery> hdQueries = _repo.GetHelpDeskCallSummaryByLegalEntityKey(leKey);
                Assert.Greater(hdQueries.Count, 0);
            }

        }

		[Test]
		public void X2AutoArchive2AM_Update_FromRepository()
		{
			using (new SessionScope())
			{
				bool result = false;

				HelpDeskQuery_DAO helpDeskQuery_DAO = HelpDeskQuery_DAO.FindFirst();
				if (helpDeskQuery_DAO == null)
					Assert.Ignore("No Help Desk records available");

				IHelpDeskRepository hdRepo = RepositoryFactory.GetRepository<IHelpDeskRepository>();
				if (hdRepo != null)
					result = hdRepo.X2AutoArchive2AM_Update(helpDeskQuery_DAO.Key);
				else
					Assert.Ignore("Failed to load Help Desk Repository");

				Assert.IsTrue(result);
			}
		}

		[Test]
		public void GetHelpDeskQueryByInstanceID_FromRepository()
		{
		    using (new SessionScope(FlushAction.Never))
		    {
                IHelpDeskRepository hdRepo = RepositoryFactory.GetRepository<IHelpDeskRepository>();

                // NOTE: Inner join to Instance needed as foreign keys have not been set up yet - they will be once 
                // the archiving issues have been sorted out and then the join can be removed from this query
                string query = @"select top 1 InstanceID 
                                from X2.X2DATA.Help_Desk (nolock) 
                                where HelpDeskQueryKey > 0
                                and HelpDeskQueryKey is not null
                                order by InstanceID desc";
				DataTable DT = base.GetQueryResults(query);
                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data");
				Int64 instanceID = Convert.ToInt32(DT.Rows[0][0]);
                DT.Dispose();

                IReadOnlyEventList<IHelpDeskQuery> list = hdRepo.GetHelpDeskQueryByInstanceID(instanceID);
                Assert.IsNotNull(list);
				Assert.IsTrue(list.Count > 0);

                // older records sometimes have a HelpDeskQueryKey of null - make sure this works correctly if there
                // are any of these records remaining
                query = @"select top 1 InstanceID 
                        from X2.X2DATA.Help_Desk (nolock) 
                        where HelpDeskQueryKey is null
                        order by InstanceID desc";
                DT = base.GetQueryResults(query);
                if (DT.Rows.Count > 0)
                {
                    instanceID = Convert.ToInt32(DT.Rows[0][0]);
                    list = hdRepo.GetHelpDeskQueryByInstanceID(instanceID);
                    Assert.IsNull(list);
                }
                DT.Dispose();


		    }
		}

		[Test]
		public void GetHelpDeskQueryByHelpDeskQueryKey_FromRepository()
		{
			using (new SessionScope())
			{
				string query = "select top 1 HDQ.HelpDeskQueryKey from [2AM].dbo.HelpDeskQuery HDQ (nolock) ";
				DataTable DT = base.GetQueryResults(query);

				Assert.That(DT.Rows.Count == 1);
				int helpDeskQueryKey = Convert.ToInt32(DT.Rows[0][0]);
				IReadOnlyEventList<IHelpDeskQuery> list = null;
				IHelpDeskRepository hdRepo = RepositoryFactory.GetRepository<IHelpDeskRepository>();
				if (hdRepo != null)
					list = hdRepo.GetHelpDeskQueryByHelpDeskQueryKey(helpDeskQueryKey);
				else
					Assert.Ignore("Failed to load Help Desk Repository");

				if (list == null)
					Assert.Ignore("No Help Desk records available");

				Assert.IsTrue(list.Count > 0);
			}
		}

        [Test]
		public void CreateEmptyHelpDeskQuery_FromRepository()
		{
            IHelpDeskRepository hdRepo = RepositoryFactory.GetRepository<IHelpDeskRepository>();
            IHelpDeskQuery helpDeskQuery = hdRepo.CreateEmptyHelpDeskQuery();
            Assert.IsNotNull(helpDeskQuery);
		}
	}
}