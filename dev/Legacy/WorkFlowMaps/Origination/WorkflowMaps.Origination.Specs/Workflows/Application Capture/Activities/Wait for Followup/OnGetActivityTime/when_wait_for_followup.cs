using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.ApplicationCapture.Specs.Workflows.Application_Capture.Activities.Wait_for_Followup.OnGetActivityTime
{
    [Subject("Activity => Wait_for_Followup => OnGetActivityTime")]
    internal class when_wait_for_followup : WorkflowSpecApplicationCapture
    {
        private static DateTime result;
        private static DateTime expectedResult;

        private Establish context = () =>
        {
            result = DateTime.MinValue;
            expectedResult = DateTime.MaxValue;
            var client = An<ICommon>();
            client.WhenToldTo(x => x.GetFollowupTime(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(expectedResult);
            domainServiceLoader.RegisterMockForType<ICommon>(client);
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_Wait_for_Followup(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_date_time = () =>
        {
            result.ShouldEqual<DateTime>(expectedResult);
        };
    }
}