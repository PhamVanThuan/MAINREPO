using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;

namespace SAHL.DomainManager.DomainProcesses.Specs.DomainProcess.StateManagement
{
    public class when_all_address_substates_events_have_been_received : WithFakes
    {
        private static Guid guid;
        private static int applicationNumber;
        private static ApplicationStateMachine applicationStateMachine;
        private static NewPurchaseApplicationCreationModel applicationCreationModel;

        private Establish context = () =>
        {
            applicationNumber = 1324;
            guid = Guid.NewGuid();
            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;

            applicationStateMachine = new ApplicationStateMachine();
            Common.getApplicationStateMachineWithCriticalPathCaptured(applicationNumber, applicationStateMachine);
        };

        private Because of = () =>
        {
            applicationStateMachine.Machine.Fire(applicationStateMachine.AddressCapturedTrigger, Guid.NewGuid());
            applicationStateMachine.Machine.Fire(applicationStateMachine.AddressCapturedTrigger, Guid.NewGuid());
            applicationStateMachine.Machine.Fire(applicationStateMachine.AddressCapturedTrigger, Guid.NewGuid());
        };

        private It should_auto_transition_to_parent = () =>
        {
            applicationStateMachine.Machine.State.ShouldEqual(ApplicationState.AllAddressesCaptured);
        };

        private It should_be_able_to_fire_mailing_address_added = () =>
        {
            applicationStateMachine.Machine.CanFire(ApplicationStateTransitionTrigger.ApplicationMailingAddressCaptureConfirmed).ShouldBeTrue();
        };

        private It should_be_able_to_fire_domicilium_address_capture_confirmed_trigger = () =>
        {
            applicationStateMachine.Machine.CanFire(ApplicationStateTransitionTrigger.DomiciliumAddressCaptureConfirmed).ShouldBeTrue();
        };

        private It should_not_be_able_to_add_more_addresses = () =>
        {
            applicationStateMachine.Machine.CanFire(ApplicationStateTransitionTrigger.AddressCaptureConfirmed).ShouldBeFalse();
            applicationStateMachine.Machine.CanFire(ApplicationStateTransitionTrigger.AllAddressesCaptured).ShouldBeFalse();
        };
    }
}