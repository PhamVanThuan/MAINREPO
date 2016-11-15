using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.EXT_DataStore_FL_Doc_Recieved.OnGetStageTransition
{
    [Subject("Activity => EXT_DataStore_FL_Doc_Recieved => OnGetStageTransition")] // AutoGenerated
    internal class when_ext_datastore_fl_doc_recieved : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_EXT_DataStore_FL_Doc_Recieved(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}