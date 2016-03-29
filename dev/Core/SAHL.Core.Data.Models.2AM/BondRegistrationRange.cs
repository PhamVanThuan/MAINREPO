using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BondRegistrationRangeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public BondRegistrationRangeDataModel(int? originationSourceProductKey, double? range, double? minimumBond)
        {
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.Range = range;
            this.MinimumBond = minimumBond;
		
        }
		[JsonConstructor]
        public BondRegistrationRangeDataModel(int bondRegistrationRangeKey, int? originationSourceProductKey, double? range, double? minimumBond)
        {
            this.BondRegistrationRangeKey = bondRegistrationRangeKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.Range = range;
            this.MinimumBond = minimumBond;
		
        }		

        public int BondRegistrationRangeKey { get; set; }

        public int? OriginationSourceProductKey { get; set; }

        public double? Range { get; set; }

        public double? MinimumBond { get; set; }

        public void SetKey(int key)
        {
            this.BondRegistrationRangeKey =  key;
        }
    }
}