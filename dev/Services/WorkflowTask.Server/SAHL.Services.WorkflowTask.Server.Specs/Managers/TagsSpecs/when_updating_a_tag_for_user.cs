using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.WorkflowTask.Commands;
using SAHL.Services.WorkflowTask.Server.CommandHandlers;
using SAHL.Services.WorkflowTask.Server.Managers;
using SAHL.Services.WorkflowTask.Server.Statements;
using System;

namespace SAHL.Services.WorkflowTask.Server.Specs.Tags
{
    public class when_updating_a_tag_for_user : WithFakes
    {
        private static UpdateUserTagCommand command;
        private static UpdateUserTagCommandHandler handler;
        private static WorkflowTaskDataManager dataManager;
        private static FakeDbFactory dbFactory;
        private static IServiceRequestMetadata serviceRequestMetadata;
        private static string adUsername = "testUser";

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = An<WorkflowTaskDataManager>(dbFactory);

            command = new UpdateUserTagCommand(Guid.NewGuid(), "#FFFFFF", "#000000", "Test Tag");

            serviceRequestMetadata = An<IServiceRequestMetadata>();
            handler = new UpdateUserTagCommandHandler(dataManager);
            serviceRequestMetadata.WhenToldTo(x => x.UserName).Return(adUsername);
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, serviceRequestMetadata);
        };

        private It should_call_data_manager_to_update = () =>
        {
            dataManager.WasToldTo(x => x.UpdateUserTag(command));
        };

        private It should_update_the_database = () =>
        {
            dbFactory.FakedDb.InWorkflowContext().WasToldTo(x => x.Update<UserTagsDataModel>(Arg.Is<UpdateUserTagWithGivenValuesStatement>(tag =>
            tag.Id == command.Id &&
            tag.BackColour == command.BackColour &&
            tag.ForeColour == command.ForeColour &&
            tag.Caption == command.Caption)));
        };
    }
}