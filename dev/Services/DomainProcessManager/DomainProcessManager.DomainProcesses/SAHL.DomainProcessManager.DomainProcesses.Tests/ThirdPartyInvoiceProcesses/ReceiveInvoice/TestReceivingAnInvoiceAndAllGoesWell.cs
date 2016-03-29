using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Core.Data;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Communication;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using SAHL.Services.Interfaces.Communications.Enums;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Tests.ThirdPartyInvoiceProcesses.ReceiveInvoice
{
    [TestFixture]
    public class TestReceivingAnInvoiceAndAllGoesWell
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
        private ThirdPartyInvoiceSubmissionAcceptedEvent thirdPartyInvoiceSubmissionAcceptedEvent;
        private string sahlReference;
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

            financeDomainService.Expect(f => f.PerformCommand(Arg<AcceptThirdPartyInvoiceCommand>.Matches(m => m.AccountNumber == loanNumber)
                , Arg<IServiceRequestMetadata>.Is.Anything)).Return(SystemMessageCollection.Empty());

            var attorneyInvoiceDocumentModel = new AttorneyInvoiceDocumentModel(
                     loanNumber.ToString()
                   , dateReceived
                   , DateTime.Now
                   , mailFrom
                   , mailSubject
                   , fileName
                   , fileExtension
                   , string.Empty
                   , base64encodedFileContents
                );
            sahlReference = "SAHL03-2015/04/1";
            thirdPartyInvoiceSubmissionAcceptedEvent = new ThirdPartyInvoiceSubmissionAcceptedEvent(DateTime.Now, sahlReference, loanNumber, attorneyInvoiceDocumentModel);

            messages = SystemMessageCollection.Empty();
            acceptThirdPartyInvoiceStateMachine.Expect(x => x.SystemMessages).Return(messages);

            attorneyInvoiceProcessModel.Start(attorneyInvoice, string.Empty);

            var metadata = new DomainProcessServiceRequestMetadata(attorneyInvoiceProcessModel.DomainProcessId, Guid.NewGuid());
            attorneyInvoiceProcessModel.HandleEvent(thirdPartyInvoiceSubmissionAcceptedEvent, metadata);
        }

        [Test]
        public void Should_accept_third_party_invoice()
        {
            financeDomainService.AssertWasCalled(f => f.PerformCommand(Arg<AcceptThirdPartyInvoiceCommand>.Matches(
                m => m.AccountNumber == loanNumber
                  && m.InvoiceDocument.InvoiceFileExtension.Equals(fileExtension, StringComparison.Ordinal)
                  && m.InvoiceDocument.EmailSubject.Equals(mailSubject, StringComparison.Ordinal)
                  && m.InvoiceDocument.InvoiceFileName.Equals(fileName, StringComparison.Ordinal)
                  && m.InvoiceDocument.FileContentAsBase64.Equals(base64encodedFileContents, StringComparison.Ordinal)
                )
                , Arg<IServiceRequestMetadata>.Is.Anything
            ));
        }

        [Test]
        public void Should_give_success_feedback()
        {
            //---------------Execute Test ----------------------
            attorneyInvoiceProcessModel.Start(attorneyInvoice, string.Empty);

            //---------------Test Result -----------------------
            communicationManager.AssertWasCalled(c => c.SendAcceptInvoiceConfirmationEmail(
                 Arg<InvoiceTemplateType>.Matches(m => m.Equals(InvoiceTemplateType.SuccessfulInvoiceEmailTemplate))
               , Arg<Guid>.Matches(m => m.Equals(attorneyInvoiceProcessModel.DomainProcessId))
               , Arg<SuccessfulInvoiceSubmissionEmailModel>.Is.TypeOf
           ));
        }

        [Test]
        public void Should_not_add_any_system_messages()
        {
            //---------------Execute Test ----------------------
            attorneyInvoiceProcessModel.Start(attorneyInvoice, string.Empty);

            //---------------Test Result -----------------------
            Assert.IsTrue(messages.ErrorMessages().Count() == 0);
        }
    }
}