using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Triggers
{
    public class when_in_basic_application_created : WithFakes
    {
        private static IApplicationStateMachine applicationStateMachine;
        private static List<ApplicationStateTransitionTrigger> permittedTriggers;

        private static ApplicationCreationModel applicationCreationModel { get; set; }

        private static int applicationNumber = 123456;

        private Establish context = () =>
        {
            permittedTriggers = new List<ApplicationStateTransitionTrigger> {
                ApplicationStateTransitionTrigger.ApplicantAdditionConfirmed,
                ApplicationStateTransitionTrigger.CriticalErrorReported
            };

            applicationStateMachine = new ApplicationStateMachine();
            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;

            applicationStateMachine.CreateStateMachine(applicationCreationModel, Guid.NewGuid());
        };

        private Because of = () =>
        {
            applicationStateMachine.TriggerBasicApplicationCreated(Guid.NewGuid(), applicationNumber);
        };

        private It should_transition_state_to_basic_application_created_state = () =>
        {
            applicationStateMachine.Machine.IsInState(ApplicationState.BasicApplicationCreated).ShouldBeTrue();
        };
    }
}
