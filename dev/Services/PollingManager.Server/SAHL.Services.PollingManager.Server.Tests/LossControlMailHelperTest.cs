using NUnit.Framework;
using SAHL.Core.Exchange;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.PollingManager.Helpers;
using System.IO;
using System.Linq;

namespace SAHL.Services.PollingManager.Server.Tests
{
    [TestFixture]
    public class LossControlMailHelperTest
    {
        [Test]
        public void PrepareMailAttachements_GivenMailWithSingleAttachment_ShouldReturnAttachmentName()
        {
            //arrange
            LossControlMailHelper mailHelper = new LossControlMailHelper();

            IMailMessage mailMessage = FakeMailMessageFactory.GetMailMessageWithSingleAttachment();
            string attachments = mailHelper.GetAttachmentsAsString(mailMessage);

            //action

            //assert
            Assert.AreEqual(attachments, "document.pdf");
        }

        [Test]
        public void PrepareMailAttachements_GivenMailWithTwoAttachment_ShouldReturnAttachmentNamesWithCrLf()
        {
            //arrange
            LossControlMailHelper mailHelper = new LossControlMailHelper();

            IMailMessage mailMessage = FakeMailMessageFactory.GetMailMessageWithTwoAttachments();
            string attachments = mailHelper.GetAttachmentsAsString(mailMessage);

            //action

            //assert
            Assert.AreEqual(attachments, "document.pdf \n\r document.pdf");
        }

        [Test]
        public void PrepareMailAttachments_GivenMailWithThreeAttachment_ShouldReturnAttachmentNamesWithCrLf()
        {
            //arrange
            LossControlMailHelper mailHelper = new LossControlMailHelper();

            IMailMessage mailMessage = FakeMailMessageFactory.GetMailMessageWithThreeAttachments();
            string attachments = mailHelper.GetAttachmentsAsString(mailMessage);

            //action

            //assert
            Assert.AreEqual(attachments, "document.pdf \n\r document.pdf \n\r document.pdf");
        }

        [Test]
        public void PreparePositiveMailResponse_GivenMailWithValidAccount_ShouldPrepareValidPositiveResponse()
        {
            //arrange
            LossControlMailHelper mailHelper = new LossControlMailHelper();

            IMailMessage receivedMail = FakeMailMessageFactory.GetMailMessageWithSingleAttachment();

            //action
            IMailMessage responseMail = mailHelper.PreparePositiveMailResponse(receivedMail);

            //assert
            Assert.AreEqual(receivedMail.From, responseMail.To);
            Assert.AreEqual(receivedMail.To, responseMail.From);
            Assert.AreEqual("Received - " + receivedMail.Subject, responseMail.Subject);
        }

        [TestCase("Test.pdf", "Test", "pdf")]
        [TestCase("Test", "Test", "")]
        [TestCase("Test.", "Test", "")]
        [TestCase("Test.pdf.pdf", "Test.pdf", "pdf")]
        public void FindAttachementFileDetails_GivenFileNameWithExtention_ShouldReturnFileNameAndExtention(string testFileName, string expectedFielName, string expectedExtention)
        {
            //arrange
            string fileName = "";
            string fileExtention = "";

            LossControlMailHelper helper = new LossControlMailHelper();

            //action
            helper.GetAttachmentFileDetails(testFileName, out fileName, out fileExtention);

            //assert
            Assert.AreEqual(expectedFielName, fileName);
            Assert.AreEqual(expectedExtention, fileExtention);
        }

        [Test]
        public void PrepareNegativeMailResponse_GivenMailWithValidAccount_ShouldPrepareValidPositiveResponse()
        {
            //arrange
            LossControlMailHelper mailHelper = new LossControlMailHelper();

            IMailMessage receivedMail = FakeMailMessageFactory.GetMailMessageWithSingleAttachment();

            //action
            IMailMessage responseMail = mailHelper.PrepareNegativeMailResponse(receivedMail);

            //assert
            Assert.AreEqual(receivedMail.From, responseMail.To);
            Assert.AreEqual(receivedMail.To, responseMail.From);
            Assert.AreEqual("Failed - " + receivedMail.Subject, responseMail.Subject);
        }

        [Test]
        public void GetAttachedInvoice_GivenOneAttachmentInPdfFormatAndOneJpgFormat()
        {
            //arrange
            LossControlMailHelper mailHelper = new LossControlMailHelper();
            var mail = FakeMailMessageFactory.GetMailMessageWithSingleValidAttachment();
            
            //action
            var attachment = mailHelper.GetAttachedInvoice(mail);

            //assert
            Assert.That(attachment != null);

            Assert.That(attachment.AttachmentType.Equals(".pdf"));
        }

        [Test]
        public void GetAttachedInvoice_GivenOneInvalidAttachment()
        {
            //arrange
            LossControlMailHelper mailHelper = new LossControlMailHelper();
            var mail = FakeMailMessageFactory.GetMailMessageWithInValidAttachments();

            //action
            var attachment = mailHelper.GetAttachedInvoice(mail);

            //assert
            Assert.IsNull(attachment);
        }

        [Test]
        public void GetPreprocessedAttorneyInvoiceProcessModel_GivenValidMailMessage()
        {
            LossControlMailHelper mailHelper = new LossControlMailHelper();
            int loanNumber = 1408282;
            var email = FakeMailMessageFactory.GetMailMessageWithSingleValidAttachment();
            var attachedInvoice = email.Attachments.First(x => x.AttachmentType.Equals(".pdf"));
            var fileName = Path.GetFileNameWithoutExtension(attachedInvoice.AttachmentName);
            var fileExtension = "pdf";
            var expectedInvoiceProcessModel = new ReceiveAttorneyInvoiceProcessModel(loanNumber, email.DateRecieved,
                email.From, email.Subject, fileName, fileExtension, string.Empty, attachedInvoice.ContentAsBase64);
            //action
            var attorneyInvoiceProcessModel = mailHelper.GetPreProcessedThirdPartyInvoice(loanNumber, email);
            //assert
            Assert.That(attorneyInvoiceProcessModel != null);
            Assert.AreEqual(attorneyInvoiceProcessModel.Category, expectedInvoiceProcessModel.Category);
            Assert.AreEqual(attorneyInvoiceProcessModel.DateReceived, expectedInvoiceProcessModel.DateReceived);
            Assert.AreEqual(attorneyInvoiceProcessModel.EmailSubject, expectedInvoiceProcessModel.EmailSubject);
            Assert.AreEqual(attorneyInvoiceProcessModel.FileContentAsBase64, expectedInvoiceProcessModel.FileContentAsBase64);
            Assert.AreEqual(attorneyInvoiceProcessModel.InvoiceFileExtension, expectedInvoiceProcessModel.InvoiceFileExtension);
            Assert.AreEqual(attorneyInvoiceProcessModel.FromEmailAddress, expectedInvoiceProcessModel.FromEmailAddress);
            Assert.AreEqual(attorneyInvoiceProcessModel.InvoiceFileName, expectedInvoiceProcessModel.InvoiceFileName);
            Assert.AreEqual(attorneyInvoiceProcessModel.LoanNumber, expectedInvoiceProcessModel.LoanNumber);
        }
    }
}