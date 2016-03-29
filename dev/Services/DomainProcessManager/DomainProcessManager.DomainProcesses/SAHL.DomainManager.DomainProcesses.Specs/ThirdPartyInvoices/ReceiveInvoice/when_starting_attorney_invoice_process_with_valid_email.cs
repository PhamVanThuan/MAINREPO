using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Communication;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using SAHL.Services.Interfaces.Communications.Enums;
using SAHL.Services.Interfaces.DocumentManager.Models;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.ReceiveInvoice
{
    [Ignore("Issues with NSubstitute's fuzzy matchers that have side effects in other independent specs. Replaced with NUnit test instead")]
    public class when_starting_receive_attorney_invoice_process_with_valid_email : WithFakes
    {
        private static IAcceptThirdPartyInvoiceStateMachine acceptThirdPartyInvoiceStateMachine;
        private static IFinanceDomainServiceClient financeDomainService;
        private static ICommunicationManager communicationManager;
        private static ICombGuid combGuidGenerator;
        private static ReceiveAttorneyInvoiceProcess attorneyInvoiceProcessModel;
        private static ReceiveAttorneyInvoiceProcessModel attorneyInvoice;
        private static string mailFrom, mailSubject;
        private static int loanNumber;
        private static DateTime dateReceived;
        private static string fileName, base64encodedFileContents, fileExtension;
        private static IServiceRequestMetadata metadata;
        private static ThirdPartyInvoiceSubmissionAcceptedEvent thirdPartyInvoiceSubmissionAcceptedEvent;
        private static string sahlReference;

        private static IRawLogger rawLogger;
        private static ILoggerSource loggerSource;
        private static ILoggerAppSource loggerAppSource;

        private Establish context = () =>
        {
            acceptThirdPartyInvoiceStateMachine = An<IAcceptThirdPartyInvoiceStateMachine>();
            financeDomainService = An<IFinanceDomainServiceClient>();
            combGuidGenerator = An<ICombGuid>();
            communicationManager = An<ICommunicationManager>();

            rawLogger       = An<IRawLogger>();
            loggerSource    = An<ILoggerSource>();
            loggerAppSource = An<ILoggerAppSource>();

            loanNumber = 12345;
            mailFrom = "lawyer@attorneys.co.za";
            mailSubject = "342 My invoice";
            fileName = "My Invoice Document.pdf";
            fileExtension = "pdf";
            dateReceived = DateTime.Now.AddDays(-2);
            base64encodedFileContents = "VGhpcyBpcyBhIHRlc3QgYXR0YWNoZW1lbnQ=";

            attorneyInvoice = new ReceiveAttorneyInvoiceProcessModel(loanNumber, dateReceived, mailFrom, mailSubject, fileName, fileExtension, string.Empty, base64encodedFileContents);

            var attorneyInvoiceDocumentModel = new AttorneyInvoiceDocumentModel(
                     attorneyInvoice.LoanNumber.ToString()
                   , attorneyInvoice.DateReceived
                   , DateTime.Now
                   , attorneyInvoice.FromEmailAddress
                   , attorneyInvoice.EmailSubject
                   , attorneyInvoice.InvoiceFileName
                   , attorneyInvoice.InvoiceFileExtension
                   , string.Empty
                   , attorneyInvoice.FileContentAsBase64
                );

            attorneyInvoiceProcessModel = new ReceiveAttorneyInvoiceProcess(acceptThirdPartyInvoiceStateMachine, financeDomainService, combGuidGenerator, communicationManager,
                                                                     rawLogger, loggerSource, loggerAppSource);

            sahlReference = "SAHL03-2015/04/1";
            thirdPartyInvoiceSubmissionAcceptedEvent = new ThirdPartyInvoiceSubmissionAcceptedEvent(DateTime.Now, sahlReference, loanNumber, attorneyInvoiceDocumentModel);

            financeDomainService.WhenToldTo(f => f.PerformCommand(Param<AcceptThirdPartyInvoiceCommand>.Matches(
                m => m.AccountNumber == attorneyInvoice.LoanNumber
                  && m.InvoiceDocument.InvoiceFileExtension.Equals(attorneyInvoice.InvoiceFileExtension, StringComparison.Ordinal)
                  && m.InvoiceDocument.EmailSubject.Equals(attorneyInvoice.EmailSubject, StringComparison.Ordinal)
                  && m.InvoiceDocument.InvoiceFileName.Equals(attorneyInvoice.InvoiceFileName, StringComparison.Ordinal)
                  && m.InvoiceDocument.FileContentAsBase64.Equals(attorneyInvoice.FileContentAsBase64, StringComparison.Ordinal)
                )
                , Param.IsAny<IServiceRequestMetadata>())).Return(SystemMessageCollection.Empty());
        };

        private Because of = () =>
        {
            attorneyInvoiceProcessModel.Start(attorneyInvoice, string.Empty);

            metadata = new DomainProcessServiceRequestMetadata(attorneyInvoiceProcessModel.DomainProcessId, Guid.NewGuid());
            attorneyInvoiceProcessModel.HandleEvent(thirdPartyInvoiceSubmissionAcceptedEvent, metadata);
        };

        private It should_accept_third_party_invoice = () =>
        {
            financeDomainService.WasToldTo(f => f.PerformCommand(Param<AcceptThirdPartyInvoiceCommand>.Matches(
                m => m.AccountNumber == attorneyInvoice.LoanNumber
                  && m.InvoiceDocument.InvoiceFileExtension.Equals(attorneyInvoice.InvoiceFileExtension, StringComparison.Ordinal)
                  && m.InvoiceDocument.EmailSubject.Equals(attorneyInvoice.EmailSubject, StringComparison.Ordinal)
                  && m.InvoiceDocument.InvoiceFileName.Equals(attorneyInvoice.InvoiceFileName, StringComparison.Ordinal)
                  && m.InvoiceDocument.FileContentAsBase64.Equals(attorneyInvoice.FileContentAsBase64, StringComparison.Ordinal)
                )
                , Param.IsAny<IServiceRequestMetadata>()
            ));
        };

        private It should_give_success_feedback = () =>
        {
            communicationManager.WasToldTo(c => c.SendAcceptInvoiceConfirmationEmail(
                 Param<InvoiceTemplateType>.Matches(m => m.Equals(InvoiceTemplateType.SuccessfulInvoiceEmailTemplate))
               , Param<Guid>.Matches(m => m.Equals(attorneyInvoiceProcessModel.DomainProcessId))
               , Param<IEmailModel>.IsA<SuccessfulInvoiceSubmissionEmailModel>()
           ));
        };

        private It should_not_add_any_system_messages = () =>
        {
            acceptThirdPartyInvoiceStateMachine.SystemMessages.ErrorMessages().ShouldBeEmpty();
        };
    }
}