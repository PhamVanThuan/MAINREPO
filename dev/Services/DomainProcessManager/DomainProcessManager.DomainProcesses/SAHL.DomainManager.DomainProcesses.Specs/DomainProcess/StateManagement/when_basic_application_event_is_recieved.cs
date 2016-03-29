using System;

using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;

namespace SAHL.DomainManager.DomainProcesses.Specs.DomainProcess.StateManagement
{
    public class when_basic_application_event_is_recieved : WithFakes
    {
        private static Guid guid;
        private static int applicationNumber;
        private static NewPurchaseApplicationCreationModel applicationCreationModel;
        private static ApplicationStateMachine applicationStateMachine;

        private Establish context = () =>
        {
            applicationNumber = 1324;
            guid = Guid.NewGuid();
            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;

            applicationStateMachine = new ApplicationStateMachine();
            applicationStateMachine.CreateStateMachine(applicationCreationModel, Guid.NewGuid());
        };

        private Because of = () =>
        {
            applicationStateMachine.Machine.Fire(applicationStateMachine.BasicApplicationCreatedTrigger, guid, applicationNumber);
        };

        private It should_record_the_application_number_for_later_use = () =>
        {
            applicationStateMachine.ApplicationNumber.ShouldEqual(applicationNumber);
        };

        private It should_transition_to_permitted_states_based_on_fired_trigger = () =>
        {
            applicationStateMachine.Machine.State.ShouldEqual(ApplicationState.BasicApplicationCreated);
        };

        private It should_record_the_state_in_the_state_breadcrumb = () =>
        {
            applicationStateMachine.StatesBreadCrumb.Keys.Contains(guid);
        };
    }
}