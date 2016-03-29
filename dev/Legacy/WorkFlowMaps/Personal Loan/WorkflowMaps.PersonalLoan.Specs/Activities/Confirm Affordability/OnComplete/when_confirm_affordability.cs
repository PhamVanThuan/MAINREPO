using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Confirm_Affordability.OnComplete
{
    [Subject("Activity => Confirm_Affordability => OnComplete")]
    internal class when_confirm_affordability : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static string message;
        private static ICommon client;

        private Establish context = () =>
        {
            workflowData.ApplicationKey = 1;
            ((InstanceDataStub)instanceData).ID = 1;
            result = false;
            client = An<ICommon>();
            domainServiceLoader.RegisterMockForType(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Confirm_Affordability(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_confirm_application_affordability_assessments = () =>
        {
            client.WasToldTo(x => x.ConfirmApplicationAffordabilityAssessments((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };
    }
}