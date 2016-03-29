using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Core.Data;
using SAHL.Core.Exchange;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Communication;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Tests.ThirdPartyInvoiceProcesses.ReceiveInvoice
{
    [TestFixture]
    public class TestReceivingAnInvoiceWhenAcceptingInvoiceFails
    {
        private IAcceptThirdPartyInvoiceStateMachine acceptThirdPartyInvoiceStateMachine;
        private IDataModel attorneyInvoice;
        private ReceiveAttorneyInvoiceProcess attorneyInvoiceProcessModel;
        private string base64encodedFileContents;
        private ICombGuid combGuidGenerator;
        private ICommunicationManager communicationManager;
        private DateTime dateReceived;
        private string fileExtension;
        private string fileName;
        private IFinanceDomainServiceClient financeDomainService;
        private int loanNumber;
        private ILoggerAppSource loggerAppSource;
        private ILoggerSource loggerSource;
        private string mailFrom;
        private string mailSubject;
        private IRawLogger rawLogger;
        private ISystemMessageCollection messages;

        [TestFixtureSetUp]
        public void TestContextSetup()
        {
            //---------------Set up test pack-------------------
            acceptThirdPartyInvoiceStateMachine = MockRepository.GenerateMock<IAcceptThirdPartyInvoiceStateMachine>();
            financeDomainService = MockRepository.GenerateMock<IFinanceDomainServiceClient>();
            combGuidGenerator = MockRepository.GenerateMock<ICombGuid>();
            communicationManager = MockRepository.GenerateMock<ICommunicationManager>();
            rawLogger = NSubstitute.Substitute.For<IRawLogger>();
            loggerSource = NSubstitute.Substitute.For<ILoggerSource>();
            loggerAppSource = NSubstitute.Substitute.For<ILoggerAppSource>();

            loanNumber = 9843;
            mailFrom = "lawyer@attorneys.co.za";
            mailSubject = "342 My invoice";
            fileName = "MyDocument";
            fileExtension = "pdf";
            dateReceived = DateTime.Now.AddDays(-2);
            base64encodedFileContents = "VGhpcyBpcyBhIHRlc3QgYXR0YWNoZW1lbnQ=";

            attorneyInvoice = new ReceiveAttorneyInvoiceProcessModel(loanNumber, dateReceived, mailFrom, mailSubject, fileName, fileExtension, string.Empty, base64encodedFileContents);

            attorneyInvoiceProcessModel = new ReceiveAttorneyInvoiceProcess(acceptThirdPartyInvoiceStateMachine, financeDomainService, combGuidGenerator, communicationManager,
                                                                     rawLogger, loggerSource, loggerAppSource);

            var errorMessages = SystemMessageCollection.Empty();
            errorMessages.AddMessage(new SystemMessage("Internal server error", SystemMessageSeverityEnum.Error));
            financeDomainService.Expect(f => f.PerformCommand(Arg<AcceptThirdPartyInvoiceCommand>.Matches(m => m.AccountNumber == loanNumber)
                , Arg<IServiceRequestMetadata>.Is.Anything)).Return(errorMessages);

            messages = SystemMessageCollection.Empty();
            acceptThirdPartyInvoiceStateMachine.Expect(x => x.SystemMessages).Return(messages);
        }

        [Test]
        public void Should_add_error_message_to_collection()
        {
            //---------------Execute Test ----------------------
            attorneyInvoiceProcessModel.Start(attorneyInvoice, string.Empty);

            //---------------Test Result -----------------------
            Assert.IsTrue(messages.ErrorMessages().Any(msg => msg.Message.Contains("Internal server error")));
        }

        [Test]
        public void Should_notify_support()
        {
            //---------------Execute Test ----------------------
            attorneyInvoiceProcessModel.Start(attorneyInvoice, string.Empty);

            //---------------Test Result -----------------------
            communicationManager.AssertWasCalled(c => c.SendProcessingFailedEmailToLossControl(Arg<String>.Is.Anything,
                  Arg<List<IMailAttachment>>.Is.Anything, Arg<ISystemMessageCollection>.Is.Anything));
        }

        [Test]
        public void Should_format_the_attachment_correctly()
        {
            //---------------Execute Test ----------------------
            attorneyInvoiceProcessModel.Start(attorneyInvoice, string.Empty);

            //---------------Test Result -----------------------
            communicationManager.AssertWasCalled(c => c.SendProcessingFailedEmailToLossControl(Arg<String>.Is.Anything,
                Arg<List<IMailAttachment>>.Matches(
                    x => x.First().AttachmentName ==
                       string.Format("{0}.{1}", attorneyInvoiceProcessModel.DataModel.InvoiceFileName, attorneyInvoiceProcessModel.DataModel.InvoiceFileExtension)
                    && x.First().AttachmentType ==
                       string.Format(".{0}", attorneyInvoiceProcessModel.DataModel.InvoiceFileExtension)), Arg<ISystemMessageCollection>.Is.Anything));
        }
    }
}