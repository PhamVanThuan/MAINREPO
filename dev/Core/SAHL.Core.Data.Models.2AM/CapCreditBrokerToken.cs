using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CapCreditBrokerTokenDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CapCreditBrokerTokenDataModel(int brokerKey, bool lastAssigned)
        {
            this.BrokerKey = brokerKey;
            this.LastAssigned = lastAssigned;
		
        }
		[JsonConstructor]
        public CapCreditBrokerTokenDataModel(int capCreditBrokerKey, int brokerKey, bool lastAssigned)
        {
            this.CapCreditBrokerKey = capCreditBrokerKey;
            this.BrokerKey = brokerKey;
            this.LastAssigned = lastAssigned;
		
        }		

        public int CapCreditBrokerKey { get; set; }

        public int BrokerKey { get; set; }

        public bool LastAssigned { get; set; }

        public void SetKey(int key)
        {
            this.CapCreditBrokerKey =  key;
        }
    }
}