using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Contact_Client.OnEnter
{
    [Subject("State => Contact_Client => OnEnter")]
    internal class when_contact_client : WorkflowSpecApplicationCapture
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
            result = workflow.OnEnter_Contact_Client(instanceData, workflowData, paramsData, messages);
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