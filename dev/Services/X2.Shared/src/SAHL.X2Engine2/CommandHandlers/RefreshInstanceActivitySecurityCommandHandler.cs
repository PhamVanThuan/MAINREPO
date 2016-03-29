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
    public class RefreshInstanceActivitySecurityCommandHandler : IServiceCommandHandler<RefreshInstanceActivitySecurityCommand>
    {
        IWorkflowDataProvider WorkflowDataProvider { get; set; }

        IMessageCollectionFactory messageCollectionFactory;

        public RefreshInstanceActivitySecurityCommandHandler(IWorkflowDataProvider workflowDataProvider, IMessageCollectionFactory messageCollectionFactory)
        {
            this.WorkflowDataProvider = workflowDataProvider;
            this.messageCollectionFactory = messageCollectionFactory;
        }

        public ISystemMessageCollection HandleCommand(RefreshInstanceActivitySecurityCommand command, IServiceRequestMetadata metadata)
        {
            var messages = messageCollectionFactory.CreateEmptyCollection();
            using (var db = new Db().InWorkflowContext())
            {
                StateDataModel stateDataModel = WorkflowDataProvider.GetStateById((int)command.Instance.StateID);
                WorkFlowDataModel workflowDataModel = WorkflowDataProvider.GetWorkflow(command.Instance);
                db.DeleteWhere<InstanceActivitySecurityDataModel>("InstanceID=@InstanceID", new { InstanceID = command.Instance.ID });
                List<InstanceActivitySecurityDataModel> instanceActivitySecurityDataModels = new List<InstanceActivitySecurityDataModel>();
                IEnumerable<ActivityDataModel> userActivities = WorkflowDataProvider.GetUserActivitiesForState((int)command.Instance.StateID);
                foreach (var userActivity in userActivities)
                {
                    IX2Params param = new X2Params(userActivity.Name, stateDataModel.Name, workflowDataModel.Name, true, "X2", null);
                    IEnumerable<ActivitySecurityDataModel> activitySecurities = WorkflowDataProvider.GetActivitySecurityForActivity(userActivity.ID);
                    foreach (var activitySecurity in activitySecurities)
                    {
                        string ADUserName = string.Empty;
                        var securityGroup = WorkflowDataProvider.GetSecurityGroup(activitySecurity.SecurityGroupID);
                        if (securityGroup.IsDynamic)
                        {
                            ADUserName = command.Map.GetDynamicRole(command.Instance, command.ContextualDataProvider, securityGroup.Name, param, messages);
                        }
                        else
                        {
                            ADUserName = securityGroup.Name;
                        }
                        InstanceActivitySecurityDataModel instanceActivitySecurityDataModel = new InstanceActivitySecurityDataModel(command.Instance.ID, userActivity.ID, ADUserName);
                        instanceActivitySecurityDataModels.Add(instanceActivitySecurityDataModel);
                    }
                }
                if (instanceActivitySecurityDataModels.Count > 0)
                {
                    db.Insert<InstanceActivitySecurityDataModel>(instanceActivitySecurityDataModels);
                }
                db.Complete();
            }
            return messages;
        }
    }
}