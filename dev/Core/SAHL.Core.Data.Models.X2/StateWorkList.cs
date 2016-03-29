using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class StateWorkListDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public StateWorkListDataModel(int stateID, int securityGroupID)
        {
            this.StateID = stateID;
            this.SecurityGroupID = securityGroupID;
		
        }
		[JsonConstructor]
        public StateWorkListDataModel(int iD, int stateID, int securityGroupID)
        {
            this.ID = iD;
            this.StateID = stateID;
            this.SecurityGroupID = securityGroupID;
		
        }		

        public int ID { get; set; }

        public int StateID { get; set; }

        public int SecurityGroupID { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}