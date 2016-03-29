using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Linq;
using SAHL.Core.Exchange;
using SAHL.Core.Exchange.Provider;
using SAHL.Core.Data.Models.EventStore;
using SAHL.Core.Data.Models.EventProjection;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System.Collections.Generic;
using System;

namespace SAHL.Testing.Services.Tests.FinanceDomain
{
    public class when_rejecting_an_invoice : ServiceTestBase<IFinanceDomainServiceClient>
    {
        public IMailMessage mailMessage { get; set; }
        public int _thirdPartyInvoiceKey { get; set; }
        public OpenThirdPartyInvoicesDataModel openThirdPartyInvoices { get; set; }

        [SetUp]
        public void OnTestSetup()
        {
            mailMessage = new MailMessage();

            //archive all the mail in the mailbox
            WaitForMessagesToBeDeliveredQuery Query = new WaitForMessagesToBeDeliveredQuery(1);

                base.PerformQuery(Query);
                foreach(var element in Query.Result.Results)
                {
                    var archiveCommand = new ArchiveEmailCommand(element);
                    PerformCommand(archiveCommand);
                }
            SetHeaderMetadataForUser("InvoiceProcessor");
        }
        
        [Test]
        public void when_unsuccessful()
        {
            //Test Setup
            int invalidInvoiceKey = 0;
            string comments = "invalid invoice key";
            
            //Perform the RejectThirdPartyInvoiceCommand with an invalid ThirdPartyInvoiceKey
            RejectThirdPartyInvoiceCommand rejectThirdPartyInvoiceCommand = new RejectThirdPartyInvoiceCommand(invalidInvoiceKey, comments);
            base.Execute(rejectThirdPartyInvoiceCommand);

            //Assert that an error message is returned
            Assert.AreEqual(1, base.messages.ErrorMessages().Where(x => x.Message == string.Format("No Third Party Invoice with Key {0} exists.", invalidInvoiceKey)).Count(),
                string.Format("Expected error message: No Third Party Invoice with Key {0} exists.", invalidInvoiceKey));

        }

        [Test]
        public void when_successfully_rejecting_pre_approval()
        { 
            //Find a ThirdPartyInvoice at a State where the Reject Invoice can be performed (check in X3 Workflow designer).  Might have to find/create a query in FrontendTest Service
            var thirdPartyInvoice = TestApiClient.GetAny<ThirdPartyInvoiceDataModel>(new { InvoiceStatusKey = (int)InvoiceStatus.Captured}, 1000);

            var thirdparty = TestApiClient.Get<ThirdPartyDataModel>(new { Id = thirdPartyInvoice.ThirdPartyId });
            var legalEntity = TestApiClient.Get<LegalEntityDataModel>(new { LegalEntityKey = thirdparty.FirstOrDefault().LegalEntityKey });
            var projectionBefore = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { AttorneyName = legalEntity.FirstOrDefault().RegisteredName });
            //To make sure that the rejection email gets sent to a known address you will need to update the received from address of the ThirdPartyInvoice to HaloUser@Sahomeloans.com.  Will have to create a command in FrontEndTest Service
            //Perform the RejectThirdPartyInvoiceCommand with the ThirdPartyInvoiceKey you identified
            var oldEmailAddress = thirdPartyInvoice.ReceivedFromEmailAddress;
            thirdPartyInvoice.ReceivedFromEmailAddress = "halouser@sahomeloans.com";
            messages.Aggregate(_feTestClient.PerformCommand(new UpdateThirdPartyInvoiceCommand(thirdPartyInvoice), metaData));
            Assert.False(messages.HasErrors);
            Assert.False(messages.HasExceptions);

            RejectThirdPartyInvoiceCommand rejectThirdPartyInvoiceCommand = new RejectThirdPartyInvoiceCommand(thirdPartyInvoice.ThirdPartyInvoiceKey, "Sucessful Invoice Rejection due to existing third party invoice key : "+thirdPartyInvoice.ThirdPartyInvoiceKey);
            Execute(rejectThirdPartyInvoiceCommand).WithoutErrors();
            
            //Check that the status of the ThirdPartyInvoice record has been udpdated.  Should be able to use TestApiClient for this query
            var result = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(thirdPartyInvoice.ThirdPartyInvoiceKey);
            Assert.AreEqual(result.InvoiceStatusKey, 5);

            //Check the rejection email was sent to HaloUser.  Use the existing query in FrontEndTestService
            WaitForMessagesToBeDeliveredQuery Query = new WaitForMessagesToBeDeliveredQuery(120);
            PerformQuery(Query);
            mailMessage.Body = Query.Result.Results.FirstOrDefault().ToString();
            var resultsAsArray = Query.Result.Results.ToArray();
            for (int i = 0; i < resultsAsArray.Length; i++)
            {
                if (resultsAsArray[i].Body.Contains(rejectThirdPartyInvoiceCommand.RejectionComments.ToString()))
                {
                    Assert.That(Query.Result.Results.ElementAt(i).Body.Contains(rejectThirdPartyInvoiceCommand.RejectionComments.ToString()));
                    break;
                }
            }
            Assert.False(messages.HasErrors, "Messages contained errors: "+messages);
                var thirdPartyInvoiceRejectionEvent = TestApiClient.Get<EventDataModel>(new { EventTypeKey = 24, GenericKey = rejectThirdPartyInvoiceCommand.ThirdPartyInvoiceKey, GenericKeyTypeKey = (int)GenericKeyType.ThirdPartyInvoice });
            Assert.IsNotNull(thirdPartyInvoiceRejectionEvent, "No rejection event was kicked off.");
            bool eventWasKickedOff = false;
            for (int i = 0; i < thirdPartyInvoiceRejectionEvent.Count(); i++)
            {
                var singleEvent = thirdPartyInvoiceRejectionEvent.ElementAt(i);
                if (singleEvent.EventTypeKey == 24 && singleEvent.GenericKey == rejectThirdPartyInvoiceCommand.ThirdPartyInvoiceKey && singleEvent.Data.Contains("ThirdPartyInvoiceRejectedPreApprovalEvent"))
                {
                    Assert.True(thirdPartyInvoiceRejectionEvent.ElementAt(i).Data.Contains("ThirdPartyInvoiceRejectedPreApprovalEvent"));
                    eventWasKickedOff = true;
                    break;
                }
            }
            Assert.IsTrue(thirdPartyInvoiceRejectionEvent.FirstOrDefault().Data.Contains(rejectThirdPartyInvoiceCommand.ThirdPartyInvoiceKey.ToString()) && eventWasKickedOff);
        }

        [Test]
        public void when_successfully_rejecting_post_approval()
        {
            var thirdPartyInvoice = TestApiClient.GetAny<ThirdPartyInvoiceDataModel>(new { InvoiceStatusKey = (int)InvoiceStatus.Approved }, 1000);
           
            var thirdparty = TestApiClient.Get<ThirdPartyDataModel>(new { Id = thirdPartyInvoice.ThirdPartyId });
            var legalEntity = TestApiClient.Get<LegalEntityDataModel>(new { LegalEntityKey = thirdparty.FirstOrDefault().LegalEntityKey });
            var projectionBefore = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { AttorneyName = legalEntity.FirstOrDefault().RegisteredName });
            RejectThirdPartyInvoiceCommand rejectThirdPartyInvoiceCommand = new RejectThirdPartyInvoiceCommand(thirdPartyInvoice.ThirdPartyInvoiceKey, "Sucessful Invoice Rejection due to existing third party invoice key : " + thirdPartyInvoice.ThirdPartyInvoiceKey);
            Execute(rejectThirdPartyInvoiceCommand).WithoutErrors();

            WaitForMessagesToBeDeliveredQuery Query = new WaitForMessagesToBeDeliveredQuery(120);
            PerformQuery(Query);
            mailMessage.Body = Query.Result.Results.FirstOrDefault().ToString();
            var resultsAsArray = Query.Result.Results.ToArray();
            for (int i = 0; i < resultsAsArray.Length; i++)
            {
                if (resultsAsArray[i].Body.Contains(rejectThirdPartyInvoiceCommand.RejectionComments.ToString()))
                {
                    Assert.That(Query.Result.Results.ElementAt(i).Body.Contains(rejectThirdPartyInvoiceCommand.RejectionComments.ToString()));
                    break;
                }
            }
            Assert.False(messages.HasErrors, "Messages contained errors: " + messages);
            
            var thirdPartyInvoiceRejectionEvent = TestApiClient.Get<EventDataModel>(new { EventTypeKey = 24, GenericKey = rejectThirdPartyInvoiceCommand.ThirdPartyInvoiceKey, GenericKeyTypeKey = (int)GenericKeyType.ThirdPartyInvoice});
            Assert.IsNotNull(thirdPartyInvoiceRejectionEvent, "No rejection event was kicked off.");
            bool eventWasKickedOff = false;
            for(int i=0; i<thirdPartyInvoiceRejectionEvent.Count(); i++)
            {
                var singleEvent = thirdPartyInvoiceRejectionEvent.ElementAt(i);
                if(singleEvent.EventTypeKey == 24 && singleEvent.GenericKey == rejectThirdPartyInvoiceCommand.ThirdPartyInvoiceKey && singleEvent.Data.Contains("ThirdPartyInvoiceRejectedPostApprovalEvent"))
                {
                    eventWasKickedOff = true;
                    break;
                }
            }
            Assert.False(messages.HasErrors);
            Assert.False(messages.HasExceptions, "messages have exceptions : "+messages);
            Assert.False(messages.HasErrors, "Messages contained errors: " + messages);
            Assert.True(eventWasKickedOff, "Event was not projected for the case: "+rejectThirdPartyInvoiceCommand.ThirdPartyInvoiceKey);
            var projectionAfter = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { AttorneyName = legalEntity.FirstOrDefault().RegisteredName });
            Assert.That(projectionBefore.FirstOrDefault().Rejected == projectionAfter.FirstOrDefault().Rejected - 1);
        }
        
        [Test]
        public void when_successfully_rejecting_pre_approval_event_projection_test()
        {
            var emptyInvoice = TestApiClient.GetAny<EmptyThirdPartyInvoicesDataModel>();
            _thirdPartyInvoiceKey = emptyInvoice.ThirdPartyInvoiceKey;

            var thirdParty = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney }, 1);
            Guid thirdPartyId = thirdParty.Id;
            decimal lineItemAmount = 100.00M;
            decimal lineItem2Amount = 200.00M;
            List<InvoiceLineItemModel> lineItems = new List<InvoiceLineItemModel>()
            {
                new InvoiceLineItemModel(null, _thirdPartyInvoiceKey, 1, lineItemAmount, true),
                new InvoiceLineItemModel(null, _thirdPartyInvoiceKey, 1, lineItem2Amount, false)
            };
            DateTime invoiceDate = DateTime.Now;
            string paymentReference = "Test_Payment_Ref";
            ThirdPartyInvoiceModel invoice = new ThirdPartyInvoiceModel(_thirdPartyInvoiceKey, thirdPartyId, string.Concat("SAHL-InvNo-", Convert.ToBase64String(Guid.NewGuid().ToByteArray())),
                invoiceDate, lineItems, false, paymentReference);
            var command = new CaptureThirdPartyInvoiceCommand(invoice);
            base.Execute<CaptureThirdPartyInvoiceCommand>(command).WithoutErrors();
        }

    }
}
