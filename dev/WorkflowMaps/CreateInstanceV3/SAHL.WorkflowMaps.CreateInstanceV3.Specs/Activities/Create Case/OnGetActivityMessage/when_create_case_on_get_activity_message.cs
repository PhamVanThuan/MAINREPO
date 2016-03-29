using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.WorkflowMaps.CreateInstanceV3.Specs.Activities.Create_Case
{
    internal class when_create_case_on_get_activity_message : WorkflowSpecCreateInstanceV3
    {
        private static string result;        

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Create_Case(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}
