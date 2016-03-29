using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.OnFollowup.OnGetActivityTime
{
    [Subject("Activity => OnFollowup => OnGetActivityTime")]
    internal class when_on_followup : WorkflowSpecApplicationManagement
    {
        private static DateTime result;
        private static DateTime expectedResult;
        private static ICommon common;

        private Establish context = () =>
        {
            result = DateTime.MinValue;
            expectedResult = DateTime.MaxValue;
            workflowData.GenericKey = 1;
            common = An<ICommon>();
            common.WhenToldTo(x => x.GetFollowupTime(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()))
                .Return(expectedResult);
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_OnFollowup(instanceData, workflowData, paramsData, messages);
        };

        private It should_get_followup_time = () =>
        {
            common.WasToldTo(x => x.GetFollowupTime((IDomainMessageCollection)messages, workflowData.GenericKey));
        };

        private It should_return_followup_time = () =>
        {
            result.ShouldEqual(expectedResult);
        };
    }
}