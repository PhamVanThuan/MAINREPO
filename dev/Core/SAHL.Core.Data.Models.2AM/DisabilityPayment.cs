using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DisabilityPaymentDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DisabilityPaymentDataModel(int disabilityClaimKey, DateTime paymentDate, decimal amount, int disabilityPaymentStatusKey)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
            this.PaymentDate = paymentDate;
            this.Amount = amount;
            this.DisabilityPaymentStatusKey = disabilityPaymentStatusKey;
		
        }
		[JsonConstructor]
        public DisabilityPaymentDataModel(int disabilityPaymentKey, int disabilityClaimKey, DateTime paymentDate, decimal amount, int disabilityPaymentStatusKey)
        {
            this.DisabilityPaymentKey = disabilityPaymentKey;
            this.DisabilityClaimKey = disabilityClaimKey;
            this.PaymentDate = paymentDate;
            this.Amount = amount;
            this.DisabilityPaymentStatusKey = disabilityPaymentStatusKey;
		
        }		

        public int DisabilityPaymentKey { get; set; }

        public int DisabilityClaimKey { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal Amount { get; set; }

        public int DisabilityPaymentStatusKey { get; set; }

        public void SetKey(int key)
        {
            this.DisabilityPaymentKey =  key;
        }
    }
}