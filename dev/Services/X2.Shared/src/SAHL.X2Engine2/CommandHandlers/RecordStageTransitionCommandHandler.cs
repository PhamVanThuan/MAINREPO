using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Core.X2.Events;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class RecordStageTransitionCommandHandler : IServiceCommandHandler<RecordStageTransitionCommand>
    {
        private IEventRaiser eventRaiser;
        private IWorkflowDataProvider workflowDataProvider;

        public RecordStageTransitionCommandHandler(IWorkflowDataProvider workflowDataProvider, IEventRaiser eventRaiser)
        {
            this.workflowDataProvider = workflowDataProvider;
            this.eventRaiser = eventRaiser;
        }

        public ISystemMessageCollection HandleCommand(RecordStageTransitionCommand command, IServiceRequestMetadata metadata)
        {
            var workflow = workflowDataProvider.GetWorkflow(command.Instance);
            string storageKey = workflow.StorageKey;

            StringBuilder sb = new StringBuilder("<X2Response>");
            IEnumerable<StageActivityDataModel> stageActivityDataModels = workflowDataProvider.GetStageActivities(command.Activity.ActivityID);
            List<StageTransitionDataModel> stageTransitions = new List<StageTransitionDataModel>();

            var processData = workflowDataProvider.GetProcessById(workflow.ProcessID);

            ADUserDataModel aduser = workflowDataProvider.GetADUser(command.UserName);
            int genericKey = Convert.ToInt32(command.ContextualDataProvider.GetDataField(storageKey));
            sb.Append("<StageTransitions>");

            WriteWorkflowTransitionEvent(command, workflow, aduser.ADUserName, genericKey, processData, metadata);
            if (!processData.IsLegacy)
            {
                return SystemMessageCollection.Empty();
            }

            foreach (var stageActivityDataModel in stageActivityDataModels)
            {
                var stageTransition = new StageTransitionDataModel(genericKey, aduser.ADUserKey, command.StartTime, command.StageTransitionComments, (int)stageActivityDataModel.StageDefinitionStageDefinitionGroupKey, DateTime.Now);
                stageTransitions.Add(stageTransition);

                Dictionary<string, string> values = new Dictionary<string, string>
                {
                    { "Key", stageTransition.StageTransitionKey.ToString() }
                };
                string nodeName = "StageTransition";
                sb.AppendFormat("<{0} ", nodeName);
                string[] keys = new string[values.Keys.Count];
                values.Keys.CopyTo(keys, 0);
                for (int i = 0; i < keys.Length; i++)
                {
                    string key = keys[i];
                    string value = values[key];
                    sb.AppendFormat(" {0}=\"{1}\" ", key, value);
                    sb.AppendLine();
                }
                sb.Append("/>");
            }
            sb.Append("</StageTransitions>");
            sb.Append("</X2Response>");
            command.XmlResult = sb.ToString();

            using (var db = new Db().InWorkflowContext())
            {
                db.Insert<StageTransitionDataModel>(stageTransitions);
                db.Complete();
            }

            return SystemMessageCollection.Empty();
        }

        private void WriteWorkflowTransitionEvent(RecordStageTransitionCommand command, WorkFlowDataModel workflow, string adUserName, int genericKey, ProcessDataModel processData, IServiceRequestMetadata metadata)
        {
            var @event = new WorkflowTransitionEvent(
                  command.Instance.ID
                , adUserName
                , processData.Name
                , workflow.Name
                , genericKey
                , workflow.GenericKeyTypeKey
                , command.Activity.ActivityName
                , command.StartTime
                , command.Activity.StateId
                , command.Activity.ToStateId
                , command.Activity.StateName
                , command.Activity.ToStateName
                );

            if (metadata == null)
            {
                metadata = new ServiceRequestMetadata();
                metadata.Add("UserName", adUserName);
            }

            eventRaiser.RaiseEvent(
                 command.StartTime
               , @event
               , genericKey
               , @event.GenericKeyTypeKey.HasValue ? @event.GenericKeyTypeKey.Value : 0
               , metadata
               );
        }
    }
}