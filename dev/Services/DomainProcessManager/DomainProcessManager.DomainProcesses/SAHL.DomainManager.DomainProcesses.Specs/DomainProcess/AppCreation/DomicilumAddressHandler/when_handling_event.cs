using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.DomicilumAddress
{
    public class when_handling_event : WithNewPurchaseDomainProcess
    {
        private static NewPurchaseApplicationCreationModel applicationCreationModel;
        private static DomiciliumAddressLinkedToApplicantEvent domiciliumAddressLinkedToApplicantEvent;
        private static int applicationNumber;

        private static int clientKey, clientDomiciliumKey;

        private Establish context = () =>
        {
            clientDomiciliumKey = 1001;
            clientKey = 5656;

            applicationNumber = 123;
            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            domainProcess.DataModel = applicationCreationModel;

            domiciliumAddressLinkedToApplicantEvent = new DomiciliumAddressLinkedToApplicantEvent(DateTime.Now, 1, clientKey, applicationNumber, clientDomiciliumKey);

            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
            combGuidGenerator.WhenToldTo(x => x.Generate()).Return(new Guid());
        };

        private Because of = () =>
        {
            domainProcess.Handle(domiciliumAddressLinkedToApplicantEvent, serviceRequestMetadata);
        };

        private It should_fire_the_application_linking_to_external_vendor_confirmed_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(Arg.Is<ApplicationStateTransitionTrigger>(y => y == ApplicationStateTransitionTrigger.DomiciliumAddressCaptureConfirmed), domiciliumAddressLinkedToApplicantEvent.Id));
        };
    }
}