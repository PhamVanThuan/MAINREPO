using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.WorkflowTask.Commands;
using SAHL.Services.WorkflowTask.Server.CommandHandlers;
using SAHL.Services.WorkflowTask.Server.Managers;
using System;

namespace SAHL.Services.WorkflowTask.Server.Specs.Tags
{
    public class when_deleting_a_tag_from_a_workflow_Item : WithFakes
    {
        private static DeleteTagFromWorkflowInstanceCommand command;
        private static DeleteTagFromWorkflowInstanceCommandHandler handler;
        private static IWorkflowTaskDataManager dataManager;
        private static FakeDbFactory dbFactory = new FakeDbFactory();
        private static IServiceRequestMetadata serviceRequestMetadata;
        private static WorkflowItemUserTagsDataModel workflowItemUserTag;
        private static string adUsername = "testUser";

        private Establish context = () =>
        {
            dataManager = An<WorkflowTaskDataManager>(dbFactory);
            command = new DeleteTagFromWorkflowInstanceCommand(Guid.NewGuid(), 1);

            serviceRequestMetadata = An<IServiceRequestMetadata>();
            handler = new DeleteTagFromWorkflowInstanceCommandHandler(dataManager);
            serviceRequestMetadata.WhenToldTo(x => x.UserName).Return(adUsername);
            workflowItemUserTag = new WorkflowItemUserTagsDataModel(1, "", Guid.NewGuid(), DateTime.Now);
            dbFactory.FakedDb.InWorkflowContext().WhenToldTo(
                x => x.SelectOneWhere<WorkflowItemUserTagsDataModel>(Arg.Any<string>(), Arg.Any<object>())).Return(workflowItemUserTag);
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, serviceRequestMetadata);
        };

        private It should_call_data_manager_get_the_id_Of_the_join = () =>
        {
            dataManager.WasToldTo(x => x.GetWorkflowUserTagKey(command.TagId, command.WorkflowInstanceId));
        };

        private It should_fetch_the_key_from_the_database = () =>
        {
            dbFactory.FakedDb.InWorkflowContext()
                .WasToldTo(x => x.SelectOneWhere<WorkflowItemUserTagsDataModel>(Arg.Any<string>(), Arg.Any<object>()));
        };

        private It should_call_data_manager_to_delete_the_item_by_key = () =>
        {
            dataManager.WasToldTo(x => x.DeleteWorkflowUserTagByKey(1));
        };

        private It should_delete_the_tag_from_database = () =>
        {
            dbFactory.FakedDb.InWorkflowContext().WasToldTo(x => x.DeleteByKey<WorkflowItemUserTagsDataModel, int>(1));
        };
    }
}