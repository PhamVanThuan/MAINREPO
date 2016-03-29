using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AllocationMandateSetDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AllocationMandateSetDataModel(int allocationMandateSetGroupKey)
        {
            this.AllocationMandateSetGroupKey = allocationMandateSetGroupKey;
		
        }
		[JsonConstructor]
        public AllocationMandateSetDataModel(int allocationMandateSetKey, int allocationMandateSetGroupKey)
        {
            this.AllocationMandateSetKey = allocationMandateSetKey;
            this.AllocationMandateSetGroupKey = allocationMandateSetGroupKey;
		
        }		

        public int AllocationMandateSetKey { get; set; }

        public int AllocationMandateSetGroupKey { get; set; }

        public void SetKey(int key)
        {
            this.AllocationMandateSetKey =  key;
        }
    }
}