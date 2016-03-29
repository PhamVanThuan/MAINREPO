using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class ActiveExternalActivityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ActiveExternalActivityDataModel(int externalActivityID, int workFlowID, long? activatingInstanceID, DateTime activationTime, string activityXMLData, string workFlowProviderName)
        {
            this.ExternalActivityID = externalActivityID;
            this.WorkFlowID = workFlowID;
            this.ActivatingInstanceID = activatingInstanceID;
            this.ActivationTime = activationTime;
            this.ActivityXMLData = activityXMLData;
            this.WorkFlowProviderName = workFlowProviderName;
		
        }
		[JsonConstructor]
        public ActiveExternalActivityDataModel(int iD, int externalActivityID, int workFlowID, long? activatingInstanceID, DateTime activationTime, string activityXMLData, string workFlowProviderName)
        {
            this.ID = iD;
            this.ExternalActivityID = externalActivityID;
            this.WorkFlowID = workFlowID;
            this.ActivatingInstanceID = activatingInstanceID;
            this.ActivationTime = activationTime;
            this.ActivityXMLData = activityXMLData;
            this.WorkFlowProviderName = workFlowProviderName;
		
        }		

        public int ID { get; set; }

        public int ExternalActivityID { get; set; }

        public int WorkFlowID { get; set; }

        public long? ActivatingInstanceID { get; set; }

        public DateTime ActivationTime { get; set; }

        public string ActivityXMLData { get; set; }

        public string WorkFlowProviderName { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}