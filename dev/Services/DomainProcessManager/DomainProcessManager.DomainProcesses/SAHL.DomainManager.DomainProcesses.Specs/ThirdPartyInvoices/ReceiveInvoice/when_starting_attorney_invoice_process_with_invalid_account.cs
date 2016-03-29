using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Communication;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using SAHL.Services.Interfaces.Communications.Enums;
using SAHL.Services.Interfaces.DocumentManager.Models;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess
{
    public class when_starting_receive_attorney_invoice_process_with_invalid_account : WithFakes
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
        private static ThirdPartyInvoiceSubmissionRejectedEvent thirdPartyInvoiceSubmissionRejectedEvent;
        private static IEnumerable<string> rejectionReasons;

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

            loanNumber = 0;
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
                   , null
                   , attorneyInvoice.FileContentAsBase64
                );

            rejectionReasons = new string[] { "Invalid account number" };
            thirdPartyInvoiceSubmissionRejectedEvent = new ThirdPartyInvoiceSubmissionRejectedEvent(DateTime.Now, loanNumber, attorneyInvoiceDocumentModel, rejectionReasons);

            attorneyInvoiceProcessModel = new ReceiveAttorneyInvoiceProcess(acceptThirdPartyInvoiceStateMachine, financeDomainService, combGuidGenerator, communicationManager,
                                                                     rawLogger, loggerSource, loggerAppSource);
        };

        private Because of = () =>
        {
            attorneyInvoiceProcessModel.Start(attorneyInvoice, string.Empty);

            metadata = new DomainProcessServiceRequestMetadata(attorneyInvoiceProcessModel.DomainProcessId, Guid.NewGuid());
            attorneyInvoiceProcessModel.HandleEvent(thirdPartyInvoiceSubmissionRejectedEvent, metadata);
        };

        private It should_handle_rejection = () =>
        {
            communicationManager.WasToldTo(c => c.SendAcceptInvoiceConfirmationEmail(
                  Param<InvoiceTemplateType>.Matches(m => m.Equals(InvoiceTemplateType.UnSuccessfulInvoiceEmailTemplate))
                , Param<Guid>.Matches(m => m.Equals(attorneyInvoiceProcessModel.DomainProcessId))
                , Param<IEmailModel>.IsA<UnSuccessfulInvoiceSubmissionEmailModel>()
            ));
        };

        private It should_not_add_any_system_message = () =>
        {
            acceptThirdPartyInvoiceStateMachine.SystemMessages.ErrorMessages().ShouldBeEmpty();
        };
    }
}