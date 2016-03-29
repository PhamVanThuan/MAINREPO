using System;
using System.Net.Mail;

using SAHL.Core.DomainProcess;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Communication;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using SAHL.Services.Interfaces.Communications.Enums;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess
{
    public abstract class ReceiveThirdPartyInvoiceDomainProcess<T> : DomainProcessBase<T>
        , IDomainProcessEvent<ThirdPartyInvoiceSubmissionAcceptedEvent>
        , IDomainProcessEvent<ThirdPartyInvoiceSubmissionRejectedEvent>
        where T : ReceiveThirdPartyInvoiceProcessModel
    {
        protected readonly IAcceptThirdPartyInvoiceStateMachine acceptThirdPartyInvoiceStateMachine;
        protected readonly IFinanceDomainServiceClient financeDomainService;
        protected readonly ICombGuid combGuidGenerator;
        protected readonly ICommunicationManager communicationManager;
        protected readonly int thirdPartyTypeKey;

        public ReceiveThirdPartyInvoiceDomainProcess(IAcceptThirdPartyInvoiceStateMachine acceptThirdPartyInvoiceStateMachine, 
                                              IFinanceDomainServiceClient financeDomainService, ICombGuid combGuidGenerator, 
                                              ICommunicationManager communicationManager, int thirdPartyTypeKey,
                                              IRawLogger rawLogger,
                                              ILoggerSource loggerSource,
                                              ILoggerAppSource loggerAppSource)
            : base(rawLogger, loggerSource, loggerAppSource)
        {
            this.acceptThirdPartyInvoiceStateMachine = acceptThirdPartyInvoiceStateMachine;
            this.financeDomainService = financeDomainService;
            this.combGuidGenerator = combGuidGenerator;
            this.communicationManager = communicationManager;
            this.thirdPartyTypeKey = thirdPartyTypeKey;
        }

        public void Handle(ThirdPartyInvoiceSubmissionAcceptedEvent thirdPartyInvoiceSubmissionAcceptedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            var successfulInvoiceEmailModel = new SuccessfulInvoiceSubmissionEmailModel(this.DataModel.FromEmailAddress, this.DataModel.EmailSubject, MailPriority.High,
                thirdPartyInvoiceSubmissionAcceptedEvent.SAHLReference);
            communicationManager.SendAcceptInvoiceConfirmationEmail(InvoiceTemplateType.SuccessfulInvoiceEmailTemplate, this.DomainProcessId, successfulInvoiceEmailModel);
        }

        public void Handle(ThirdPartyInvoiceSubmissionRejectedEvent thirdPartyInvoiceSubmissionRejectedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            var unSuccessfulInvoiceEmailModel = new UnSuccessfulInvoiceSubmissionEmailModel(this.DataModel.FromEmailAddress, this.DataModel.EmailSubject, MailPriority.High,
                thirdPartyInvoiceSubmissionRejectedEvent.RejectionReasons);
            communicationManager.SendAcceptInvoiceConfirmationEmail(InvoiceTemplateType.UnSuccessfulInvoiceEmailTemplate, this.DomainProcessId, unSuccessfulInvoiceEmailModel);
        }

        public override void HandledEvent(IServiceRequestMetadata metadata)
        {
            OnCompleted(this.DomainProcessId);
        }
    }
}