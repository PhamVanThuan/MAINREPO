using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Queries;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;
using System.Linq;
using SAHL.Testing.Common.Helpers;
using System.Collections.Generic;
using SAHL.Core.Data.Models.EventStore;

namespace SAHL.Testing.Services.Tests.FinanceDomain
{
    public class when_removing_a_third_party_invoice_payment_batch_item : ServiceTestBase<IFinanceDomainServiceClient>
    {
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

        [Test]
        public void given_the_batch_key_and_third_party_invoice_key_it_should_remove_the_relevant_line_item()
        {
            //---------------Set up test pack-------------------
            var catsClient = base.GetInstance<ICATSServiceClient>();
            var getNewThirdPartyPaymentBatchReferenceQuery = new GetNewThirdPartyPaymentBatchReferenceQuery(CATSPaymentBatchType.ThirdPartyInvoice);
            catsClient.PerformQuery(getNewThirdPartyPaymentBatchReferenceQuery).WithoutMessages();
            int batchKey = getNewThirdPartyPaymentBatchReferenceQuery.Result.Results.First().BatchKey;

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

            var invoice = new ThirdPartyInvoiceModel(this.thirdPartyInvoice.ThirdPartyInvoiceKey, thirdParty.Id, string.Concat("SAHL-InvNo-", Convert.ToBase64String(Guid.NewGuid().ToByteArray())),
                DateTime.Now, lineItems, true, string.Concat("PayRef-", Convert.ToBase64String(Guid.NewGuid().ToByteArray())).Remove(30));

            SetHeaderMetadataForUser("InvoiceProcessor");

            var captureThirdPartyInvoiceCommand = new CaptureThirdPartyInvoiceCommand(invoice);
            base.Execute<CaptureThirdPartyInvoiceCommand>(captureThirdPartyInvoiceCommand).WithoutErrors();

            SetHeaderMetadataForUser("InvoiceApprover");

            var approveThirdPartyInvoiceCommand = new ApproveThirdPartyInvoiceCommand(this.thirdPartyInvoice.ThirdPartyInvoiceKey);
            base.Execute(approveThirdPartyInvoiceCommand).WithoutErrors();

            this.thirdPartyInvoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(thirdPartyInvoice.ThirdPartyInvoiceKey);
            var invoiceLineItems = TestApiClient.Get<InvoiceLineItemDataModel>(new { ThirdPartyInvoiceKey = this.thirdPartyInvoice.ThirdPartyInvoiceKey });

            SetHeaderMetadataForUser("InvoicePmtProcessor");

            var addThirdPartyInvoiceToPaymentBatchCommand = new AddThirdPartyInvoiceToPaymentBatchCommand(batchKey, this.thirdPartyInvoice.ThirdPartyInvoiceKey);
            Execute(addThirdPartyInvoiceToPaymentBatchCommand).WithoutErrors();

            //---------------Assert Precondition----------------
            var catsPaymentBatchItemsBefore = TestApiClient.Get<CATSPaymentBatchItemDataModel>(new { CATSPaymentBatchKey = batchKey, GenericKey = this.thirdPartyInvoice.ThirdPartyInvoiceKey, GenericTypeKey = (int)GenericKeyType.ThirdPartyInvoice});
            Assert.AreEqual(1, catsPaymentBatchItemsBefore.Count(), "Expected CATSPaymentBatchItem record to be created for CATSPaymentBatchKey: {0} GenericKey: {1} GenericTypeKey: {2}", batchKey, this.thirdPartyInvoice.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice);

            //---------------Execute Test ----------------------
            var command = new RemoveThirdPartyInvoiceFromPaymentBatchCommand(batchKey, this.thirdPartyInvoice.ThirdPartyInvoiceKey);
            Execute(command).WithoutErrors();
            var thirdPartyInvoiceRemoveFromBatchEvent = TestApiClient.Get<EventDataModel>(new { EventTypeKey = 24, GenericKey = command.ThirdPartyInvoiceKey, GenericKeyTypeKey = (int)GenericKeyType.ThirdPartyInvoice });
            Assert.IsNotNull(thirdPartyInvoiceRemoveFromBatchEvent, "No event was kicked off.");

            bool eventWasKickedOff = false;
            for (int i = 0; i < thirdPartyInvoiceRemoveFromBatchEvent.Count(); i++)
            {
                var singleEvent = thirdPartyInvoiceRemoveFromBatchEvent.ElementAt(i);
                if (singleEvent.EventTypeKey == 24 && singleEvent.GenericKey == command.ThirdPartyInvoiceKey && singleEvent.Data.Contains("ThirdPartyInvoiceRemovedFromBatchEvent"))
                {
                    eventWasKickedOff = true;
                    break;
                }
            }

            var catsPaymentBatchItemsAfter = TestApiClient.Get<CATSPaymentBatchItemDataModel>(new { CATSPaymentBatchKey = batchKey, GenericKey = this.thirdPartyInvoice.ThirdPartyInvoiceKey, GenericTypeKey = (int)GenericKeyType.ThirdPartyInvoice });
            Assert.False(catsPaymentBatchItemsAfter.FirstOrDefault().Processed.GetValueOrDefault(true), "Expected CATSPaymentBatchItem record to be deleted where CATSPaymentBatchKey: {0} GenericKey: {1} GenericTypeKey: {2}", batchKey, this.thirdPartyInvoice.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice);
            Assert.True(eventWasKickedOff, "ThirdPartyInvoiceRemovedFromBatchEvent event was not projected for the case.");
        }

    }
}
