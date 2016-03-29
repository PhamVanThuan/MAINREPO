using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Specs.RequestFactorySpecs
{
    public class when_asked_to_create_request_for_new_scheduled_activity : WithFakes
    {
        private static AutoMocker<RequestFactory> automocker;
        private static NotificationOfNewScheduledActivityCommand command;
        private static long instanceId = 10;
        private static int activityId = 13;
        private static IX2Request result;
        private static ScheduledActivityDataModel newScheduledActivityDataModel;
        private static ActivityDataModel newActivityDataModel;

        Establish context = () =>
            {
                automocker = new NSubstituteAutoMocker<RequestFactory>();
                newScheduledActivityDataModel = new ScheduledActivityDataModel(instanceId, DateTime.Now.AddSeconds(100), activityId, 1, "");
                newActivityDataModel = new ActivityDataModel(activityId, 1, "Name", 1, 1, 2, false, 1, 1, "", null, null, null, null, 1, Guid.NewGuid());
                automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetScheduledActivity(instanceId, activityId)).Return(newScheduledActivityDataModel);
                automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivity(activityId)).Return(newActivityDataModel);
                command = new NotificationOfNewScheduledActivityCommand(instanceId, activityId);
            };

        Because of = () =>
            {
                result = automocker.ClassUnderTest.CreateRequest(command);
            };

        It should_create_a_request = () =>
            {
                result.ShouldNotBeNull();
            };

        It should_create_a_system_request_group = () =>
            {
                result.ShouldBeOfExactType<X2SystemRequestGroup>();
            };

        It should_create_a_timer_system_request_group = () =>
        {
            result.RequestType.ShouldEqual(X2RequestType.Timer);
        };

        It should_create_a_valid_request = () =>
        {
            result.InstanceId.ShouldEqual(instanceId);
        };
    }
}
