using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class RCSDataModel :  IDataModel
    {
        public RCSDataModel(long instanceID, string fromStage, string incomeConfirmed, string moveToStage, int? reference, DateTime? registrationValidationDate, string userName, string validDisbursement, string valuationReceived, int? offerKey, int? accountKey)
        {
            this.InstanceID = instanceID;
            this.FromStage = fromStage;
            this.IncomeConfirmed = incomeConfirmed;
            this.MoveToStage = moveToStage;
            this.Reference = reference;
            this.RegistrationValidationDate = registrationValidationDate;
            this.UserName = userName;
            this.ValidDisbursement = validDisbursement;
            this.ValuationReceived = valuationReceived;
            this.offerKey = offerKey;
            this.accountKey = accountKey;
		
        }		

        public long InstanceID { get; set; }

        public string FromStage { get; set; }

        public string IncomeConfirmed { get; set; }

        public string MoveToStage { get; set; }

        public int? Reference { get; set; }

        public DateTime? RegistrationValidationDate { get; set; }

        public string UserName { get; set; }

        public string ValidDisbursement { get; set; }

        public string ValuationReceived { get; set; }

        public int? offerKey { get; set; }

        public int? accountKey { get; set; }
    }
}