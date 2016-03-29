using NUnit.Framework;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Queries;

namespace SAHL.Testing.Services.Tests.FinanceDomain
{
    [TestFixture]
    public class when_retrieving_invoice_approvers_by_invoice_key : ServiceTestBase<IFinanceDomainServiceClient>
    {
        private static ThirdPartyInvoiceDataModel thirdPartyInvoice;

        [SetUp]
        public void OnTestSetup()
        {
            thirdPartyInvoice = TestApiClient.GetAny<ThirdPartyInvoiceDataModel>();
        }

        [TearDown]
        public void OnTestTearDown()
        {
            if (thirdPartyInvoice != null)
            {
                thirdPartyInvoice = null;
            }
        }

        [Test]
        public void when_successful()
        {
            var query = new GetMandatedUsersForThirdPartyInvoiceEscalationQuery(thirdPartyInvoice.ThirdPartyInvoiceKey);

            base.Execute(query).WithoutErrors();

            Assert.IsFalse(messages.HasErrors);
            Assert.IsNotNull(query.Result.Results);
        }
    }
}