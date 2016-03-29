using Castle.ActiveRecord;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class RecoveriesRepositoryTest : TestBase
    {
        private IRecoveriesRepository _repo;

        [SetUp]
        public void Setup()
        {
            _repo = RepositoryFactory.GetRepository<IRecoveriesRepository>();
            // set the strategy to default so we actually go to the database
            SetRepositoryStrategy(TypeFactoryStrategy.Default);
            _mockery = new MockRepository();
        }

        [TearDown]
        public void TearDown()
        {
            MockCache.Flush();
        }

        [Test]
        public void CreateEmptyRecoveriesProposal()
        {
            using (new SessionScope())
            {
                IRecoveriesProposal recoveriesProposal = _repo.CreateEmptyRecoveriesProposal();
                Assert.IsNotNull(recoveriesProposal);
            }
        }

        [Test]
        public void GetRecoveriesProposalByKey()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                int recoveriesProposalKey = Convert.ToInt32(base.GetPrimaryKey("[recoveries].RecoveriesProposal", "RecoveriesProposalKey"));

                // if we got no data then lets go insert a record and get the key again
                if (recoveriesProposalKey <= 0)
                {
                    int accountKey = Convert.ToInt32(base.GetPrimaryKey("Account", "AccountKey", " AccountStatusKey = 1 "));
                    recoveriesProposalKey = InsertRecoveriesProposal(accountKey, GeneralStatuses.Active);
                }

                IRecoveriesProposal recoveriesProposal = _repo.GetRecoveriesProposalByKey(recoveriesProposalKey);
                Assert.IsTrue(recoveriesProposal.Key == recoveriesProposalKey);
            }
        }

        [Test]
        public void GetRecoveriesProposalsByAccountKey()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                int accountKey = -1;
                int recoveriesProposalKey = -1;
                DataTable dt = base.GetQueryResults("select top 1 RecoveriesProposalKey, AccountKey from [2am].[recoveries].RecoveriesProposal (nolock) where GeneralStatusKey = 1 order by 1 desc");

                // if we got no data then lets go insert a record and get the key again
                if (dt == null || dt.Rows.Count <= 0)
                {
                    accountKey = Convert.ToInt32(base.GetPrimaryKey("Account", "AccountKey", " AccountStatusKey = 1 "));
                    recoveriesProposalKey = InsertRecoveriesProposal(accountKey, GeneralStatuses.Active);
                }
                else
                {
                    recoveriesProposalKey = Convert.ToInt32(dt.Rows[0][0]);
                    accountKey = Convert.ToInt32(dt.Rows[0][1]);
                }

                List<IRecoveriesProposal> recoveriesProposals = _repo.GetRecoveriesProposalsByAccountKey(accountKey);
                Assert.IsTrue(recoveriesProposals.Count > 0);
                Assert.IsTrue(recoveriesProposals[0].Account.Key == accountKey);

                List<IRecoveriesProposal> recoveriesProposalsActive = _repo.GetRecoveriesProposalsByAccountKey(accountKey, GeneralStatuses.Active);
                Assert.IsTrue(recoveriesProposalsActive.Count > 0);
                Assert.IsTrue(recoveriesProposalsActive[0].Account.Key == accountKey);
                Assert.IsTrue(recoveriesProposalsActive[0].GeneralStatus.Key == (int)GeneralStatuses.Active);
            }
        }

        [Test]
        public void GetActiveRecoveriesProposalByAccountKey()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                int accountKey = -1;
                int recoveriesProposalKey = -1;
                DataTable dt = base.GetQueryResults("select top 1 RecoveriesProposalKey, AccountKey from [2am].[recoveries].RecoveriesProposal (nolock) where GeneralStatusKey = 1 order by 1 desc");

                // if we got no data then lets go insert a record and get the key again
                if (dt == null || dt.Rows.Count <= 0)
                {
                    accountKey = Convert.ToInt32(base.GetPrimaryKey("Account", "AccountKey", " AccountStatusKey = 1 "));
                    recoveriesProposalKey = InsertRecoveriesProposal(accountKey, GeneralStatuses.Active);
                }
                else
                {
                    recoveriesProposalKey = Convert.ToInt32(dt.Rows[0][0]);
                    accountKey = Convert.ToInt32(dt.Rows[0][1]);
                }

                IRecoveriesProposal recoveriesProposal = _repo.GetActiveRecoveriesProposalByAccountKey(accountKey);
                Assert.IsTrue(recoveriesProposal.Account.Key == accountKey);
                Assert.IsTrue(recoveriesProposal.GeneralStatus.Key == (int)GeneralStatuses.Active);
            }
        }

        #region Helper Methods

        private int InsertRecoveriesProposal(int accountKey, GeneralStatuses generalStatus)
        {
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            IOrganisationStructureRepository osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

            IRecoveriesProposal rp = _repo.CreateEmptyRecoveriesProposal();
            rp.Account = accRepo.GetAccountByKey(accountKey);
            rp.ShortfallAmount = 100;
            rp.RepaymentAmount = 100;
            rp.StartDate = DateTime.Now;
            rp.AcknowledgementOfDebt = true;
            rp.ADUser = osRepo.GetAdUserForAdUserName("System");
            rp.CreateDate = DateTime.Now;
            rp.GeneralStatus = RepositoryFactory.GetRepository<ILookupRepository>().GeneralStatuses[generalStatus];

            _repo.SaveRecoveriesProposal(rp);

            return rp.Key;
        }

        #endregion Helper Methods
    }
}