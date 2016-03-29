using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferDeclarationQuestionAnswerDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferDeclarationQuestionAnswerDataModel(int offerDeclarationQuestionKey, int offerDeclarationAnswerKey)
        {
            this.OfferDeclarationQuestionKey = offerDeclarationQuestionKey;
            this.OfferDeclarationAnswerKey = offerDeclarationAnswerKey;
		
        }
		[JsonConstructor]
        public OfferDeclarationQuestionAnswerDataModel(int offerDeclarationQuestionAnswerKey, int offerDeclarationQuestionKey, int offerDeclarationAnswerKey)
        {
            this.OfferDeclarationQuestionAnswerKey = offerDeclarationQuestionAnswerKey;
            this.OfferDeclarationQuestionKey = offerDeclarationQuestionKey;
            this.OfferDeclarationAnswerKey = offerDeclarationAnswerKey;
		
        }		

        public int OfferDeclarationQuestionAnswerKey { get; set; }

        public int OfferDeclarationQuestionKey { get; set; }

        public int OfferDeclarationAnswerKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferDeclarationQuestionAnswerKey =  key;
        }
    }
}