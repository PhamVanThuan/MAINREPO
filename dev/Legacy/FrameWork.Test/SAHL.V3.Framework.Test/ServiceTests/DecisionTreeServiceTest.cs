using NUnit.Framework;
using SAHL.Services.Interfaces.DecisionTree.Queries;
using SAHL.V3.Framework.Model;
using SAHL.V3.Framework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Test.ServiceTests
{
    [TestFixture]
    public class DecisionTreeServiceTest
    {
        [Ignore("integration test")]
        [Test]
        public void TestDecisionTreeServiceCall()
        {
            IV3ServiceManager manager = V3ServiceManager.Instance;
            IDecisionTreeService service = manager.Get<IDecisionTreeService>();

            CapitecOriginationCreditPricing_Query treeQuery = new CapitecOriginationCreditPricing_Query(
            "NewPurchase",
            "81b73055-cfe4-4676-9b8e-a2d500ac88c8",
            "dbea5e1c-a711-48dc-9cb6-a2d500ab5a72",
            55000.00,
            955000,
            55000,
            -1, -1, -1,
            -1, -1,
            240,
            -999,
            -1,
            -999,
            -1,
            true,
            (double)5500,
            (double)0.1,
            false);

            var result = service.DecisionTreeServiceClient.PerformQuery(treeQuery);
            Console.WriteLine(result);
        }

        [Ignore("integration test")]
        [Test]
        public void TestDecisionTreeServiceCall_NCRAssessmentGuideline()
        {
            IV3ServiceManager manager = V3ServiceManager.Instance;
            IDecisionTreeService service = manager.Get<IDecisionTreeService>();
            DetermineNCRGuidelineMinMonthlyFixedExpensesQuery query = new DetermineNCRGuidelineMinMonthlyFixedExpensesQuery(23000);
            service.DetermineNCRGuidelineMinMonthlyFixedExpenses(query);
            Console.WriteLine(query.Result);
        }

    }
}
