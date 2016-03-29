using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Sys_Appro_FL_and_FA.OnComplete
{
    [Subject("State => Sys_Appro_FL_And_FA => OnComplete")]
    internal class when_sys_approve_FL_and_FA : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static ICommon common;
        private static string message;
        private static int expectedApplicationKey;

        private Establish context = () =>
        {
            result = false;
            expectedApplicationKey = 1234;
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            common.WhenToldTo(x => x.GetApplicationKeyFromSourceInstanceID((IDomainMessageCollection)messages, instanceData.ID)).Return(1234);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Sys_Appro_FL_and_FA(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_instanceData_name = () =>
        {
            instanceData.Name.ShouldMatch("1234");
        };

        private It should_set_applicationkey = () =>
        {
            workflowData.ApplicationKey.ShouldEqual<int>(expectedApplicationKey);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}