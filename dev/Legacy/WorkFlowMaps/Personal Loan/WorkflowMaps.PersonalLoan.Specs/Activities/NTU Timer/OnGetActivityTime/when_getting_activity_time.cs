using Machine.Specifications;
using System;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.NTU_Timer.OnGetActivityTime
{
    [Subject("Activity => NTU_Timer => OnGetActivityTime")]
    internal class when_getting_activity_time : WorkflowSpecPersonalLoans
    {
        private static DateTime timerValue;

        private Establish context = () =>
        {
            timerValue = DateTime.MinValue;
        };

        private Because of = () =>
        {
            timerValue = workflow.GetActivityTime_NTU_Timer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_thirty_days_from_today = () =>
        {
            timerValue.ShouldBeCloseTo(DateTime.Now.AddDays(30), TimeSpan.FromSeconds(10));
        };
    }
}