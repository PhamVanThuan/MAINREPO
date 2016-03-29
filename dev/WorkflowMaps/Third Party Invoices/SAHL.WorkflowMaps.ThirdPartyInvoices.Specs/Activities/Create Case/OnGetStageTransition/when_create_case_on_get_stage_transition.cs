using Machine.Specifications;
using System;
using System.Linq;

namespace SAHL.WorkflowMaps.ThirdPartyInvoices.Specs.Create_Case
{
    internal class when_creating_a_case_on_get_stage_transition : WorkflowSpecThirdPartyInvoices
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_a_null = () =>
        {
            result.ShouldBeNull();
        };
    }
}