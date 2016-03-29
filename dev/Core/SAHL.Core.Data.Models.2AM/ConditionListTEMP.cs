using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ConditionListTEMPDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ConditionListTEMPDataModel(int? fixedConditionNumber, int? conditionNumber, int? conditionTypeNumber, int? prospectNumber, int? loanNumber, int? offerKey)
        {
            this.FixedConditionNumber = fixedConditionNumber;
            this.ConditionNumber = conditionNumber;
            this.ConditionTypeNumber = conditionTypeNumber;
            this.ProspectNumber = prospectNumber;
            this.LoanNumber = loanNumber;
            this.OfferKey = offerKey;
		
        }
		[JsonConstructor]
        public ConditionListTEMPDataModel(int pk, int? fixedConditionNumber, int? conditionNumber, int? conditionTypeNumber, int? prospectNumber, int? loanNumber, int? offerKey)
        {
            this.pk = pk;
            this.FixedConditionNumber = fixedConditionNumber;
            this.ConditionNumber = conditionNumber;
            this.ConditionTypeNumber = conditionTypeNumber;
            this.ProspectNumber = prospectNumber;
            this.LoanNumber = loanNumber;
            this.OfferKey = offerKey;
		
        }		

        public int pk { get; set; }

        public int? FixedConditionNumber { get; set; }

        public int? ConditionNumber { get; set; }

        public int? ConditionTypeNumber { get; set; }

        public int? ProspectNumber { get; set; }

        public int? LoanNumber { get; set; }

        public int? OfferKey { get; set; }

        public void SetKey(int key)
        {
            this.pk =  key;
        }
    }
}