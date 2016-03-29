using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.WorkflowTask.Commands;
using SAHL.Services.WorkflowTask.Server.CommandHandlers;
using SAHL.Services.WorkflowTask.Server.Managers;

namespace SAHL.Services.WorkflowTask.Server.Specs.Tags
{
    public class when_adding_a_users_tag_against_a_workflow_item : WithFakes
    {
        private static AddTagToWorkflowInstanceCommand command;
        private static AddTagToWorkflowInstanceCommandHandler handler;
        private static WorkflowTaskDataManager dataManager;
        private static FakeDbFactory dbFactory;
        private static IServiceRequestMetadata serviceRequestMetadata;
        private static string adUsername;

        private Establish context = () =>
        {
            adUsername = "testUser";
            command = new AddTagToWorkflowInstanceCommand(Guid.NewGuid(), 1, "");
            dbFactory = new FakeDbFactory();
            dataManager = An<WorkflowTaskDataManager>(dbFactory);

            dbFactory.WhenToldTo(x => x.FakedDb.InReadOnlyAppContext().SelectOneWhere<UserTagsDataModel>(Arg.Any<string>(), Arg.Any<object>()))
                .Return(new UserTagsDataModel(Guid.NewGuid(), "", "", "", "", DateTime.Now));
            serviceRequestMetadata = An<IServiceRequestMetadata>();
            handler = new AddTagToWorkflowInstanceCommandHandler(dataManager);
            serviceRequestMetadata.WhenToldTo(x => x.UserName).Return(adUsername);
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, serviceRequestMetadata);
        };

        private It should_call_the_data_manager_to_save_new_tag_against_user_for_workflow_item = () =>
        {
            dataManager.WasToldTo(x => x.AddTagToWorkFlowItem(command));
        };

        private It should_send_this_down_to_database = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Insert(Arg.Any<WorkflowItemUserTagsDataModel>()));
        };
    }
}
