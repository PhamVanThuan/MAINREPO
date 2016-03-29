using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Exchange;
using SAHL.Core.Exchange.Provider;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Constants.Workflows;
using SAHL.Services.Interfaces.FrontEndTest;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using SAHL.Testing.Common;
using SAHL.Testing.Services.Tests.Extensions;
using SAHL.Testing.Services.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ThirdPartyInvoice
{
    public class when_capturing_attorney_invoice : ServiceTestBase<IFrontEndTestServiceClient>
    {
        private static IEnumerable<IMailMessage> receivedMessages;
        private static int accountKey = 0;

        [TestFixtureSetUp]
        new public void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            var query = new WaitForMessagesToBeDeliveredQuery(3);
            base.PerformQuery(query);
            receivedMessages = query.Result.Results;
            CleanUp(receivedMessages);
        }

        [TearDown]
        new public void OnTestTearDown()
        {
            CleanUp(receivedMessages);
            if (accountKey != 0)
            {
                PerformCommand(new RemoveAccountFromOpenMortgageLoanAccountsCommand(accountKey));
                accountKey = 0;
            }
            base.OnTestTearDown();
        }

        [Test]
        public void given_email_fails_validation_should_send_invoice_submission_rejected_email()
        {
            var whereAccountStatusOpen = new WhereAccountStatus(AccountStatus.Open);
            var account = TestApiClient.Get<AccountDataModel>(whereAccountStatusOpen, 1);
            var randomNumberGenerator = new Random();
            var invoiceNumber = randomNumberGenerator.Next(10000, 99999);
            var subject = string.Format("{0} - AutomationTest{1}", account.FirstOrDefault().AccountKey, invoiceNumber);
            var emailMessage = new MailMessage()
            {
                To = MailboxHelper.legalfeeInvoicesMailBox,
                Subject = subject,
                Body = "",
                Attachments = new List<IMailAttachment>(),
                DateSent = DateTime.Now
            };
            var command = new SendEmailCommand(emailMessage);
            base.PerformCommand(command);
            var query = new WaitForMessagesToBeDeliveredQuery(90);
            base.PerformQuery(query);
            receivedMessages = query.Result.Results;
            Assert.That(receivedMessages.Count() > 0, "No email was received in the given time period");
            receivedMessages.Where(x => x.From == MailboxHelper.haloMailBox && x.Subject == string.Concat("INVOICE SUBMISSION REJECTED: ", emailMessage.Subject))
                .FirstOrDefault().AssertBodyTextContains("The email did not contain an attachment in either PDF or TIFF format.");
        }

        [Test]
        public void given_domain_rules_fail_should_send_processing_failed_email_to_loss_control()
        {
            var accountKey = 1000000;
            var randomNumberGenerator = new Random();
            var invoiceNumber = randomNumberGenerator.Next(10000, 99999);
            var subject = string.Format("{0} - AutomationTest{1}", accountKey, invoiceNumber);

            var mailAttachments = new List<IMailAttachment>();
            var mailAttachment = new MailAttachment()
            {
                AttachmentName = "pdf-sample.pdf",
                AttachmentType = "pdf",
                ContentAsBase64 = Convert.ToBase64String(ResourcesHelper.GetResourceBytes("pdf-sample.pdf"))
            };
            mailAttachments.Add(mailAttachment);

            var emailMessage = new MailMessage()
            {
                To = MailboxHelper.legalfeeInvoicesMailBox,
                Subject = subject,
                Body = "",
                Attachments = mailAttachments,
                DateSent = DateTime.Now
            };

            var command = new SendEmailCommand(emailMessage);
            base.PerformCommand(command);
            var query = new WaitForMessagesToBeDeliveredQuery(90);
            base.PerformQuery(query);
            receivedMessages = query.Result.Results;
            Assert.That(receivedMessages.Count() > 0, "No email was received in the given time period");
        }

        [Test]
        public void given_a_valid_attorney_invoice_email_should_accept_third_party_invoice()
        {
            var startTime = DateTime.Now;
            accountKey = TestApiClient.GetAny<OpenMortgageLoanAccountsDataModel>(new { HasThirdPartyInvoice = false }, 1000).AccountKey;
            var randomNumberGenerator = new Random();
            var invoiceNumber = string.Format("A{0}", randomNumberGenerator.Next(10000, 99999));
            var subject = string.Format("{0}-{1}", accountKey, invoiceNumber);

            var mailAttachments = new List<IMailAttachment>();
            var mailAttachmentContent = Convert.ToBase64String(ResourcesHelper.GetResourceBytes("pdf-sample.pdf"));
            var mailAttachment = new MailAttachment()
            {
                AttachmentName = "pdf-sample",
                AttachmentType = "pdf",
                ContentAsBase64 = mailAttachmentContent
            };
            mailAttachments.Add(mailAttachment);

            var emailMessage = new MailMessage()
            {
                To = MailboxHelper.legalfeeInvoicesMailBox,
                Subject = subject,
                Body = "SAHL Integration Test - "+accountKey,
                Attachments = mailAttachments,
                DateSent = DateTime.Now
            };
            var command = new SendEmailCommand(emailMessage);
            base.PerformCommand(command);

            //check for empty invoice
            var thirdPartyInvoice = WaitForEmptyThirdPartyInvoice(accountKey, MailboxHelper.testExchangeProviderCredentials.MailBox, startTime);

            //Check for response email
            var query = new WaitForMessagesToBeDeliveredQuery(120);
            base.PerformQuery(query);
            receivedMessages = query.Result.Results;
            
            receivedMessages.Where(x => x.From == MailboxHelper.haloMailBox && x.Subject.Contains(string.Format("INVOICE SUBMISSION RECEIVED: {0}", emailMessage.Subject)))
                .FirstOrDefault().AssertBodyTextContains(thirdPartyInvoice.SahlReference);

            //check for loss control attorney invoice in document stor
            var getLossControlAttorneyInvoiceDocumentQuery = new GetLossControlAttorneyInvoiceDocumentQuery(thirdPartyInvoice.ThirdPartyInvoiceKey);
            PerformQuery(getLossControlAttorneyInvoiceDocumentQuery);
            var document = getLossControlAttorneyInvoiceDocumentQuery.Result.Results.FirstOrDefault();
            Assert.AreEqual(mailAttachmentContent, document);

            //Check for x2 case
            var getThirdPartyInvoiceInstanceDetailsQuery = new GetThirdPartyInvoicesInstanceDetailsQuery(thirdPartyInvoice.ThirdPartyInvoiceKey);
            PerformQuery(getThirdPartyInvoiceInstanceDetailsQuery);
            Assert.AreEqual(1, getThirdPartyInvoiceInstanceDetailsQuery.Result.Results.Count(),
                string.Format(@"Check for x2 case assertion failed when checking for ThirdPartyInvoiceInstanceDetails where ThirdPartyInvoiceKey = {0}", thirdPartyInvoice.ThirdPartyInvoiceKey));
            var thirdPartyInvoiceInstanceDetails = getThirdPartyInvoiceInstanceDetailsQuery.Result.Results.FirstOrDefault();
            Assert.AreEqual(ThirdPartyInvoices.States.LossControlInvoiceReceived, thirdPartyInvoiceInstanceDetails.StateName,
                string.Format(@"Check for x2 case assertion failed when checking the name of the state that the case is at for ThirdPartyInvoiceKey = {0}", thirdPartyInvoice.ThirdPartyInvoiceKey));
        }

        private void CleanUp(IEnumerable<IMailMessage> mailMessage)
        {
            if (receivedMessages != null)
            {
                foreach (var recievedMessage in receivedMessages)
                {
                    WaitForMessagesToBeDeliveredQuery query = new WaitForMessagesToBeDeliveredQuery(3);
                    PerformQuery(query);
                    var command = new ArchiveEmailCommand(recievedMessage);
                    PerformCommand(command);
                    receivedMessages = null;
                    accountKey = 0;
                }
            }
        }

        private ThirdPartyInvoiceDataModel WaitForEmptyThirdPartyInvoice(int accountKey, string recievedFromEmailAddress, DateTime sentDate)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            IEnumerable<ThirdPartyInvoiceDataModel> thirdPartyInvoices = new List<ThirdPartyInvoiceDataModel>();
            while (sw.Elapsed < TimeSpan.FromSeconds(120))
            {
                thirdPartyInvoices = TestApiClient.Get<ThirdPartyInvoiceDataModel>(new WhereAccountAndReceivedFrom(accountKey, recievedFromEmailAddress));
                if (thirdPartyInvoices.Count() > 0)
                {
                    break;
                }
            }
            sw.Stop();
            Assert.AreEqual(1, thirdPartyInvoices.Count(),
                 string.Format(@"Check for empty invoice assertion failed when checking for ThirdPartyInvoice record where AccountKey = {0} and ReceivedFromEmailAddress = {1}", accountKey, recievedFromEmailAddress));
            var thirdPartyInvoice = thirdPartyInvoices.FirstOrDefault();
            var invoiceLineItems = TestApiClient.Get<InvoiceLineItemDataModel>(new WhereThirdPartyInvoice(thirdPartyInvoice.ThirdPartyInvoiceKey));
            Assert.AreEqual(0, invoiceLineItems.Count(),
                string.Format(@"Check for empty invoice assertion failed when checking for InvoiceLineItem records where ThirdPartyInvoiceKey = {0}", thirdPartyInvoice.ThirdPartyInvoiceKey));
            return thirdPartyInvoice;
        }

        internal class WhereAccountStatus
        {
            public WhereAccountStatus(AccountStatus accountStatus)
            {
                this.AccountStatusKey = (int)accountStatus;
            }

            public int AccountStatusKey { get; set; }
        }

        internal class WhereAccountAndReceivedFrom
        {
            public WhereAccountAndReceivedFrom(int accountKey, string receivedFromEmailAddress)
            {
                this.AccountKey = accountKey;
                this.ReceivedFromEmailAddress = receivedFromEmailAddress;
            }

            public int AccountKey { get; set; }

            public string ReceivedFromEmailAddress { get; set; }
        }

        internal class WhereThirdPartyInvoice
        {
            public WhereThirdPartyInvoice(int thirdPartyInvoiceKey)
            {
                this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            }

            public int ThirdPartyInvoiceKey { get; set; }
        }
    }
}