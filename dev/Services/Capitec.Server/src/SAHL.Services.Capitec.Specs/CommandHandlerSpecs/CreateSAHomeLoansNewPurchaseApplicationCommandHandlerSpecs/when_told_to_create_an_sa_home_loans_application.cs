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

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.CreateSAHomeLoansNewPurchaseApplicationCommandHandlerSpecs
{
    public class when_told_to_create_an_sa_home_loans_application : WithFakes
    {
        private static CreateSAHomeLoansNewPurchaseApplicationCommand command;
        private static CreateSAHomeLoansNewPurchaseApplicationCommandHandler handler;
        private static IRequestPublisherManager capitecRequestPublisher;
        private static NewPurchaseApplication capitecApplication;
        private static ICapitecApplicationManager capitecApplicationRepository;
        private static SAHL.Services.Interfaces.Capitec.ViewModels.Apply.NewPurchaseApplication newPurchaseApplication;
        static ServiceRequestMetadata metadata;
        private static Guid applicationId;

        private Establish context = () =>
        {
            applicationId = Guid.NewGuid();
            newPurchaseApplication = new SAHL.Services.Interfaces.Capitec.ViewModels.Apply.NewPurchaseApplication(null, null, new Guid(), "1184050800000-0700");
            capitecApplicationRepository = An<ICapitecApplicationManager>();
            capitecRequestPublisher = An<IRequestPublisherManager>();
            handler = new CreateSAHomeLoansNewPurchaseApplicationCommandHandler(capitecRequestPublisher, capitecApplicationRepository);
            command = new CreateSAHomeLoansNewPurchaseApplicationCommand(0, SystemMessageCollection.Empty(), Enumerations.ApplicationStatusEnums.InProgress, newPurchaseApplication, applicationId);
            capitecApplication = new NewPurchaseApplication(1, 1, DateTime.Now, null, null, 1, null, null);
            capitecApplicationRepository.WhenToldTo(x => x.CreateCapitecApplicationFromNewPurchaseApplication(command.ApplicationNumber, command.ApplicationType, command.NewPurchaseApplication, command.SystemMessageCollection, applicationId)).Return(capitecApplication);
            capitecRequestPublisher.WhenToldTo(x => x.PublishWithRetry(capitecApplication)).Return(true);
        };

        private Because of = () =>
            {
                handler.HandleCommand(command, metadata);
            };

        private It should_notify_sa_home_loans_to_create_an_application = () =>
            {
                capitecRequestPublisher.WasToldTo(x => x.PublishWithRetry(capitecApplication));
            };
    }
}