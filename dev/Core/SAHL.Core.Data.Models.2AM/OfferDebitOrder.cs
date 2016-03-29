using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferDebitOrderDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferDebitOrderDataModel(int offerKey, int? bankAccountKey, double percentage, int debitOrderDay, int financialServicePaymentTypeKey)
        {
            this.OfferKey = offerKey;
            this.BankAccountKey = bankAccountKey;
            this.Percentage = percentage;
            this.DebitOrderDay = debitOrderDay;
            this.FinancialServicePaymentTypeKey = financialServicePaymentTypeKey;
		
        }
		[JsonConstructor]
        public OfferDebitOrderDataModel(int offerDebitOrderKey, int offerKey, int? bankAccountKey, double percentage, int debitOrderDay, int financialServicePaymentTypeKey)
        {
            this.OfferDebitOrderKey = offerDebitOrderKey;
            this.OfferKey = offerKey;
            this.BankAccountKey = bankAccountKey;
            this.Percentage = percentage;
            this.DebitOrderDay = debitOrderDay;
            this.FinancialServicePaymentTypeKey = financialServicePaymentTypeKey;
		
        }		

        public int OfferDebitOrderKey { get; set; }

        public int OfferKey { get; set; }

        public int? BankAccountKey { get; set; }

        public double Percentage { get; set; }

        public int DebitOrderDay { get; set; }

        public int FinancialServicePaymentTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferDebitOrderKey =  key;
        }
    }
}