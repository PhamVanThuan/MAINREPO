using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Opt_Out_SuperLo.OnComplete
{
    [Subject("Activity => Opt_Out_SuperLo => OnComplete")]
    internal class when_cannot_opt_out_of_super_lo : WorkflowSpecReadvancePayments
    {
        static bool result;
        static string message;
        static IFL client;
        Establish context = () =>
        {
            client = An<IFL>();
            result = true;
            client.WhenToldTo(x => x.OptOutSuperLo(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<string>()))
                .Return(false);
            domainServiceLoader.RegisterMockForType<IFL>(client);
        };
        Because of = () =>
        {
            result = workflow.OnCompleteActivity_Opt_Out_SuperLo(instanceData, workflowData, paramsData, messages, message);
        };

        It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}