using Castle.ActiveRecord;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Test;
using System;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class EstateAgentRepositoryTest : TestBase
    {
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
            // GC.Collect();
        }

        [Test]
        public void GetEstateAgentInfoWithHistory()
        {
            using (new SessionScope())
            {
                string query = @"SELECT TOP (1) ofr.LegalEntityKey, ofr.StatusChangeDate
                                FROM OfferRole ofr
                                WHERE ofr.OfferRoleTypeKey = 940
                                order by 1 desc";

                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                int key = Convert.ToInt32(DT.Rows[0][0]);
                DateTime dt = Convert.ToDateTime(DT.Rows[0][1]);

                IEstateAgentRepository repo = RepositoryFactory.GetRepository<IEstateAgentRepository>();

                ILegalEntity company;
                ILegalEntity branch;
                ILegalEntity principal;

                repo.GetEstateAgentInfoWithHistory(key, dt, out company, out branch, out principal);

                Assert.That(company != null || branch != null || principal != null);
            }
        }
    }
}