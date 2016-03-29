using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class LogDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LogDataModel(DateTime date, int? processID, int? workFlowID, int? instanceID, int? stateID, int? activityID, string aDUserName, string message, string stackTrace)
        {
            this.Date = date;
            this.ProcessID = processID;
            this.WorkFlowID = workFlowID;
            this.InstanceID = instanceID;
            this.StateID = stateID;
            this.ActivityID = activityID;
            this.ADUserName = aDUserName;
            this.Message = message;
            this.StackTrace = stackTrace;
		
        }
		[JsonConstructor]
        public LogDataModel(int iD, DateTime date, int? processID, int? workFlowID, int? instanceID, int? stateID, int? activityID, string aDUserName, string message, string stackTrace)
        {
            this.ID = iD;
            this.Date = date;
            this.ProcessID = processID;
            this.WorkFlowID = workFlowID;
            this.InstanceID = instanceID;
            this.StateID = stateID;
            this.ActivityID = activityID;
            this.ADUserName = aDUserName;
            this.Message = message;
            this.StackTrace = stackTrace;
		
        }		

        public int ID { get; set; }

        public DateTime Date { get; set; }

        public int? ProcessID { get; set; }

        public int? WorkFlowID { get; set; }

        public int? InstanceID { get; set; }

        public int? StateID { get; set; }

        public int? ActivityID { get; set; }

        public string ADUserName { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}