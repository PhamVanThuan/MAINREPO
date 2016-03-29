using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class WorkFlowActivityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public WorkFlowActivityDataModel(int workFlowID, string name, int nextWorkFlowID, int nextActivityID, int? stateID, int? returnActivityID)
        {
            this.WorkFlowID = workFlowID;
            this.Name = name;
            this.NextWorkFlowID = nextWorkFlowID;
            this.NextActivityID = nextActivityID;
            this.StateID = stateID;
            this.ReturnActivityID = returnActivityID;
		
        }
		[JsonConstructor]
        public WorkFlowActivityDataModel(int iD, int workFlowID, string name, int nextWorkFlowID, int nextActivityID, int? stateID, int? returnActivityID)
        {
            this.ID = iD;
            this.WorkFlowID = workFlowID;
            this.Name = name;
            this.NextWorkFlowID = nextWorkFlowID;
            this.NextActivityID = nextActivityID;
            this.StateID = stateID;
            this.ReturnActivityID = returnActivityID;
		
        }		

        public int ID { get; set; }

        public int WorkFlowID { get; set; }

        public string Name { get; set; }

        public int NextWorkFlowID { get; set; }

        public int NextActivityID { get; set; }

        public int? StateID { get; set; }

        public int? ReturnActivityID { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}