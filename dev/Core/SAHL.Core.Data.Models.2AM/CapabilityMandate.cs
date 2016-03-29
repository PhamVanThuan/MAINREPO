using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CapabilityMandateDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CapabilityMandateDataModel(int mandateTypeKey, int capabilityKey, decimal? startRange, decimal? endRange)
        {
            this.MandateTypeKey = mandateTypeKey;
            this.CapabilityKey = capabilityKey;
            this.StartRange = startRange;
            this.EndRange = endRange;
		
        }
		[JsonConstructor]
        public CapabilityMandateDataModel(int capabilityMandateKey, int mandateTypeKey, int capabilityKey, decimal? startRange, decimal? endRange)
        {
            this.CapabilityMandateKey = capabilityMandateKey;
            this.MandateTypeKey = mandateTypeKey;
            this.CapabilityKey = capabilityKey;
            this.StartRange = startRange;
            this.EndRange = endRange;
		
        }		

        public int CapabilityMandateKey { get; set; }

        public int MandateTypeKey { get; set; }

        public int CapabilityKey { get; set; }

        public decimal? StartRange { get; set; }

        public decimal? EndRange { get; set; }

        public void SetKey(int key)
        {
            this.CapabilityMandateKey =  key;
        }
    }
}