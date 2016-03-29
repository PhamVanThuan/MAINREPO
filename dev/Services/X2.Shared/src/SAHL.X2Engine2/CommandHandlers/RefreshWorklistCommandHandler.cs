using System;
using System.Collections.Generic;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2;
using SAHL.Core.X2.Factories;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class RefreshWorklistCommandHandler : IServiceCommandHandler<RefreshWorklistCommand>
    {
        public IWorkflowDataProvider WorkflowDataProvider { get; protected set; }

        private IMessageCollectionFactory messageCollectionFactory;

        public RefreshWorklistCommandHandler(IWorkflowDataProvider workflowDataProvider, IMessageCollectionFactory messageCollectionFactory)
        {
            this.WorkflowDataProvider = workflowDataProvider;
            this.messageCollectionFactory = messageCollectionFactory;
        }

        public ISystemMessageCollection HandleCommand(RefreshWorklistCommand command, IServiceRequestMetadata metadata)
        {
            var messages = messageCollectionFactory.CreateEmptyCollection();
            using (var db = new Db().InWorkflowContext())
            {
                WorkFlowDataModel workflowDataModel = WorkflowDataProvider.GetWorkflow(command.Instance);
                db.DeleteWhere<WorkListDataModel>("InstanceId=@InstanceId", new { InstanceId = command.Instance.ID });
                List<WorkListDataModel> worklistDataModels = new List<WorkListDataModel>();
                IEnumerable<StateWorkListDataModel> stateWorkListDataModels = WorkflowDataProvider.GetStateWorkList((int)command.Instance.StateID);
                foreach (var stateWorkList in stateWorkListDataModels)
                {
                    SecurityGroupDataModel securityGroup = WorkflowDataProvider.GetSecurityGroup(stateWorkList.SecurityGroupID);
                    string ADUserName = string.Empty;
                    if (securityGroup.IsDynamic)
                    {
                        IX2Params param = new X2Params("", "", workflowDataModel.Name, true, "X2", metadata);
                        ADUserName = command.Map.GetDynamicRole(command.Instance, command.ContextualDataProvider, securityGroup.Name, param, messages);
                    }
                    else
                    {
                        ADUserName = securityGroup.Name;
                    }
                    if (!string.IsNullOrEmpty(ADUserName))
                    {
                        WorkListDataModel worklistDataModel = new WorkListDataModel(command.Instance.ID, ADUserName, DateTime.Now, command.AlertMessage);
                        worklistDataModels.Add(worklistDataModel);
                    }
                }
                if (worklistDataModels.Count > 0)
                {
                    db.Insert<WorkListDataModel>(worklistDataModels);
                }
                db.Complete();
            }
            return messages;
        }
    }
}