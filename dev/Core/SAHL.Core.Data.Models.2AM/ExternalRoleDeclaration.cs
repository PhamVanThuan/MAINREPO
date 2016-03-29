using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ExternalRoleDeclarationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ExternalRoleDeclarationDataModel(int externalRoleKey, int offerDeclarationQuestionKey, int offerDeclarationAnswerKey, DateTime? externalRoleDeclarationDate)
        {
            this.ExternalRoleKey = externalRoleKey;
            this.OfferDeclarationQuestionKey = offerDeclarationQuestionKey;
            this.OfferDeclarationAnswerKey = offerDeclarationAnswerKey;
            this.ExternalRoleDeclarationDate = externalRoleDeclarationDate;
		
        }
		[JsonConstructor]
        public ExternalRoleDeclarationDataModel(int externalRoleDeclarationKey, int externalRoleKey, int offerDeclarationQuestionKey, int offerDeclarationAnswerKey, DateTime? externalRoleDeclarationDate)
        {
            this.ExternalRoleDeclarationKey = externalRoleDeclarationKey;
            this.ExternalRoleKey = externalRoleKey;
            this.OfferDeclarationQuestionKey = offerDeclarationQuestionKey;
            this.OfferDeclarationAnswerKey = offerDeclarationAnswerKey;
            this.ExternalRoleDeclarationDate = externalRoleDeclarationDate;
		
        }		

        public int ExternalRoleDeclarationKey { get; set; }

        public int ExternalRoleKey { get; set; }

        public int OfferDeclarationQuestionKey { get; set; }

        public int OfferDeclarationAnswerKey { get; set; }

        public DateTime? ExternalRoleDeclarationDate { get; set; }

        public void SetKey(int key)
        {
            this.ExternalRoleDeclarationKey =  key;
        }
    }
}