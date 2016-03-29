using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class MarketingSourceRepositoryTest : TestBase
    {
        [Test]
        public void GetMarketingSourceByKey()
        {
            using (new SessionScope())
            {
                string sqlQuery = "Select Top 1 OfferSourceKey " +
                                  "From OfferSource";
                ParameterCollection parameters = new ParameterCollection();

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);

                if (obj != null)
                {
                    int iOfferSourceKey = Convert.ToInt32(obj);
                    IMarketingSourceRepository mRepo = RepositoryFactory.GetRepository<IMarketingSourceRepository>();

                    IApplicationSource appSource = mRepo.GetMarketingSourceByKey(iOfferSourceKey);
                }
                else
                {
                    Assert.Fail("No valid keys for this test");
                }
            }
        }

        [Test]
        public void GetMarketingSources()
        {
            IMarketingSourceRepository mRepo = RepositoryFactory.GetRepository<IMarketingSourceRepository>();
            IReadOnlyEventList<IApplicationSource> lstMarketingSources = mRepo.GetMarketingSources();
            Assert.IsTrue(lstMarketingSources.Count > 0);
        }

        [Test]
        public void SaveApplicationSource()
        {
            using (new SessionScope())
            {
                string sqlQuery = "Select Top 1 OfferSourceKey " +
                                  "From OfferSource";
                ParameterCollection parameters = new ParameterCollection();

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);

                if (obj != null)
                {
                    int iOfferSourceKey = Convert.ToInt32(obj);
                    IMarketingSourceRepository mRepo = RepositoryFactory.GetRepository<IMarketingSourceRepository>();

                    IApplicationSource appSource = mRepo.GetMarketingSourceByKey(iOfferSourceKey);
                    mRepo.SaveApplicationSource(appSource);
                }
                else
                {
                    Assert.Fail("No valid keys for this test");
                }
            }
        }

        [Test]
        public void ApplicationSourceExists()
        {
            using (new SessionScope())
            {
                string sqlQuery = "Select Top 1 Description " +
                                  "From OfferSource";
                ParameterCollection parameters = new ParameterCollection();

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);

                if (obj != null)
                {
                    string strDescription = Convert.ToString(obj);
                    IMarketingSourceRepository mRepo = RepositoryFactory.GetRepository<IMarketingSourceRepository>();

                    bool bExists = mRepo.ApplicationSourceExists(strDescription);
                    Assert.IsTrue(bExists);
                }
                else
                {
                    Assert.Fail("No valid descriptions for this test");
                }
            }
        }

        [Test]
        public void GetEmptyApplicationSource()
        {
            IMarketingSourceRepository mRepo = RepositoryFactory.GetRepository<IMarketingSourceRepository>();
            IApplicationSource appSource = mRepo.GetEmptyApplicationSource();
            Assert.IsNotNull(appSource);
        }
    }
}