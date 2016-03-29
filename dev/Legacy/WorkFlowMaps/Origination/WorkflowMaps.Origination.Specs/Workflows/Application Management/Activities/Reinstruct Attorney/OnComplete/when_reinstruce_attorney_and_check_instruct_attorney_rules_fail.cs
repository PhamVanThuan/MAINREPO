﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Reinstruct_Attorney.OnComplete
{
    [Subject("Activity => Reinstruct_Attorney => OnComplete")]
    internal class when_reinstruce_attorney_and_check_instruct_attorney_rules_fail : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IApplicationManagement appMan;

        private Establish context = () =>
        {
            result = true;
            message = string.Empty;
            appMan = An<IApplicationManagement>();
            workflowData.ApplicationKey = 1;
            ((ParamsDataStub)paramsData).IgnoreWarning = true;
            appMan.WhenToldTo(x => x.CheckInstructAttorneyRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                .Return(false);
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Reinstruct_Attorney(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_check_instruct_attorney_rules = () =>
        {
            appMan.WasToldTo(x => x.CheckInstructAttorneyRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}