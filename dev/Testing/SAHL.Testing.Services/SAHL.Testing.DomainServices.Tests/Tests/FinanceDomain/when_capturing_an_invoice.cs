using FluentAssert;
using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.EventProjection;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SAHL.Testing.Services.Tests.FinanceDomain
{
    public class when_capturing_an_invoice : ServiceTestBase<IFinanceDomainServiceClient>
    {
        private int _thirdPartyInvoiceKey = 0;
        private EmptyThirdPartyInvoicesDataModel emptyInvoice;

        [SetUp]
        public void TestSetup()
        {
            emptyInvoice = TestApiClient.GetAny<EmptyThirdPartyInvoicesDataModel>();
            _thirdPartyInvoiceKey = emptyInvoice.ThirdPartyInvoiceKey;
        }

        [TearDown]
        public void TestTearDown()
        {
            if (emptyInvoice != null)
            {
                var command = new RemoveEmptyThirdPartyInvoiceCommand(_thirdPartyInvoiceKey);
                PerformCommand(command);
                _thirdPartyInvoiceKey = 0;
                emptyInvoice = null;
            }
        }

        [Test]
        public void when_successful()
        {
            var thirdParty = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney }, 1000);
            Guid thirdPartyId = thirdParty.Id;
            decimal lineItemAmount = 100.00M;
            decimal lineItem2Amount = 200.00M;
            List<InvoiceLineItemModel> lineItems = new List<InvoiceLineItemModel>()
            {
                new InvoiceLineItemModel(null, _thirdPartyInvoiceKey, 1, lineItemAmount, true),
                new InvoiceLineItemModel(null, _thirdPartyInvoiceKey, 1, lineItem2Amount, false)
            };
            decimal vatAmount = 14.00M;
            decimal totalIncludingVat = 114.00M;
            DateTime invoiceDate = DateTime.Now;
            string paymentReference = "Test_Payment_Ref";
            ThirdPartyInvoiceModel invoice = new ThirdPartyInvoiceModel(_thirdPartyInvoiceKey, thirdPartyId, string.Concat("Test-InvNo-", Convert.ToBase64String(Guid.NewGuid().ToByteArray())),
                invoiceDate, lineItems, false, paymentReference);
            var command = new CaptureThirdPartyInvoiceCommand(invoice);
            var projectionBefore = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { attorneyid = thirdPartyId }).FirstOrDefault();
            int countbefore = projectionBefore == null ? 0 : projectionBefore.Unprocessed;
            Execute(command).WithoutErrors();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            AttorneyInvoiceMonthlyBreakdownDataModel projectionAfter = null;
            int countAfter = 0;
            while (sw.Elapsed < TimeSpan.FromSeconds(70))
            {
                projectionAfter = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { attorneyid = thirdPartyId }).FirstOrDefault();
                countAfter = projectionAfter == null ? 0 : projectionAfter.Unprocessed;
                if (countbefore != countAfter)
                {
                    break;
                }
            }
            sw.Stop();
            //Assert projection
            Assert.Greater(countAfter, countbefore, "Unprocessed count after capture command should be greater for AttorneyId : {0}", thirdPartyId);
            //Assert Line Items
            IEnumerable<InvoiceLineItemDataModel> addedLineItems = TestApiClient.Get<InvoiceLineItemDataModel>(new { thirdpartyinvoicekey = _thirdPartyInvoiceKey });
            Assert.That(addedLineItems.Count().Equals(2));
            //Assert vatable line item
            addedLineItems.First().Amount.ShouldBeEqualTo(lineItems.First().AmountExcludingVAT);
            addedLineItems.First().VATAmount.ShouldBeEqualTo(vatAmount);
            addedLineItems.First().TotalAmountIncludingVAT.ShouldBeEqualTo(totalIncludingVat);
            //Assert non-vatable line item
            addedLineItems.Last().Amount.ShouldBeEqualTo(lineItems.Last().AmountExcludingVAT);
            addedLineItems.Last().VATAmount.ShouldBeEqualTo(0.00M);
            addedLineItems.Last().TotalAmountIncludingVAT.ShouldBeEqualTo(lineItems.Last().AmountExcludingVAT);
            //Assert Header
            ThirdPartyInvoiceDataModel invoiceHeader = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(_thirdPartyInvoiceKey);
            invoiceHeader.InvoiceDate.GetValueOrDefault().ToShortDateString().ShouldBeEqualTo(invoiceDate.ToShortDateString());
            invoiceHeader.ThirdPartyId.GetValueOrDefault().ShouldBeEqualTo(thirdPartyId);
            invoiceHeader.InvoiceNumber.ShouldBeEqualTo(invoice.InvoiceNumber);
            invoiceHeader.TotalAmountIncludingVAT.ShouldBeEqualTo(totalIncludingVat + lineItem2Amount);
            invoiceHeader.VATAmount.ShouldBeEqualTo(vatAmount);
            invoiceHeader.AmountExcludingVAT.ShouldBeEqualTo(lineItem2Amount + lineItemAmount);
            invoiceHeader.CapitaliseInvoice.ShouldBeEqualTo(false);
        }

        [Test]
        public void when_unsuccessful()
        {
            var thirdParty = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney }, 1);
            Guid thirdPartyId = thirdParty.Id;
            DateTime invoiceDate = DateTime.Now;
            string paymentReference = "Test_Payment_Ref";
            ThirdPartyInvoiceModel invoice = new ThirdPartyInvoiceModel(_thirdPartyInvoiceKey, thirdPartyId, string.Concat("Test-InvNo-", 
                Convert.ToBase64String(Guid.NewGuid().ToByteArray())), invoiceDate, new List<InvoiceLineItemModel>(), true, paymentReference);
            var command = new CaptureThirdPartyInvoiceCommand(invoice);
            Execute(command).AndExpectThatErrorMessagesContain("The Invoice does not contain any line items.");
        }

        [Test]
        public void when_invoice_number_already_exists()
        {
            var existingInvoice = TestApiClient.Get<ThirdPartyInvoiceDataModel>(new { invoicestatuskey = (int)InvoiceStatus.Captured })
                .Where(x => x.ThirdPartyId.HasValue)
                .OrderBy(x => Guid.NewGuid())
                .Take(1)
                .FirstOrDefault();
            decimal lineItemAmount = 100.00M;
            string paymentReference = "Test_Payment_Ref";
            List<InvoiceLineItemModel> lineItems = new List<InvoiceLineItemModel>() { new InvoiceLineItemModel(null, _thirdPartyInvoiceKey, 1, lineItemAmount, true) };
            ThirdPartyInvoiceModel invoice = new ThirdPartyInvoiceModel(_thirdPartyInvoiceKey, existingInvoice.ThirdPartyId.Value, existingInvoice.InvoiceNumber, DateTime.Now,
                lineItems, false, paymentReference);
            var command = new CaptureThirdPartyInvoiceCommand(invoice);
            base.Execute(command)
                .AndExpectThatErrorMessagesContain("The invoice number provided has already been captured against another invoice for the same third party.");
        }

        [Test]
        public void when_invoice_number_already_exists_for_another_third_party()
        {
            ThirdPartyInvoiceModel invoice = null;
            try
            {
                var thirdParty = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney }, 1);
                var existingInvoice = TestApiClient.Get<ThirdPartyInvoiceDataModel>(new { invoicestatuskey = (int)InvoiceStatus.Captured })
                    .Where(x => x.ThirdPartyId.HasValue && x.ThirdPartyId.GetValueOrDefault() != thirdParty.Id)
                    .OrderBy(x => Guid.NewGuid())
                    .Take(1)
                    .FirstOrDefault();
                decimal lineItemAmount = 100.00M;
                string paymentReference = "Test_Payment_Ref";
                List<InvoiceLineItemModel> lineItems = new List<InvoiceLineItemModel>() { new InvoiceLineItemModel(null, _thirdPartyInvoiceKey, 1, lineItemAmount, true) };
                invoice = new ThirdPartyInvoiceModel(_thirdPartyInvoiceKey, thirdParty.Id, existingInvoice.InvoiceNumber, DateTime.Now,
                    lineItems, false, paymentReference);
                var command = new CaptureThirdPartyInvoiceCommand(invoice);
                base.Execute(command).WithoutErrors();
            }
            finally
            {
                //cleanup invoice used
                var invoiceUsed = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(invoice.ThirdPartyInvoiceKey);
                var newInvoiceDetails = new ThirdPartyInvoiceDataModel(invoiceUsed.ThirdPartyInvoiceKey, invoiceUsed.SahlReference, invoiceUsed.InvoiceStatusKey, invoiceUsed.AccountKey,
                    invoiceUsed.ThirdPartyId, Convert.ToBase64String(Guid.NewGuid().ToByteArray()), invoiceUsed.InvoiceDate, invoiceUsed.ReceivedFromEmailAddress,
                    invoiceUsed.AmountExcludingVAT, invoiceUsed.VATAmount, invoiceUsed.TotalAmountIncludingVAT, invoiceUsed.CapitaliseInvoice, invoiceUsed.ReceivedDate,
                    invoiceUsed.PaymentReference);
                var updateInvoiceCommand = new UpdateThirdPartyInvoiceCommand(newInvoiceDetails);
                base.PerformCommand(updateInvoiceCommand);
            }
        }
    }
}