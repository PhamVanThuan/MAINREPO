using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class InstanceLogDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public InstanceLogDataModel(long? requestID, long? instanceID, long? parentID, long? sourceID, string activityName, DateTime? time, string message, int? errorLevel, bool? isUserActivity, string aDUser, string wFName)
        {
            this.RequestID = requestID;
            this.InstanceID = instanceID;
            this.ParentID = parentID;
            this.SourceID = sourceID;
            this.ActivityName = activityName;
            this.Time = time;
            this.Message = message;
            this.ErrorLevel = errorLevel;
            this.IsUserActivity = isUserActivity;
            this.ADUser = aDUser;
            this.WFName = wFName;
		
        }
		[JsonConstructor]
        public InstanceLogDataModel(int iD, long? requestID, long? instanceID, long? parentID, long? sourceID, string activityName, DateTime? time, string message, int? errorLevel, bool? isUserActivity, string aDUser, string wFName)
        {
            this.ID = iD;
            this.RequestID = requestID;
            this.InstanceID = instanceID;
            this.ParentID = parentID;
            this.SourceID = sourceID;
            this.ActivityName = activityName;
            this.Time = time;
            this.Message = message;
            this.ErrorLevel = errorLevel;
            this.IsUserActivity = isUserActivity;
            this.ADUser = aDUser;
            this.WFName = wFName;
		
        }		

        public int ID { get; set; }

        public long? RequestID { get; set; }

        public long? InstanceID { get; set; }

        public long? ParentID { get; set; }

        public long? SourceID { get; set; }

        public string ActivityName { get; set; }

        public DateTime? Time { get; set; }

        public string Message { get; set; }

        public int? ErrorLevel { get; set; }

        public bool? IsUserActivity { get; set; }

        public string ADUser { get; set; }

        public string WFName { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}