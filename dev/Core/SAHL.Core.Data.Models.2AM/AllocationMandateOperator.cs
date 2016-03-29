using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AllocationMandateOperatorDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AllocationMandateOperatorDataModel(int allocationMandateSetKey, int? allocationMandateKey, int order, int? operatorKey)
        {
            this.AllocationMandateSetKey = allocationMandateSetKey;
            this.AllocationMandateKey = allocationMandateKey;
            this.Order = order;
            this.OperatorKey = operatorKey;
		
        }
		[JsonConstructor]
        public AllocationMandateOperatorDataModel(int allocationMandateOperatorKey, int allocationMandateSetKey, int? allocationMandateKey, int order, int? operatorKey)
        {
            this.AllocationMandateOperatorKey = allocationMandateOperatorKey;
            this.AllocationMandateSetKey = allocationMandateSetKey;
            this.AllocationMandateKey = allocationMandateKey;
            this.Order = order;
            this.OperatorKey = operatorKey;
		
        }		

        public int AllocationMandateOperatorKey { get; set; }

        public int AllocationMandateSetKey { get; set; }

        public int? AllocationMandateKey { get; set; }

        public int Order { get; set; }

        public int? OperatorKey { get; set; }

        public void SetKey(int key)
        {
            this.AllocationMandateOperatorKey =  key;
        }
    }
}