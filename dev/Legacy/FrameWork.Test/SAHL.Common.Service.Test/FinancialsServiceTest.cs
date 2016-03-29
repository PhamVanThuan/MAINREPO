using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Castle.ActiveRecord;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Test;

namespace SAHL.Common.Service.Test
{
    [TestFixture]
    public class FinancialsServiceTest : TestBase
    {
        [Test]
        public void GetCreditCriteriaByCategory()
        {
            //this is a data specific test that will work only for new business, because it uses category 5
            using (new SessionScope())
            {
                IDomainMessageCollection Messages = new DomainMessageCollection();
                FinancialsService service = new FinancialsService();

                string query = "SELECT TOP 1 CC.CreditCriteriaKey FROM [2AM].[dbo].[CreditCriteria] CC (NOLOCK) "
                   + "JOIN [2AM].[dbo].[CreditMatrix] CM (NOLOCK) ON CC.CreditMatrixKey = CM.CreditMatrixKey "
                   + "JOIN [2AM].[dbo].[Margin] M (NOLOCK) ON M.MarginKey = CC.MarginKey "
                   + "JOIN [2AM].[dbo].[OriginationSourceProductCreditMatrix] OSPCM (NOLOCK) ON CM.CreditMatrixKey = OSPCM.CreditMatrixKey "
                   + "JOIN [2AM].[dbo].[OriginationSourceProduct] OSP (NOLOCK) ON OSPCM.OriginationSourceProductKey = OSP.OriginationSourceProductKey "
                   + "WHERE CC.MortgageLoanPurposeKey = 3 AND CC.EmploymentTypeKey = 1 AND CC.MaxLoanAmount >= 200000 "
                   + "AND OSP.OriginationSourceKey = 1 AND OSP.ProductKey = 9 AND CM.NewBusinessIndicator = 'Y' "
                   + "AND CC.CategoryKey = 3 "
                   + "ORDER BY CC.MaxLoanAmount asc, M.Value asc, CC.LTV asc";
                DataTable DT = base.GetQueryResults(query);

                ICreditCriteria cc = service.GetCreditCriteriaByCategory(Messages, 1, 9, 3, 1, 200000, 1000000, 3);

                Assert.That(cc.Key == Convert.ToInt32(DT.Rows[0][0]));
            }
        }

        [Test]
        public void GetCreditCriteria()
        {
            using (new SessionScope())
            {
                IDomainMessageCollection Messages = new DomainMessageCollection();
                FinancialsService service = new FinancialsService();

                string query = "SELECT TOP 1 CC.CreditCriteriaKey FROM [2AM].[dbo].[CreditCriteria] CC (NOLOCK) "
                   + "JOIN [2AM].[dbo].[CreditMatrix] CM (NOLOCK) ON CC.CreditMatrixKey = CM.CreditMatrixKey "
                   + "JOIN [2AM].[dbo].[Margin] M (NOLOCK) ON M.MarginKey = CC.MarginKey "
                   + "JOIN [2AM].[dbo].[OriginationSourceProductCreditMatrix] OSPCM (NOLOCK) ON CM.CreditMatrixKey = OSPCM.CreditMatrixKey "
                   + "JOIN [2AM].[dbo].[OriginationSourceProduct] OSP (NOLOCK) ON OSPCM.OriginationSourceProductKey = OSP.OriginationSourceProductKey "
                   + "WHERE CC.MortgageLoanPurposeKey = 2 AND CC.EmploymentTypeKey = 1 AND CC.MaxLoanAmount >= 2000000 "
                   + "AND OSP.OriginationSourceKey = 1 AND OSP.ProductKey = 1 AND CM.NewBusinessIndicator = 'Y' AND CC.ExceptionCriteria = 0"
                   + "ORDER BY CC.MaxLoanAmount asc, M.Value asc, CC.LTV asc";
                DataTable DT = base.GetQueryResults(query);

                ICreditCriteria cc = service.GetCreditCriteria(Messages, 1, 1, 2, 1, 2000000);

                Assert.That(cc.Key == Convert.ToInt32(DT.Rows[0][0]));
            }
        }

        [Test]
        public void GetMaxLoanAmount()
        {
            using (new SessionScope())
            {
                IDomainMessageCollection Messages = new DomainMessageCollection();
                FinancialsService service = new FinancialsService();

                string query = "SELECT TOP 1 CC.MaxLoanAmount FROM [2AM].[dbo].[CreditCriteria] CC (NOLOCK) "
                   + "JOIN [2AM].[dbo].[CreditMatrix] CM (NOLOCK) ON CC.CreditMatrixKey = CM.CreditMatrixKey "
                   + "JOIN [2AM].[dbo].[OriginationSourceProductCreditMatrix] OSPCM (NOLOCK) ON CM.CreditMatrixKey = OSPCM.CreditMatrixKey "
                   + "JOIN [2AM].[dbo].[OriginationSourceProduct] OSP (NOLOCK) ON OSPCM.OriginationSourceProductKey = OSP.OriginationSourceProductKey "
                   + "WHERE CC.MortgageLoanPurposeKey = 2 AND CC.EmploymentTypeKey = 1 "
                   + "AND OSP.OriginationSourceKey = 1 AND OSP.ProductKey = 1 AND CM.NewBusinessIndicator = 'Y' AND CC.ExceptionCriteria = 0"
                   + "ORDER BY CC.MaxLoanAmount desc";
                DataTable DT = base.GetQueryResults(query);

                double amount = service.GetMaxLoanAmount(Messages, 1, 1, 2, 1);

                Assert.That(amount == Convert.ToDouble(DT.Rows[0][0]));
            }
        }

        [Test]
        public void GetMaxLoanAmountForFurtherLending()
        {
            using (new SessionScope())
            {
                IDomainMessageCollection Messages = new DomainMessageCollection();
                FinancialsService service = new FinancialsService();

                string query = "SELECT TOP 1 CC.LTV / 100 * 2000000 FROM [2AM].[dbo].[CreditCriteria] CC (NOLOCK) "
                   + "JOIN [2AM].[dbo].[CreditMatrix] CM (NOLOCK) ON CC.CreditMatrixKey = CM.CreditMatrixKey "
                   + "JOIN [2AM].[dbo].[OriginationSourceProductCreditMatrix] OSPCM (NOLOCK) ON CM.CreditMatrixKey = OSPCM.CreditMatrixKey "
                   + "JOIN [2AM].[dbo].[OriginationSourceProduct] OSP (NOLOCK) ON OSPCM.OriginationSourceProductKey = OSP.OriginationSourceProductKey "
                   + "WHERE CC.MortgageLoanPurposeKey = 2 AND CC.EmploymentTypeKey = 1 "
                   + "AND OSP.OriginationSourceKey = 1 AND OSP.ProductKey = 1 AND CM.NewBusinessIndicator = 'Y' AND CC.ExceptionCriteria = 0 "
                   + "ORDER BY CC.LTV desc";
                DataTable DT = base.GetQueryResults(query);

                double amount = service.GetMaxLoanAmountForFurtherLending(Messages, 1, 1, 2, 1, 2000000);

                Assert.That(amount == Convert.ToDouble(DT.Rows[0][0]));
            }
        }
    }
}