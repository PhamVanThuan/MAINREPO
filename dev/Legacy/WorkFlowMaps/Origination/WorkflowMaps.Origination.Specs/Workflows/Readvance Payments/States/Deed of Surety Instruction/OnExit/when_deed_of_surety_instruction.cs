using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.States.Deed_of_Surety_Instruction.OnExit
{
    [Subject("State => Deed_of_Surety_Instruction => OnExit")] // AutoGenerated
    internal class when_deed_of_surety_instruction : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Deed_of_Surety_Instruction(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}