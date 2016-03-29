using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Application_Capture.States.UserAssignCheck.OnEnter
{
    [Subject("State => UserAssignCheck => OnEnter")]
    internal class when_userassigncheck : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static ICommon common;
        private static string caseNameExpected;

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
            result = workflow.OnEnter_UserAssignCheck(instanceData, workflowData, paramsData, messages);
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