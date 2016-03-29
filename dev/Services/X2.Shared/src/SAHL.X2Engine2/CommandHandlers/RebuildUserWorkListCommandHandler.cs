using System;
using System.Collections.Generic;
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
    public class RebuildUserWorkListCommandHandler : IServiceCommandHandler<RebuildUserWorkListCommand>
    {
        private readonly IX2ServiceCommandRouter commandHandler;
        private readonly IWorkflowDataProvider workflowDataProvider;
        private readonly IX2ProcessProvider processProvider;
        IMessageCollectionFactory messageCollectionFactory;

        public RebuildUserWorkListCommandHandler(IX2ServiceCommandRouter commandHandler, IWorkflowDataProvider workflowDataProvider, IX2ProcessProvider processProvider, IMessageCollectionFactory messageCollectionFactory)
        {
            this.commandHandler = commandHandler;
            this.workflowDataProvider = workflowDataProvider;
            this.processProvider = processProvider;
            this.messageCollectionFactory = messageCollectionFactory;
        }

        public ISystemMessageCollection HandleCommand(RebuildUserWorkListCommand command, IServiceRequestMetadata metadata)
        {
            var messages = messageCollectionFactory.CreateEmptyCollection();
            var instance = workflowDataProvider.GetInstanceDataModel(command.InstanceID);
            WorkFlowDataModel workflowDataModel = workflowDataProvider.GetWorkflow(instance);
            IEnumerable<StateWorkListDataModel> stateWorkLists = workflowDataProvider.GetStateWorkList(instance.StateID.Value);
            var map = processProvider.GetProcessForInstance(instance.ID).GetWorkflowMap(workflowDataProvider.GetWorkflowName(instance));
            List<WorkListDataModel> worklistDataModels = new List<WorkListDataModel>();
            foreach (var stateWorkList in stateWorkLists)
            {
                var securityGroup = workflowDataProvider.GetSecurityGroup(stateWorkList.SecurityGroupID);
                if (securityGroup == null)
                {
                    throw new Exception(String.Format("The security group ({0}) could not be found", stateWorkList.SecurityGroupID));
                }
                messages.Aggregate(commandHandler.HandleCommand(new ClearWorkListCommand(instance.ID), metadata));
                if (securityGroup.IsDynamic)
                {
                    var process = processProvider.GetProcessForInstance(instance.ID);
                    var workflowMap = process.GetWorkflowMap(workflowDataProvider.GetWorkflowName(instance));
                    var contextualDataProvider = workflowMap.GetContextualData(instance.ID);
                    IX2Params param = new X2Params("", "", workflowDataModel.Name, true, "X2", metadata);
                    var adUsername = workflowMap.GetDynamicRole(instance, contextualDataProvider, securityGroup.Name, param, messages);
                    WorkListDataModel workListDataModel = new WorkListDataModel(instance.ID, adUsername, DateTime.Now, command.ActivityMessage);
                    worklistDataModels.Add(workListDataModel);
                }
                else
                {
                    var isOriginatorRole = securityGroup.Name.Equals("originator", StringComparison.InvariantCultureIgnoreCase);
                    var adUsername = isOriginatorRole ? instance.CreatorADUserName : securityGroup.Name;
                    WorkListDataModel workListDataModel = new WorkListDataModel(instance.ID, adUsername, DateTime.Now, command.ActivityMessage);
                    worklistDataModels.Add(workListDataModel);
                }
            }
            if (worklistDataModels.Count > 0)
            {
                messages.Aggregate(commandHandler.HandleCommand(new CreateWorkListCommand(worklistDataModels), metadata));
            }
            return messages;
        }
    }
}