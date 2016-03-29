using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferDeclarationQuestionDataModel :  IDataModel
    {
        public OfferDeclarationQuestionDataModel(int offerDeclarationQuestionKey, string description, bool displayQuestionDate, int displaySequence)
        {
            this.OfferDeclarationQuestionKey = offerDeclarationQuestionKey;
            this.Description = description;
            this.DisplayQuestionDate = displayQuestionDate;
            this.DisplaySequence = displaySequence;
		
        }		

        public int OfferDeclarationQuestionKey { get; set; }

        public string Description { get; set; }

        public bool DisplayQuestionDate { get; set; }

        public int DisplaySequence { get; set; }
    }
}