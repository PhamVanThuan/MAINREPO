using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Triggers
{
    public class when_firing_address_captured_trigger_on_last_address : WithFakes
    {
        private static IApplicationStateMachine applicationStateMachine;
        private static int applicationNumber, addressExpected;

        private Establish context = () =>
        {
            applicationStateMachine = new ApplicationStateMachine();
            applicationNumber = 123456;

            var applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;

            Common.getApplicationStateMachineWithCriticalPathCaptured(applicationNumber, applicationStateMachine, applicationCreationModel);

            addressExpected = applicationCreationModel.Applicants.SelectMany(x => x.Addresses).Count();
        };

        private Because of = () =>
        {
            for (int i = 0; i < addressExpected; i++)
            {
                applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.AddressCaptureConfirmed, Guid.NewGuid());
            }
        };

        private It should_transition_to_all_address_captured_state = () =>
        {
            applicationStateMachine.Machine.IsInState(ApplicationState.AllAddressesCaptured).ShouldBeTrue();
        };
    }
}
