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

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Managers.Communication
{
    public class when_sending_non_critical_errors_email : WithFakes
    {
        private static CommunicationManager manager;
        private static ICommunicationsServiceClient communicationsService;
        private static ICombGuid combGuidGenerator;
        private static ICommunicationManagerSettings settings;

        private static int applicationNumber;
        private static string camTeamEmailAddress;
        private static ISystemMessageCollection systemMessages;
        private static string errorMessageText;
        private static string exceptionMessageText;

        private Establish context = () =>
        {
            communicationsService = An<ICommunicationsServiceClient>();
            combGuidGenerator = An<ICombGuid>();
            settings = An<ICommunicationManagerSettings>();
            manager = new CommunicationManager(communicationsService, combGuidGenerator, settings);

            applicationNumber = 44536;
            camTeamEmailAddress = "CamTeam@sahomeloans.com";
            settings.WhenToldTo(x => x.CamTeamEmailAddress).Return(camTeamEmailAddress);

            errorMessageText = "Could not find city of Gold";
            systemMessages = SystemMessageCollection.Empty();
            systemMessages.AddMessage(new SystemMessage(errorMessageText, SystemMessageSeverityEnum.Error));

            exceptionMessageText = "Object reference not found";
            systemMessages.AddMessage(new SystemMessage(exceptionMessageText, SystemMessageSeverityEnum.Exception));
        };

        private Because of = () =>
        {
            manager.SendNonCriticalErrorsEmail(systemMessages, applicationNumber);
        };

        private It should_send_an_email_to_cam_team = () =>
        {
            communicationsService.WasToldTo(x => x.PerformCommand(Param<SendInternalEmailCommand>.Matches(m =>
                m.EmailTemplate.EmailModel.To == camTeamEmailAddress &&
                m.EmailTemplate.GetType() == typeof(StandardEmailTemplate) &&
                ((StandardEmailModel)m.EmailTemplate.EmailModel).Body.Contains(errorMessageText)),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_not_send_exceptions_in_email_to_cam_team = () =>
        {
            communicationsService.WasNotToldTo(x => x.PerformCommand(Param<SendInternalEmailCommand>.Matches(m =>
                m.EmailTemplate.EmailModel.To == camTeamEmailAddress &&
                m.EmailTemplate.GetType() == typeof(StandardEmailTemplate) &&
                ((StandardEmailModel)m.EmailTemplate.EmailModel).Body.Contains(exceptionMessageText)),
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}