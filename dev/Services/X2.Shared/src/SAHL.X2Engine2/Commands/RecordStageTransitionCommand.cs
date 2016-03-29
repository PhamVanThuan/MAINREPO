using System;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Commands
{
    public class RecordStageTransitionCommand : ServiceCommand
    {
        public InstanceDataModel Instance { get; protected set; }

        public string StageTransitionComments { get; protected set; }

        public IX2ContextualDataProvider ContextualDataProvider { get; protected set; }

        public string UserName { get; protected set; }

        public string XmlResult { get; set; }

        public Activity Activity { get; protected set; }

        public DateTime StartTime { get; protected set; }

        public int StageFrom { get; protected set; }

        public int StageTo { get; protected set; }

        public RecordStageTransitionCommand(InstanceDataModel instance, string stageTransitionComments, IX2ContextualDataProvider contextualDataProvider, string userName, Activity activity, DateTime startTime) //, int stageFrom, int stageTo)
        {
            this.Instance = instance;
            this.StageTransitionComments = stageTransitionComments;
            this.ContextualDataProvider = contextualDataProvider;
            this.UserName = userName;
            this.Activity = activity;
            this.StartTime = startTime;
        }
    }
}