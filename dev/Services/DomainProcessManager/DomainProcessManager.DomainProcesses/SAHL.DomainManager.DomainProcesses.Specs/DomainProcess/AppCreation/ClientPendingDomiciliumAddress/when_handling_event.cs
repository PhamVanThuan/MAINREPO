using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.ClientPendingDomiciliumAddress
{
    public class when_handling_event : WithNewPurchaseDomainProcess
    {
        private static NewPurchaseApplicationCreationModel applicationCreationModel;
        private static ClientAddressAsPendingDomiciliumAddedEvent clientAddressAsPendingDomiciliumAddedEvent;
        private static int applicationNumber, clientKey, clientAddressKey, clientDomiciliumKey;

        private Establish context = () =>
        {
            clientAddressKey = 5656;
            clientKey = 1234124;
            applicationNumber = 123;
            clientDomiciliumKey = 3324;

            clientAddressAsPendingDomiciliumAddedEvent = new ClientAddressAsPendingDomiciliumAddedEvent(DateTime.Now, clientAddressKey, clientDomiciliumKey);

            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);

            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            domainProcess.DataModel = applicationCreationModel;
            domainProcess.ProcessState = applicationStateMachine;
            clientDataManager.WhenToldTo(x => x.GetClientKeyForClientAddress(clientAddressKey)).Return(clientKey);

            combGuidGenerator.WhenToldTo(x => x.Generate()).Return(new Guid());
        };

        private Because of = () =>
        {
            domainProcess.Handle(clientAddressAsPendingDomiciliumAddedEvent, serviceRequestMetadata);
        };

        private It should_fire_the_client_pending_domicilium_capture_confirmed_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(Arg.Is<ApplicationStateTransitionTrigger>(
                y => y == ApplicationStateTransitionTrigger.ClientPendingDomiciliumCaptureConfirmed), clientAddressAsPendingDomiciliumAddedEvent.Id));
        };
    }
}