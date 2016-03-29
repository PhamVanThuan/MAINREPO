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

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class MarketRateTest : TestBase
    {
        [Test]
        public void GetForCreditMatrixCalc()
        {
            using (new SessionScope())
            {
                string query = "Select MR.* from [2AM].[dbo].[OriginationSourceProduct] OSP (NOLOCK) "
                   + "join [2AM].[dbo].[OriginationSourceProductConfiguration] OSPC (NOLOCK) on OSPC.OriginationSourceProductKey = OSP.OriginationSourceProductKey "
                   + "join [2AM].[dbo].[MarketRate] MR (NOLOCK) on OSPC.MarketRateKey = MR.MarketRateKey "
                   + "where OSP.OriginationSourceKey = 1 "
                   + "and OSP.ProductKey = 1 "
                   + "and OSPC.FinancialServiceTypeKey = 1";
                DataTable DT = base.GetQueryResults(query);

                IMarketRate mr = MarketRate.GetForCreditMatrixCalc(1,1,1);

                
                Assert.That((int)DT.Rows[0][0] == mr.Key);
            }
        }

    }
}
