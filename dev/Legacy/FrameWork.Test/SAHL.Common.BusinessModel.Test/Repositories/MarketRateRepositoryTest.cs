using NUnit.Framework;
using SAHL.Test;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
	[TestFixture]
	public class MarketRateRepositoryTest : TestBase
	{
		[Test]
		public void GetMarketRates_FromRepository()
		{
			IMarketRateRepository repo = RepositoryFactory.GetRepository<IMarketRateRepository>();
			IReadOnlyEventList<IMarketRate> marketRates = repo.GetMarketRates();
			Assert.IsTrue(marketRates.Count > 0);
		}

		[Test]
		public void GetMarketRateByKey_FromRepository()
		{
			int marketRateKey = MarketRate_DAO.FindFirst().Key;

			IMarketRateRepository repo = RepositoryFactory.GetRepository<IMarketRateRepository>();
			IMarketRate marketRate = repo.GetMarketRateByKey(marketRateKey);
			Assert.IsTrue(marketRate != null);
		}

		[Test]
		public void UpdateMarketRate_FromRepository()
		{
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                int marketRateKey = 1;
			    IMarketRateRepository repo = RepositoryFactory.GetRepository<IMarketRateRepository>();
			    IMarketRate marketRate = repo.GetMarketRateByKey(marketRateKey);
    		    const double oldval = 0.2;
			    marketRate.Value = 0.1;
			    repo.UpdateMarketRate(marketRate, oldval, "test");
                IMarketRate newMarketRate = repo.GetMarketRateByKey(marketRateKey);
                Assert.That(newMarketRate.Value == marketRate.Value);
            }     
		}
	}
}