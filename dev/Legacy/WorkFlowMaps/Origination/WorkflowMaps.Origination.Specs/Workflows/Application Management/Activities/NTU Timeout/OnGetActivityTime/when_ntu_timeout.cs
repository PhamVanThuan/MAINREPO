using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.NTU_Timeout.OnGetActivityTime
{
    [Subject("Activity => NTU_Timeout => OnGetActivityTime")]
    internal class when_ntu_timeout : WorkflowSpecApplicationManagement
    {
        private static DateTime result;
        private static DateTime expectedResult;
        private static ICommon common;

        private Establish context = () =>
        {
            result = DateTime.MinValue;
            expectedResult = DateTime.MaxValue;
            common = An<ICommon>();
            common.WhenToldTo(x => x.GetnWorkingDaysFromToday(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()))
                .Return(expectedResult);
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_NTU_Timeout(instanceData, workflowData, paramsData, messages);
        };

        private It should_get_11_working_days_from_today = () =>
        {
            common.WasToldTo(x => x.GetnWorkingDaysFromToday((IDomainMessageCollection)messages, 11));
        };

        private It should_return_get_n_working_days_from_today_result = () =>
        {
            result.ShouldEqual(expectedResult);
        };
    }
}