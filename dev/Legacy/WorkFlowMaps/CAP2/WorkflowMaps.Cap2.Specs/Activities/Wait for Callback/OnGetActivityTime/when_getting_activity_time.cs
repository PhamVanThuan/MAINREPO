using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Cap2.Specs.Activities.Wait_for_Callback.OnGetActivityTime
{
    [Subject("Activity => Wait_for_Callback => OnGetActivityTime")]
    internal class when_getting_activity_time : WorkflowSpecCap2
    {
        private static DateTime followUpTime;
        private static DateTime activityTime;
        private static ICommon client;

        private Establish context = () =>
        {
            client = An<ICommon>();
            followUpTime = new DateTime(2012, 1, 1);
            domainServiceLoader.RegisterMockForType<ICommon>(client);
            client.WhenToldTo(x => x.GetFollowupTime(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(followUpTime);
        };

        private Because of = () =>
        {
            activityTime = workflow.GetActivityTime_Wait_for_Callback(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_the_followup_time = () =>
        {
            activityTime.ShouldEqual(followUpTime);
        };
    }
}