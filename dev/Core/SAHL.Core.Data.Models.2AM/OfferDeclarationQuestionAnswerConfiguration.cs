using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferDeclarationQuestionAnswerConfigurationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferDeclarationQuestionAnswerConfigurationDataModel(int offerDeclarationQuestionKey, int legalEntityTypeKey, int genericKey, int originationSourceProductKey, int genericKeyTypeKey)
        {
            this.OfferDeclarationQuestionKey = offerDeclarationQuestionKey;
            this.LegalEntityTypeKey = legalEntityTypeKey;
            this.GenericKey = genericKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
		
        }
		[JsonConstructor]
        public OfferDeclarationQuestionAnswerConfigurationDataModel(int offerDeclarationQuestionAnswerConfigurationKey, int offerDeclarationQuestionKey, int legalEntityTypeKey, int genericKey, int originationSourceProductKey, int genericKeyTypeKey)
        {
            this.OfferDeclarationQuestionAnswerConfigurationKey = offerDeclarationQuestionAnswerConfigurationKey;
            this.OfferDeclarationQuestionKey = offerDeclarationQuestionKey;
            this.LegalEntityTypeKey = legalEntityTypeKey;
            this.GenericKey = genericKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
		
        }		

        public int OfferDeclarationQuestionAnswerConfigurationKey { get; set; }

        public int OfferDeclarationQuestionKey { get; set; }

        public int LegalEntityTypeKey { get; set; }

        public int GenericKey { get; set; }

        public int OriginationSourceProductKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferDeclarationQuestionAnswerConfigurationKey =  key;
        }
    }
}