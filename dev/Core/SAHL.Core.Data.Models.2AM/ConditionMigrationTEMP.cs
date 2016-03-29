using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ConditionMigrationTEMPDataModel :  IDataModel
    {
        public ConditionMigrationTEMPDataModel(int? offerKey, int? conditionNumber, string conditionPhrase, string afrikaansConditionPhrase)
        {
            this.OfferKey = offerKey;
            this.ConditionNumber = conditionNumber;
            this.ConditionPhrase = conditionPhrase;
            this.AfrikaansConditionPhrase = afrikaansConditionPhrase;
		
        }		

        public int? OfferKey { get; set; }

        public int? ConditionNumber { get; set; }

        public string ConditionPhrase { get; set; }

        public string AfrikaansConditionPhrase { get; set; }
    }
}