using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.Interfaces.CATS.Queries;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Testing.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.Services.Tests.CATSService
{
    [TestFixture]
    public class when_adding_cats_payment_batch_item : ServiceTestBase<ICATSServiceClient>
    {
        private EmptyThirdPartyInvoicesDataModel emptyThirdPartyInvoice;
        private ThirdPartyInvoiceDataModel thirdPartyInvoice;
        private ThirdPartyInvoiceModel financeDomainModel;
        private CATSPaymentBatchItemModel paymentBatchItem;
        private ThirdPartyDataModel thirdParty;
        private InvoiceLineItemModel lineItem;
        private List<InvoiceLineItemModel> invoiceLineItems;
        private AddCATSPaymentBatchItemCommand setupCATScommand;
        private Random numberGenerator = new Random();
        private IFinanceDomainServiceClient financeClient;
        private static string sourceReferenceNumber, sahlReferenceNumber, targetName, externalReference, emailAddress;
        private int lineItemDescriptionKey, accountKey, legalEntityKey, targetBankAccountKey, sourceBankAccountKey;
        private decimal invoiceTotal;

        [SetUp]
        public void OnTestSetUp()
        {
            base.OnTestSetup();
            sourceReferenceNumber = "SourceReferenceNumber";
            sahlReferenceNumber = "SAHL";
            targetBankAccountKey = 1;
            sourceBankAccountKey = 1;
            lineItemDescriptionKey = 1;
            invoiceTotal = 1.0m;
            targetName = "Required_targetName";
            externalReference = "Required_externalReference";
            emailAddress = "vishavp@sahomeloans.com";
            thirdParty = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney });
            legalEntityKey = thirdParty.LegalEntityKey;

            var inv = TestApiClient.GetAny<InvoiceLineItemDescriptionDataModel>();
            emptyThirdPartyInvoice = TestApiClient.GetAny<EmptyThirdPartyInvoicesDataModel>();
            lineItem = new InvoiceLineItemModel(null, emptyThirdPartyInvoice.ThirdPartyInvoiceKey, inv.InvoiceLineItemDescriptionKey, invoiceTotal, true);
            invoiceLineItems = new List<InvoiceLineItemModel>();
            lineItemDescriptionKey = lineItem.InvoiceLineItemDescriptionKey;
            invoiceLineItems.Add(lineItem);

            var query = new GetNewThirdPartyPaymentBatchReferenceQuery(CATSPaymentBatchType.ThirdPartyInvoice);
            Execute(query).WithoutErrors();
            thirdPartyInvoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(emptyThirdPartyInvoice.ThirdPartyInvoiceKey);
            accountKey = thirdPartyInvoice.AccountKey;
            thirdPartyInvoice.InvoiceNumber = "Test-InvNo-" + Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            thirdPartyInvoice.PaymentReference = sourceReferenceNumber;
            financeDomainModel = new ThirdPartyInvoiceModel(thirdPartyInvoice.ThirdPartyInvoiceKey, thirdParty.Id, thirdPartyInvoice.InvoiceNumber, DateTime.Now, invoiceLineItems, true, thirdPartyInvoice.PaymentReference);

            CaptureThirdPartyInvoiceCommand captureCommand = new CaptureThirdPartyInvoiceCommand(financeDomainModel);
            financeClient = GetInstance<IFinanceDomainServiceClient>();
            financeClient.PerformCommand(captureCommand, metaData).WithoutMessages();
        }

        [Test]
        public void given_invoice_and_payment_batch_it_should_add_payment_batch_item()
        {
            var query = new GetNewThirdPartyPaymentBatchReferenceQuery(CATSPaymentBatchType.ThirdPartyInvoice);
            Execute(query).WithoutErrors();

            paymentBatchItem = new CATSPaymentBatchItemModel(thirdParty.LegalEntityKey, emptyThirdPartyInvoice.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, thirdPartyInvoice.AccountKey,
                                                             invoiceTotal, query.Result.Results.FirstOrDefault().BatchKey, sourceBankAccountKey, targetBankAccountKey,
                                                             sahlReferenceNumber + "-" + numberGenerator.Next(), sourceReferenceNumber + "-" + numberGenerator.Next(), targetName, externalReference + "-" + numberGenerator.Next(), emailAddress, true);
            setupCATScommand = new AddCATSPaymentBatchItemCommand(paymentBatchItem);
            Execute(setupCATScommand).WithoutErrors();
            bool paymentBatchIsFound = false;
            var dataQuery = new GetCatsPaymentBatchItemsByBatchReferenceQuery(paymentBatchItem.CATSPaymentBatchKey);
            Execute(dataQuery).WithoutErrors();
            for (int i = 0; i < dataQuery.Result.Results.Count(); i++)
            {
                if (dataQuery.Result.Results.ElementAt(i).SahlReferenceNumber.Equals(paymentBatchItem.SahlReferenceNumber))
                {
                    paymentBatchIsFound = true;
                    break;
                }
            }
            Assert.IsTrue(paymentBatchIsFound, "PaymentBatchItem has not been found, setup unsuccessful.");
            Assert.That(messages.HasErrors == false && messages.HasExceptions == false);
        }

        [TearDown]
        new public void OnTestTearDown()
        {
            if (emptyThirdPartyInvoice.ThirdPartyInvoiceKey != 0)
            {
                var command = new RemoveEmptyThirdPartyInvoiceCommand(emptyThirdPartyInvoice.ThirdPartyInvoiceKey);
                _feTestClient.PerformCommand(command, metaData);
            }
            base.OnTestTearDown();
        }
    }
}
