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
    public class when_creating_commands_for_a_user_create_complete_request : WithFakes
    {
        private static AutoMocker<CommandFactory> automocker = new NSubstituteAutoMocker<CommandFactory>();
        private static IX2Request request;
        private static Activity activity;
        private static long instanceId = 12;
        private static string userName = "userName", activityName = "activityName";
        private static IEnumerable<IServiceCommand> commands;
        private static IServiceRequestMetadata serviceRequestMetadata;

        Establish context = () =>
        {
            activity = new Activity(1, "doSomething", 1, "fromState", 2, "toState", 1, true);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivityForInstanceAndName(instanceId, activityName)).Return(activity);
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            request = new X2RequestForExistingInstance(Guid.NewGuid(), instanceId, X2RequestType.CreateComplete, serviceRequestMetadata, activityName, false);
        };

        Because of = () =>
        {
            commands = automocker.ClassUnderTest.CreateCommands(request);
        };

        It should_return_the_user_request_complete_create_command = () =>
        {
            commands.First().ShouldBe(typeof(UserRequestCompleteCreateCommand));
        };

        It should_get_an_activityviewmodel_for_the_requested_activity = () =>
        {
            automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetActivityForInstanceAndName(instanceId, activityName));
        };
    }
}