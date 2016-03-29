using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class SaveStageTransitionCommandHandler : IServiceCommandHandler<SaveStageTransitionCommand>
    {
        private IWorkflowDataProvider workflowDataProvider;

        public SaveStageTransitionCommandHandler(IWorkflowDataProvider workflowDataProvider)
        {
            this.workflowDataProvider = workflowDataProvider;
        }

        public ISystemMessageCollection HandleCommand(SaveStageTransitionCommand command, IServiceRequestMetadata metadata)
        {
            string storageKey = workflowDataProvider.GetWorkflow(command.Instance).StorageKey;
            StringBuilder sb = new StringBuilder("<X2Response>");
            IEnumerable<StageActivityDataModel> stageActivityDataModels = workflowDataProvider.GetStageActivities(command.Activity.ActivityID);
            List<StageTransitionDataModel> stageTransitions = new List<StageTransitionDataModel>();

            ADUserDataModel aduser = workflowDataProvider.GetADUser(command.UserName);
            int genericKey = Convert.ToInt32(command.ContextualDataProvider.GetDataField(storageKey));
            sb.Append("<StageTransitions>");
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
            return new SystemMessageCollection();
        }
    }
}