using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;
using System;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class MemoRepositoryTest : TestBase
    {
        [Test]
        public void SaveMemo()
        {
            // Test by the Load Save Load Test
            // No logic implemented in this method
        }

        [Test]
        public void GetMemoByGenericKey()
        {
            using (new SessionScope())
            {
                string query = @"Select Top 1 GenericKey, GenericKeyTypeKey From Memo (nolock)";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int iGenericKey = Convert.ToInt32(DT.Rows[0][0]);
                int iGenericKeyTypeKey = Convert.ToInt32(DT.Rows[0][1]);

                IMemoRepository _mRepo = RepositoryFactory.GetRepository<IMemoRepository>();
                IEventList<IMemo> lstmemo = _mRepo.GetMemoByGenericKey(iGenericKey, iGenericKeyTypeKey);
                Assert.IsNotNull(lstmemo);
            }
        }

        [Test]
        public void GetMemoRelatedToAccount()
        {
            using (new SessionScope())
            {
                string query = @"Select Top 1 GenericKey, GenericKeyTypeKey, GeneralStatusKey From Memo (nolock)";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int iGenericKey = Convert.ToInt32(DT.Rows[0][0]);
                int iGenericKeyTypeKey = Convert.ToInt32(DT.Rows[0][1]);
                int iGeneralStatusKey = Convert.ToInt32(DT.Rows[0][2]);

                IMemoRepository _mRepo = RepositoryFactory.GetRepository<IMemoRepository>();
                IEventList<IMemo> lstmemo = _mRepo.GetMemoRelatedToAccount(iGenericKey, iGenericKeyTypeKey, iGeneralStatusKey);
                Assert.IsNotNull(lstmemo);
            }
        }

        [Test]
        public void GetMemoByGenericKeyADUserAndDate()
        {
            using (new SessionScope())
            {
                string query = @"Select Top 1 GenericKey, GenericKeyTypeKey, InsertedDate, ADUserKey From Memo (nolock)";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int iGenericKey = Convert.ToInt32(DT.Rows[0][0]);
                int iGenericKeyTypeKey = Convert.ToInt32(DT.Rows[0][1]);
                DateTime dtInsertedDate = Convert.ToDateTime(DT.Rows[0][2]);
                int iADUserKey = Convert.ToInt32(DT.Rows[0][3]);

                IOrganisationStructureRepository _userRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IADUser user = _userRepo.GetADUserByKey(iADUserKey);

                IMemoRepository _mRepo = RepositoryFactory.GetRepository<IMemoRepository>();
                IEventList<IMemo> lstmemo = _mRepo.GetMemoByGenericKeyADUserAndDate(iGenericKey, iGenericKeyTypeKey, dtInsertedDate, user);
            }
        }

        [Test]
        public void GetMemoByKey()
        {
            using (new SessionScope())
            {
                string query = @"Select Top 1 MemoKey From Memo (nolock)";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int iMemoKey = Convert.ToInt32(DT.Rows[0][0]);

                IMemoRepository _mRepo = RepositoryFactory.GetRepository<IMemoRepository>();
                IMemo memo = _mRepo.GetMemoByKey(iMemoKey);
                Assert.IsNotNull(memo);
            }
        }

        [Test]
        public void CreateMemo()
        {
            IMemoRepository _mRepo = RepositoryFactory.GetRepository<IMemoRepository>();
            IMemo memo = _mRepo.CreateMemo();
            Assert.IsNotNull(memo);
        }

        [Test]
        public void GetMemoByGenericKeyAndStatus()
        {
            using (new SessionScope())
            {
                string query = @"Select Top 1 GenericKey, GenericKeyTypeKey From Memo (nolock)";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int iGenericKey = Convert.ToInt32(DT.Rows[0][0]);
                int iGenericKeyTypeKey = Convert.ToInt32(DT.Rows[0][1]);

                IMemoRepository _mRepo = RepositoryFactory.GetRepository<IMemoRepository>();
                IEventList<IMemo> lstmemo = _mRepo.GetMemoByGenericKey(iGenericKey, iGenericKeyTypeKey, (int)GeneralStatuses.Active);
                Assert.IsNotNull(lstmemo);
            }
        }
    }
}