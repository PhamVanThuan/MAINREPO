using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LanguageDataModel :  IDataModel
    {
        public LanguageDataModel(int languageKey, string description, bool translatable)
        {
            this.LanguageKey = languageKey;
            this.Description = description;
            this.Translatable = translatable;
		
        }		

        public int LanguageKey { get; set; }

        public string Description { get; set; }

        public bool Translatable { get; set; }
    }
}