using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Credit.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Check_Failed.OnStart
{
    [Subject("Activity => Check_Failed => OnStart")]
    internal class when_check_failed_and_credit_signature_checks_fail : WorkflowSpecCredit
    {
        private static bool result;
        private static ICredit client;

        private Establish context = () =>
        {
            result = false;
            workflowData.ExceptionsDeclineWithOffer = false;

            workflowData.ApplicationKey = 1;
            ((InstanceDataStub)instanceData).ID = 1;

            client = An<ICredit>();
            domainServiceLoader.RegisterMockForType<ICredit>(client);
            client.WhenToldTo(x => x.DoesNotMeetCreditSignatureRequirements(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                Param.IsAny<long>())).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Check_Failed(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_the_credit_decision_check_authorisation_rules = () =>
        {
            client.WasToldTo(x => x.DoesNotMeetCreditSignatureRequirements((IDomainMessageCollection)messages, workflowData.ApplicationKey, instanceData.ID));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}