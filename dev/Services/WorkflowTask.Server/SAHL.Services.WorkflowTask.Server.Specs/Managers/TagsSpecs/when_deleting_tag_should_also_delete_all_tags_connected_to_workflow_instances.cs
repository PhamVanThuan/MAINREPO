using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.WorkflowTask.Commands;
using SAHL.Services.WorkflowTask.Server.CommandHandlers;
using SAHL.Services.WorkflowTask.Server.CommandHandlers.Statements;
using SAHL.Services.WorkflowTask.Server.Managers;
using System;

namespace SAHL.Services.WorkflowTask.Server.Specs.Tags
{
    public class when_deleting_tag_should_also_delete_all_tags_connected_to_workflow_instances : WithFakes
    {
        private static DeleteTagForUserCommand command;
        private static DeleteTagForUserCommandHandler handler;
        private static IWorkflowTaskDataManager dataManager;
        private static FakeDbFactory dbFactory = new FakeDbFactory();
        private static IServiceRequestMetadata serviceRequestMetadata;
        private static string adUsername = "testUser";

        private Establish context = () =>
        {
            dataManager = An<WorkflowTaskDataManager>(dbFactory);
            command = new DeleteTagForUserCommand(Guid.NewGuid());
            serviceRequestMetadata = An<IServiceRequestMetadata>();
            handler = new DeleteTagForUserCommandHandler(dataManager);
            serviceRequestMetadata.WhenToldTo(x => x.UserName).Return(adUsername);
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, serviceRequestMetadata);
        };

        private It should_call_to_remove_all_current_instances_of_this_tag_on_workflow_items = () =>
        {
            dataManager.WasToldTo(x => x.DeleteAllInstancesOfTagOnWorkflowItems(command.Id));
        };

        private It should_delete_all_instances_from_database = () =>
        {
            dbFactory.FakedDb.InWorkflowContext()
                .WasToldTo(x => x.Delete(Arg.Any<DeleteAllInstancesOfTagOnWorkflowItemsStatement>()));
        };

        private It should_call_to_remove_the_tag_from_the_user_tag_list = () =>
        {
            dataManager.WasToldTo(x => x.DeleteTagForUser(command.Id));
        };

        private It should_delete_tag_from_database = () =>
        {
            dbFactory.FakedDb.InWorkflowContext().WasToldTo(x => x.DeleteByKey<UserTagsDataModel, Guid>(command.Id));
        };
    }
}