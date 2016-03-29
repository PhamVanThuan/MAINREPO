using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.DebtCounselling.Specs.Activities._5days.OnGetActivityTime
{
    [Subject("Activity => 5_Days => OnGetActivityTime")]
    internal class when_5_days : WorkflowSpecDebtCounselling
    {
        private static DateTime result;
        private static DateTime expectedDate;
        private static ICommon common;

        private Establish context = () =>
        {
            expectedDate = DateTime.Now.AddDays(5);

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            common.WhenToldTo(x => x.GetnWorkingDaysFromToday(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(expectedDate);
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_5days(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_value_from_GetnWorkingDaysFromToday = () =>
        {
            result.ShouldEqual(expectedDate);
        };

        private It should_return_date_time_5_working_days_from_now = () =>
        {
            common.WasToldTo(x => x.GetnWorkingDaysFromToday((IDomainMessageCollection)messages, 5));
        };
    }
}