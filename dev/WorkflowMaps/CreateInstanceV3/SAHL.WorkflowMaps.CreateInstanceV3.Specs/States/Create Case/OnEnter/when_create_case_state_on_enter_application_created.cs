using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.WorkflowMaps.CreateInstanceV3.Specs.Activities.Create_Case
{
    internal class when_create_case_state_on_enter_application_created : WorkflowSpecCreateInstanceV3
    {
        private static bool result;        

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Application_Created(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}
