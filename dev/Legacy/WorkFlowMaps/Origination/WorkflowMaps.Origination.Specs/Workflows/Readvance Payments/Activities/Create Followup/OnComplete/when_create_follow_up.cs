using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Create_Followup.OnComplete
{
    [Subject("Activity => Create_Followup => OnComplete")]
    internal class when_create_follow_up : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static string message;
        private static string caseName;
        private static ICommon commonClient;

        private Establish context = () =>
            {
                caseName = "CaseName";
                workflowData.ApplicationKey = 1234567;
                result = false;
                commonClient = An<ICommon>();
                commonClient.WhenToldTo(x => x.GetCaseName(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(caseName);
                domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Create_Followup(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_set_the_instance_data_name = () =>
            {
                instanceData.Name.ShouldEqual(workflowData.ApplicationKey.ToString());
            };

        private It should_set_the_instance_data_subject = () =>
            {
                instanceData.Subject.ShouldEqual(caseName);
            };
    }
}