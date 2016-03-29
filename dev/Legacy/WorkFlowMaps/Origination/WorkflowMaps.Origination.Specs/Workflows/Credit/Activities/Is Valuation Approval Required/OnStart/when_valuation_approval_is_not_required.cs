using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Credit.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Is_Valuation_Approval_Required.OnStart
{
    [Subject("Activity => Is_Valuation_Approval_Required => OnStart")]
    internal class when_valuation_approval_is_not_required : WorkflowSpecCredit
    {
        private static bool result;
        private static ICredit client;

        private Establish context = () =>
        {
            result = true;
            ((InstanceDataStub)instanceData).ID = 1;

            client = An<ICredit>();
            domainServiceLoader.RegisterMockForType<ICredit>(client);
            client.WhenToldTo(x => x.IsValuationApprovalRequired(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>())).Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Is_Valuation_Approval_Required(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_if_valuation_approval_is_required = () =>
        {
            client.WasToldTo(x => x.IsValuationApprovalRequired((IDomainMessageCollection)messages, instanceData.ID));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}