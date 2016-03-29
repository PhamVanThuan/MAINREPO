using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.StateManagement
{
    public class when_adding_employments_and_not_all_have_been_added : WithFakes
    {
        private static NewPurchaseApplicationCreationModel applicationCreationModel;
        private static int applicationNumber;
        private static ApplicationStateMachine applicationStateMachine;
        private static Exception thrownException;

        private Establish context = () =>
        {
            applicationNumber = 1234;

            var employments = new List<EmploymentModel> {
                ApplicationCreationTestHelper.PopulateSalariedEmploymentModel(),
                ApplicationCreationTestHelper.PopulateSalariedEmploymentModel()
            };
            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            var applicant = applicationCreationModel.Applicants.First();
            applicant.Employments = employments;

            applicationStateMachine = new ApplicationStateMachine();
            applicationStateMachine.CreateStateMachine(applicationCreationModel, Guid.NewGuid());
            applicationStateMachine.TriggerBasicApplicationCreated(Guid.NewGuid(), applicationNumber);
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicantAdditionConfirmed, Guid.NewGuid());
            applicationStateMachine.TriggerEmploymentAdded(Guid.NewGuid(), 123);
        };

        private Because of = () =>
        {
            thrownException = Catch.Exception(() => applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed, Guid.NewGuid()));
        };

        private It should_not_allow_a_trigger_to_the_next_state = () =>
        {
            thrownException.ShouldNotBeNull();
        };
    }
}