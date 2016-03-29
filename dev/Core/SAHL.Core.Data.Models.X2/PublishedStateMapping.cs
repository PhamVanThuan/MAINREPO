using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class PublishedStateMappingDataModel :  IDataModel
    {
        public PublishedStateMappingDataModel(int oldWorkFlowID, int oldStateID, int newWorkFlowID, int newStateID)
        {
            this.OldWorkFlowID = oldWorkFlowID;
            this.OldStateID = oldStateID;
            this.NewWorkFlowID = newWorkFlowID;
            this.NewStateID = newStateID;
		
        }		

        public int OldWorkFlowID { get; set; }

        public int OldStateID { get; set; }

        public int NewWorkFlowID { get; set; }

        public int NewStateID { get; set; }
    }
}