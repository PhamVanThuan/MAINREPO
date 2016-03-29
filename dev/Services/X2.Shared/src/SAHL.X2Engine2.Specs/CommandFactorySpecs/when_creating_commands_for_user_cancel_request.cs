using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandFactorySpecs
{
    public class when_creating_commands_for_user_cancel_request : WithFakes
    {
        private static AutoMocker<CommandFactory> automocker = new NSubstituteAutoMocker<CommandFactory>();
        private static IX2Request request;
        private static Activity activity;
        private static long instanceId = 12;
        private static string userName = "userName", activityName = "activityName";
        private static IEnumerable<IServiceCommand> commands;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            activity = new Activity(1, "doSomething", 1, "fromState", 2, "toState", 1, false);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivityForInstanceAndName(instanceId, activityName)).Return(activity);
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            request = new X2RequestForExistingInstance(Guid.NewGuid(), instanceId, X2RequestType.UserCancel, serviceRequestMetadata, activityName, false);
        };

        private Because of = () =>
        {
            commands = automocker.ClassUnderTest.CreateCommands(request);
        };

        private It should_get_an_ativityviewmodel_for_the_requested_activity = () =>
        {
            automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetActivityForInstanceAndName(instanceId, activityName));
        };

        private It should_return_a_userrequestcancelactivitycommand = () =>
        {
            commands.First().ShouldBe(typeof(UserRequestCancelActivityCommand));
        };
    }
}