using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Workflow.CreateInstanceV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.WorkflowMaps.CreateInstanceV3.Specs.Activities.Create_Case
{
    internal class when_creating_a_case_on_complete : WorkflowSpecCreateInstanceV3
    {
        private static bool result;
        private static string message;
        private static ICreateInstanceV3DomainProcess createInstanceService;

        private Establish context = () =>
        {
            result = false;
            createInstanceService = An<ICreateInstanceV3DomainProcess>();
            domainServiceLoader.RegisterMockForType<ICreateInstanceV3DomainProcess>(createInstanceService);
            createInstanceService.WhenToldTo(x => x.CreateCase(Param.IsAny<ISystemMessageCollection>())).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Create_Case(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}
