using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using System.Collections.Generic;

namespace SAHL.X2Engine2.Specs.NodeHandlerSpecs.UserHandlerSpecs
{
    public class when_handling_cancel_activity_request_not_for_create : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<UserRequestCancelActivityCommandHandler> autoMocker;

        private static UserRequestCancelActivityCommand userRequestCancelActivityCommand;
        private static InstanceDataModel instance;
        static Activity activity;
        static IServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<UserRequestCancelActivityCommandHandler>();

            metadata = new ServiceRequestMetadata(new Dictionary<string, string>() 
                            {
                                { SAHL.Core.Services.ServiceRequestMetadata.HEADER_USERNAME, "userName" }
                            });
            activity = new Activity(1, "activity", 1, "state1", 2, "state2", 1, false);
            instance = new InstanceDataModel(9, 1, null, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            userRequestCancelActivityCommand = new UserRequestCancelActivityCommand(instance.ID, activity, "userName");
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(userRequestCancelActivityCommand.InstanceId)).Return(instance);
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(userRequestCancelActivityCommand, metadata);
        };

        private It should_get_the_instance = () =>
        {
            autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetInstanceDataModel(userRequestCancelActivityCommand.InstanceId));
        };

        private It should_check_activity_is_valid_for_current_state = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.CheckRuleCommand(
                Arg.Is<CheckActivityIsValidForStateCommand>(command => command.StateId == userRequestCancelActivityCommand.Activity.StateId
                    && command.Instance == instance), metadata));
        };

        private It should_unlock_the_instance = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<UnlockInstanceCommand>(command => command.InstanceID == userRequestCancelActivityCommand.InstanceId), metadata));
        };
    }
}