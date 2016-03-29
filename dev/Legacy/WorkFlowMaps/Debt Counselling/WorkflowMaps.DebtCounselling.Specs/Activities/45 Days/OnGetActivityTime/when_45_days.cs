using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities._45_Days.OnGetActivityTime
{
    [Subject("Activity => 45_Days => OnGetActivityTime")]
    internal class when_45_days : WorkflowSpecDebtCounselling
    {
        private static DateTime result;
        private static DateTime expectedDate;
        private static IDebtCounselling client;

        private Establish context = () =>
        {
            expectedDate = DateTime.Now.AddDays(45);
            workflowData.DebtCounsellingKey = 1;
            client = An<IDebtCounselling>();
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
            client.WhenToldTo(x => x.GetSeventeenPointOneDateDays(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                Param.IsAny<int>())).Return(expectedDate);
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_45_Days(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_value_from_GetSeventeenPointOneDateDays = () =>
        {
            result.ShouldEqual(expectedDate);
        };

        private It should_return_date_time_45_days_from_17_point_1_date = () =>
        {
            client.WasToldTo(x => x.GetSeventeenPointOneDateDays((IDomainMessageCollection)messages, workflowData.DebtCounsellingKey, 45));
        };
    }
}