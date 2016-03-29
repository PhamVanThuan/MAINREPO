using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class TranslatedTextDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public TranslatedTextDataModel(int translatableItemKey, int languageKey, string translatedText)
        {
            this.TranslatableItemKey = translatableItemKey;
            this.LanguageKey = languageKey;
            this.TranslatedText = translatedText;
		
        }
		[JsonConstructor]
        public TranslatedTextDataModel(int translatedTextKey, int translatableItemKey, int languageKey, string translatedText)
        {
            this.TranslatedTextKey = translatedTextKey;
            this.TranslatableItemKey = translatableItemKey;
            this.LanguageKey = languageKey;
            this.TranslatedText = translatedText;
		
        }		

        public int TranslatedTextKey { get; set; }

        public int TranslatableItemKey { get; set; }

        public int LanguageKey { get; set; }

        public string TranslatedText { get; set; }

        public void SetKey(int key)
        {
            this.TranslatedTextKey =  key;
        }
    }
}