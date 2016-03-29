using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Credit.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Check_Failed.OnStart
{
    [Subject("Activity => Check_Failed => OnStart")]
    internal class when_check_failed_and_exceptions_decline_with_offer_action_performed : WorkflowSpecCredit
    {
        private static bool result;
        private static ICredit client;

        private Establish context = () =>
        {
            result = true;
            workflowData.ExceptionsDeclineWithOffer = true;

            workflowData.ApplicationKey = 1;
            ((InstanceDataStub)instanceData).ID = 1;

            client = An<ICredit>();
            domainServiceLoader.RegisterMockForType<ICredit>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Check_Failed(instanceData, workflowData, paramsData, messages);
        };

        private It should_not_check_the_credit_decision_check_authorisation_rules = () =>
        {
            client.WasNotToldTo(x => x.DoesNotMeetCreditSignatureRequirements((IDomainMessageCollection)messages, workflowData.ApplicationKey, instanceData.ID));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}