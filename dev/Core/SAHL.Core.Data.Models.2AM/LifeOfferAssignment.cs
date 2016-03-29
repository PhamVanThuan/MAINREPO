using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LifeOfferAssignmentDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LifeOfferAssignmentDataModel(int offerKey, int loanAccountKey, int loanOfferKey, int loanOfferTypeKey, DateTime dateAssigned, string aDUserName)
        {
            this.OfferKey = offerKey;
            this.LoanAccountKey = loanAccountKey;
            this.LoanOfferKey = loanOfferKey;
            this.LoanOfferTypeKey = loanOfferTypeKey;
            this.DateAssigned = dateAssigned;
            this.ADUserName = aDUserName;
		
        }
		[JsonConstructor]
        public LifeOfferAssignmentDataModel(int lifeOfferAssignmentKey, int offerKey, int loanAccountKey, int loanOfferKey, int loanOfferTypeKey, DateTime dateAssigned, string aDUserName)
        {
            this.LifeOfferAssignmentKey = lifeOfferAssignmentKey;
            this.OfferKey = offerKey;
            this.LoanAccountKey = loanAccountKey;
            this.LoanOfferKey = loanOfferKey;
            this.LoanOfferTypeKey = loanOfferTypeKey;
            this.DateAssigned = dateAssigned;
            this.ADUserName = aDUserName;
		
        }		

        public int LifeOfferAssignmentKey { get; set; }

        public int OfferKey { get; set; }

        public int LoanAccountKey { get; set; }

        public int LoanOfferKey { get; set; }

        public int LoanOfferTypeKey { get; set; }

        public DateTime DateAssigned { get; set; }

        public string ADUserName { get; set; }

        public void SetKey(int key)
        {
            this.LifeOfferAssignmentKey =  key;
        }
    }
}