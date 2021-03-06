﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Submit_Application.OnComplete
{
    [Subject("Activity => Submit_Application => OnComplete")]
    internal class when_chk_branch_submit_app_promote_lead_to_main_app_rules_false : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static string message;
        private static IApplicationCapture client;
        private static ICommon common;

        private Establish context = () =>
        {
            result = true;
            message = string.Empty;
            client = An<IApplicationCapture>();
            client.WhenToldTo(x => x.CheckBranchSubmitApplicationRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>())).Return(true);
            client.WhenToldTo(x => x.PromoteLeadToMainApplicant(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>())).Return(false);
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(client);
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Submit_Application(instanceData, workflowData, paramsData, messages, ref message);
        };

        It should_not_perform_pricing_for_risk = () =>
        {
            common.WasNotToldTo(x => x.PricingForRisk(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}