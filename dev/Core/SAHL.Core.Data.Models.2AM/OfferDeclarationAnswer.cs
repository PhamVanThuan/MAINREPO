using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferDeclarationAnswerDataModel :  IDataModel
    {
        public OfferDeclarationAnswerDataModel(int offerDeclarationAnswerKey, string description)
        {
            this.OfferDeclarationAnswerKey = offerDeclarationAnswerKey;
            this.Description = description;
		
        }		

        public int OfferDeclarationAnswerKey { get; set; }

        public string Description { get; set; }
    }
}