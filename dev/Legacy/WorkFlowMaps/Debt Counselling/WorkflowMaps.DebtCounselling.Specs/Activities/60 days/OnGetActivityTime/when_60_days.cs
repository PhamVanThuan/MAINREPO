using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities._60_days.OnGetActivityTime
{
    [Subject("Activity => 60_Days => OnGetActivityTime")]
    internal class when_60_days : WorkflowSpecDebtCounselling
    {
        private static DateTime result;
        private static DateTime expectedDate;
        private static IDebtCounselling client;

        private Establish context = () =>
        {
            expectedDate = DateTime.Now.AddDays(60);
            workflowData.DebtCounsellingKey = 1;

            client = An<IDebtCounselling>();
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
            client.WhenToldTo(x => x.GetSeventeenPointOneDateDays(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                Param.IsAny<int>())).Return(expectedDate);
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_60_days(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_value_from_GetSeventeenPointOneDateDays = () =>
        {
            result.ShouldEqual(expectedDate);
        };

        private It should_return_date_time_60_days_from_17_point_1_date = () =>
        {
            client.WasToldTo(x => x.GetSeventeenPointOneDateDays((IDomainMessageCollection)messages, workflowData.DebtCounsellingKey, 60));
        };
    }
}