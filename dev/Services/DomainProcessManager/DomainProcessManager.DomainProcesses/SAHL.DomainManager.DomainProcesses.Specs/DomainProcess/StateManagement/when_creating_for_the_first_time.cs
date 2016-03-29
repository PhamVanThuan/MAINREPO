using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.StateManagement
{
    public class when_creating_for_the_first_time : WithFakes
    {
        private static NewPurchaseApplicationCreationModel applicationCreationModel;
        private static ApplicationStateMachine applicationStateMachine;

        private Establish context = () =>
        {
            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
        };

        private Because of = () =>
        {
            applicationStateMachine = new ApplicationStateMachine();
            applicationStateMachine.CreateStateMachine(applicationCreationModel, Guid.NewGuid());
        };

        private It should_initialize_to_processing_state = () =>
        {
            applicationStateMachine.Machine.State.ShouldEqual(ApplicationState.Processing);
        };

        private It should_setup_triggers_with_correlationIds_for_duplicate_event_detection = () =>
        {
            applicationStateMachine.AddressCapturedTrigger.ShouldNotBeNull();
        };
    }
}