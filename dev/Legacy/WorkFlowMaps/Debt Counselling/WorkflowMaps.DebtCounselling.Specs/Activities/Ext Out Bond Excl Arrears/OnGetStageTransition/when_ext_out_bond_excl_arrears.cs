using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Ext_Out_Bond_Excl_Arrears.OnGetStageTransition
{
    [Subject("Activity => Ext_Out_Bond_Excl_Arrears => OnGetStageTransition")] // AutoGenerated
    internal class when_ext_out_bond_excl_arrears : WorkflowSpecDebtCounselling
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Ext_Out_Bond_Excl_Arrears(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}