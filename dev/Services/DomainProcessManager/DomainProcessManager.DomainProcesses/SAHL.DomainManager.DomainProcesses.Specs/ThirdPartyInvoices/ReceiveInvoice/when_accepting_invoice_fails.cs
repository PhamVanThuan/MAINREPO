using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Exchange;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Communication;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.Logging;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.ReceiveInvoice
{
    [Ignore("Issues with NSubstitute's fuzzy matchers that have side effects in other independent specs. Replaced with NUnit test instead")]
    public class when_accepting_invoice_fails : WithFakes
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
        private static ISystemMessageCollection messages;

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
            financeDomainService.WhenToldTo(f => f.PerformCommand(Param<AcceptThirdPartyInvoiceCommand>.Matches(m => m.AccountNumber == loanNumber)
                , Param.IsAny<IServiceRequestMetadata>())).Return(errorMessages);

            messages = SystemMessageCollection.Empty();
            acceptThirdPartyInvoiceStateMachine.WhenToldTo(x => x.SystemMessages).Return(messages);
        };

        private Because of = () =>
        {
            attorneyInvoiceProcessModel.Start(attorneyInvoice, string.Empty);
        };

        private It should_add_error_to_message_collection = () =>
        {
            acceptThirdPartyInvoiceStateMachine.SystemMessages.ErrorMessages().Any(msg => msg.Message.Contains("Internal server error"));
        };

        private It should_notify_support = () =>
        {
             communicationManager.WasToldTo(c => c.SendProcessingFailedEmailToLossControl(Param.IsAny<String>(),
                  Param.IsAny<List<IMailAttachment>>(), Param.IsAny<ISystemMessageCollection>()));    
        };

        private It should_format_the_attachment_correctly = () =>
        {
            communicationManager.WasToldTo(c => c.SendProcessingFailedEmailToLossControl(Param.IsAny<String>(),
                Arg.Is<List<IMailAttachment>>(
                x => x.First().AttachmentName == string.Format("{0}.{1}",attorneyInvoiceProcessModel.DataModel.InvoiceFileName, attorneyInvoiceProcessModel.DataModel.InvoiceFileExtension) 
                && x.First().AttachmentType == string.Format(".{0}",attorneyInvoiceProcessModel.DataModel.InvoiceFileExtension)), Param.IsAny<ISystemMessageCollection>()));  
        };
    }
}