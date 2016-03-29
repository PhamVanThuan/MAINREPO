using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class WorkFlowSecurityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public WorkFlowSecurityDataModel(int workFlowID, int securityGroupID)
        {
            this.WorkFlowID = workFlowID;
            this.SecurityGroupID = securityGroupID;
		
        }
		[JsonConstructor]
        public WorkFlowSecurityDataModel(int iD, int workFlowID, int securityGroupID)
        {
            this.ID = iD;
            this.WorkFlowID = workFlowID;
            this.SecurityGroupID = securityGroupID;
		
        }		

        public int ID { get; set; }

        public int WorkFlowID { get; set; }

        public int SecurityGroupID { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}