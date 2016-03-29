using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DebtCounsellingGroupDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DebtCounsellingGroupDataModel(DateTime createdDate)
        {
            this.CreatedDate = createdDate;
		
        }
		[JsonConstructor]
        public DebtCounsellingGroupDataModel(int debtCounsellingGroupKey, DateTime createdDate)
        {
            this.DebtCounsellingGroupKey = debtCounsellingGroupKey;
            this.CreatedDate = createdDate;
		
        }		

        public int DebtCounsellingGroupKey { get; set; }

        public DateTime CreatedDate { get; set; }

        public void SetKey(int key)
        {
            this.DebtCounsellingGroupKey =  key;
        }
    }
}