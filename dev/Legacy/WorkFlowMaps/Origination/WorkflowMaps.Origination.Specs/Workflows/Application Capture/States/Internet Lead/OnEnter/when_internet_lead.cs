using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.States.Internet_Lead.OnEnter
{
    [Subject("State => Internet_Lead => OnEnter")]
    internal class when_internet_lead : WorkflowSpecApplicationCapture
    {
        private static string caseNameExpected;
        private static bool result;
        private static ICommon common;

        private Establish context = () =>
        {
            instanceData.Subject = string.Empty;
            caseNameExpected = "Test";
            result = false;
            common = An<ICommon>();
            common.WhenToldTo(x => x.GetCaseName(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(caseNameExpected);
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Internet_Lead(instanceData, workflowData, paramsData, messages);
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