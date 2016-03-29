using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.Interfaces.CATS.Queries;
using SAHL.Core.Data.Models.FETest;
using System;
using System.Linq;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System.Collections.Generic;
using SAHL.Testing.Common.Helpers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Testing.Services.Tests.CATSService
{
    [TestFixture]
    public class when_removing_an_invoice_from_a_cats_payment_batch : ServiceTestBase<ICATSServiceClient>
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

        [SetUp]
        new public void OnTestSetup()
        {
            base.OnTestSetup();
            string sourceReferenceNumber = "SourceReferenceNumber";
            string sahlReferenceNumber = "SAHL";
            int targetBankAccountKey = 1;
            int sourceBankAccountKey = 1;
            int lineItemDescriptionKey = 1;
            decimal invoiceTotal = 1.0m;
            int accountKey;
            int legalEntityKey;
            string targetName = "Required_targetName",
                   externalReference = "Required_externalReference",
                   emailAddress = "vishavp@sahomeloans.com";
            thirdParty = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney });
            legalEntityKey = thirdParty.LegalEntityKey;

            var inv = TestApiClient.GetAny<InvoiceLineItemDescriptionDataModel>();
            emptyThirdPartyInvoice = TestApiClient.GetAny<EmptyThirdPartyInvoicesDataModel>();
            lineItem = new InvoiceLineItemModel(null,emptyThirdPartyInvoice.ThirdPartyInvoiceKey,inv.InvoiceLineItemDescriptionKey,invoiceTotal,true);
            invoiceLineItems = new List<InvoiceLineItemModel>();
            lineItemDescriptionKey = lineItem.InvoiceLineItemDescriptionKey;
            invoiceLineItems.Add(lineItem);

            var query = new GetNewThirdPartyPaymentBatchReferenceQuery(CATSPaymentBatchType.ThirdPartyInvoice);
            Execute(query).WithoutErrors();
            thirdPartyInvoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(emptyThirdPartyInvoice.ThirdPartyInvoiceKey);
            accountKey = thirdPartyInvoice.AccountKey;
            thirdPartyInvoice.InvoiceNumber = "Test-InvNo-" + Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            thirdPartyInvoice.PaymentReference = sourceReferenceNumber;
            financeDomainModel = new ThirdPartyInvoiceModel(thirdPartyInvoice.ThirdPartyInvoiceKey,thirdParty.Id,thirdPartyInvoice.InvoiceNumber,DateTime.Now,invoiceLineItems, true, thirdPartyInvoice.PaymentReference);

            CaptureThirdPartyInvoiceCommand captureCommand = new CaptureThirdPartyInvoiceCommand(financeDomainModel);
            financeClient = GetInstance<IFinanceDomainServiceClient>();
            financeClient.PerformCommand(captureCommand, metaData).WithoutMessages();

            paymentBatchItem = new CATSPaymentBatchItemModel(thirdParty.LegalEntityKey, emptyThirdPartyInvoice.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, thirdPartyInvoice.AccountKey,
                                                             invoiceTotal, query.Result.Results.FirstOrDefault().BatchKey, sourceBankAccountKey, targetBankAccountKey,
                                                             sahlReferenceNumber+"-"+numberGenerator.Next(), sourceReferenceNumber+"-"+numberGenerator.Next(), targetName, externalReference+"-"+numberGenerator.Next(), emailAddress, true);
            setupCATScommand = new AddCATSPaymentBatchItemCommand(paymentBatchItem);
            Execute(setupCATScommand).WithoutErrors();
        }

        [TearDown]
        new public void OnTestTearDown()
        {
            if (emptyThirdPartyInvoice != null)
            {
                var command = new RemoveEmptyThirdPartyInvoiceCommand(emptyThirdPartyInvoice.ThirdPartyInvoiceKey);
                _feTestClient.PerformCommand(command, metaData);
                emptyThirdPartyInvoice = null;
            }
            base.OnTestTearDown();
        }

        [Test]
        public void when_successfully_removes_invoice_after_being_added()
        {
            bool paymentBatchIsFound = false;
            var dataQuery = new GetCatsPaymentBatchItemsByBatchReferenceQuery(paymentBatchItem.CATSPaymentBatchKey);
            Execute(dataQuery).WithoutErrors();
            Assert.IsTrue(dataQuery.Result.Results.Where(x => x.GenericKey == thirdPartyInvoice.ThirdPartyInvoiceKey
                && x.GenericTypeKey == (int)GenericKeyType.ThirdPartyInvoice).FirstOrDefault().Processed.GetValueOrDefault(false),
                "PaymentBatchItem for ThirdPartyInvoice: {0} has not been marked as processed true", thirdPartyInvoice.ThirdPartyInvoiceKey);   

            bool paymentBatchHasBeenRemoved = true;
            var command = new RemoveCATSPaymentBatchItemCommand(paymentBatchItem.CATSPaymentBatchKey, paymentBatchItem.GenericKey, paymentBatchItem.GenericTypeKey);
            Execute(command).WithoutErrors();
            Execute(dataQuery).WithoutErrors();
            Assert.IsFalse(dataQuery.Result.Results.Where(x => x.GenericKey == thirdPartyInvoice.ThirdPartyInvoiceKey 
                && x.GenericTypeKey == (int)GenericKeyType.ThirdPartyInvoice).FirstOrDefault().Processed.GetValueOrDefault(true), 
                "PaymentBatchItem for ThirdPartyInvoice: {0} has not been marked as processed false", thirdPartyInvoice.ThirdPartyInvoiceKey);                     
        }
    }
}