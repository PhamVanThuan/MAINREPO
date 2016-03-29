using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using X2DomainService.Interface.Life;

namespace WorkflowMaps.Life.Specs.Activities.Wait_for_Callback.OnGetActivityTime
{
    [Subject("Activity => Wait_for_Callback => OnGetActivityTime")]
    internal class when_getting_activity_time : WorkflowSpecLife
    {
        private static ILife client;
        private static DateTime activityTime;
        private static DateTime expectedDate;

        private Establish context = () =>
        {
            expectedDate = DateTime.Now;
            client = An<ILife>();
            client.WhenToldTo(x => x.GetActivityTimeWaitForCallback(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()))
                .Return(expectedDate);
            domainServiceLoader.RegisterMockForType<ILife>(client);
        };

        private Because of = () =>
        {
            activityTime = workflow.GetActivityTime_Wait_for_Callback(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_the_activity_time = () =>
        {
            activityTime.ShouldEqual(expectedDate);
        };
    }
}