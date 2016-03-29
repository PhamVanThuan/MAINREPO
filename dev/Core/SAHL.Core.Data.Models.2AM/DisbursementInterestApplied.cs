using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DisbursementInterestAppliedDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DisbursementInterestAppliedDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public DisbursementInterestAppliedDataModel(int interestAppliedTypeKey, string description)
        {
            this.InterestAppliedTypeKey = interestAppliedTypeKey;
            this.Description = description;
		
        }		

        public int InterestAppliedTypeKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.InterestAppliedTypeKey =  key;
        }
    }
}