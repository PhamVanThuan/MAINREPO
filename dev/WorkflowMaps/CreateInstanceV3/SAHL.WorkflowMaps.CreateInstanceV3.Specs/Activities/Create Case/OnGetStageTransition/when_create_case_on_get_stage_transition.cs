using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.WorkflowMaps.CreateInstanceV3.Specs.Activities.Create_Case
{
    internal class when_creating_a_case_on_get_stage_transition : WorkflowSpecCreateInstanceV3
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
