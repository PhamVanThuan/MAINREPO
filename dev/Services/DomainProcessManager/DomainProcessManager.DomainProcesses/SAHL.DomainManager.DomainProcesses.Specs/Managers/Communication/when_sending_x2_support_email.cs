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

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Managers.Communication
{
    public class when_sending_x2_support_email : WithFakes
    {
        private static CommunicationManager manager;
        private static ICommunicationsServiceClient communicationsService;
        private static ICombGuid combGuidGenerator;
        private static ICommunicationManagerSettings settings;

        private static int applicationNumber;
        private static string frontTeamEmailAddress;

        private static Guid domainProcessId;
        private static ISystemMessageCollection systemMessages;
        private static string supportMessage;

        private Establish context = () =>
        {
            communicationsService = An<ICommunicationsServiceClient>();
            combGuidGenerator = An<ICombGuid>();
            settings = An<ICommunicationManagerSettings>();
            manager = new CommunicationManager(communicationsService, combGuidGenerator, settings);

            domainProcessId = combGuidGenerator.Generate();

            applicationNumber = 44536;

            frontTeamEmailAddress = "_SAHLITFrontEndScrumTeam@sahomeloans.com";
            settings.WhenToldTo(x => x.ITFrontEndTeamEmailAddress).Return(frontTeamEmailAddress);

            supportMessage = "Object not set to an instance of an object";
            systemMessages = SystemMessageCollection.Empty();
            systemMessages.AddMessage(new SystemMessage(supportMessage, SystemMessageSeverityEnum.Exception));
        };

        private Because of = () =>
        {
            manager.SendX2CaseCreationFailedSupportEmail(systemMessages, domainProcessId, applicationNumber);
        };

        private It should_send_an_email_to_frontend_team = () =>
        {
            communicationsService.WasToldTo(x => x.PerformCommand(Param<SendInternalEmailCommand>.Matches(m =>
                m.EmailTemplate.EmailModel.To == frontTeamEmailAddress &&
                m.EmailTemplate.GetType() == typeof(StandardEmailTemplate) &&
                ((StandardEmailModel)m.EmailTemplate.EmailModel).Body.Contains(supportMessage)),
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}