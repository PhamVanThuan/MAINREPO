using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Queries;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.Services.Tests.CATSService
{
    public class when_checking_if_cats_payment_batch_exists_for_today : ServiceTestBase<ICATSServiceClient>
    {
        private int catsPaymentBatchKey;
        private IEnumerable<CATSPaymentBatchDataModel> todaysCATSPaymentBatches;

        [TestFixtureSetUp]
        public void OnTestFixtureSetup()
        {
            DateTime testFixtureStartTime = DateTime.Now;
            //For all CATSPaymentBatches created today set CreatedDate to yesterday
            todaysCATSPaymentBatches = TestApiClient.Get<CATSPaymentBatchDataModel>(new { CATSPaymentBatchTypeKey = (int)CATSPaymentBatchType.ThirdPartyInvoice })
                .Where(x => x.CreatedDate > testFixtureStartTime.Subtract(testFixtureStartTime.TimeOfDay));
            foreach (var catsPaymentBatch in todaysCATSPaymentBatches)
            {
                var updateCATSPaymentBatch = new CATSPaymentBatchDataModel(
                    catsPaymentBatch.CATSPaymentBatchKey,
                    catsPaymentBatch.CATSPaymentBatchTypeKey,
                    testFixtureStartTime.AddDays(-1),
                    catsPaymentBatch.ProcessedDate,
                    catsPaymentBatch.CATSPaymentBatchStatusKey,
                    catsPaymentBatch.CATSFileSequenceNo,
                    catsPaymentBatch.CATSFileName);
                var updateCATSPaymentBatchCommand = new UpdateCATSPaymentBatchCommand(updateCATSPaymentBatch);
                PerformCommand(updateCATSPaymentBatchCommand);
            }
        }

        [TearDown]
        public void OnTestTearDown()
        {
            if (catsPaymentBatchKey != 0)
            {
                var removeCATSPaymentBatchCommand = new RemoveCATSPaymentBatchCommand(catsPaymentBatchKey);
                PerformCommand(removeCATSPaymentBatchCommand);
                catsPaymentBatchKey = 0;
            }
        }

        [TestFixtureTearDown]
        public void OnTestFixtureTearDown()
        {
            //For all CATSPaymentBatches created today set CreatedDate back to today
            if (todaysCATSPaymentBatches != null)
            {
                foreach (var catsPaymentBatch in todaysCATSPaymentBatches)
                {
                    var updateCATSPaymentBatchCommand = new UpdateCATSPaymentBatchCommand(catsPaymentBatch);
                    PerformCommand(updateCATSPaymentBatchCommand);
                }
            }
        }

        [Test]
        public void given_a_processed_batch_exists_today_it_should_return_true()
        {
            PerformCheckCATSPaymentBatchForTodayExistsQueryAndAssert(
                new CATSPaymentBatchDataModel((int)CATSPaymentBatchType.ThirdPartyInvoice, TestStartTime, TestStartTime, (int)CATSPaymentBatchStatus.Processed, 1, "FileDoesNotExist"),
                true);
        }

        [Test]
        public void given_a_processing_batch_exists_today_it_should_return_true()
        {
            PerformCheckCATSPaymentBatchForTodayExistsQueryAndAssert(
                new CATSPaymentBatchDataModel((int)CATSPaymentBatchType.ThirdPartyInvoice, TestStartTime, null, (int)CATSPaymentBatchStatus.Processing, null, null),
                true);
        }

        [Test]
        public void given_a_failed_batch_exists_today_it_should_return_false()
        {
            PerformCheckCATSPaymentBatchForTodayExistsQueryAndAssert(
                new CATSPaymentBatchDataModel((int)CATSPaymentBatchType.ThirdPartyInvoice, TestStartTime, null, (int)CATSPaymentBatchStatus.Failed, null, null),
                false);
        }

        [Test]
        public void given_no_batch_exists_today_it_should_return_false()
        {
            PerformCheckCATSPaymentBatchForTodayExistsQueryAndAssert(
                null,
                false);
        }

        private void PerformCheckCATSPaymentBatchForTodayExistsQueryAndAssert(CATSPaymentBatchDataModel insertCATSPaymentBatch, bool expectedResult)
        {
            if (insertCATSPaymentBatch != null)
            {
                var insertCATSPaymentBatchCommand = new InsertCATSPaymentBatchCommand(insertCATSPaymentBatch, base.linkedGuid);
                PerformCommand(insertCATSPaymentBatchCommand);
                catsPaymentBatchKey = linkedKeyManager.RetrieveLinkedKey(base.linkedGuid);
            }
            var doesCatsPaymentBatchForTodayExistQuery = new DoesCatsPaymentBatchForTodayExistQuery();
            Execute(doesCatsPaymentBatchForTodayExistQuery).WithoutErrors();
            Assert.AreEqual(expectedResult, doesCatsPaymentBatchForTodayExistQuery.Result.Results.FirstOrDefault().BatchExists);
        }
    }
}