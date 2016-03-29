using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class InstanceActivitySecurityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public InstanceActivitySecurityDataModel(long instanceID, int activityID, string aDUserName)
        {
            this.InstanceID = instanceID;
            this.ActivityID = activityID;
            this.ADUserName = aDUserName;
		
        }
		[JsonConstructor]
        public InstanceActivitySecurityDataModel(int iD, long instanceID, int activityID, string aDUserName)
        {
            this.ID = iD;
            this.InstanceID = instanceID;
            this.ActivityID = activityID;
            this.ADUserName = aDUserName;
		
        }		

        public int ID { get; set; }

        public long InstanceID { get; set; }

        public int ActivityID { get; set; }

        public string ADUserName { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}