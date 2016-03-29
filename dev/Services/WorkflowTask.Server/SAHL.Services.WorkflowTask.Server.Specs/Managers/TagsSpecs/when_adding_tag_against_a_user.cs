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
    public class when_adding_tag_against_a_user : WithFakes
    {
        private static CreateTagForUserCommand command = new CreateTagForUserCommand(Guid.NewGuid(), "Test Tag Text", "#000000", "#FFFFFF", "");
        private static CreateTagForUserCommandHandler addNewTagForUserCommandHandler;
        private static IWorkflowTaskDataManager dataManager;
        private static FakeDbFactory dbFactory;
        private static IServiceRequestMetadata serviceRequestMetadata;
        private static string adUsername = "testUser";

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = An<WorkflowTaskDataManager>(dbFactory);
            serviceRequestMetadata = An<IServiceRequestMetadata>();

            serviceRequestMetadata.WhenToldTo(x => x.UserName).Return(adUsername);

            addNewTagForUserCommandHandler = new CreateTagForUserCommandHandler(dataManager);
        };

        private Because of = () =>
        {
            addNewTagForUserCommandHandler.HandleCommand(command, serviceRequestMetadata);
        };

        private It should_call_the_data_manager_to_save_tag = () =>
        {
            dataManager.WasToldTo(x => x.AddTagForUser(command));
        };

        private It should_call_database_to_set_model = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Insert(Arg.Any<UserTagsDataModel>()));
        };
    }
}