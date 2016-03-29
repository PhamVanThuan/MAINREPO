using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.MachineStates.Processing
{
    public class when_firing_basic_application_creation_confirmed : WithFakes
    {
        private static IApplicationStateMachine applicationStateMachine;

        private Establish context = () =>
        {
            applicationStateMachine = new ApplicationStateMachine();

            var applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;

            applicationStateMachine.CreateStateMachine(applicationCreationModel, Guid.NewGuid());
        };

        private Because of = () =>
        {
            applicationStateMachine.TriggerBasicApplicationCreated(Guid.NewGuid(), 123456);
        };

        private It should_transition_to_the_correct_state = () =>
        {
            applicationStateMachine.Machine.IsInState(ApplicationState.BasicApplicationCreated).ShouldBeTrue();
        };
    }
}
