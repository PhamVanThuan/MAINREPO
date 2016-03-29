using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Queries;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using SAHL.Testing.Common.Helpers;
using SAHL.Testing.Services.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace SAHL.Testing.Services.Tests.CATSService
{
    public class when_processing_a_cats_payment_batch : ServiceTestBase<ICATSServiceClient>
    {
        private int catsPaymentBatchKey;
        private ThirdPartyInvoiceDataModel thirdPartyInvoice;

        [TearDown]
        public void OnTestTearDown()
        {
            if (this.thirdPartyInvoice != null)
            {
                var command = new RemoveEmptyThirdPartyInvoiceCommand(thirdPartyInvoice.ThirdPartyInvoiceKey);
                base.PerformCommand(command);
                thirdPartyInvoice = null;
            }
        }

        [SetUp]
        public void OnTestSetup()
        {
            var financeDomainServiceClient = base.GetInstance<IFinanceDomainServiceClient>();

            var getNewThirdPartyPaymentBatchReferenceQuery = new GetNewThirdPartyPaymentBatchReferenceQuery(CATSPaymentBatchType.ThirdPartyInvoice);
            Execute(getNewThirdPartyPaymentBatchReferenceQuery).WithoutErrors();
            catsPaymentBatchKey = getNewThirdPartyPaymentBatchReferenceQuery.Result.Results.First().BatchKey;

            var thirdParty = TestApiClient.GetAny<ThirdPartiesDataModel>(new { HasBankAccount = true, GenericKeyTypeKey = (int)GenericKeyType.Attorney });
            var emptyInvoice = TestApiClient.GetAny<EmptyThirdPartyInvoicesDataModel>();
            this.thirdPartyInvoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(emptyInvoice.ThirdPartyInvoiceKey);
            var account = TestApiClient.GetByKey<AccountDataModel>(this.thirdPartyInvoice.AccountKey);
            var spv = TestApiClient.GetByKey<SPVsDataModel>((int)account.SPVKey);
            List<InvoiceLineItemModel> lineItems = new List<InvoiceLineItemModel>()
            {
                new InvoiceLineItemModel(null, this.thirdPartyInvoice.ThirdPartyInvoiceKey, 1, 100.00M, true),
                new InvoiceLineItemModel(null, thirdPartyInvoice.ThirdPartyInvoiceKey, 2, 200.00M, false)
            };

            var invoice = new ThirdPartyInvoiceModel(this.thirdPartyInvoice.ThirdPartyInvoiceKey, thirdParty.Id, string.Concat("Test-InvNo-", Convert.ToBase64String(Guid.NewGuid().ToByteArray())),
                DateTime.Now, lineItems, true, "Test_Payment_Ref");

            var invoiceProcessor = TestApiClient.Get<HaloUsersDataModel>(new { ADUserName = "InvoiceProcessor" }).FirstOrDefault();
            _metaDataDictionary.Clear();
            _metaDataDictionary.Add(ServiceRequestMetadata.HEADER_USERORGANISATIONSTRUCTUREKEY, invoiceProcessor.UserOrganisationStructureKey.ToString());
            _metaDataDictionary.Add(ServiceRequestMetadata.HEADER_CURRENTUSERCAPABILITIES, invoiceProcessor.Capabilities);
            _metaDataDictionary.Add(ServiceRequestMetadata.HEADER_USERNAME, string.Concat(@"SAHL\", invoiceProcessor.ADUserName));

            var captureThirdPartyInvoiceCommand = new CaptureThirdPartyInvoiceCommand(invoice);
            financeDomainServiceClient.PerformCommand<CaptureThirdPartyInvoiceCommand>(captureThirdPartyInvoiceCommand, new ServiceRequestMetadata(_metaDataDictionary)).WithoutMessages();

            var invoiceApprover = TestApiClient.Get<HaloUsersDataModel>(new { ADUserName = "InvoiceApprover" }).FirstOrDefault();
            _metaDataDictionary.Clear();
            _metaDataDictionary.Add(ServiceRequestMetadata.HEADER_USERORGANISATIONSTRUCTUREKEY, invoiceApprover.UserOrganisationStructureKey.ToString());
            _metaDataDictionary.Add(ServiceRequestMetadata.HEADER_CURRENTUSERCAPABILITIES, invoiceApprover.Capabilities);
            _metaDataDictionary.Add(ServiceRequestMetadata.HEADER_USERNAME, string.Concat(@"SAHL\", invoiceApprover.ADUserName));

            var approveThirdPartyInvoiceCommand = new ApproveThirdPartyInvoiceCommand(this.thirdPartyInvoice.ThirdPartyInvoiceKey);
            financeDomainServiceClient.PerformCommand(approveThirdPartyInvoiceCommand, new ServiceRequestMetadata(_metaDataDictionary)).WithoutMessages();

            this.thirdPartyInvoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(thirdPartyInvoice.ThirdPartyInvoiceKey);
            var invoiceLineItems = TestApiClient.Get<InvoiceLineItemDataModel>(new { ThirdPartyInvoiceKey = this.thirdPartyInvoice.ThirdPartyInvoiceKey });

            var addThirdPartyInvoiceToPaymentBatchCommand = new AddThirdPartyInvoiceToPaymentBatchCommand(catsPaymentBatchKey, this.thirdPartyInvoice.ThirdPartyInvoiceKey);
            financeDomainServiceClient.PerformCommand(addThirdPartyInvoiceToPaymentBatchCommand, new ServiceRequestMetadata(_metaDataDictionary)).WithoutMessages();
        }

        [Test]
        public void given_a_cats_payment_batch_with_status_processing_it_should_process_the_batch()
        {
            var catsPaymentBatchTypeBefore = TestApiClient.GetByKey<CATSPaymentBatchTypeDataModel>((int)CATSPaymentBatchType.ThirdPartyInvoice);
            var controlBefore = TestApiClient.GetByKey<ControlDataModel>(114);
            string catsFileLocation = CatsAppConfigSettings.CATSOutputFileLocation;

            //because of
            var processCATSPaymentBatchCommand = new ProcessCATSPaymentBatchCommand(catsPaymentBatchKey);
            Execute(processCATSPaymentBatchCommand).WithoutErrors();

            //it should update the CATSPaymentBatchStatus
            var catsPaymentBatch = TestApiClient.GetByKey<CATSPaymentBatchDataModel>(catsPaymentBatchKey);
            Assert.AreEqual((int)CATSPaymentBatchStatus.Processed, catsPaymentBatch.CATSPaymentBatchStatusKey, "Expected the CATSPaymentBatchStatus of CATSPaymentBatch: {0} to be updated to {1}", catsPaymentBatch, CATSPaymentBatchStatus.Processed);
            
            //it should record the CATSFileSequenceNo of the CATSPaymentBatch
            Assert.AreEqual(controlBefore.ControlNumeric + 1, catsPaymentBatch.CATSFileSequenceNo, "Expected the CATSFileSequenceNo of CATSPaymentBatch: {0} to be set to {1}", catsPaymentBatch.CATSPaymentBatchKey, controlBefore.ControlNumeric + 1);

            //it should record the FileName of the CATSPaymentBatch
            StringAssert.StartsWith(catsPaymentBatchTypeBefore.CATSFileNamePrefix, catsPaymentBatch.CATSFileName, "Expected the FileName of CATSPaymentBatch: {0} to contain {1}", catsPaymentBatchKey, catsPaymentBatchTypeBefore.CATSFileNamePrefix);

            //it should timestamp the FileName
            long timestamp;
            bool isANumber = long.TryParse(catsPaymentBatch.CATSFileName.Substring(catsPaymentBatch.CATSFileName.Length - 14), out timestamp);
            Assert.IsTrue(isANumber, "Expected CATSFileName: {0} of CATSPaymentBatch: {1} to contain a timestamp", catsPaymentBatch.CATSFileName, catsPaymentBatchKey);
            long testStartTimeUpper = Convert.ToInt64(base.TestStartTime.ToString("yyyyMMddHHmmss")) + 60;
            long testStartTimeLower = Convert.ToInt64(base.TestStartTime.ToString("yyyyMMddHHmmss"));
            Assert.That<long>(ref timestamp, Is.GreaterThan(testStartTimeLower).And.LessThan(testStartTimeUpper), "Expected the timestamp of the CATSFileName: {0} for the CATSPaymentBatch: {1} to be within 60 seconds of TestStartTime: {2}", catsPaymentBatch.CATSFileName, catsPaymentBatchKey, base.TestStartTime);
                        
            //it should save a CATSPaymentFile
            string catsFullFileName = string.Concat(catsFileLocation, catsPaymentBatch.CATSFileName);
            var getFileUsingPathQuery = new GetFileUsingPathQuery(catsFullFileName);
            PerformQuery(getFileUsingPathQuery);
            Assert.AreEqual(1, getFileUsingPathQuery.Result.Results.Count(), string.Format("Expected a CATS file {0} to be saved at {1} for CATSPaymentBatchKey: {2}", catsPaymentBatch.CATSFileName, catsFileLocation, catsPaymentBatchKey));
            StringAssert.Contains((controlBefore.ControlNumeric + 1).ToString(), getFileUsingPathQuery.Result.Results.FirstOrDefault(), string.Format("Expected the {0} CATS File for CATSPaymentBatchKey: {1} to containt the SequenceNo {2}", catsFileLocation, catsPaymentBatchKey, controlBefore.ControlNumeric + 1));

            //it should increment the NextCATSFileSequenceNo
            var controlAfter = TestApiClient.GetByKey<ControlDataModel>(114);
            Assert.AreEqual(controlBefore.ControlNumeric + 1, controlAfter.ControlNumeric, "Expected the NextCatsFileSequenceNo to be incremented to {0}", controlBefore.ControlNumeric + 1);
                
        }
    }
}