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
    public class when_creating_commands_for_system_request_group_with_two_decisions : WithFakes
    {
        private static AutoMocker<CommandFactory> automocker = new NSubstituteAutoMocker<CommandFactory>();
        private static IX2Request request;
        private static List<string> activityNames = new List<string>();
        private static long instanceId = 12;
        private static string activityName1 = "Decision1WithSplit";
        private static string activityName2 = "Decision2WithSplit";
        private static IEnumerable<IServiceCommand> commands;
        private static Activity activity1;
        private static Activity activity2;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            activity1 = new Activity(1, activityName1, 1, "FromState", 2, "ToState", 1, false);
            activity2 = new Activity(2, activityName2, 1, "FromState", 3, "OtherState", 1, false);
            activityNames.Add(activityName1);
            activityNames.Add(activityName2);
            serviceRequestMetadata = new ServiceRequestMetadata(new Dictionary<string, string>() 
                            {
                                { ServiceRequestMetadata.HEADER_USERNAME, "X2" }
                            });
            request = new X2SystemRequestGroup(Guid.NewGuid(), serviceRequestMetadata, X2RequestType.SystemRequestGroup, instanceId, activityNames);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivityForInstanceAndName(instanceId, activityName1)).Return(activity1);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivityForInstanceAndName(instanceId, activityName2)).Return(activity2);
        };

        private Because of = () =>
        {
            commands = automocker.ClassUnderTest.CreateCommands(request);
        };

        private It should_get_the_first_decision = () =>
        {
            automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetActivityForInstanceAndName(instanceId, activityName1));
        };

        private It should_get_the_second_decision = () =>
        {
            automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetActivityForInstanceAndName(instanceId, activityName2));
        };

        private It should_return_a_list_with_two_commands = () =>
        {
            commands.Count().ShouldEqual(2);
        };

        private It should_return_a_list_with_two_commands_that_are_a_systemrequestwithnosplitnoworkflowactivities = () =>
        {
            commands.First().ShouldBe(typeof(HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand));
        };
    }
}