using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.StateManagement
{
    public class when_application_employment_and_income_has_been_determined : WithFakes
    {
        private static ApplicationStateMachine applicationStateMachine;
        private static NewPurchaseApplicationCreationModel creationModel;
        private static int applicationNumber = 1;

        private Establish context = () =>
        {
            creationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;

            applicationStateMachine = new ApplicationStateMachine();
            applicationStateMachine.CreateStateMachine(creationModel, Guid.NewGuid());
            applicationStateMachine.TriggerBasicApplicationCreated(Guid.NewGuid(), applicationNumber);
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicantAdditionConfirmed, Guid.NewGuid());
            applicationStateMachine.TriggerEmploymentAdded(Guid.NewGuid(), 123);
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed, Guid.NewGuid());
        };

        private Because of = () =>
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationHouseHoldIncomeDeterminationConfirmed, Guid.NewGuid());
        };

        private It should_allow_application_to_be_priced = () =>
        {
            applicationStateMachine.Machine.CanFire(ApplicationStateTransitionTrigger.ApplicationPricingConfirmed).ShouldBeTrue();
        };
    }
}