using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class StateDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public StateDataModel(int workFlowID, string name, int type, bool forwardState, int? sequence, int? returnWorkflowID, int? returnActivityID, Guid? x2ID)
        {
            this.WorkFlowID = workFlowID;
            this.Name = name;
            this.Type = type;
            this.ForwardState = forwardState;
            this.Sequence = sequence;
            this.ReturnWorkflowID = returnWorkflowID;
            this.ReturnActivityID = returnActivityID;
            this.X2ID = x2ID;
		
        }
		[JsonConstructor]
        public StateDataModel(int iD, int workFlowID, string name, int type, bool forwardState, int? sequence, int? returnWorkflowID, int? returnActivityID, Guid? x2ID)
        {
            this.ID = iD;
            this.WorkFlowID = workFlowID;
            this.Name = name;
            this.Type = type;
            this.ForwardState = forwardState;
            this.Sequence = sequence;
            this.ReturnWorkflowID = returnWorkflowID;
            this.ReturnActivityID = returnActivityID;
            this.X2ID = x2ID;
		
        }		

        public int ID { get; set; }

        public int WorkFlowID { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public bool ForwardState { get; set; }

        public int? Sequence { get; set; }

        public int? ReturnWorkflowID { get; set; }

        public int? ReturnActivityID { get; set; }

        public Guid? X2ID { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}