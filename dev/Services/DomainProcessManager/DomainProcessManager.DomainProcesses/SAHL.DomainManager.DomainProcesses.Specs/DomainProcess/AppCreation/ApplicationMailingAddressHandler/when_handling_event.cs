using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.ApplicationMailingAddressHandler
{
    public class when_handling_event : WithNewPurchaseDomainProcess
    {
        private static int applicationNumber;
        private static int clientAddressKey;
        private static MailingAddressAddedToApplicationEvent mailingAddressAddedToApplicationEvent;

        private Establish context = () =>
        {
            applicationNumber = 123412214;
            clientAddressKey = 345;
            mailingAddressAddedToApplicationEvent = new MailingAddressAddedToApplicationEvent(DateTime.Now,
                applicationNumber,
                clientAddressKey,
                CorrespondenceLanguage.English,
                true,
                OnlineStatementFormat.HTML,
                CorrespondenceMedium.Email,
                12345);

            applicationDomainService.WhenToldTo(
                x => x.PerformCommand(Param.IsAny<AddApplicationMailingAddressCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                .Return(SystemMessageCollection.Empty());

            applicationStateMachine.WhenToldTo(x => x.ContainsStateInBreadCrumb(ApplicationState.AllAddressesCaptured)).Return(true);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(mailingAddressAddedToApplicationEvent, serviceRequestMetadata);
        };

        private It should_raise_applicant_mailing_address_added_trigger = () =>
        {
            applicationStateMachine.WasToldTo(
                x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationMailingAddressCaptureConfirmed, mailingAddressAddedToApplicationEvent.Id));
        };
    }
}
