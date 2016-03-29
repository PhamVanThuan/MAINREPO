using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Communication;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.Communications.EmailTemplates;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using SAHL.Services.Interfaces.Communications.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Managers.Communication
{
    public class when_sending_third_party_invoice_accepted_email : WithFakes
    {
        private static CommunicationManager manager;
        private static ICommunicationsServiceClient communicationsService;
        private static ICombGuid combGuidGenerator;
        private static ICommunicationManagerSettings settings;
        
        private static InvoiceTemplateType templateName;
        private static IEmailModel model;
        private static Guid domainProcessId;
        private static ISystemMessageCollection systemMessages;
        private static string mailTo;
        private static string sahlReference;

        private Establish context = () =>
        {
            communicationsService = An<ICommunicationsServiceClient>();
            combGuidGenerator = An<ICombGuid>();
            settings = An<ICommunicationManagerSettings>();
            manager = new CommunicationManager(communicationsService, combGuidGenerator, settings);
            mailTo = "person@domain.com";
            sahlReference = "SAHL-2014/09/2";
            domainProcessId = combGuidGenerator.Generate();
            templateName = InvoiceTemplateType.SuccessfulInvoiceEmailTemplate;
            systemMessages = SystemMessageCollection.Empty();
            model = new SuccessfulInvoiceSubmissionEmailModel(mailTo, "subjective", System.Net.Mail.MailPriority.Normal, sahlReference);
            communicationsService.WhenToldTo(x => x.PerformCommand(Param.IsAny<SendInternalEmailCommand>(), Param.IsAny<IServiceRequestMetadata>())).Return(systemMessages);            
        };

        private Because of = () =>
        {
            manager.SendAcceptInvoiceConfirmationEmail(templateName, domainProcessId, model);
        };

        private It should_send_an_email_to_frontend_team = () =>
        {
            communicationsService.WasToldTo(x =>
                x.PerformCommand(Param<SendInternalEmailCommand>.Matches(m =>
                    m.EmailTemplate.TemplateName.ToString() == templateName.ToString() &&
                    m.EmailTemplate.EmailModel.To == mailTo &&
                    m.EmailTemplate.GetType() == typeof(InvoiceEmailTemplate) 
                    ), 
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}