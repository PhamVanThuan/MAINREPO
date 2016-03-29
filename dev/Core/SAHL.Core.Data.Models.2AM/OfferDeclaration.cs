using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferDeclarationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferDeclarationDataModel(int offerRoleKey, int offerDeclarationQuestionKey, int offerDeclarationAnswerKey, DateTime? offerDeclarationDate)
        {
            this.OfferRoleKey = offerRoleKey;
            this.OfferDeclarationQuestionKey = offerDeclarationQuestionKey;
            this.OfferDeclarationAnswerKey = offerDeclarationAnswerKey;
            this.OfferDeclarationDate = offerDeclarationDate;
		
        }
		[JsonConstructor]
        public OfferDeclarationDataModel(int offerDeclarationKey, int offerRoleKey, int offerDeclarationQuestionKey, int offerDeclarationAnswerKey, DateTime? offerDeclarationDate)
        {
            this.OfferDeclarationKey = offerDeclarationKey;
            this.OfferRoleKey = offerRoleKey;
            this.OfferDeclarationQuestionKey = offerDeclarationQuestionKey;
            this.OfferDeclarationAnswerKey = offerDeclarationAnswerKey;
            this.OfferDeclarationDate = offerDeclarationDate;
		
        }		

        public int OfferDeclarationKey { get; set; }

        public int OfferRoleKey { get; set; }

        public int OfferDeclarationQuestionKey { get; set; }

        public int OfferDeclarationAnswerKey { get; set; }

        public DateTime? OfferDeclarationDate { get; set; }

        public void SetKey(int key)
        {
            this.OfferDeclarationKey =  key;
        }
    }
}