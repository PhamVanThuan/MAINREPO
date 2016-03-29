using NUnit.Framework;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System.Collections.Generic;
using System;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Core.BusinessModel.Enums;
using System.Linq;

namespace SAHL.Testing.Services.Tests.FinanceDomain
{
    [TestFixture]
    public class when_escalating_an_invoice_for_approval : ServiceTestBase<IFinanceDomainServiceClient>
    {
        private ThirdPartyInvoiceModel thirdPartyInvoice;

        [SetUp]
        public void OnTestSetup()
        {
            SetHeaderMetadataForUser("InvoiceProcessor");
            
            var emptyInvoice = TestApiClient.GetAny<EmptyThirdPartyInvoicesDataModel>();
            var thirdParty = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney }, 1);
            Guid thirdPartyId = thirdParty.Id;
            List<InvoiceLineItemModel> lineItems = new List<InvoiceLineItemModel>()
            {
                new InvoiceLineItemModel(null, emptyInvoice.ThirdPartyInvoiceKey, 1, 100.00M, true),
                new InvoiceLineItemModel(null, emptyInvoice.ThirdPartyInvoiceKey, 1, 200.00M, false)
            };
            DateTime invoiceDate = DateTime.Now;
            string paymentReference = "SAHL - " + DateTime.Now.Date;
            thirdPartyInvoice = new ThirdPartyInvoiceModel(emptyInvoice.ThirdPartyInvoiceKey, thirdPartyId, string.Concat("SAHL-EscalationTest-", Convert.ToBase64String(Guid.NewGuid().ToByteArray())),
                invoiceDate, lineItems, false, paymentReference);
            var command = new CaptureThirdPartyInvoiceCommand(thirdPartyInvoice);
            Execute(command).WithoutErrors();
        }

        [Test]
        public void when_successfully_escalates()
        {
            var command = new EscalateThirdPartyInvoiceForApprovalCommand(thirdPartyInvoice.ThirdPartyInvoiceKey, Convert.ToInt32(_metaDataDictionary.SingleOrDefault(x=>x.Key == ServiceRequestMetadata.HEADER_USERORGANISATIONSTRUCTUREKEY).Value)); 
            Execute(command).WithoutErrors();
            var escalatedForAssertion = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(thirdPartyInvoice.ThirdPartyInvoiceKey);
            Assert.That(escalatedForAssertion.InvoiceStatusKey == (int)InvoiceStatus.AwaitingApproval, 
                "Invoice was not approved because of {0}", messages);
        }

        [TearDown]
        public void OnTestTearDown()
        {
            if (thirdPartyInvoice != null)
            {
                var command = new RemoveEmptyThirdPartyInvoiceCommand(thirdPartyInvoice.ThirdPartyInvoiceKey);
                PerformCommand(command);
                thirdPartyInvoice = null;
            }    
        }
    }
}
