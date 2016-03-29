using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class ActivitySecurityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ActivitySecurityDataModel(int activityID, int securityGroupID)
        {
            this.ActivityID = activityID;
            this.SecurityGroupID = securityGroupID;
		
        }
		[JsonConstructor]
        public ActivitySecurityDataModel(int iD, int activityID, int securityGroupID)
        {
            this.ID = iD;
            this.ActivityID = activityID;
            this.SecurityGroupID = securityGroupID;
		
        }		

        public int ID { get; set; }

        public int ActivityID { get; set; }

        public int SecurityGroupID { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}