using Castle.ActiveRecord;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class CapRepositoryTest : TestBase
    {
        [SetUp]
        public void Setup()
        {
            _mockery = new MockRepository();
        }

        [TearDown]
        public void TearDown()
        {
            MockCache.Flush();
        }

        private ICapRepository _repo = RepositoryFactory.GetRepository<ICapRepository>();

        [Test, Ignore]
        public void GetCurrentCAPResetConfigDatesTest()
        {
            using (new SessionScope())
            {
                DataTable DT = _repo.GetCurrentCAPResetConfigDates();
                if (DT.Rows.Count == 0)
                    Assert.Fail(@"Method GetCurrentCAPResetConfigDates should return data.
                                Make sure ResetConfiguration, CapTypeConfiguration & CapTypeConfigurationDetail
                                has been synced from day before [2am] production db restore.");
            }
        }

        [Test]
        public void GetCapOffersByBrokerKeyTest()
        {
            using (new SessionScope())
            {
                string query = @"SELECT TOP 1 CO.BrokerKey
                                FROM [2AM].[dbo].[CapOfferDetail] COD (NOLOCK)
                                JOIN [2AM].[dbo].[CapOffer] CO (NOLOCK)
	                                ON CO.CapOfferKey = COD.CapOfferKey
                                JOIN [2AM].[dbo].[CapTypeConfiguration] CTC (NOLOCK)
	                                ON CO.CapTypeConfigurationKey = CTC.CapTypeConfigurationKey
                                LEFT OUTER JOIN  [X2].[X2Data].[CAP2_Offers] COData (NOLOCK)
	                                ON CO.CapOfferKey = COData.CapOfferKey
                                WHERE
	                                 CO.CapStatusKey   in (1,6,8,10,11,12,13)
                                 and COD.CapStatusKey   in (1,6,8,10,11,12,13)
                                group by
	                                CO.BrokerKey
                                order by
	                                count(*) asc";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(CapApplication_DAO), new ParameterCollection());
                if (o != null)
                {
                    int BrokerKey = Convert.ToInt32(o);
                    DataTable DT = _repo.GetCapOffersByBrokerKey(BrokerKey);
                    Assert.IsTrue(DT.Rows.Count > 0);
                }
            }
        }

        [Test]
        public void GetCapCreditOffersByBrokerKeyTest()
        {
            using (new SessionScope())
            {
                string query = @"SELECT TOP 1 B.BrokerKey
                                FROM
	                                   [2AM].[dbo].[CapOfferDetail] COD (NOLOCK)
                                JOIN  [2AM].[dbo].[CapOffer] CO (NOLOCK)
                                ON    CO.CapOfferKey = COD.CapOfferKey
                                JOIN  [2AM].[dbo].[CapTypeConfiguration] CTC (NOLOCK)
                                ON    CO.CapTypeConfigurationKey = CTC.CapTypeConfigurationKey
                                JOIN  [X2].[X2Data].[CAP2_Offers] COData (NOLOCK)
                                ON    CO.CapOfferKey = COData.CapOfferKey
                                JOIN  [2AM].[dbo].[Broker] B (NOLOCK)
                                ON COData.CapCreditBroker = B.ADUserName
                                WHERE
	                                CO.CapStatusKey = 9
                                and
	                                COD.CapStatusKey = 9
                                group by
	                                B.BrokerKey
                                order by
	                                count(*)";
                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(CapApplication_DAO), new ParameterCollection());
                if (o != null)
                {
                    int BrokerKey = Convert.ToInt32(o);
                    DataTable DT = _repo.GetCapCreditOffersByBrokerKey(BrokerKey);
                    Assert.IsTrue(DT.Rows.Count > 0);
                }
                else
                {
                    DataTable DT = _repo.GetCapCreditOffersByBrokerKey(0);
                    Assert.IsTrue(DT.Rows.Count == 0);
                }
            }
        }

        [Test]
        public void UpdateX2CapCaseDataTest()
        {
            string TestCaseQ = @"select top 1 co.CapOfferKey, co.BrokerKey
            from [2am].[dbo].CapOffer co (NOLOCK)
            join [2am].[dbo].Broker b (NOLOCK)
	            on co.BrokerKey = b.BrokerKey
            join [2am].[dbo].ADUser ad (NOLOCK)
	            on b.ADUserKey = ad.ADUserKey
            join [X2].[X2Data].[CAP2_Offers] xco (NOLOCK)
	            on co.CapOfferKey = xco.CapOfferKey
            join [X2].[X2].[WorkList] xwl (NOLOCK)
	            on xco.InstanceID = xwl.InstanceID
            join [X2].[X2].[InstanceActivitySecurity] xias (NOLOCK)
	            on xias.InstanceID = xco.InstanceID";

            DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(TestCaseQ, typeof(CapApplication_DAO), new ParameterCollection());

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int CapOfferKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                int BrokerKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);

                string TestBrokerQ = string.Format(@"select top 1 [br].BrokerKey
                from [2am].[dbo].[OfferRoleTypeOrganisationStructureMapping] ortosm (NOLOCK)
                inner join [2am].[dbo].[UserOrganisationStructure] uos (NOLOCK)
	                on uos.OrganisationStructureKey = ortosm.OrganisationStructureKey
                inner join [2am].[dbo].[aduser] ad (NOLOCK)
	                on ad.ADUserKey = uos.ADUserKey
                inner join [2am].[dbo].[broker] br (NOLOCK)
	                on br.ADUserKey = ad.ADUserKey
                left join [2am].[dbo].[CapCreditBrokerToken] ccbr (NOLOCK)
	                on br.BrokerKey = ccbr.BrokerKey
                where
	                ortosm.OfferRoleTypeKey in (14)
                and
	                ad.generalstatuskey = 1
                and
	                ccbr.BrokerKey is null
                and
	                br.BrokerKey <> {0}", BrokerKey);

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(TestBrokerQ, typeof(CapApplication_DAO), new ParameterCollection());
                if (o != null)
                {
                    int AssignToBrokerKey = Convert.ToInt32(o);
                    TransactionScope tx = new TransactionScope();
                    try
                    {
                        _repo.UpdateX2CapCaseData(CapOfferKey, AssignToBrokerKey);
                    }
                    finally
                    {
                        tx.VoteRollBack();
                        tx.Dispose();
                    }
                }
            }
            else
            {
                //TransactionScope tx = new TransactionScope();
                //try
                //{
                //    _repo.UpdateX2CapCaseData(0, 0); // CapOfferKey = 0 BrokerKey = 0 ??? 
                //}
                //finally
                //{
                //    tx.VoteRollBack();
                //    tx.Dispose();
                //}

                Assert.Ignore("No Data to perform test.");
            }
        }

        [Test]
        public void UpdateX2CapCreditCaseDataTest()
        {
            string TestCaseQ = @"select top 1 co.CapOfferKey, cb.BrokerKey
            from [2am].[dbo].CapOffer co (NOLOCK)
            join [2am].[dbo].Broker b (NOLOCK)
	            on co.BrokerKey = b.BrokerKey
            join [2am].[dbo].ADUser ad (NOLOCK)
	            on b.ADUserKey = ad.ADUserKey
            join [X2].[X2Data].[CAP2_Offers] xco (NOLOCK)
	            on co.CapOfferKey = xco.CapOfferKey
            join [2am].[dbo].Broker cb (NOLOCK)
	            on xco.CapCreditBroker = cb.ADUserName
            join [X2].[X2].[WorkList] xwl (NOLOCK)
	            on xco.InstanceID = xwl.InstanceID
            join [X2].[X2].[InstanceActivitySecurity] xias (NOLOCK)
	            on xias.InstanceID = xco.InstanceID ";

            DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(TestCaseQ, typeof(CapApplication_DAO), new ParameterCollection());

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int CapOfferKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                int BrokerKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);

                string TestBrokerQ = string.Format(@"select top 1 [br].brokerKey
                from [2am].[dbo].[UserOrganisationStructure] uos (NOLOCK)
                inner join [2am].[dbo].[aduser] ad (NOLOCK)
                on ad.ADUserKey = uos.ADUserKey
                inner join [2am].[dbo].[broker] br (NOLOCK)
                on br.ADUserKey = ad.ADUserKey
                inner join [2am].[dbo].[CapCreditBrokerToken] ccbr (NOLOCK)
                on br.BrokerKey = ccbr.BrokerKey
                where
	                ad.generalstatuskey = 1
                and
	                uos.OrganisationStructureKey in (2017)
                and
	                br.BrokerKey <> {0}", BrokerKey);

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(TestBrokerQ, typeof(CapApplication_DAO), new ParameterCollection());
                if (o != null)
                {
                    int AssignToBrokerKey = Convert.ToInt32(o);
                    TransactionScope tx = new TransactionScope();
                    try
                    {
                        _repo.UpdateX2CapCreditCaseData(CapOfferKey, AssignToBrokerKey);
                    }
                    finally
                    {
                        tx.VoteRollBack();
                        tx.Dispose();
                    }
                }
            }
        }

        [Test]
        public void CapAccountSearchTest()
        {
            string query = @"select top 1 a.Accountkey
                                from [2am].[dbo].[account] a (nolock)
                                join [2am].[dbo].[OriginationSourceProduct] osp (nolock) on osp.ProductKey = a.RRR_ProductKey
                                join [2am].[dbo].[OSPFinancialAdjustmentTypeSource] ofats (nolock) on ofats.OriginationSourceProductKey = osp.OriginationSourceProductKey
                                join [2am].[fin].[FinancialAdjustmentTypeSource] fats (nolock) on fats.FinancialAdjustmentTypeSourceKey = ofats.FinancialAdjustmentTypeSourceKey
                                order by a.Accountkey desc";
            DataTable DT = base.GetQueryResults(query);
            Assert.That(DT.Rows.Count == 1);
            try
            {
                List<IAccount> acclist = _repo.CapAccountSearch((int)DT.Rows[0]["Accountkey"]);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetCapOfferByAccountkeyAndStatusTest()
        {
            string query = @"select top 1 accountkey , capstatuskey from capoffer (nolock) order by accountkey desc";
            DataTable DT = base.GetQueryResults(query);
            Assert.That(DT.Rows.Count == 1);

            int acckey = (int)DT.Rows[0]["Accountkey"];
            int statusKey = (int)DT.Rows[0]["capstatuskey"];
            try
            {
                IList<ICapApplication> acclist = _repo.GetCapOfferByAccountKeyAndStatus(acckey, statusKey);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetCapDeclineReasonsTest()
        {
            try
            {
                IList<ICapNTUReason> caplist = _repo.GetCapDeclineReasons();
                Assert.IsNotNull(caplist);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetCapNTUReasonsTest()
        {
            try
            {
                IList<ICapNTUReason> caplist = _repo.GetCapNTUReasons();
                Assert.IsNotNull(caplist);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void AccountSearchTest()
        {
            string query = @"select top 1 a.Accountkey
                                from [2am].[dbo].[account] a (nolock)
                                inner join [2am].[dbo].[OriginationSourceProduct] osp (nolock) on osp.ProductKey = a.RRR_ProductKey
                                inner join [2am].[dbo].[OSPFinancialAdjustmentTypeSource] ofats (nolock) on ofats.OriginationSourceProductKey = osp.OriginationSourceProductKey
                                inner join [2am].[fin].[FinancialAdjustmentTypeSource] fats (nolock) on fats.FinancialAdjustmentTypeSourceKey = ofats.FinancialAdjustmentTypeSourceKey
                                order by a.Accountkey desc";

            DataTable DT = base.GetQueryResults(query);
            Assert.That(DT.Rows.Count == 1);
            try
            {
                List<IAccount> acclist = _repo.CapAccountSearch((int)DT.Rows[0]["Accountkey"]);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetAcceptedHistoryForCancelTest()
        {
            string query = @"select top 1 a.Accountkey
                                from [2am].[dbo].[account] a (nolock)
                                inner join [2am].[dbo].[OriginationSourceProduct] osp (nolock) on osp.ProductKey = a.RRR_ProductKey
                                inner join [2am].[dbo].[OSPFinancialAdjustmentTypeSource] ofats (nolock) on ofats.OriginationSourceProductKey = osp.OriginationSourceProductKey
                                inner join [2am].[fin].[FinancialAdjustmentTypeSource] fats (nolock) on fats.FinancialAdjustmentTypeSourceKey = ofats.FinancialAdjustmentTypeSourceKey
                                order by a.Accountkey desc";

            DataTable DT = base.GetQueryResults(query);
            Assert.That(DT.Rows.Count == 1);
            try
            {
                IList<ICapApplication> acclist = _repo.GetAcceptedHistoryForCancel((int)DT.Rows[0]["Accountkey"]);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetCapOfferByKey()
        {
            using (new SessionScope())
            {
                CapApplication_DAO dao = CapApplication_DAO.FindFirst();

                ICapApplication capOffer = _repo.GetCapOfferByKey(dao.Key);

                Assert.IsTrue(capOffer.Key == dao.Key);
            }
        }

        [Test]
        public void GetBrokerByFullNameTest()
        {
            string sql = "select top 1 BrokerKey, FullName from [2am]..Broker with (nolock)";
            DataTable dt = GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data");
            int brokerKey = Convert.ToInt32(dt.Rows[0]["BrokerKey"]);
            string fullName = Convert.ToString(dt.Rows[0]["FullName"]);
            dt.Dispose();

            using (new SessionScope())
            {
                IBroker broker = _repo.GetBrokerByFullName(fullName.ToLower());
                Assert.IsNotNull(broker);
                Assert.AreEqual(brokerKey, broker.Key);

                IBroker broker2 = _repo.GetBrokerByFullName(fullName.ToUpper());
                Assert.IsNotNull(broker2);
                Assert.AreEqual(brokerKey, broker2.Key);
            }
        }

        [Test]
        public void GetBrokerByADUserKeyTest()
        {
            string sql = "select top 1 ADUserKey from broker with (nolock) where aduserkey is not null";
            DataTable dt = GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data");
            int ADUserKey = Convert.ToInt32(dt.Rows[0]["ADUserKey"]);

            dt.Dispose();

            using (new SessionScope())
            {
                IBroker broker = _repo.GetBrokerByADUserKey(ADUserKey);
                Assert.IsNotNull(broker);
            }
        }

        [Test]
        public void GetBrokerByKeyTest()
        {
            string sql = "select top 1 BrokerKey from broker with (nolock) where aduserkey is not null order by 1 desc";
            DataTable dt = GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data");
            int BrokerKey = Convert.ToInt32(dt.Rows[0]["BrokerKey"]);

            dt.Dispose();

            using (new SessionScope())
            {
                IBroker broker = _repo.GetBrokerByBrokerKey(BrokerKey);
                Assert.IsNotNull(broker);
            }
        }

        [Test]
        public void GetBrokerByBrokerTest()
        {
            string sql = "select top 1 BrokerKey from broker with (nolock) where aduserkey is not null order by 1 desc";
            DataTable dt = GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data");
            int BrokerKey = Convert.ToInt32(dt.Rows[0]["BrokerKey"]);

            dt.Dispose();

            using (new SessionScope())
            {
                IBroker broker = _repo.GetBrokerByBrokerKey(BrokerKey);
                Assert.IsNotNull(broker);
            }
        }

        [Test]
        public void GetCapBokers()
        {
            using (new SessionScope())
            {
                IList<IBroker> brokers = _repo.GetCapBrokers();

                Assert.IsTrue(brokers.Count > 0);
            }
        }

        [Test]
        public void GetCapCreditBokers()
        {
            using (new SessionScope())
            {
                IList<IBroker> brokers = _repo.GetCapCreditBrokers();

                Assert.IsTrue(brokers.Count > 0);
            }
        }

        [Test]
        public void GetNextCapCreditBroker()
        {
            using (new SessionScope())
            {
                string broker = _repo.GetNextCapCreditBroker();

                Assert.IsFalse(String.IsNullOrEmpty(broker));
            }
        }

        [Test]
        public void IsReAdvanceTest()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 co.CapOfferKey
                from [2am].[dbo].CapOffer co (nolock)
                inner join [2am].[dbo].Account acc (nolock)
	                on co.AccountKey = acc.AccountKey
                where acc.AccountStatusKey = 1";
                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(CapApplication_DAO), new ParameterCollection());
                if (o != null)
                {
                    int capOfferKey = Convert.ToInt32(o);
                    ICapApplication capApp = _repo.GetCapOfferByKey(capOfferKey);
                    bool? result = _repo.IsReAdvance(capApp);
                    Assert.IsTrue(result.HasValue);
                }
            }
        }

        [Test]
        public void CreateCapApplication()
        {
            ICapApplication cap = _repo.CreateCapApplication();
            Assert.IsNotNull(cap);
        }

        [Test]
        public void CreateCapApplicationDetail()
        {
            ICapApplicationDetail cap = _repo.CreateCapApplicationDetail();
            Assert.IsNotNull(cap);
        }

        [Test]
        public void CreateTrade()
        {
            ITrade cap = _repo.CreateTrade();
            Assert.IsNotNull(cap);
        }

        [Test]
        public void GetCapNTUReasonsMock()
        {
            IList<ICapNTUReason> capmock = new List<ICapNTUReason>();
            ICapNTUReason capres = _mockery.StrictMock<ICapNTUReason>();
            capmock.Add(capres);
            ICapRepository CapRep = _mockery.StrictMock<ICapRepository>();
            Expect.Call(CapRep.GetCapNTUReasons()).Return(capmock).IgnoreArguments();
            _mockery.ReplayAll();
            IList<ICapNTUReason> cap = CapRep.GetCapNTUReasons();
            Assert.IsNotNull(cap.Count > 0);
        }

        [Test]
        public void GetCapCancellationReasons()
        {
            IList<ICancellationReason> cap = _repo.GetCapCancellationReasons();
            Assert.IsNotNull(cap.Count > 0);
        }

        [Test]
        public void CreateCapTypeConfigurationDetailMock()
        {
            ICapTypeConfigurationDetail capmock = _mockery.StrictMock<ICapTypeConfigurationDetail>();
            SetupResult.For(capmock.Key).Return(1);
            ICapRepository CapRep = _mockery.StrictMock<ICapRepository>();
            Expect.Call(CapRep.CreateCapTypeConfigurationDetail()).Return(capmock).IgnoreArguments();
            _mockery.ReplayAll();
            ICapTypeConfigurationDetail cap = CapRep.CreateCapTypeConfigurationDetail();
            Assert.IsNotNull(cap);
        }

        [Test]
        public void CreateCapTypeConfiguration()
        {
            ICapTypeConfiguration cap = _repo.CreateCapTypeConfiguration();
            Assert.IsNotNull(cap);
        }

        [Test]
        public void CreateCapTypeConfigurationDetail()
        {
            ICapTypeConfigurationDetail cap = _repo.CreateCapTypeConfigurationDetail();
            Assert.IsNotNull(cap);
        }

        [Test]
        public void GetCapTypes()
        {
            IList<ICapType> GetCapTypes = _repo.GetCapTypes();
            Assert.IsNotNull(GetCapTypes);
        }

        [Test]
        public void GetCurrentCAPResetConfigDates()
        {
            using (new SessionScope())
            {
                try
                {
                    DataTable CAPResets = _repo.GetCurrentCAPResetConfigDates();
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }

        [Test]
        public void GetCapTypeConfigByResetDateTest()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 resetdate from dbo.CapTypeConfiguration (nolock) order by captypeconfigurationkey desc";

                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);
                try
                {
                    IList<ICapTypeConfiguration> datelist = _repo.GetCapTypeConfigByResetDate((DateTime)DT.Rows[0]["resetdate"]);
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }

        [Test]
        public void GetCapTypeConfigByKey()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 CapTypeConfigurationKey from dbo.CapTypeConfiguration (nolock) order by captypeconfigurationkey desc";

                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);
                try
                {
                    ICapTypeConfiguration keylist = _repo.GetCapTypeConfigByKey((int)DT.Rows[0]["CapTypeConfigurationKey"]);
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }

        [Test]
        public void GetPreviousResetByResetDateAndRCKeyTest()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 resetdate ,resetConfigurationKey from dbo.CapTypeConfiguration (nolock) order by captypeconfigurationkey desc";

                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);
                try
                {
                    IReset datelist = _repo.GetPreviousResetByResetDateAndRCKey((DateTime)DT.Rows[0]["resetdate"], (int)DT.Rows[0]["resetConfigurationKey"]);
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }

        [Test]
        public void CheckLTVThresholdTest()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 co.CapOfferKey
                from [2am].[dbo].CapOffer co (nolock)
                inner join [2am].[dbo].Account acc (nolock)
	                on co.AccountKey = acc.AccountKey
                where acc.AccountStatusKey = 1";
                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(CapApplication_DAO), new ParameterCollection());
                if (o != null)
                {
                    int capOfferKey = Convert.ToInt32(o);
                    ICapApplication capApp = _repo.GetCapOfferByKey(capOfferKey);
                    bool? result = _repo.CheckLTVThreshold(capApp);
                    Assert.IsTrue(result.HasValue);
                }
            }
        }

        [Test]
        public void GetCapPaymentOptionsTest()
        {
            using (new SessionScope())
            {
                IList<ICapPaymentOption> capPaymentOptions = _repo.GetCapPaymentOptions();
                Assert.IsNotNull(capPaymentOptions);
                Assert.IsTrue(capPaymentOptions.Count > 0);
            }
        }

        [Test]
        public void GetCapTypesForTradeTest()
        {
            using (new SessionScope())
            {
                IList<ICapType> capTypes = _repo.GetCapTypesForTrade();
                Assert.IsNotNull(capTypes);
                Assert.IsTrue(capTypes.Count > 0);
            }
        }

        [Test]
        public void GetCurrentCapTypeConfigByResetConfigKeyTest()
        {
            using (new SessionScope())
            {
                string sql = @"select ct.ResetConfigurationKey
                from [2am].[dbo].CapTypeConfiguration ct (nolock)
                where
	                ct.offerStartDate <= getdate()
		                and
	                ct.OfferEndDate >= getdate()
		                and
	                ct.GeneralStatusKey = 1
                order by
	                ct.OfferStartDate";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                if (o != null)
                {
                    int resetConfigurationKey = Convert.ToInt32(o);
                    ICapTypeConfiguration capTypeConfiguration = _repo.GetCurrentCapTypeConfigByResetConfigKey(resetConfigurationKey);
                    Assert.IsNotNull(capTypeConfiguration);
                }
                else
                    Assert.Ignore("NO DATA : Please sync by following steps in - svn://sahls31/sahl/Project Documents 2010/IT Admin/Sync Cap Config.docx");
            }
        }

        [Test]
        public void GetResetConfigurationByKeyTest()
        {
            using (new SessionScope())
            {
                ResetConfiguration_DAO rc_dao = ResetConfiguration_DAO.FindFirst();
                IResetConfiguration resetConfig = _repo.GetResetConfigurationByKey(rc_dao.Key);
                Assert.IsNotNull(resetConfig);
            }
        }

        [Test]
        public void GetResetConfigurationsTest()
        {
            using (new SessionScope())
            {
                IList<IResetConfiguration> resetConfigs = _repo.GetResetConfigurations();
                Assert.IsNotNull(resetConfigs);
                Assert.IsTrue(resetConfigs.Count > 0);
            }
        }

        [Test]
        public void GetTradeByGroupingTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 t.CapTypeKey, t.ResetConfigurationKey, t.StartDate, t.EndDate
                from [2am].[dbo].Trade t (nolock)";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                int capTypeKey = Convert.ToInt32(ds.Tables[0].Rows[0]["CapTypeKey"]);
                int resetConfigurationKey = Convert.ToInt32(ds.Tables[0].Rows[0]["ResetConfigurationKey"]);
                DateTime startDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"]);
                DateTime endDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["EndDate"]);

                IList<ITrade> trades = _repo.GetTradeByGrouping(capTypeKey, resetConfigurationKey, startDate, endDate);
                Assert.IsNotNull(trades);
                Assert.IsTrue(trades.Count > 0);
            }
        }

        #region Trade Method Tests

        [Test]
        public void GetResetDatesByTradeTypeTest()
        {
            using (new SessionScope())
            {
                string testQuery = @"SELECT TOP 1 T.TradeType
                FROM
	                [2AM].[dbo].[ResetConfiguration] RC (NOLOCK)
                JOIN
	                [2AM].[dbo].[Trade] T (NOLOCK)
                ON
	                RC.ResetConfigurationKey = T.ResetConfigurationKey";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(testQuery, typeof(CapApplication_DAO), new ParameterCollection());
                if (o != null)
                {
                    string tt = Convert.ToString(o);
                    DataTable dt = _repo.GetResetDatesByTradeType(tt);
                    Assert.IsTrue(dt.Rows.Count > 0);
                }
            }
        }

        [Test]
        public void GetResetDatesForAddingByTradeTypeTest()
        {
            using (new SessionScope())
            {
                string testQuery = @"SELECT TOP 1 T.TradeType
                FROM
	                [2AM].[dbo].[ResetConfiguration] RC (NOLOCK)
                JOIN
	                [2AM].[dbo].[Trade] T (NOLOCK)
                ON
	                RC.ResetConfigurationKey = T.ResetConfigurationKey";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(testQuery, typeof(CapApplication_DAO), new ParameterCollection());
                if (o != null)
                {
                    string tt = Convert.ToString(o);
                    DataTable dt = _repo.GetResetDatesForAddingByTradeType(tt);
                    Assert.IsTrue(dt.Rows.Count > 0);
                }
            }
        }

        [Test]
        public void GetTradeGroupingsByResetConfigurationKeyTest()
        {
            using (new SessionScope())
            {
                string testQuery = @"SELECT TOP 1 T.RESETCONFIGURATIONKEY, T.TRADETYPE
                FROM [2AM].[DBO].[TRADE] T (NOLOCK)
                LEFT OUTER JOIN [2AM].[DBO].[CAPTYPE] CT (NOLOCK)
                    ON T.CAPTYPEKEY = CT.CAPTYPEKEY";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(testQuery, typeof(CapApplication_DAO), new ParameterCollection());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int resetConfigurationKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    string tradeType = Convert.ToString(ds.Tables[0].Rows[0][1]);
                    DataTable dt = _repo.GetTradeGroupingsByResetConfigurationKey(resetConfigurationKey, tradeType);
                    if (dt.Rows.Count == 0)
                        Assert.Fail();
                }
            }
        }

        [Test]
        public void GetTradeGroupsByResetConfigKeyForDeleteTest()
        {
            using (new SessionScope())
            {
                string testQuery = @"DECLARE @CurrentDate datetime;
                SET @CurrentDate = CONVERT(varchar(10), getdate(), 121);

                SELECT top 1 T.ResetConfigurationKey,T.TradeType
                FROM
	                [2AM].[dbo].[Trade] T (NOLOCK)
                LEFT OUTER JOIN
	                [2AM].[dbo].[CapType] CT (NOLOCK)
                ON
	                T.CapTypeKey = CT.CapTypeKey
                WHERE
	                T.EndDate > @CurrentDate";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(testQuery, typeof(CapApplication_DAO), new ParameterCollection());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int resetConfigurationKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    string tradeType = Convert.ToString(ds.Tables[0].Rows[0][1]);
                    DataTable dt = _repo.GetTradeGroupsByResetConfigKeyForDelete(resetConfigurationKey, tradeType);
                    if (dt.Rows.Count == 0)
                        Assert.Fail();
                }
            }
        }

        [Test]
        public void GetActiveTradeGroupingsByResetConfigurationKeyTest()
        {
            using (new SessionScope())
            {
                string testQuery = @"SELECT TOP 1 T.ResetConfigurationKey, T.TradeType
                FROM
	                [2AM].[dbo].[Trade] T (NOLOCK)
                LEFT OUTER JOIN
	                [2AM].[dbo].[CapType] CT (NOLOCK)
                ON
	                T.CapTypeKey = CT.CapTypeKey";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(testQuery, typeof(CapApplication_DAO), new ParameterCollection());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int resetConfigurationKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    string tradeType = Convert.ToString(ds.Tables[0].Rows[0][1]);
                    DataTable dt = _repo.GetActiveTradeGroupingsByResetConfigurationKey(resetConfigurationKey, tradeType);
                    if (dt.Rows.Count == 0)
                        Assert.Fail();
                }
            }
        }

        #endregion Trade Method Tests

        [Test]
        public void CapTypeDetermineAppTypeTest()
        {
            using (new SessionScope())
            {
                // get latest cap offer
                string sql = @"select top 1 * from [2am].[dbo].CapOffer c (nolock)";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                int accountKey = Convert.ToInt32(ds.Tables[0].Rows[0]["AccountKey"]);
                int capTypeConfigurationKey = Convert.ToInt32(ds.Tables[0].Rows[0]["CapTypeConfigurationKey"]);
                int capOfferKey = Convert.ToInt32(ds.Tables[0].Rows[0]["CapOfferKey"]);

                double committedLoanValue = 0;
                string appType1 = "";
                string appType2 = "";
                string appType3 = "";

                _repo.CapTypeDetermineAppType(accountKey, capTypeConfigurationKey, capOfferKey, out committedLoanValue, out appType1, out appType2, out appType3);

                Assert.IsNotNull(appType1);
            }
        }
    }
}