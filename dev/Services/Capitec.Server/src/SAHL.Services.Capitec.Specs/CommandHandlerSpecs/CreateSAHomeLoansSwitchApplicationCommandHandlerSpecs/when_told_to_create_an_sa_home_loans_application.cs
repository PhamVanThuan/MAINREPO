using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.CommandHandlers;
using SAHL.Services.Capitec.Managers.CapitecApplication;
using SAHL.Services.Capitec.Managers.RequestPublisher;
using SAHL.Services.Capitec.Models.Shared;
using SAHL.Services.Interfaces.Capitec.Commands;
using SAHL.Services.Interfaces.Capitec.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.Services;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.CreateSAHomeLoansSwitchApplicationCommandHandlerSpecs
{
    public class when_told_to_create_an_sa_home_loans_application : WithFakes
    {
        private static CreateSAHomeLoansSwitchApplicationCommand command;
        private static CreateSAHomeLoansSwitchApplicationCommandHandler handler;
        private static IRequestPublisherManager capitecRequestPublisher;
        private static SwitchLoanApplication capitecApplication;
        private static ICapitecApplicationManager capitecApplicationRepository;
        private static SAHL.Services.Interfaces.Capitec.ViewModels.Apply.SwitchLoanApplication SwitchApplication;
        private static IList<string> sharedMessages = new List<string>();
        static ServiceRequestMetadata metadata;
 
        Establish context = () =>
            {
                SwitchApplication = new SAHL.Services.Interfaces.Capitec.ViewModels.Apply.SwitchLoanApplication(null, null, new Guid(), "1184050800000-0700");
                capitecApplicationRepository = An<ICapitecApplicationManager>();
                capitecRequestPublisher = An<IRequestPublisherManager>();
                handler = new CreateSAHomeLoansSwitchApplicationCommandHandler(capitecRequestPublisher, capitecApplicationRepository);
                command = new CreateSAHomeLoansSwitchApplicationCommand(0, SystemMessageCollection.Empty(), Enumerations.ApplicationStatusEnums.InProgress, SwitchApplication, Guid.NewGuid());
                capitecApplication = new SwitchLoanApplication(1, 1, DateTime.Now, null, null, 1, null, sharedMessages);
                capitecApplicationRepository.WhenToldTo(x => x.CreateCapitecApplicationFromSwitchLoanApplication(command.ApplicationNumber, command.ApplicationType, command.SwitchLoanApplication, command.SystemMessageCollection, command.ApplicationId)).Return(capitecApplication);
                capitecRequestPublisher.WhenToldTo(x => x.PublishWithRetry(capitecApplication)).Return(true);
            };

        private Because of = () =>
            {
                handler.HandleCommand(command,metadata);
            };

        private It should_notify_sa_home_loans_to_create_an_application = () =>
            {
                capitecRequestPublisher.WasToldTo(x => x.PublishWithRetry(capitecApplication));
            };
    }
}