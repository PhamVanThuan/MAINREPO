using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Testing.Services.Tests.Tests.CATSService
{
    public class when_marking_cats_payment_batch_as_failed : ServiceTestBase<ICATSServiceClient>
    {
        [Test]
        public void given_a_cats_payment_batch_with_status_processing_it_should_updated_the_status_to_failed()
        {
            var catsPaymentBatchBefore = TestApiClient.Get<CATSPaymentBatchDataModel>(new { }).Where(x=>x.CATSPaymentBatchStatusKey != (int)CATSPaymentBatchStatus.Failed).FirstOrDefault();
            var markCATSPaymentBatchAsFailedCommand = new MarkCATSPaymentBatchAsFailedCommand(catsPaymentBatchBefore.CATSPaymentBatchKey);
            base.Execute(markCATSPaymentBatchAsFailedCommand).WithoutErrors();
            var catsPaymentBatchAfter = TestApiClient.GetByKey<CATSPaymentBatchDataModel>(catsPaymentBatchBefore.CATSPaymentBatchKey);
            Assert.AreEqual((int)CATSPaymentBatchStatus.Failed, catsPaymentBatchAfter.CATSPaymentBatchStatusKey, "Expected CATSPaymentBatchStatusKey for CATSPaymentBatch: {0} to be updated to Failed", catsPaymentBatchBefore.CATSPaymentBatchKey);
        }
    }
}
