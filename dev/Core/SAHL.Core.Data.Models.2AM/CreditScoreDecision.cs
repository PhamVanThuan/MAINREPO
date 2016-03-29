using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CreditScoreDecisionDataModel :  IDataModel
    {
        public CreditScoreDecisionDataModel(int creditScoreDecisionKey, string description)
        {
            this.CreditScoreDecisionKey = creditScoreDecisionKey;
            this.Description = description;
		
        }		

        public int CreditScoreDecisionKey { get; set; }

        public string Description { get; set; }
    }
}