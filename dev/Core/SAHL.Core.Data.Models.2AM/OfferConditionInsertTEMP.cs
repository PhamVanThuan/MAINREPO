using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferConditionInsertTEMPDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferConditionInsertTEMPDataModel(int? conditionKey, int? offerKey, int? languageKey, string translatableItemDesc, string translatedText)
        {
            this.ConditionKey = conditionKey;
            this.OfferKey = offerKey;
            this.LanguageKey = languageKey;
            this.TranslatableItemDesc = translatableItemDesc;
            this.TranslatedText = translatedText;
		
        }
		[JsonConstructor]
        public OfferConditionInsertTEMPDataModel(int pk, int? conditionKey, int? offerKey, int? languageKey, string translatableItemDesc, string translatedText)
        {
            this.pk = pk;
            this.ConditionKey = conditionKey;
            this.OfferKey = offerKey;
            this.LanguageKey = languageKey;
            this.TranslatableItemDesc = translatableItemDesc;
            this.TranslatedText = translatedText;
		
        }		

        public int pk { get; set; }

        public int? ConditionKey { get; set; }

        public int? OfferKey { get; set; }

        public int? LanguageKey { get; set; }

        public string TranslatableItemDesc { get; set; }

        public string TranslatedText { get; set; }

        public void SetKey(int key)
        {
            this.pk =  key;
        }
    }
}