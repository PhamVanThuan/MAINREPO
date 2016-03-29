using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.CATSService
{
    [TestFixture]
    public class when_retrieving_third_party_payment_batch_reff : ServiceTestBase<ICATSServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var catsPaymentBatches = TestApiClient.Get<CATSPaymentBatchDataModel>(new { CATSPaymentBatchTypeKey = (int)CATSPaymentBatchType.ThirdPartyInvoice });
            int maxCATSPaymentBatchKey = catsPaymentBatches.OrderByDescending(x => x.CATSPaymentBatchKey).FirstOrDefault().CATSPaymentBatchKey;
            var query = new GetNewThirdPartyPaymentBatchReferenceQuery(CATSPaymentBatchType.ThirdPartyInvoice);
            base.Execute<GetNewThirdPartyPaymentBatchReferenceQuery>(query).WithoutErrors();
            Assert.That(query.Result.Results.First().BatchKey > maxCATSPaymentBatchKey, string.Format("Expected a BatchKey greater than {0} but got {1}", maxCATSPaymentBatchKey, query.Result.Results.First().BatchKey));
        }
    }
}
