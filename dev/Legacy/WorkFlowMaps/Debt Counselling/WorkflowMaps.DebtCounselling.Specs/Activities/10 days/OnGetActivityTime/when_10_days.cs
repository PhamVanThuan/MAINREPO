using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.DebtCounselling.Specs.Activities._10_days.OnGetActivityTime
{
    [Subject("Activity => 10_Days => OnGetActivityTime")]
    internal class when_10_days : WorkflowSpecDebtCounselling
    {
        private static DateTime result;
        private static DateTime expectedDate;
        private static ICommon common;

        private Establish context = () =>
        {
            expectedDate = DateTime.Now.AddDays(10);

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            common.WhenToldTo(x => x.GetnWorkingDaysFromToday(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(expectedDate);
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_10_days(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_value_from_GetnWorkingDaysFromToday = () =>
        {
            result.ShouldEqual(expectedDate);
        };

        private It should_return_date_time_10_working_days_from_now = () =>
        {
            common.WasToldTo(x => x.GetnWorkingDaysFromToday((IDomainMessageCollection)messages, 10));
        };
    }
}