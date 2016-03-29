using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2;
using SAHL.Core.X2.Factories;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class UserRequestCreateInstanceCommandHandler : IServiceCommandHandler<UserRequestCreateInstanceCommand>
    {
        private IX2ServiceCommandRouter commandHandler;
        private IX2ProcessProvider processProvider;
        private IWorkflowDataProvider workflowDataProvider;
        private IMessageCollectionFactory messageCollectionFactory;

        public UserRequestCreateInstanceCommandHandler(IX2ServiceCommandRouter commandHandler, IX2ProcessProvider processProvider, IWorkflowDataProvider workflowDataProvider, IMessageCollectionFactory messageCollectionFactory)
        {
            this.commandHandler = commandHandler;
            this.processProvider = processProvider;
            this.workflowDataProvider = workflowDataProvider;
            this.messageCollectionFactory = messageCollectionFactory;
        }

        public ISystemMessageCollection HandleCommand(UserRequestCreateInstanceCommand command, IServiceRequestMetadata metadata)
        {
            var messages = messageCollectionFactory.CreateEmptyCollection();
            // check that the requested action actually exists and is a creation action
            CheckCreationActivityAccessCommand checkActivityIsACreationActivityCommand = new CheckCreationActivityAccessCommand(command.Activity);
            var result = commandHandler.CheckRuleCommand<CheckCreationActivityAccessCommand>(checkActivityIsACreationActivityCommand, metadata);
            var checkRuleResultCommand = new CheckRuleResultCommand(result, "CheckCreationActivityAccessCommand");
            commandHandler.HandleCommand(checkRuleResultCommand, metadata);

            // check the activity security, creation actions cannot have dynamic roles
            CheckCreateActivityHasOnlyStaticRoleCommand checkActivitySecurityForCreationAccessCommand = new CheckCreateActivityHasOnlyStaticRoleCommand(command.Activity, command.Username);
            result = commandHandler.CheckRuleCommand<CheckCreateActivityHasOnlyStaticRoleCommand>(checkActivitySecurityForCreationAccessCommand, metadata);
            checkRuleResultCommand = new CheckRuleResultCommand(result, "CheckCreateActivityHasOnlyStaticRoleCommand");
            commandHandler.HandleCommand(checkRuleResultCommand, metadata);

            // create the instance
            CreateInstanceCommand createInstanceCommand = new CreateInstanceCommand(command.ProcessName, command.WorkflowName, command.Activity.ActivityName, command.Username, command.WorkflowProviderName, command.ParentInstanceId, command.SourceInstanceId, command.ReturnActivityId);
            messages.Aggregate(commandHandler.HandleCommand(createInstanceCommand, metadata));

            // get the newly created instance
            InstanceDataModel instance = workflowDataProvider.GetInstanceDataModel(createInstanceCommand.NewlyCreatedInstanceId);

            // get the map for the newly created instance
            var process = processProvider.GetProcessForInstance(instance.ID);
            var workflow = workflowDataProvider.GetWorkflowById(instance.WorkFlowID);
            var map = process.GetWorkflowMap(workflowDataProvider.GetWorkflowName(instance));

            // get the contextual data
            var contextualData = map.GetContextualData(instance.ID);

            // create the contextual data (workflow data record) linked to the newly created instance
            CreateContextualDataCommand createContextualDataCommand = new CreateContextualDataCommand(instance, contextualData, command.MapVariables);
            messages.Aggregate(commandHandler.HandleCommand(createContextualDataCommand, metadata));

            // call onstart activity
            IX2Params param = new X2Params(command.Activity.ActivityName, command.Activity.StateName, workflow.Name, command.IgnoreWarnings, command.Username, metadata, command.Data);
            result = map.StartActivity(instance, contextualData, param, messages);
            HandleMapReturnCommand handleMapReturnCommand = new HandleMapReturnCommand(instance.ID, result, messages, param.ActivityName, WorkflowMapCodeSectionEnum.OnStart);
            commandHandler.HandleCommand<HandleMapReturnCommand>(handleMapReturnCommand, metadata);
            if (!handleMapReturnCommand.Result)
            {
                return messages;
            }

            // save the contextual data
            SaveContextualDataCommand saveContextualDataCommand = new SaveContextualDataCommand(contextualData, instance.ID);
            messages.Aggregate(commandHandler.HandleCommand<SaveContextualDataCommand>(saveContextualDataCommand, metadata));

            // save the newly created instance
            SaveInstanceCommand saveInstanceCommand = new SaveInstanceCommand(instance);
            messages.Aggregate(commandHandler.HandleCommand<SaveInstanceCommand>(saveInstanceCommand, metadata));
            command.NewlyCreatedInstanceId = saveInstanceCommand.Instance.ID;

            return messages;
        }
    }
}