using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.EventStore;
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
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SAHL.Testing.Services.Tests.CATSService
{
    public class when_notifying_cats_payment_batch_recipients : ServiceTestBase<ICATSServiceClient>
    {
        private int catsPaymentBatchKey;
        private Dictionary<int, ThirdPartyInvoiceDataModel> thirdPartyInvoices;

        [TearDown]
        public void OnTestTearDown()
        {
            if (thirdPartyInvoices != null && thirdPartyInvoices.Count() > 0)
            {
                foreach (var thirdPartyInvoice in thirdPartyInvoices)
                {
                    var command = new RemoveEmptyThirdPartyInvoiceCommand(thirdPartyInvoice.Value.ThirdPartyInvoiceKey);
                    PerformCommand(command);
                }
                thirdPartyInvoices = null;
            }
        }

        [SetUp]
        public void OnTestSetup()
        {
            var query = new WaitForMessagesToBeDeliveredQuery(1);
            PerformQuery(query);
            if (query.Result.Results.Count() > 0)
            {
                foreach (var message in query.Result.Results)
                {
                    var command = new ArchiveEmailCommand(message);
                    base.PerformCommand(command);
                }
            }
        }

        [Test]
        public void given_a_processed_payment_batch_it_should_notify_payment_batch_recipients()
        {
            //Establish context
            var testStartTime = DateTime.Now;

            var getNewThirdPartyPaymentBatchReferenceQuery = new GetNewThirdPartyPaymentBatchReferenceQuery(CATSPaymentBatchType.ThirdPartyInvoice);
            Execute(getNewThirdPartyPaymentBatchReferenceQuery).WithoutErrors();
            catsPaymentBatchKey = getNewThirdPartyPaymentBatchReferenceQuery.Result.Results.First().BatchKey;

            var financeDomainServiceClient = base.GetInstance<IFinanceDomainServiceClient>();
            var thirdParties = TestApiClient.Get<ThirdPartiesDataModel>(new { HasBankAccount = true, GenericKeyTypeKey = (int)GenericKeyType.Attorney }, 2);
            var thirdPartyInvoices = new Dictionary<int, ThirdPartyInvoiceDataModel>();

            foreach(var thirdParty in thirdParties)
            {
                var emptyInvoice = TestApiClient.GetAny<EmptyThirdPartyInvoicesDataModel>();
                var removeEmptyThirdPartyInvoiceCommand = new RemoveEmptyThirdPartyInvoiceCommand(emptyInvoice.ThirdPartyInvoiceKey);

                var thirdPartyInvoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(emptyInvoice.ThirdPartyInvoiceKey);
                base.PerformCommand(removeEmptyThirdPartyInvoiceCommand);
                thirdPartyInvoices.Add(thirdParty.LegalEntityKey, thirdPartyInvoice);

                var account = TestApiClient.GetByKey<AccountDataModel>(thirdPartyInvoice.AccountKey);
                var spv = TestApiClient.GetByKey<SPVsDataModel>((int)account.SPVKey);
                List<InvoiceLineItemModel> lineItems = new List<InvoiceLineItemModel>()
                {
                    new InvoiceLineItemModel(null, thirdPartyInvoice.ThirdPartyInvoiceKey, 1, 100.00M, true),
                    new InvoiceLineItemModel(null, thirdPartyInvoice.ThirdPartyInvoiceKey, 2, 200.00M, false)
                };

                var invoice = new ThirdPartyInvoiceModel(thirdPartyInvoice.ThirdPartyInvoiceKey, thirdParty.Id, string.Concat("Test-InvNo-", Convert.ToBase64String(Guid.NewGuid().ToByteArray())),
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

                var approveThirdPartyInvoiceCommand = new ApproveThirdPartyInvoiceCommand(thirdPartyInvoice.ThirdPartyInvoiceKey);
                financeDomainServiceClient.PerformCommand(approveThirdPartyInvoiceCommand, new ServiceRequestMetadata(_metaDataDictionary)).WithoutMessages();

                thirdPartyInvoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(thirdPartyInvoice.ThirdPartyInvoiceKey);
                var invoiceLineItems = TestApiClient.Get<InvoiceLineItemDataModel>(new { ThirdPartyInvoiceKey = thirdPartyInvoice.ThirdPartyInvoiceKey });

                var addThirdPartyInvoiceToPaymentBatchCommand = new AddThirdPartyInvoiceToPaymentBatchCommand(catsPaymentBatchKey, thirdPartyInvoice.ThirdPartyInvoiceKey);
                financeDomainServiceClient.PerformCommand(addThirdPartyInvoiceToPaymentBatchCommand, new ServiceRequestMetadata(_metaDataDictionary)).WithoutMessages();
            }

            var processCATSPaymentBatchCommand = new ProcessCATSPaymentBatchCommand(catsPaymentBatchKey);
            Execute(processCATSPaymentBatchCommand).WithoutErrors();

            //Because of
            var notifyCATSPaymentBatchRecipientsCommand = new NotifyCATSPaymentBatchRecipientsCommand(catsPaymentBatchKey);
            Execute(notifyCATSPaymentBatchRecipientsCommand).WithoutErrors();

            //It should raise SummarisedPaymentsToRecipientEvent per legal entity
            foreach(var thirdParty in thirdParties)
            {
                var summarisedPaymentsToRecipientEvents = TestApiClient.Get<EventDataModel>(new { GenericKey = thirdParty.LegalEntityKey, GenericKeyTypeKey = (int)GenericKeyType.LegalEntity });
                Assert.AreEqual(1, summarisedPaymentsToRecipientEvents.Where(x=>x.EventInsertDate > testStartTime && x.Data.Contains("SummarisedPaymentsToRecipient")).Count(), "Expected an event to be raised for Legal Entity: {0}", thirdParty.LegalEntityKey);
            }

            //It should raise CATSPaymentBatchRecipientsNotifiedEvent
            var catsPaymentBatchRecipientsNotifiedEvents = TestApiClient.Get<EventDataModel>(new { GenericKey = catsPaymentBatchKey, GenericKeyTypeKey = (int)GenericKeyType.CATSPaymentBatch });
            Assert.AreEqual(1, catsPaymentBatchRecipientsNotifiedEvents.Where(x => x.EventInsertDate > testStartTime && x.Data.Contains("CATSPaymentBatchRecipientsNotified")).Count(), "Expected an CATSPaymentBatchRecipientsNotified event to be raised for CATS Payment Batch: {0} with Event Start Date > {1}", catsPaymentBatchKey, testStartTime);
        }
    }
}