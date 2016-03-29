using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.PersonalLoan.Specs.States.NTU.OnEnter
{
    [Subject("State => NTU => OnEnter")]
    internal class when_NTU : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static ICommon commonClient;
        private static string prevState;

        private Establish context = () =>
            {
                result = false;
                commonClient = An<ICommon>();
                domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
                prevState = "Workflow State";
                commonClient.WhenToldTo(x => x.GetPreviousStateName((IDomainMessageCollection)messages, instanceData.ID)).Return(prevState);
            };

        private Because of = () =>
            {
                result = workflow.OnEnter_NTU(instanceData, workflowData, paramsData, messages);
            };

        private It should_set_the_previous_state_to_the_result_of_get_previous_state_name_common_method = () =>
            {
                workflowData.PreviousState = prevState;
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };
    }
}