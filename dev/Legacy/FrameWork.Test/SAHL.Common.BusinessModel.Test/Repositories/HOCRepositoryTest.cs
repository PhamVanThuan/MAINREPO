using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using SAHL.Test;
using System;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class HOCRepositoryTest : TestBase
    {
        [SetUp()]
        public void Setup()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.DomainMessages.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.DomainMessages.Clear();
        }

        [Test]
        public void GetHOCByKey()
        {
            using (new SessionScope())
            {
                string query = @"SELECT TOP 1 FinancialServiceKey FROM [2AM].[dbo].[HOC] h (nolock)";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int key = Convert.ToInt32(DT.Rows[0][0]);

                IHOCRepository repo = RepositoryFactory.GetRepository<IHOCRepository>();
                IHOC hoc = repo.GetHOCByKey(key);
                Assert.IsNotNull(hoc);
            }
        }

        [Test]
        public void GetHOCHistoryDetailByFinancialServiceKey()
        {
            using (new SessionScope())
            {
                string query = "SELECT TOP 1 h.FinancialServiceKey, count(hhd.HOCHistoryDetailKey) "
                + "FROM [2AM].[dbo].[HOC] h (nolock) "
                + "join  [2AM].[dbo].[HOCHistory] hh (nolock) on hh.FinancialServiceKey = h.FinancialServiceKey "
                + "join  [2AM].[dbo].[HOCHistoryDetail] hhd (nolock) on hhd.HOCHistoryKey = hh.HOCHistoryKey "
                + "group by h.FinancialServiceKey order by count(hhd.HOCHistoryDetailKey) desc ";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                IHOCRepository repo = RepositoryFactory.GetRepository<IHOCRepository>();
                IReadOnlyEventList<IHOCHistoryDetail> list = repo.GetHOCHistoryDetailByFinancialServiceKey(Convert.ToInt32(DT.Rows[0][0]));
                Assert.That(list.Count == Convert.ToInt32(DT.Rows[0][1]));
            }
        }

        [Test]
        public void GetLastestHOCHistoryDetail()
        {
            using (new SessionScope())
            {
                string query = "SELECT TOP 1 h.FinancialServiceKey, count(hhd.HOCHistoryDetailKey) "
                + "FROM [2AM].[dbo].[HOC] h (nolock) "
                + "join  [2AM].[dbo].[HOCHistory] hh (nolock) on hh.FinancialServiceKey = h.FinancialServiceKey "
                + "join  [2AM].[dbo].[HOCHistoryDetail] hhd (nolock) on hhd.HOCHistoryKey = hh.HOCHistoryKey "
                + "group by h.FinancialServiceKey order by count(hhd.HOCHistoryDetailKey) desc ";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int financialServiceKey = Convert.ToInt32(DT.Rows[0][0]);

                IHOCRepository repo = RepositoryFactory.GetRepository<IHOCRepository>();

                IHOCHistoryDetail hocHistoryDetail = repo.GetLastestHOCHistoryDetail(financialServiceKey, 'I');
                Assert.IsTrue(hocHistoryDetail != null);
            }
        }

        [Test]
        public void CreateEmptyHOC()
        {
            using (new SessionScope())
            {
                IHOCRepository repo = RepositoryFactory.GetRepository<IHOCRepository>();
                IHOC hoc = repo.CreateEmptyHOC();
                Assert.IsNotNull(hoc);
            }
        }

        [Test]
        public void CreateEmptyHOCHistoryDetail()
        {
            using (new SessionScope())
            {
                IHOCRepository repo = RepositoryFactory.GetRepository<IHOCRepository>();
                IHOCHistoryDetail hoc = repo.CreateEmptyHOCHistoryDetail();
                Assert.IsNotNull(hoc);
            }
        }

        [Test]
        public void CreateEmptyHOCHistory()
        {
            using (new SessionScope())
            {
                IHOCRepository repo = RepositoryFactory.GetRepository<IHOCRepository>();
                IHOCHistory hoc = repo.CreateEmptyHOCHistory();
                Assert.IsNotNull(hoc);
            }
        }

        [Test]
        public void GetHOCHistoryByKey()
        {
            using (new SessionScope())
            {
                string query = @"SELECT TOP 1 HOCHistoryKey FROM [2AM].[dbo].[HOCHistory] h (nolock)";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int key = Convert.ToInt32(DT.Rows[0][0]);

                IHOCRepository repo = RepositoryFactory.GetRepository<IHOCRepository>();
                IHOCHistory hoc = repo.GetHOCHistoryByKey(key);
                Assert.That(hoc.Key == key);
            }
        }

        [Test]
        public void SaveHOC()
        {
            using (new SessionScope())
            {
                string query = @"SELECT TOP 1 FinancialServiceKey, HOCInsurerKey FROM [2AM].[dbo].[HOC] h (nolock) where HOCInsurerKey <> 2";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int key = Convert.ToInt32(DT.Rows[0][0]);
                int insurer = Convert.ToInt32(DT.Rows[0][1]);

                ILookupRepository lookup = RepositoryFactory.GetRepository<ILookupRepository>();
                IHOCRepository repo = RepositoryFactory.GetRepository<IHOCRepository>();
                IHOC hoc = repo.GetHOCByKey(key);
                string policyNo = hoc.HOCPolicyNumber;
                hoc.HOCInsurer = lookup.HOCInsurers.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.HOCInsurers.SAHLHOC)];
                repo.SaveHOC(hoc);
                Assert.That(hoc.HOCPolicyNumber == hoc.FinancialService.Account.Key.ToString());

                hoc.HOCInsurer = lookup.HOCInsurers.ObjectDictionary[Convert.ToString(insurer)];
                hoc.HOCPolicyNumber = policyNo;
                repo.SaveHOC(hoc);
            }
        }

        [Test]
        public void UpdateHOCWithHistory()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string query = "SELECT TOP 1 * FROM [2AM].[dbo].[HOC] (nolock) "
                    + "WHERE HOCInsurerKey <> 2 AND CancellationDate is null ";
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IHOC hoc = BMTM.GetMappedType<IHOC>(HOC_DAO.Find(DT.Rows[0][0]));
                int fsKey = Convert.ToInt32(DT.Rows[0][0]);
                int oldInsurer = hoc.HOCInsurer.Key;
                int oldHistoryKey = hoc.HOCHistory.Key;
                int newInsurerKey = 2;

                IHOCInsurer HI = BMTM.GetMappedType<IHOCInsurer>(HOCInsurer_DAO.Find(newInsurerKey));
                hoc.HOCInsurer = HI;

                IHOCRepository repo = RepositoryFactory.GetRepository<IHOCRepository>();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                repo.UpdateHOCWithHistory(Messages, oldInsurer, hoc, 'U');

                query = "SELECT HOCInsurerKey, HOCHistoryKey FROM [2AM].[dbo].[HOC] (nolock) "
                    + "WHERE FinancialServiceKey = " + fsKey.ToString();
                DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);
                int newHistoryKey = Convert.ToInt32(DT.Rows[0][1]);

                Assert.That(Convert.ToInt32(DT.Rows[0][0]) == newInsurerKey);

                query = "SELECT CancellationDate FROM [2AM].[dbo].[HOCHistory] (nolock) "
                    + "WHERE HOCHistoryKey = " + oldHistoryKey.ToString();
                DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);
                Assert.That(DT.Rows[0][0] != null);

                query = "SELECT top 1 HOCHistoryDetailKey, UpdateType FROM [2AM].[dbo].[HOCHistoryDetail] (nolock) "
                    + "WHERE HOCHistoryKey = " + newHistoryKey.ToString() + " order by HOCHistoryDetailKey desc";
                DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);
                Assert.That(DT.Rows[0][1].ToString() == "I");
                int detailKey = Convert.ToInt32(DT.Rows[0][0]);
            }
        }

        [Test]
        public void RetrieveHOCByOfferKeyTest()
        {
            // Test - RetrieveHOCByOfferKey
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 OFR.OfferKey
                                FROM [2AM].DBO.[Offer] OFR (nolock)
                                INNER JOIN [2AM].DBO.[Account] ACC (nolock) ON Acc.AccountKey = OFR.AccountKey
                                INNER JOIN [2AM].DBO.[Account] HACC (nolock) ON HACC.ParentAccountKey = ACC.AccountKey
                                WHERE
                                OFR.OfferTypeKey in (2,3,4) AND ACC.AccountStatusKey = 1
                                AND HACC.RRR_ProductKey = 3 AND HACC.AccountStatusKey = 1";
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int appKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                IAccountHOC hocAccount = null;

                IHOCRepository hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
                bool useAccount = false;

                // Test both overloaded methods
                // 1
                hocAccount = hocRepo.RetrieveHOCByOfferKey(appKey, ref useAccount);
                Assert.IsNotNull(hocAccount);

                // 2
                hocAccount = hocRepo.RetrieveHOCByOfferKey(appKey);
                Assert.IsNotNull(hocAccount);
            }
        }

        [Test]
        public void RetrieveHOCByAccountKeyTest()
        {
            // Test - RetrieveHOCByAccountKey
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 ACC.AccountKey
			                    FROM [2AM].DBO.[Account] ACC (nolock)
			                    INNER JOIN [2AM].DBO.[Account] HACC (nolock)
			                    ON HACC.ParentAccountKey = ACC.AccountKey
			                    WHERE ACC.AccountStatusKey = 1
                                AND HACC.RRR_ProductKey = 3
                                AND HACC.AccountStatusKey = 1";
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int accKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                IAccountHOC hocAccount = null;

                IHOCRepository hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
                bool useAccount = false;

                // Test both overloaded methods
                // 1
                hocAccount = hocRepo.RetrieveHOCByAccountKey(accKey, ref useAccount);
                Assert.IsNotNull(hocAccount);

                // 2
                hocAccount = hocRepo.RetrieveHOCByAccountKey(accKey);
                Assert.IsNotNull(hocAccount);
            }
        }

        [Test]
        public void GetHOCInsurerByKeyTest()
        {
            // Test - GetHOCInsurerByKey
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 HI.HOCInsurerKey
                FROM [2AM].[DBO].[HOCInsurer] HI (nolock)";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int hocInsurerKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                IHOCRepository hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
                IHOCInsurer hocInsurer = hocRepo.GetHOCInsurerByKey(hocInsurerKey);
                Assert.IsNotNull(hocInsurer);
            }
        }

        [Test]
        public void GetLastestHOCHistoryDetailFSKeyTest()
        {
            IHOCRepository hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();

            using (new SessionScope())
            {
                string sql = @"select top 1 h.FinancialServiceKey, hhd.HOCHistoryDetailKey
                from [2am].[dbo].[Hoc] h (nolock)
                inner join
                        (select max(hochistoryKey) as hochistoryKey, FinancialServiceKey
                        from [2am].[dbo].[HocHistory] (nolock)
                        group by FinancialServiceKey) as mhh
                    on mhh.FinancialServiceKey = h.FinancialServiceKey
                inner join
                        (select max(HocHistoryDetailKey) as HocHistoryDetailKey, hochistoryKey
                        from [2am].[dbo].[HocHistoryDetail] (nolock)
                        group by hochistoryKey) as mhhd
                    on mhhd.hochistoryKey = mhh.hochistoryKey
                inner join [2am].[dbo].[HocHistoryDetail] hhd (nolock)
                    on mhhd.HocHistoryDetailKey = hhd.HocHistoryDetailKey";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int fsKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    int hocHDKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);

                    IHOCHistoryDetail hocD = hocRepo.GetLastestHOCHistoryDetail(fsKey);

                    Assert.That(hocD.Key == hocHDKey);
                }
            }
        }

        [Test]
        public void GetLastestHOCHistoryDetailFSKeyAndUpdateTypeTest()
        {
            IHOCRepository hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();

            using (new SessionScope())
            {
                DataTable ut = GetHocHistoryDetailUpdateTypes();

                if (ut != null && ut.Rows.Count > 0)
                {
                    foreach (DataRow dr in ut.Rows)
                    {
                        char updateType = Convert.ToChar(dr[0]);

                        DataTable dt = GetHOCHistoryDetailForTest(updateType);

                        if (dt != null)
                        {
                            int fsKey = Convert.ToInt32(dt.Rows[0][0]);
                            int HocHistoryDetailKey = Convert.ToInt32(dt.Rows[0][1]);

                            IHOCHistoryDetail hocD = hocRepo.GetLastestHOCHistoryDetail(fsKey, updateType);

                            Assert.That(String.Compare(HocHistoryDetailKey.ToString(), hocD.Key.ToString(), true) == 0);
                        }
                    }
                }
                else
                    Assert.Fail("No HOCHistoryDetail update types found");
            }
        }

        private DataTable GetHOCHistoryDetailForTest(char updateType)
        {
            using (new SessionScope())
            {
                string sql = String.Format(@"select top 1 h.FinancialServiceKey, hhd.HOCHistoryDetailKey
                    from [2am].[dbo].[Hoc] h (nolock)
                    inner join
                            (select max(hochistoryKey) as hochistoryKey, FinancialServiceKey
                            from [2am].[dbo].[HocHistory] (nolock)
                            group by FinancialServiceKey) as mhh
                        on mhh.FinancialServiceKey = h.FinancialServiceKey
                    inner join
                            (select max(HocHistoryDetailKey) as HocHistoryDetailKey, hochistoryKey
                            from [2am].[dbo].[HocHistoryDetail] (nolock)
                            where [HocHistoryDetail].UpdateType = '{0}'
                            group by hochistoryKey) as mhhd
                        on mhhd.hochistoryKey = mhh.hochistoryKey
                    inner join [2am].[dbo].[HocHistoryDetail] hhd (nolock)
                        on mhhd.HocHistoryDetailKey = hhd.HocHistoryDetailKey", updateType);

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0];

                return null;
            }
        }

        private DataTable GetHocHistoryDetailUpdateTypes()
        {
            using (new SessionScope())
            {
                string sql = "select distinct [HocHistoryDetail].UpdateType from [HocHistoryDetail]";
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0];

                return null;
            }
        }

        [Test]
        public void CreateHOCTest()
        {
            IHOCRepository hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
            using (new TransactionScope(OnDispose.Rollback))
            {
                //---------------Set up test pack-------------------
                ILookupRepository lkRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                //---------------Execute Test ----------------------
                IHOC hoc = hocRepo.CreateHOC(GetTestApplicationKey());

                //---------------Test Result -----------------------
                Assert.IsNotNull(hoc);
            }
        }

        [Test]
        public void UpdateHOCPremiumTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                //---------------Set up test pack-------------------
                IHOCRepository hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
                ILookupRepository lkRepo = RepositoryFactory.GetRepository<ILookupRepository>();
                IHOC hoc = hocRepo.CreateHOC((GetTestApplicationKey()));

                //---------------Assert Precondition----------------
                Assert.IsNotNull(hoc);

                hoc.HOCSubsidence = lkRepo.HOCSubsidence[0];
                hoc.ChangeDate = DateTime.Now;
                hoc.HOCConventionalAmount = 777000.00;
                hoc.HOCShingleAmount = 555.00;
                hoc.HOCThatchAmount = 666.00;
                hoc.HOCInsurer = hocRepo.GetHOCInsurerByKey(2);
                hoc.HOCRoof = lkRepo.HOCRoof[0];
                hoc.HOCStatus = lkRepo.HOCStatus[0];
                hocRepo.SaveHOC(hoc);
                var key = hoc.Key;
                hocRepo.SaveHOC(hoc);
                Assert.Greater(hoc.Key, 0);

                //---------------Execute Test ----------------------
                hocRepo.UpdateHOCPremium(hoc.Key);
                hoc.Refresh();
                hocRepo.SaveHOC(hoc);
                hoc = hocRepo.GetHOCByKey(key);

                //---------------Test Result -----------------------
                Assert.Greater(hoc.HOCMonthlyPremium, 0);
                Assert.Greater(hoc.HOCProrataPremium, 0);
            }
        }

        [Test]
        public void CalculatePremiumTest()
        {
            using (new SessionScope())
            {
                //---------------Set up test pack-------------------
                ILookupRepository lkRepo = RepositoryFactory.GetRepository<ILookupRepository>();
                IHOCRepository hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
                IHOC hoc = hocRepo.CreateEmptyHOC();
                hoc.HOCSubsidence = lkRepo.HOCSubsidence[0];
                hoc.HOCConventionalAmount = 777000.00;
                hoc.HOCShingleAmount = 555.00;
                hoc.HOCThatchAmount = 666.00;
                hoc.HOCInsurer = hocRepo.GetHOCInsurerByKey(2);
                hoc.HOCRoof = lkRepo.HOCRoof[0];
                hoc.HOCStatus = lkRepo.HOCStatus[0];

                //---------------Assert Precondition----------------
                Assert.IsNotNull(hoc);

                //---------------Execute Test ----------------------
                hocRepo.CalculatePremium(hoc);

                //---------------Test Result -----------------------
                Assert.Greater(hoc.HOCMonthlyPremium, 0);
                Assert.Greater(hoc.HOCProrataPremium, 0);
                Assert.Greater(hoc.PremiumThatch, 0);
                Assert.AreEqual(hoc.PremiumShingle, 0D);
                Assert.Greater(hoc.PremiumConventional, 0);
            }
        }

        private int GetTestApplicationKey()
        {
            string sql = @"select top 1 ofr.OfferKey
                        from [2am]..Offer ofr (nolock)
                        join [2am]..OfferInformation oi (nolock)
	                        on ofr.offerKey = oi.offerKey
                        join [2am]..OfferInformationVariableLoan oivl (nolock)
	                        on oivl.OfferInformationKey = oi.OfferInformationKey
                        where oivl.SPVKey is not null and oi.ProductKey is not null";
            var result = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO),
                                                                    new ParameterCollection());
            Assert.IsNotNull(result);
            return Convert.ToInt32(result);
        }
    }
}