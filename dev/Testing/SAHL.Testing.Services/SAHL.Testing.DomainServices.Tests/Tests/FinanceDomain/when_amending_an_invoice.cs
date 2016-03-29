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
    [TestFixture]
    public class when_amending_an_invoice : ServiceTestBase<IFinanceDomainServiceClient>
    {
        private ThirdPartyInvoiceDataModel thirdPartyInvoice;
        private InvoiceLineItemModel _newLineItem;
        private ThirdPartyDataModel thirdParty;
        private bool removeLastItem;
        private decimal newAmount;
        private int _newLineItemDescriptionKey;

        private string attorneyRegisteredName;
        private IEnumerable<AttorneyInvoiceMonthlyBreakdownDataModel> attorney_ThirdPartyProjection_BeforeInvoiceCapture;

        [SetUp]
        new public void OnTestSetup()
        {
            base.OnTestSetup();
            thirdParty = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney }, 1);
            Guid thirdPartyId = thirdParty.Id;
            _newLineItem = null;
            removeLastItem = false;
            newAmount = 0.00M;
            _newLineItemDescriptionKey = 0;
            var emptyInvoice = TestApiClient.GetAny<EmptyThirdPartyInvoicesDataModel>();
            thirdPartyInvoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(emptyInvoice.ThirdPartyInvoiceKey);
            thirdPartyInvoice.PaymentReference = "Test_Payment_Ref-" + base.randomizer.Next();

            decimal lineItemAmount = 100.00M;
            decimal lineItem2Amount = 200.00M;
            List<InvoiceLineItemModel> lineItems = new List<InvoiceLineItemModel>()
            {
                new InvoiceLineItemModel(null, thirdPartyInvoice.ThirdPartyInvoiceKey, 1, lineItemAmount, true),
                new InvoiceLineItemModel(null, thirdPartyInvoice.ThirdPartyInvoiceKey, 2, lineItem2Amount, false)
            };
            var invoice = new ThirdPartyInvoiceModel(thirdPartyInvoice.ThirdPartyInvoiceKey, thirdPartyId, string.Concat("Test-InvNo-", Convert.ToBase64String(Guid.NewGuid().ToByteArray())),
                DateTime.Now, lineItems, true, thirdPartyInvoice.PaymentReference);

            attorneyRegisteredName = TestApiClient.Get<LegalEntityDataModel>(new { LegalEntityKey = thirdParty.LegalEntityKey }).First().RegisteredName;
            attorney_ThirdPartyProjection_BeforeInvoiceCapture = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { AttorneyName = attorneyRegisteredName });

            var command = new CaptureThirdPartyInvoiceCommand(invoice);
            base.Execute<CaptureThirdPartyInvoiceCommand>(command).WithoutErrors();
            thirdPartyInvoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(thirdPartyInvoice.ThirdPartyInvoiceKey);
        }

        [TearDown]
        new public void OnTestTearDown()
        {
            if (thirdPartyInvoice != null)
            {
                var command = new RemoveEmptyThirdPartyInvoiceCommand(thirdPartyInvoice.ThirdPartyInvoiceKey);
                base.PerformCommand(command);

                thirdPartyInvoice = null;
                _newLineItem = null;
                removeLastItem = false;
                newAmount = 0;
                _newLineItemDescriptionKey = 0;
            }
            base.OnTestTearDown();
        }

        [Test]
        public void when_reassigning_an_invoice_to_a_new_third_party()
        {
            CheckAttorneyInvoiceMonthlyBreakdownUnprocessedCount(attorney_ThirdPartyProjection_BeforeInvoiceCapture.FirstOrDefault(), attorneyRegisteredName, 1);

            //get a new thirdparty with their projection
            var newThirdParty = TestApiClient.Get<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney })
                .Where(x => x.LegalEntityKey != thirdParty.LegalEntityKey)
                .FirstOrDefault();
            var newAttorneyRegisteredName = TestApiClient.Get<LegalEntityDataModel>(new { LegalEntityKey = newThirdParty.LegalEntityKey }).First().RegisteredName;
            var oldAttorney_ThirdPartyProjection_BeforeAmend = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { AttorneyName = attorneyRegisteredName });
            var newAttorney_ThirdPartyProjection_BeforeAmend = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { AttorneyName = newAttorneyRegisteredName });
            //amend the invoice
            var lineItems = new List<InvoiceLineItemModel>();
            lineItems.Add(new InvoiceLineItemModel(null, this.thirdPartyInvoice.ThirdPartyInvoiceKey, 1, 999, true));
            var thirdPartyInvoice = new ThirdPartyInvoiceModel(this.thirdPartyInvoice.ThirdPartyInvoiceKey, newThirdParty.Id, "Amending Third Party" + base.randomizer.Next(), DateTime.Now, lineItems, false, "Amending Third Party");
            var command = new AmendThirdPartyInvoiceCommand(thirdPartyInvoice);
            base.Execute<AmendThirdPartyInvoiceCommand>(command).WithoutErrors();

            CheckAttorneyInvoiceMonthlyBreakdownUnprocessedCount(oldAttorney_ThirdPartyProjection_BeforeAmend.FirstOrDefault(), attorneyRegisteredName, -1);
            CheckAttorneyInvoiceMonthlyBreakdownUnprocessedCount(newAttorney_ThirdPartyProjection_BeforeAmend.FirstOrDefault(), newAttorneyRegisteredName, 1);
        }

        private void CheckAttorneyInvoiceMonthlyBreakdownUnprocessedCount(AttorneyInvoiceMonthlyBreakdownDataModel attorneyInvoiceMonthlyBreakdownBefore, string attorneyName, int expectedDifference)
        {
            AttorneyInvoiceMonthlyBreakdownDataModel attorneyInvoiceMonthlyBreakdownAfter = null;
            int countBefore = attorneyInvoiceMonthlyBreakdownBefore == null ? 0 : attorneyInvoiceMonthlyBreakdownBefore.Unprocessed;
            int expectedCountAfter = countBefore + expectedDifference;
            int actualCountAfter = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed < TimeSpan.FromSeconds(50))
            {
                attorneyInvoiceMonthlyBreakdownAfter = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { AttorneyName = attorneyName }).FirstOrDefault();
                actualCountAfter = attorneyInvoiceMonthlyBreakdownAfter == null ? 0 : attorneyInvoiceMonthlyBreakdownAfter.Unprocessed;
                if (expectedCountAfter == actualCountAfter)
                {
                    break;
                }
            }
            sw.Stop();
            Assert.AreEqual(expectedCountAfter, actualCountAfter, "Expected AttorneyInvoiceMonthlyBreakdown Unprocessed count for {0} to change from {1} to {2}", attorneyName, countBefore, expectedCountAfter);
        }

        [Test]
        public void when_successfully_adding_line_items()
        {
            ThirdPartyInvoiceModel amendedInvoice;
            var invoiceLineItems = TestApiClient.Get<InvoiceLineItemDataModel>(new { thirdpartyinvoicekey = thirdPartyInvoice.ThirdPartyInvoiceKey });
            _newLineItem = new InvoiceLineItemModel(null, thirdPartyInvoice.ThirdPartyInvoiceKey, 1, 999, true);
            amendedInvoice = SetupInvoiceForTest(thirdPartyInvoice, invoiceLineItems, Guid.NewGuid());
            var command = new AmendThirdPartyInvoiceCommand(amendedInvoice);
            base.Execute<AmendThirdPartyInvoiceCommand>(command).WithoutErrors();
            //get the lineitems
            var amendedLineItems = TestApiClient.Get<InvoiceLineItemDataModel>(new { thirdpartyinvoicekey = amendedInvoice.ThirdPartyInvoiceKey });
            amendedLineItems.Count().ShouldBeEqualTo(3);
            var invoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(thirdPartyInvoice.ThirdPartyInvoiceKey);
            invoice.TotalAmountIncludingVAT.GetValueOrDefault().ShouldBeEqualTo(1452.86M);
            invoice.AmountExcludingVAT.GetValueOrDefault().ShouldBeEqualTo(1299.00M);
            invoice.VATAmount.GetValueOrDefault().ShouldBeEqualTo(153.86M);
            invoice.CapitaliseInvoice.GetValueOrDefault().ShouldBeEqualTo(true);
        }

        [Test]
        public void when_succesfully_removing_line_items()
        {
            removeLastItem = true;
            ThirdPartyInvoiceModel amendedInvoice;
            var invoiceLineItems = TestApiClient.Get<InvoiceLineItemDataModel>(new { thirdpartyinvoicekey = thirdPartyInvoice.ThirdPartyInvoiceKey });
            amendedInvoice = SetupInvoiceForTest(thirdPartyInvoice, invoiceLineItems, Guid.NewGuid());
            var command = new AmendThirdPartyInvoiceCommand(amendedInvoice);
            base.Execute<AmendThirdPartyInvoiceCommand>(command).WithoutErrors();
            //get the lineitems
            var amendedLineItems = TestApiClient.Get<InvoiceLineItemDataModel>(new { thirdpartyinvoicekey = amendedInvoice.ThirdPartyInvoiceKey });
            amendedLineItems.Count().ShouldBeEqualTo(1);
            var invoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(thirdPartyInvoice.ThirdPartyInvoiceKey);
            invoice.TotalAmountIncludingVAT.GetValueOrDefault().ShouldBeEqualTo(114.00M);
            invoice.AmountExcludingVAT.GetValueOrDefault().ShouldBeEqualTo(100.00M);
            invoice.VATAmount.GetValueOrDefault().ShouldBeEqualTo(14.00M);
        }

        [Test]
        public void when_successfully_updating_line_items_amount()
        {
            newAmount = 777.00M;
            ThirdPartyInvoiceModel amendedInvoice;
            var invoiceLineItems = TestApiClient.Get<InvoiceLineItemDataModel>(new { thirdpartyinvoicekey = thirdPartyInvoice.ThirdPartyInvoiceKey });
            amendedInvoice = SetupInvoiceForTest(thirdPartyInvoice, invoiceLineItems, Guid.NewGuid());
            var command = new AmendThirdPartyInvoiceCommand(amendedInvoice);
            base.Execute<AmendThirdPartyInvoiceCommand>(command).WithoutErrors();
            //get the lineitems
            var amendedLineItems = TestApiClient.Get<InvoiceLineItemDataModel>(new { thirdpartyinvoicekey = amendedInvoice.ThirdPartyInvoiceKey });
            amendedLineItems.Where(x => x.Amount == newAmount).Count().ShouldBeEqualTo(2);
            var invoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(thirdPartyInvoice.ThirdPartyInvoiceKey);
            invoice.TotalAmountIncludingVAT.GetValueOrDefault().ShouldBeEqualTo(1662.78M);
            invoice.AmountExcludingVAT.GetValueOrDefault().ShouldBeEqualTo(1554.00M);
            invoice.VATAmount.GetValueOrDefault().ShouldBeEqualTo(108.78M);
        }

        [Test]
        public void when_successfully_updating_a_line_item_description()
        {
            _newLineItemDescriptionKey = 3;
            ThirdPartyInvoiceModel amendedInvoice;
            var invoiceLineItems = TestApiClient.Get<InvoiceLineItemDataModel>(new { thirdpartyinvoicekey = thirdPartyInvoice.ThirdPartyInvoiceKey });
            amendedInvoice = SetupInvoiceForTest(thirdPartyInvoice, invoiceLineItems, Guid.NewGuid());
            var command = new AmendThirdPartyInvoiceCommand(amendedInvoice);
            base.Execute<AmendThirdPartyInvoiceCommand>(command).WithoutErrors();
            //get the lineitems
            var amendedLineItems = TestApiClient.Get<InvoiceLineItemDataModel>(new { thirdpartyinvoicekey = amendedInvoice.ThirdPartyInvoiceKey });
            amendedLineItems.Where(x => x.InvoiceLineItemDescriptionKey == _newLineItemDescriptionKey).Count().ShouldBeEqualTo(2);
        }

        private List<InvoiceLineItemModel> MapDbLineItemModelToDomainModel(IEnumerable<InvoiceLineItemDataModel> invoiceLineItems)
        {
            List<InvoiceLineItemModel> lineItems = new List<InvoiceLineItemModel>();
            foreach (var line in invoiceLineItems)
            {
                var lineitemAmount = line.Amount;
                int lineItemDescriptionKey = line.InvoiceLineItemDescriptionKey;
                if (newAmount > 0.00M)
                {
                    lineitemAmount = newAmount;
                }
                if (_newLineItemDescriptionKey > 0)
                {
                    lineItemDescriptionKey = _newLineItemDescriptionKey;
                }
                InvoiceLineItemModel model = new InvoiceLineItemModel(line.InvoiceLineItemKey, line.ThirdPartyInvoiceKey, lineItemDescriptionKey, lineitemAmount, line.IsVATItem);
                lineItems.Add(model);
            }
            if (removeLastItem)
            {
                lineItems.RemoveAt(1);
            }
            if (_newLineItem != null)
            {
                lineItems.Add(_newLineItem);
            }
            return lineItems;
        }

        private ThirdPartyInvoiceModel SetupInvoiceForTest(ThirdPartyInvoiceDataModel dataModel, IEnumerable<InvoiceLineItemDataModel> invoiceLineItems, Guid thirdpartyID)
        {
            var lineItems = MapDbLineItemModelToDomainModel(invoiceLineItems);
            return new ThirdPartyInvoiceModel(dataModel.ThirdPartyInvoiceKey, dataModel.ThirdPartyId.Value, dataModel.InvoiceNumber, dataModel.InvoiceDate, lineItems, true, dataModel.PaymentReference);
        }
    }
}