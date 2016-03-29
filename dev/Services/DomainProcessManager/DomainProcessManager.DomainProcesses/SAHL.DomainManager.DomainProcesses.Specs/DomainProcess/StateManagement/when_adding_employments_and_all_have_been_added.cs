using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.StateManagement
{
    public class when_adding_employments_and_all_have_been_added : WithFakes
    {
        private static NewPurchaseApplicationCreationModel applicationCreationModel;

        private static ApplicationStateMachine applicationStateMachine;

        private Establish context = () =>
        {
            const int applicationNumber = 1234;

            var employments = new List<EmploymentModel>
            {
                ApplicationCreationTestHelper.PopulateSalariedEmploymentModel(),
                ApplicationCreationTestHelper.PopulateSalaryDeductionEmploymentModel()
            };
            applicationCreationModel =
                ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            var applicant = applicationCreationModel.Applicants.First();
            applicant.Employments = employments;

            applicationStateMachine = new ApplicationStateMachine();
            applicationStateMachine.CreateStateMachine(applicationCreationModel, Guid.NewGuid());

            applicationStateMachine.TriggerBasicApplicationCreated(Guid.NewGuid(), applicationNumber);
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicantAdditionConfirmed, Guid.NewGuid());
            applicationStateMachine.TriggerEmploymentAdded(Guid.NewGuid(), 123);
            applicationStateMachine.TriggerEmploymentAdded(Guid.NewGuid(), 123456);
        };

        private Because of = () =>
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed, Guid.NewGuid());
        };

        private It should_allow_a_trigger_to_the_next_state = () =>
        {
            applicationStateMachine.IsInState(ApplicationState.ApplicationEmploymentDetermined).ShouldBeTrue();
        };
    }
}
