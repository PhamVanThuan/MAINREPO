using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AttorneyDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AttorneyDataModel(int? deedsOfficeKey, string attorneyContact, double? attorneyMandate, int? attorneyWorkFlowEnabled, double? attorneyLoanTarget, double? attorneyFurtherLoanTarget, bool? attorneyLitigationInd, int legalEntityKey, bool? attorneyRegistrationInd, int generalStatusKey)
        {
            this.DeedsOfficeKey = deedsOfficeKey;
            this.AttorneyContact = attorneyContact;
            this.AttorneyMandate = attorneyMandate;
            this.AttorneyWorkFlowEnabled = attorneyWorkFlowEnabled;
            this.AttorneyLoanTarget = attorneyLoanTarget;
            this.AttorneyFurtherLoanTarget = attorneyFurtherLoanTarget;
            this.AttorneyLitigationInd = attorneyLitigationInd;
            this.LegalEntityKey = legalEntityKey;
            this.AttorneyRegistrationInd = attorneyRegistrationInd;
            this.GeneralStatusKey = generalStatusKey;
		
        }
		[JsonConstructor]
        public AttorneyDataModel(int attorneyKey, int? deedsOfficeKey, string attorneyContact, double? attorneyMandate, int? attorneyWorkFlowEnabled, double? attorneyLoanTarget, double? attorneyFurtherLoanTarget, bool? attorneyLitigationInd, int legalEntityKey, bool? attorneyRegistrationInd, int generalStatusKey)
        {
            this.AttorneyKey = attorneyKey;
            this.DeedsOfficeKey = deedsOfficeKey;
            this.AttorneyContact = attorneyContact;
            this.AttorneyMandate = attorneyMandate;
            this.AttorneyWorkFlowEnabled = attorneyWorkFlowEnabled;
            this.AttorneyLoanTarget = attorneyLoanTarget;
            this.AttorneyFurtherLoanTarget = attorneyFurtherLoanTarget;
            this.AttorneyLitigationInd = attorneyLitigationInd;
            this.LegalEntityKey = legalEntityKey;
            this.AttorneyRegistrationInd = attorneyRegistrationInd;
            this.GeneralStatusKey = generalStatusKey;
		
        }		

        public int AttorneyKey { get; set; }

        public int? DeedsOfficeKey { get; set; }

        public string AttorneyContact { get; set; }

        public double? AttorneyMandate { get; set; }

        public int? AttorneyWorkFlowEnabled { get; set; }

        public double? AttorneyLoanTarget { get; set; }

        public double? AttorneyFurtherLoanTarget { get; set; }

        public bool? AttorneyLitigationInd { get; set; }

        public int LegalEntityKey { get; set; }

        public bool? AttorneyRegistrationInd { get; set; }

        public int GeneralStatusKey { get; set; }

        public void SetKey(int key)
        {
            this.AttorneyKey =  key;
        }
    }
}