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
using SAHL.Testing.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.Services.Tests.FinanceDomain
{
    public class when_adding_a_third_party_invoice_to_a_payment_batch : ServiceTestBase<IFinanceDomainServiceClient>
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
        public void ExecuteAddThirdPartyInvoicePaymentBatchItemCommand_GivenThirdPartyInvoicePaymentBatchItemModel_ShouldReturnNoErrorMessages()
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

            var invoice = new ThirdPartyInvoiceModel(this.thirdPartyInvoice.ThirdPartyInvoiceKey, thirdParty.Id, string.Format("SAHL-AutomatedTest-{0}", base.randomizer.Next()),
                DateTime.Now, lineItems, true, "SA HOME LOANS");

            SetHeaderMetadataForUser("InvoiceProcessor");

            var captureThirdPartyInvoiceCommand = new CaptureThirdPartyInvoiceCommand(invoice);
            base.Execute<CaptureThirdPartyInvoiceCommand>(captureThirdPartyInvoiceCommand).WithoutErrors();

            SetHeaderMetadataForUser("InvoiceApprover");

            var approveThirdPartyInvoiceCommand = new ApproveThirdPartyInvoiceCommand(this.thirdPartyInvoice.ThirdPartyInvoiceKey);
            base.Execute(approveThirdPartyInvoiceCommand).WithoutErrors();

            this.thirdPartyInvoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(thirdPartyInvoice.ThirdPartyInvoiceKey);
            var invoiceLineItems = TestApiClient.Get<InvoiceLineItemDataModel>(new { ThirdPartyInvoiceKey = this.thirdPartyInvoice.ThirdPartyInvoiceKey });

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var command = new AddThirdPartyInvoiceToPaymentBatchCommand(batchKey, this.thirdPartyInvoice.ThirdPartyInvoiceKey);
            Execute(command).WithoutErrors();

            //---------------Test Result -----------------------
            var catsPaymentBatchItem = TestApiClient.Get<CATSPaymentBatchItemDataModel>(new { AccountKey = this.thirdPartyInvoice.AccountKey, CATSPaymentBatchKey = batchKey });
            Assert.AreEqual(1, catsPaymentBatchItem.Where(x => x.EmailAddress == thirdParty.PaymentEmailAddress
                && x.ExternalReference == this.thirdPartyInvoice.PaymentReference
                && x.GenericKey == this.thirdPartyInvoice.ThirdPartyInvoiceKey
                && x.GenericTypeKey == (int)GenericKeyType.ThirdPartyInvoice
                && x.Amount == invoiceLineItems.Sum(y => y.TotalAmountIncludingVAT)
                && x.LegalEntityKey == thirdParty.LegalEntityKey
                && x.SahlReferenceNumber == this.thirdPartyInvoice.SahlReference
                && x.SourceBankAccountKey == spv.BankAccountKey
                && x.SourceReferenceNumber == string.Concat("SAHL", "      ", "SPV ", spv.SPVKey)
                && x.TargetBankAccountKey == thirdParty.BankAccountKey
                && x.TargetName == thirdParty.TradingName).Count(),
                string.Format(@"Expected CATSPaymentBatchItem record GenericKey: {0} GenericTypeKey: {1} AccountKey: {2} InvoiceTotal: {3}, SourceBankAccountKey: {4},
                    TargetBankAccountKey: {5}, CATSPaymentBatchKey: {6}, SahlReferenceNumber: {7}, SourceReferenceNumber: {8}, TargetName: {9}, ExternalReference: {10},
                    EmailAddress: {11}, LegalEntityKey: {12}", this.thirdPartyInvoice.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, thirdPartyInvoice.AccountKey, invoiceLineItems.Sum(y => y.TotalAmountIncludingVAT),
                                                             spv.BankAccountKey, thirdParty.BankAccountKey, batchKey, this.thirdPartyInvoice.SahlReference, string.Concat("SAHL", "      ", "SPV ", spv.SPVKey),
                                                             thirdParty.TradingName, this.thirdPartyInvoice.PaymentReference, thirdParty.PaymentEmailAddress, thirdParty.LegalEntityKey));
        }
    }
}