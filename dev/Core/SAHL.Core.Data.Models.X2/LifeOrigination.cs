using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class LifeOriginationDataModel :  IDataModel
    {
        public LifeOriginationDataModel(long instanceID, string lastState, int? offerKey, int? benefitsDone, int? exclusionsDone, int? rPARDone, int? declarationDone, int? fAISDone, string rPARInsurer, string lastNTUState, bool? confirmationRequired, int? exclusionsConfirmationDone, int? declarationConfirmationDone, int? fAISConfirmationDone, string contactNumber, int? loanNumber, string assignTo, int? policyFinancialServiceKey, int? policyTypeKey, bool benefitsConfirmProceed, bool benefitsConfirmRefused, int genericKey)
        {
            this.InstanceID = instanceID;
            this.LastState = lastState;
            this.OfferKey = offerKey;
            this.BenefitsDone = benefitsDone;
            this.ExclusionsDone = exclusionsDone;
            this.RPARDone = rPARDone;
            this.DeclarationDone = declarationDone;
            this.FAISDone = fAISDone;
            this.RPARInsurer = rPARInsurer;
            this.LastNTUState = lastNTUState;
            this.ConfirmationRequired = confirmationRequired;
            this.ExclusionsConfirmationDone = exclusionsConfirmationDone;
            this.DeclarationConfirmationDone = declarationConfirmationDone;
            this.FAISConfirmationDone = fAISConfirmationDone;
            this.ContactNumber = contactNumber;
            this.LoanNumber = loanNumber;
            this.AssignTo = assignTo;
            this.PolicyFinancialServiceKey = policyFinancialServiceKey;
            this.PolicyTypeKey = policyTypeKey;
            this.BenefitsConfirmProceed = benefitsConfirmProceed;
            this.BenefitsConfirmRefused = benefitsConfirmRefused;
            this.GenericKey = genericKey;
		
        }		

        public long InstanceID { get; set; }

        public string LastState { get; set; }

        public int? OfferKey { get; set; }

        public int? BenefitsDone { get; set; }

        public int? ExclusionsDone { get; set; }

        public int? RPARDone { get; set; }

        public int? DeclarationDone { get; set; }

        public int? FAISDone { get; set; }

        public string RPARInsurer { get; set; }

        public string LastNTUState { get; set; }

        public bool? ConfirmationRequired { get; set; }

        public int? ExclusionsConfirmationDone { get; set; }

        public int? DeclarationConfirmationDone { get; set; }

        public int? FAISConfirmationDone { get; set; }

        public string ContactNumber { get; set; }

        public int? LoanNumber { get; set; }

        public string AssignTo { get; set; }

        public int? PolicyFinancialServiceKey { get; set; }

        public int? PolicyTypeKey { get; set; }

        public bool BenefitsConfirmProceed { get; set; }

        public bool BenefitsConfirmRefused { get; set; }

        public int GenericKey { get; set; }
    }
}