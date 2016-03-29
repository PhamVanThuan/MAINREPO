using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Estate_Agent_Lead_Assign.OnEnter
{
    [Subject("State => Estate_Agent_Lead_Assign => OnEnter")]
    internal class when_estate_agent_lead_assign : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static string caseNameExpected;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            instanceData.Subject = string.Empty;
            caseNameExpected = "Test";
            common = An<ICommon>();
            common.WhenToldTo(x => x.GetCaseName(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(caseNameExpected);
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Estate_Agent_Lead_Assign(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_subject_property = () =>
        {
            instanceData.Subject.ShouldBeTheSameAs(caseNameExpected);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}