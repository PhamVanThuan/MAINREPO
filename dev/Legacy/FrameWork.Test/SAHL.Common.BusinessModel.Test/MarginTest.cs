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
    public class MarginTest : TestBase
    {
        [Test]
        public void GetForCreditMatrixCalc()
        {
            using (new SessionScope())
            {
                string query = "SELECT DISTINCT M.* FROM [2AM].[dbo].[CreditCriteria] CC (NOLOCK) "
                   + "JOIN [2AM].[dbo].[CreditMatrix] CM (NOLOCK) ON CC.CreditMatrixKey = CM.CreditMatrixKey "
                   + "JOIN [2AM].[dbo].[OriginationSourceProductCreditMatrix] OSPCM (NOLOCK) ON CM.CreditMatrixKey = OSPCM.CreditMatrixKey "
                   + "JOIN [2AM].[dbo].[OriginationSourceProduct] OSP (NOLOCK) ON OSPCM.OriginationSourceProductKey = OSP.OriginationSourceProductKey "
                   + "JOIN [2AM].[dbo].[Margin] M (NOLOCK) ON CC.MarginKey = M.MarginKey "
                   + "WHERE CC.MortgageLoanPurposeKey = 2 AND CC.EmploymentTypeKey = 1 "
                   + "AND OSP.OriginationSourceKey = 1 AND OSP.ProductKey = 1";
                DataTable DT = base.GetQueryResults(query);

                IReadOnlyEventList<IMargin> list = Margin.GetForCreditMatrixCalc(new DomainMessageCollection(), 2, 1, 1, 1, 300000, 10);

                Assert.That(DT.Rows.Count == list.Count);
            }
        }

        [Test]
        public void GetForCreditMatrixCalcByLTV()
        {
            using (new SessionScope())
            {
                string query = "SELECT DISTINCT M.* FROM [2AM].[dbo].[CreditCriteria] CC (NOLOCK) "
                   + "JOIN [2AM].[dbo].[CreditMatrix] CM (NOLOCK) ON CC.CreditMatrixKey = CM.CreditMatrixKey "
                   + "JOIN [2AM].[dbo].[OriginationSourceProductCreditMatrix] OSPCM (NOLOCK) ON CM.CreditMatrixKey = OSPCM.CreditMatrixKey "
                   + "JOIN [2AM].[dbo].[OriginationSourceProduct] OSP (NOLOCK) ON OSPCM.OriginationSourceProductKey = OSP.OriginationSourceProductKey "
                   + "JOIN [2AM].[dbo].[Margin] M (NOLOCK) ON CC.MarginKey = M.MarginKey "
                   + "WHERE CC.MortgageLoanPurposeKey = 2 AND CC.EmploymentTypeKey = 1 AND CC.MaxLoanAmount >= 300000 "
                   + "AND CC.LTV >= 10 AND OSP.OriginationSourceKey = 1 AND OSP.ProductKey = 1";
                DataTable DT = base.GetQueryResults(query);

                IReadOnlyEventList<IMargin> list = Margin.GetForCreditMatrixCalcByLTV(new DomainMessageCollection(), 2, 1, 1, 1, 300000, 10);

                Assert.That(DT.Rows.Count == list.Count);
            }
        }

    }
}