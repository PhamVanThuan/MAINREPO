using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.EventStore;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Testing.Services.Tests.FinanceDomain
{
    public class when_returning_third_party_invoice_to_payment_queue : ServiceTestBase<IFinanceDomainServiceClient>
    {
        private EmptyThirdPartyInvoicesDataModel emptyThirdPartyInvoice;

        [SetUp]
        public void OnTestSetup()
        {
            emptyThirdPartyInvoice = TestApiClient.GetAny<EmptyThirdPartyInvoicesDataModel>();
            var thirdPartyInvoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(emptyThirdPartyInvoice.ThirdPartyInvoiceKey);
            Assert.AreNotEqual((int)InvoiceStatus.Approved, thirdPartyInvoice.InvoiceStatusKey,
                "Expected InvoiceStatus for ThirdPartyInvoice: {0} not to be {1} on test startup",
                emptyThirdPartyInvoice.ThirdPartyInvoiceKey,
                InvoiceStatus.Approved);
        }

        [TearDown]
        public void OnTestTearDown()
        {
            if(emptyThirdPartyInvoice != null)
            {
                var removeEmptyThirdPartyInvoiceCommand = new RemoveEmptyThirdPartyInvoiceCommand(emptyThirdPartyInvoice.ThirdPartyInvoiceKey);
                PerformCommand(removeEmptyThirdPartyInvoiceCommand);
                emptyThirdPartyInvoice = null;
            }
        }
        [Test]
        public void given_a_valid_third_party_invoice_key_it_should_set_invoice_status_to_approved()
        {
            //becasue of
            var returnThirdPartyInvoiceToPaymentQueueCommand = new ReturnThirdPartyInvoiceToPaymentQueueCommand(emptyThirdPartyInvoice.ThirdPartyInvoiceKey);
            Execute(returnThirdPartyInvoiceToPaymentQueueCommand).WithoutErrors();
            
            //it should update the invoice status to approved
            var thirdPartyInvoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(emptyThirdPartyInvoice.ThirdPartyInvoiceKey);
            Assert.AreEqual((int)InvoiceStatus.Approved, thirdPartyInvoice.InvoiceStatusKey,
                "Expected InvoiceStatus for ThirdPartyInvoice: {0} to be {1}",
                emptyThirdPartyInvoice.ThirdPartyInvoiceKey,
                InvoiceStatus.Approved);

            //it should raise a ThirdPartyInvoiceReturnedToPaymentQueue event
            var events = TestApiClient.Get<EventDataModel>(new { GenericKey = emptyThirdPartyInvoice.ThirdPartyInvoiceKey, GenericKeyTypeKey = (int)GenericKeyType.ThirdPartyInvoice })
                .Where(x => x.EventInsertDate > TestStartTime && x.Data.Contains("ThirdPartyInvoiceReturnedToPaymentQueue"));
            Assert.AreEqual(1, events.Count(), "Expected a ThirdPartyInvoiceReturnedToPaymentQueue event to be raised for ThirdPartyInvoice: {0}", emptyThirdPartyInvoice.ThirdPartyInvoiceKey);
        }
    }
}
