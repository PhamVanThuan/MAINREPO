using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Communication;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.Communications.EmailTemplates;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Managers.Communication
{
    public class when_sending_client_detail_comparison_email : WithFakes
    {
        private static CommunicationManager manager;
        private static ICommunicationsServiceClient communicationsService;
        private static ICombGuid combGuidGenerator;
        private static ICommunicationManagerSettings settings;

        private static int applicationNumber;
        private static string identityNumber;
        private static string legalEntityName;
        private static Dictionary<string, string> differences;
        private static string camTeamEmailAddress;

        private Establish context = () =>
        {
            communicationsService = An<ICommunicationsServiceClient>();
            combGuidGenerator = An<ICombGuid>();
            settings = An<ICommunicationManagerSettings>();
            manager = new CommunicationManager(communicationsService, combGuidGenerator, settings);

            applicationNumber = 44536;
            identityNumber = "925541258740012";
            legalEntityName = "Harry Brown";
            differences = new Dictionary<string, string> { { "DateOfBirth", new DateTime(1960, 3, 8).ToString() }, { "FirstName", "Harry" } };
            camTeamEmailAddress = "CamTeam@sahomeloans.com";
            settings.WhenToldTo(x => x.CamTeamEmailAddress).Return(camTeamEmailAddress);
        };

        private Because of = () =>
        {
            manager.SendClientDetailComparisonFailedEmail(differences, legalEntityName, identityNumber, applicationNumber, false);
        };

        private It should_send_an_email_to_cam_team = () =>
        {
            communicationsService.WasToldTo(x => x.PerformCommand(Param<SendInternalEmailCommand>.Matches(m =>
                m.EmailTemplate.EmailModel.To == camTeamEmailAddress &&
                m.EmailTemplate.GetType() == typeof(ComcorpComparisonEmailTemplate) &&
                ((ComcorpComparisonEmailModel)m.EmailTemplate.EmailModel).DifferingRecords.Count() == differences.Count()),
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}