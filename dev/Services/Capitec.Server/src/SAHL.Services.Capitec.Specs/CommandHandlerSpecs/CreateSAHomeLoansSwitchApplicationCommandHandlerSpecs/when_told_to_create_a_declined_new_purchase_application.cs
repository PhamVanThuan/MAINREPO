using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.CommandHandlers;
using SAHL.Services.Capitec.Managers.CapitecApplication;
using SAHL.Services.Capitec.Managers.RequestPublisher;
using SAHL.Services.Capitec.Models.Shared;
using SAHL.Services.Interfaces.Capitec.Commands;
using SAHL.Services.Interfaces.Capitec.Common;
using System;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.CreateSAHomeLoansSwitchApplicationCommandHandlerSpecs
{
    public class when_told_to_create_a_declined_switch_application : WithFakes
    {
        private static CreateSAHomeLoansSwitchApplicationCommand command;
        private static CreateSAHomeLoansSwitchApplicationCommandHandler handler;
        private static IRequestPublisherManager capitecRequestPublisher;
        private static SwitchLoanApplication capitecSwitchApplication;
        private static ICapitecApplicationManager capitecApplicationRepository;
        private static SAHL.Services.Interfaces.Capitec.ViewModels.Apply.SwitchLoanApplication switchApplication;
        private static ISystemMessageCollection systemMessageCollection;
        static ServiceRequestMetadata metadata;
        private static Guid applicationId;

        private Establish context = () =>
        {
            applicationId = Guid.NewGuid();
            systemMessageCollection = SystemMessageCollection.Empty();
            switchApplication = new SAHL.Services.Interfaces.Capitec.ViewModels.Apply.SwitchLoanApplication(null, null, new Guid(), "1184050800000-0700");
            capitecApplicationRepository = An<ICapitecApplicationManager>();
            capitecRequestPublisher = An<IRequestPublisherManager>();
            capitecApplicationRepository = An<ICapitecApplicationManager>();

            command = new CreateSAHomeLoansSwitchApplicationCommand(0, systemMessageCollection, Enumerations.ApplicationStatusEnums.Decline, switchApplication, applicationId);

            capitecApplicationRepository.WhenToldTo(x => x.CreateCapitecApplicationFromSwitchLoanApplication(command.ApplicationNumber, command.ApplicationType, command.SwitchLoanApplication
                , command.SystemMessageCollection, applicationId)).Return(capitecSwitchApplication);
            capitecRequestPublisher.WhenToldTo(x => x.PublishWithRetry(capitecSwitchApplication)).Return(true);


            handler = new CreateSAHomeLoansSwitchApplicationCommandHandler(capitecRequestPublisher, capitecApplicationRepository);

        };

        Because of = () =>
        {
            handler.HandleCommand(command, metadata);
        };

        It should_create_an_application_with_a_status_of_decline = () =>
        {
            capitecApplicationRepository.WasToldTo(x => x.CreateCapitecApplicationFromSwitchLoanApplication(command.ApplicationNumber, command.ApplicationType, command.SwitchLoanApplication, command.SystemMessageCollection, applicationId));
        };

        It should_notify_sa_home_loans_to_create_an_application = () =>
        {
            capitecRequestPublisher.WasToldTo(x => x.PublishWithRetry(capitecSwitchApplication));
        };
    }
}
