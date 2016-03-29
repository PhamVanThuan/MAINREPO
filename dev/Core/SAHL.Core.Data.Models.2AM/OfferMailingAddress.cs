using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferMailingAddressDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferMailingAddressDataModel(int? offerKey, int? addressKey, bool? onlineStatement, int? onlineStatementFormatKey, int? languageKey, int? legalEntityKey, int? correspondenceMediumKey)
        {
            this.OfferKey = offerKey;
            this.AddressKey = addressKey;
            this.OnlineStatement = onlineStatement;
            this.OnlineStatementFormatKey = onlineStatementFormatKey;
            this.LanguageKey = languageKey;
            this.LegalEntityKey = legalEntityKey;
            this.CorrespondenceMediumKey = correspondenceMediumKey;
		
        }
		[JsonConstructor]
        public OfferMailingAddressDataModel(int offerMailingAddressKey, int? offerKey, int? addressKey, bool? onlineStatement, int? onlineStatementFormatKey, int? languageKey, int? legalEntityKey, int? correspondenceMediumKey)
        {
            this.OfferMailingAddressKey = offerMailingAddressKey;
            this.OfferKey = offerKey;
            this.AddressKey = addressKey;
            this.OnlineStatement = onlineStatement;
            this.OnlineStatementFormatKey = onlineStatementFormatKey;
            this.LanguageKey = languageKey;
            this.LegalEntityKey = legalEntityKey;
            this.CorrespondenceMediumKey = correspondenceMediumKey;
		
        }		

        public int OfferMailingAddressKey { get; set; }

        public int? OfferKey { get; set; }

        public int? AddressKey { get; set; }

        public bool? OnlineStatement { get; set; }

        public int? OnlineStatementFormatKey { get; set; }

        public int? LanguageKey { get; set; }

        public int? LegalEntityKey { get; set; }

        public int? CorrespondenceMediumKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferMailingAddressKey =  key;
        }
    }
}