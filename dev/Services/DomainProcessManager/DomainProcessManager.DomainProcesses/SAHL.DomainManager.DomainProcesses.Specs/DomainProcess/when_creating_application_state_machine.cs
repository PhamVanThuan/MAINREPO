using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.MachineStates
{
    internal class when_creating_application_state_machine : WithFakes
    {
        private static IApplicationStateMachine applicationStateMachine;

        private Establish context = () =>
        {
            applicationStateMachine = new ApplicationStateMachine();
        };

        private Because of = () =>
        {
            var applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            applicationStateMachine.CreateStateMachine(applicationCreationModel, Guid.NewGuid());
        };

        private It should_initialize_the_state_machine = () =>
        {
            applicationStateMachine.Machine.ShouldNotBeNull();
        };
    }
}
