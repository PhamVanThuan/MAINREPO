using Castle.ActiveRecord;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;
using System;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class CreditMatrixRepositoryTest : TestBase
    {
        private static ICreditMatrixRepository _cmRepo = RepositoryFactory.GetRepository<ICreditMatrixRepository>();

        [SetUp]
        public void Setup()
        {
            // set the strategy to default so we actually go to the database
            //SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            _mockery = new MockRepository();
        }

        [TearDown]
        public void TearDown()
        {
            MockCache.Flush();
        }

        [Test]
        public void GetMarginProductByRateConfigAndOSP()
        {
            using (new SessionScope())
            {
                string query = "select mp.* from [2AM]..OriginationSourceProductConfiguration OSPC with (nolock) "
                + "inner join [2AM]..marketrate mr with (nolock) on OSPC.MarketRateKey = mr.MarketRateKey "
                + "inner join [2AM]..marginproduct mp with (nolock) on OSPC.OriginationSourceProductKey = mp.OriginationSourceProductKey "
                + "inner join [2AM]..margin m with (nolock) on mp.MarginKey = m.MarginKey "
                + "inner join [2AM]..RateConfiguration rc with (nolock) on OSPC.MarketRateKey = rc.MarketRateKey "
                + "and mp.MarginKey = rc.MarginKey "
                + "inner join [2AM]..OriginationSourceProduct osp on OSPC.OriginationSourceProductKey = osp.OriginationSourceProductKey "
                + "where osp.ProductKey = 5 and osp.OriginationSourceKey = 1 and rc.RateConfigurationKey = 1";

                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);
                int mpKey = Convert.ToInt32(DT.Rows[0][0]);

                IMarginProduct mp = _cmRepo.GetMarginProductByRateConfigAndOSP(1, 1, 5);

                Assert.That(mp != null);
                Assert.That(mpKey == mp.Key);
            }
        }

        [Test]
        public void GetRateConfigurationByMarginKeyAndMarketRateKeyTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 m.MarginKey,mr.MarketRateKey
				from [2am].[dbo].RateConfiguration rc (nolock)
				join [2am].[dbo].Margin m (nolock)
					on rc.MarginKey = m.MarginKey
				join [2am].[dbo].MarketRate mr (nolock)
					on mr.MarketRateKey = rc.MarketRateKey";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int marginKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    int marketRateKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                    IRateConfiguration rc = _cmRepo.GetRateConfigurationByMarginKeyAndMarketRateKey(marginKey, marketRateKey);
                    Assert.IsNotNull(rc);
                }
            }
        }

        [Test]
        public void GetCategoryByKeyTest()
        {
            using (new SessionScope())
            {
                Category_DAO cat = Category_DAO.FindFirst();
                ICategory category = _cmRepo.GetCategoryByKey(cat.Key);
                Assert.IsNotNull(category);
                Assert.AreEqual(cat.Key, category.Key);
            }
        }

        [Test]
        public void GetCreditCriteriaByOSPTest()
        {
            using (new SessionScope())
            {
                DataSet ds = _cmRepo.GetCreditCriteriaByOSP((int)OriginationSources.SAHomeLoans, (int)Products.NewVariableLoan);

                Assert.IsNotNull(ds);
            }
        }

        [Test]
        public void GetCreditMatrixByKeyTest()
        {
            using (new SessionScope())
            {
                CreditMatrix_DAO cm = CreditMatrix_DAO.FindFirst();
                ICreditMatrix creditMatrix = _cmRepo.GetCreditMatrixByKey(cm.Key);
                Assert.IsNotNull(creditMatrix);
            }
        }

        [Test]
        public void GetSAHomeLoansCreditMatrix()
        {
            using (new SessionScope())
            {
                ICreditMatrix creditMatrix = _cmRepo.GetCreditMatrix(OriginationSources.SAHomeLoans);
                Assert.IsNotNull(creditMatrix);
            }
        }
    }
}