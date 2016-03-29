using Machine.Specifications;
using System;

namespace WorkflowMaps.Life.Specs.Activities.Create_Clone.OnGetActivityTime
{
    [Subject("Activity => Create_Clone => OnGetActivityTime")]
    internal class when_create_clone : WorkflowSpecLife
    {
        private static DateTime result;
        private static DateTime expectedResult;

        private Establish context = () =>
        {
            expectedResult = DateTime.Now;
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_Create_Clone(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_current_date_time = () =>
        {
            result.ShouldBeCloseTo(expectedResult, new TimeSpan(0, 0, 1));
        };
    }
}