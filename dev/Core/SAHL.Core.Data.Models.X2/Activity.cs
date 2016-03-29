using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class ActivityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ActivityDataModel(int workFlowID, string name, int type, int? stateID, int nextStateID, bool splitWorkFlow, int priority, int? formID, string activityMessage, int? raiseExternalActivity, int? externalActivityTarget, int? activatedByExternalActivity, string chainedActivityName, int? sequence, Guid? x2ID)
        {
            this.WorkFlowID = workFlowID;
            this.Name = name;
            this.Type = type;
            this.StateID = stateID;
            this.NextStateID = nextStateID;
            this.SplitWorkFlow = splitWorkFlow;
            this.Priority = priority;
            this.FormID = formID;
            this.ActivityMessage = activityMessage;
            this.RaiseExternalActivity = raiseExternalActivity;
            this.ExternalActivityTarget = externalActivityTarget;
            this.ActivatedByExternalActivity = activatedByExternalActivity;
            this.ChainedActivityName = chainedActivityName;
            this.Sequence = sequence;
            this.X2ID = x2ID;
		
        }
		[JsonConstructor]
        public ActivityDataModel(int iD, int workFlowID, string name, int type, int? stateID, int nextStateID, bool splitWorkFlow, int priority, int? formID, string activityMessage, int? raiseExternalActivity, int? externalActivityTarget, int? activatedByExternalActivity, string chainedActivityName, int? sequence, Guid? x2ID)
        {
            this.ID = iD;
            this.WorkFlowID = workFlowID;
            this.Name = name;
            this.Type = type;
            this.StateID = stateID;
            this.NextStateID = nextStateID;
            this.SplitWorkFlow = splitWorkFlow;
            this.Priority = priority;
            this.FormID = formID;
            this.ActivityMessage = activityMessage;
            this.RaiseExternalActivity = raiseExternalActivity;
            this.ExternalActivityTarget = externalActivityTarget;
            this.ActivatedByExternalActivity = activatedByExternalActivity;
            this.ChainedActivityName = chainedActivityName;
            this.Sequence = sequence;
            this.X2ID = x2ID;
		
        }		

        public int ID { get; set; }

        public int WorkFlowID { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public int? StateID { get; set; }

        public int NextStateID { get; set; }

        public bool SplitWorkFlow { get; set; }

        public int Priority { get; set; }

        public int? FormID { get; set; }

        public string ActivityMessage { get; set; }

        public int? RaiseExternalActivity { get; set; }

        public int? ExternalActivityTarget { get; set; }

        public int? ActivatedByExternalActivity { get; set; }

        public string ChainedActivityName { get; set; }

        public int? Sequence { get; set; }

        public Guid? X2ID { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}