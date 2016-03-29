using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class FinancialServiceBankAccountDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public FinancialServiceBankAccountDataModel(int financialServiceKey, int? bankAccountKey, double percentage, int debitOrderDay, int generalStatusKey, string userID, DateTime? changeDate, int financialServicePaymentTypeKey, int? paymentSplitTypeKey, int? providerKey, bool? isNaedoCompliant)
        {
            this.FinancialServiceKey = financialServiceKey;
            this.BankAccountKey = bankAccountKey;
            this.Percentage = percentage;
            this.DebitOrderDay = debitOrderDay;
            this.GeneralStatusKey = generalStatusKey;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.FinancialServicePaymentTypeKey = financialServicePaymentTypeKey;
            this.PaymentSplitTypeKey = paymentSplitTypeKey;
            this.ProviderKey = providerKey;
            this.IsNaedoCompliant = isNaedoCompliant;
		
        }
		[JsonConstructor]
        public FinancialServiceBankAccountDataModel(int financialServiceBankAccountKey, int financialServiceKey, int? bankAccountKey, double percentage, int debitOrderDay, int generalStatusKey, string userID, DateTime? changeDate, int financialServicePaymentTypeKey, int? paymentSplitTypeKey, int? providerKey, bool? isNaedoCompliant)
        {
            this.FinancialServiceBankAccountKey = financialServiceBankAccountKey;
            this.FinancialServiceKey = financialServiceKey;
            this.BankAccountKey = bankAccountKey;
            this.Percentage = percentage;
            this.DebitOrderDay = debitOrderDay;
            this.GeneralStatusKey = generalStatusKey;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.FinancialServicePaymentTypeKey = financialServicePaymentTypeKey;
            this.PaymentSplitTypeKey = paymentSplitTypeKey;
            this.ProviderKey = providerKey;
            this.IsNaedoCompliant = isNaedoCompliant;
		
        }		

        public int FinancialServiceBankAccountKey { get; set; }

        public int FinancialServiceKey { get; set; }

        public int? BankAccountKey { get; set; }

        public double Percentage { get; set; }

        public int DebitOrderDay { get; set; }

        public int GeneralStatusKey { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public int FinancialServicePaymentTypeKey { get; set; }

        public int? PaymentSplitTypeKey { get; set; }

        public int? ProviderKey { get; set; }

        public bool? IsNaedoCompliant { get; set; }

        public void SetKey(int key)
        {
            this.FinancialServiceBankAccountKey =  key;
        }
    }
}