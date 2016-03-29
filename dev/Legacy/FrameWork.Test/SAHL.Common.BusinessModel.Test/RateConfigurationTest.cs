using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SAHL.Test;
using SAHL.Common;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using System.Configuration;
using SAHL.Common.BusinessModel.Helpers;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class RateConfigurationTest : TestBase
    {
        [Test]
        public void GetHOCAccount()
        {
            using (new SessionScope())
            {
            //SELECT TOP 1 * FROM [2AM].[dbo].[RateConfiguration] rc (nolock)
            //JOIN [2AM].[dbo].[Margin] m (nolock) ON m.MarginKey = rc.MarginKey
            //JOIN [2AM].[dbo].[MarketRate] mr (nolock) on mr.MarketRateKey = rc.MarketRateKey
                string query = "SELECT TOP 1 * FROM [2AM].[dbo].[RateConfiguration] rc (nolock) "
                    + "JOIN [2AM].[dbo].[Margin] m (nolock) ON m.MarginKey = rc.MarginKey "
                    + "JOIN [2AM].[dbo].[MarketRate] mr (nolock) on mr.MarketRateKey = rc.MarketRateKey";
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                int MarketRateKey = Convert.ToInt32(DT.Rows[0][0]);
                int MarginKey = Convert.ToInt32(DT.Rows[0][1]);
                int RateConfigurationKey = Convert.ToInt32(DT.Rows[0][2]);

                IRateConfiguration rc = RateConfiguration.GetByMarginKeyAndMarketRateKey(MarginKey, MarketRateKey);

                Assert.That(rc != null);
                Assert.That(rc.Key == RateConfigurationKey);
            }
        }

    }
}
