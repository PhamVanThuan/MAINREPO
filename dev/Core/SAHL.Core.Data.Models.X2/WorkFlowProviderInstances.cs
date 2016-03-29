using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class WorkFlowProviderInstancesDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public WorkFlowProviderInstancesDataModel(string workFlowProviderName, DateTime activeDate)
        {
            this.WorkFlowProviderName = workFlowProviderName;
            this.ActiveDate = activeDate;
		
        }
		[JsonConstructor]
        public WorkFlowProviderInstancesDataModel(int iD, string workFlowProviderName, DateTime activeDate)
        {
            this.ID = iD;
            this.WorkFlowProviderName = workFlowProviderName;
            this.ActiveDate = activeDate;
		
        }		

        public int ID { get; set; }

        public string WorkFlowProviderName { get; set; }

        public DateTime ActiveDate { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}