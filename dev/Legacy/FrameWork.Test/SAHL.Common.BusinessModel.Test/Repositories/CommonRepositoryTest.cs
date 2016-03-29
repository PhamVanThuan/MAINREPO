using Castle.ActiveRecord;
using NHibernate;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class CommonRepositoryTest : TestBase
    {
        [Test]
        public void ExecuteQueryOnCastleTran()
        {
            using (new SessionScope())
            {
                string query = "select top 1 AccountKey from [2AM]..Account";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);
                int accountKey = Convert.ToInt32(DT.Rows[0][0]);

                string sql = UIStatementRepository.GetStatement("COMMON", "GetAccountGivenAccountkey");
                ParameterCollection prms = new ParameterCollection();
                prms.Add(new System.Data.SqlClient.SqlParameter("@AccountKey", accountKey));
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), prms);
                int TKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                Assert.AreEqual(accountKey, TKey);
            }
        }

        [Test]
        public void CalendarBusinessDays()
        {
            int days = Calendar.BusinessDayDiff(new DateTime(2008, 3, 20), new DateTime(2008, 3, 25));

            Assert.That(days == 2);
        }

        [Test]
        public void GetOriginationSourceProductByKey()
        {
            using (new SessionScope())
            {
                string query = "select top 1 * from [2am].[dbo].[OriginationSourceProduct] (nolock) ";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                IApplicationRepository repo = new ApplicationRepository();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                IOriginationSourceProduct osp = repo.GetOriginationSourceProductByKey(Convert.ToInt32(DT.Rows[0][0]));

                Assert.That(osp != null);
            }
        }

        [Test]
        public void GetOriginationSource()
        {
            using (new SessionScope())
            {
                string query = "select top 1 OriginationSourceKey from [2am].[dbo].[OriginationSource] (nolock) Order by OriginationSourceKey desc ";
                DataTable dataTable = base.GetQueryResults(query);
                Assert.That(dataTable.Rows.Count == 1);
                IApplicationRepository applicationRepository = new ApplicationRepository();
                IOriginationSource ios = applicationRepository.GetOriginationSource((OriginationSources)Convert.ToInt32(dataTable.Rows[0][0]));
                Assert.That(ios != null);
            }
        }

        [Test]
        public void GetOriginationSourceFail()
        {
            using (new SessionScope())
            {
                string query = "select top 1 OriginationSourceKey from [2am].[dbo].[OriginationSource] (nolock) Order by OriginationSourceKey desc ";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                IApplicationRepository repo = new ApplicationRepository();
                IDomainMessageCollection Messages = new DomainMessageCollection();
                OriginationSources os = new OriginationSources();
                int key = Convert.ToInt32(DT.Rows[0][0]) + 1;
                os = (OriginationSources)key;

                try
                {
                    IOriginationSource ios = repo.GetOriginationSource(os);
                    Assert.Fail("Non existent key has returned an object");
                }
                catch
                {
                    Assert.Greater(key, 0, "Unable to create object from non existent key");
                }
            }
        }

        [Test]
        public void GetOriginationSourceProductByKeyFail()
        {
            using (new SessionScope())
            {
                string query = "select max(OriginationSourceProductKey) from [2am].[dbo].[OriginationSourceProduct] (nolock) ";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);
                int nonProductKey = Convert.ToInt32(DT.Rows[0][0]) + 1;

                IApplicationRepository repo = new ApplicationRepository();
                IDomainMessageCollection Messages = new DomainMessageCollection();
                try
                {
                    IOriginationSourceProduct osp = repo.GetOriginationSourceProductByKey(nonProductKey);
                    Assert.Fail("Non existent Product returned");
                }
                catch
                {
                    Assert.Greater(nonProductKey, 0);
                }
            }
        }

        [Test]
        public void GetOriginationSourceProductBySourceAndProduct()
        {
            using (new SessionScope())
            {
                string query = "select top 1 * from [2am].[dbo].[OriginationSourceProduct] (nolock) ";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                IApplicationRepository repo = new ApplicationRepository();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                int originationSourceKey = Convert.ToInt32(DT.Rows[0][1]);
                int productKey = Convert.ToInt32(DT.Rows[0][2]);
                IOriginationSourceProduct osp = repo.GetOriginationSourceProductBySourceAndProduct(originationSourceKey, productKey);

                Assert.That(osp != null);
                Assert.That(osp.OriginationSource.Key == originationSourceKey);
                Assert.That(osp.Product.Key == productKey);
            }
        }

        [Test]
        public void GetOriginationProducts()
        {
            using (new SessionScope())
            {
                string query = "select * from [2am].[dbo].[Product] (nolock) "
                + "where OriginateYN = 'Y'";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count > 0);

                IApplicationRepository repo = new ApplicationRepository();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                ReadOnlyEventList<IProduct> list = repo.GetOriginationProducts();

                Assert.That(list.Count == DT.Rows.Count);
            }
        }

        [Test]
        public void GetOriginationProductsFail()
        {
            using (new SessionScope())
            {
                string query = "select ProductKey from [2am].[dbo].[Product] (nolock) "
                + "where OriginateYN = 'N'";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count > 0);

                IApplicationRepository repo = new ApplicationRepository();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                ReadOnlyEventList<IProduct> list = repo.GetOriginationProducts();
                int nonProductKey = 0;

                // Loop through list looking for Non Origination products
                foreach (DataRow dr in DT.Rows)
                {
                    foreach (DataColumn column in DT.Columns)
                    {
                        nonProductKey = Convert.ToInt32(dr[column]);

                        foreach (IProduct prod in list)
                        {
                            if (prod.Key == nonProductKey)
                            {
                                Assert.Fail("Non Origination Product found");
                            }
                        }
                    }
                }
            }
        }

        [Test]
        public void IsBusinessDay()
        {
            bool retVal = true;
            ICommonRepository repo = new CommonRepository();

            DateTime testDate = DateTime.Now;

            //test a saturday
            testDate = testDate.AddDays(6 - (int)testDate.DayOfWeek);
            retVal = repo.IsBusinessDay(testDate);

            if (retVal == true)
            {
                Assert.Fail("Saturday is not a business day");
                return;
            }

            //test a sunday
            testDate = testDate.AddDays(1);
            retVal = repo.IsBusinessDay(testDate);

            if (retVal == true)
            {
                Assert.Fail("Sunday is not a business day");
                return;
            }
        }

        [Test]
        public void GetnWorkingDaysFromTodayTest()
        {
            DateTime retDate = DateTime.Now;
            ICommonRepository repo = new CommonRepository();
            try
            {
                retDate = repo.GetnWorkingDaysFromToday(10);
                Assert.AreNotEqual(DateTime.Now.AddDays(10), retDate);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message.ToString());
            }
        }

        [Test]
        public void IsBusinessDayFail()
        {
            bool retVal = true;
            DateTime testDate = DateTime.Now;
            ICommonRepository repo = new CommonRepository();
            //test a saturday
            testDate = testDate.AddDays(6 - (int)testDate.DayOfWeek);
            retVal = repo.IsBusinessDay(testDate);

            if (retVal == false)
            {
                Assert.AreEqual(retVal, false, "Saturday is not a working day");
            }

            //test a sunday
            testDate = testDate.AddDays(1);
            retVal = repo.IsBusinessDay(testDate);

            if (retVal == false)
            {
                Assert.AreEqual(retVal, false, "Sunday is not a business day");
            }

            //test a Xmas
            testDate = DateTime.Parse("25/Dec");
            retVal = repo.IsBusinessDay(testDate);

            if (retVal == false)
            {
                Assert.AreEqual(retVal, false, "X-mas is not a business day");
            }
        }

        [Test]
        public void GetMortgageLoanPurposes()
        {
            using (new SessionScope())
            {
                string query = "select * from [2am].[dbo].[MortgageLoanPurpose] (nolock) "
                + "where MortgageLoanPurposeKey in (2,3,4)";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 3);

                IApplicationRepository repo = new ApplicationRepository();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                ReadOnlyEventList<IMortgageLoanPurpose> list = repo.GetMortgageLoanPurposes(new int[] { 2, 3, 4 });

                Assert.That(list.Count == 3);
            }
        }

        [Test]
        public void GetControlByDescription()
        {
            using (new SessionScope())
            {
                IControlRepository CR = new ControlRepository();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                Control_DAO dao = Control_DAO.FindFirst();

                IControl control = CR.GetControlByDescription(dao.ControlDescription);
                Assert.Greater(control.Key, 0);
            }
        }

        [Test]
        public void ValuationHOC()
        {
            using (new SessionScope())
            {
                IHOCRepository repo = RepositoryFactory.GetRepository<IHOCRepository>();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                string query = "SELECT TOP 1 * FROM [2AM].[dbo].[HOC] (nolock) "
                    + "WHERE HOCInsurerKey <> 2 AND CancellationDate is null ";
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                int fsKey = Convert.ToInt32(DT.Rows[0][0]);

                IHOC hoc = repo.GetHOCByKey(fsKey);

                hoc.ChangeDate = DateTime.Now;
                repo.UpdateHOCWithHistory(Messages, hoc.HOCInsurer.Key, hoc, 'V');
            }
        }

        /// <summary>
        /// Tests that the GetNHibernateSession returns a valid NHibernate session.
        /// </summary>
        [Test]
        public void GetNHibernateSession()
        {
            using (new SessionScope())
            {
                ICommonRepository commRep = RepositoryFactory.GetRepository<ICommonRepository>();

                Application_DAO dao = Application_DAO.FindFirst();
                IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IApplication app = bmtm.GetMappedType<IApplication>(dao);

                // test using an object
                ISession session = commRep.GetNHibernateSession(app);
                Assert.IsNotNull(session);

                // test using a type
                ISession session2 = commRep.GetNHibernateSession(typeof(Employment_DAO));
                Assert.IsNotNull(session2);
            }
        }

        [Test]
        public void GetGetLanguageByKeyTest()
        {
            using (new SessionScope())
            {
                ICommonRepository commRep = RepositoryFactory.GetRepository<ICommonRepository>();
                Language_DAO dao = Language_DAO.FindFirst();
                ILanguage lang = commRep.GetLanguageByKey(dao.Key);

                if (lang == null)
                    Assert.Fail();
            }
        }

        [Test]
        public void GetLinkRatesByOriginationSource()
        {
            using (new SessionScope())
            {
                ICommonRepository commRep = RepositoryFactory.GetRepository<ICommonRepository>();

                int originationSourceKey = 1;
                Dictionary<int, string> linkRates = commRep.GetLinkRatesByOriginationSource(originationSourceKey);

                Assert.That(linkRates.Count >= 0);
            }
        }

        [Test]
        public void GetLinkRatesByAccountKey()
        {
            using (new SessionScope())
            {
                ICommonRepository commRep = RepositoryFactory.GetRepository<ICommonRepository>();

                Account_DAO dao = Account_DAO.FindFirst();
                Dictionary<int, string> linkRates = commRep.GetLinkRatesByAccountKey(dao.Key);

                Assert.That(linkRates.Count >= 0);
            }
        }

        [Test]
        public void GetDefaultDebitOrderProviderKeyPass()
        {
            using (new SessionScope())
            {
                string query = "select top 1 ProviderKey from [2am].deb.Provider where IsDefault = 1 and GeneralStatusKey = 1";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);
                int expectedProviderKey = Convert.ToInt32(DT.Rows[0][0]);

                ICommonRepository repo = RepositoryFactory.GetRepository<ICommonRepository>();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                int providerKey = repo.GetDefaultDebitOrderProviderKey();

                Assert.AreEqual(expectedProviderKey, providerKey);

            }
        }
    }
}