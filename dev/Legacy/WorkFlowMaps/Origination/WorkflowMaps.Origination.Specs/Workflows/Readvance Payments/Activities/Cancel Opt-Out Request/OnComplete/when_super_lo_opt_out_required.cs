using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Cancel_Opt_Out_Request.OnComplete
{
    [Subject("Activity => Cancel_Opt_Out_Request => OnComplete")]
    internal class when_super_lo_opt_out_required : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static string message;
        private static IFL client;

        private Establish context = () =>
        {
            client = An<IFL>();
            client.WhenToldTo(x => x.CheckSuperLoOptOutRequiredRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                Param.IsAny<bool>())).Return(true);
            domainServiceLoader.RegisterMockForType<IFL>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Cancel_Opt_Out_Request(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}