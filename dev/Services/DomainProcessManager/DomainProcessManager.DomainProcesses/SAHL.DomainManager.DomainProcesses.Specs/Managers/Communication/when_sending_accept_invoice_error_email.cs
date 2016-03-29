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
using System;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Managers.Communication
{
    public class when_sending_accept_invoice_error_email : WithFakes
    {
        private static CommunicationManager manager;
        private static ICommunicationsServiceClient communicationsService;
        private static ICombGuid combGuidGenerator;
        private static ICommunicationManagerSettings settings;

        private static Guid domainProcessId;
        private static ISystemMessageCollection systemMessages, errorMessages;
        private static string mailTo;
        private static int attorneyInvoiceKey;

        private Establish context = () =>
        {
            communicationsService = An<ICommunicationsServiceClient>();
            combGuidGenerator = An<ICombGuid>();
            settings = An<ICommunicationManagerSettings>();
            manager = new CommunicationManager(communicationsService, combGuidGenerator, settings);
            domainProcessId = combGuidGenerator.Generate();
            attorneyInvoiceKey = 2478;
            mailTo = "_SAHLITFrontEndScrumTeam@sahomeloans.com";
            settings.WhenToldTo(x => x.ITFrontEndTeamEmailAddress).Return(mailTo);
            systemMessages = SystemMessageCollection.Empty();
            errorMessages = SystemMessageCollection.Empty();
            errorMessages.AddMessage(new SystemMessage("an internal error has occured", SystemMessageSeverityEnum.Error));
            communicationsService.WhenToldTo(x => x.PerformCommand(Param.IsAny<SendInternalEmailCommand>(), Param.IsAny<IServiceRequestMetadata>())).Return(systemMessages);
        };

        private Because of = () =>
        {
            manager.SendAcceptInvoiceErrorEmail(errorMessages, domainProcessId, attorneyInvoiceKey);
        };

        private It should_send_an_email_to_frontend_team = () =>
        {
            communicationsService.WasToldTo(x =>
                x.PerformCommand(Param<SendInternalEmailCommand>.Matches(m =>
                    m.EmailTemplate.EmailModel.To == mailTo &&
                    m.EmailTemplate.EmailModel.GetType() == typeof(StandardEmailModel) &&
                    (m.EmailTemplate.EmailModel as StandardEmailModel).Body.Contains(errorMessages.AllMessages.First().Message) &&
                    m.EmailTemplate.GetType() == typeof(StandardEmailTemplate)
                    ),
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}