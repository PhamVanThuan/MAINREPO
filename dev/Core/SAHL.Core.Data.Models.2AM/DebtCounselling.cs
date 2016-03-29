using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DebtCounsellingDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DebtCounsellingDataModel(int debtCounsellingGroupKey, int accountKey, int debtCounsellingStatusKey, DateTime? paymentReceivedDate, double? paymentReceivedAmount, DateTime? diaryDate, string referenceNumber)
        {
            this.DebtCounsellingGroupKey = debtCounsellingGroupKey;
            this.AccountKey = accountKey;
            this.DebtCounsellingStatusKey = debtCounsellingStatusKey;
            this.PaymentReceivedDate = paymentReceivedDate;
            this.PaymentReceivedAmount = paymentReceivedAmount;
            this.DiaryDate = diaryDate;
            this.ReferenceNumber = referenceNumber;
		
        }
		[JsonConstructor]
        public DebtCounsellingDataModel(int debtCounsellingKey, int debtCounsellingGroupKey, int accountKey, int debtCounsellingStatusKey, DateTime? paymentReceivedDate, double? paymentReceivedAmount, DateTime? diaryDate, string referenceNumber)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
            this.DebtCounsellingGroupKey = debtCounsellingGroupKey;
            this.AccountKey = accountKey;
            this.DebtCounsellingStatusKey = debtCounsellingStatusKey;
            this.PaymentReceivedDate = paymentReceivedDate;
            this.PaymentReceivedAmount = paymentReceivedAmount;
            this.DiaryDate = diaryDate;
            this.ReferenceNumber = referenceNumber;
		
        }		

        public int DebtCounsellingKey { get; set; }

        public int DebtCounsellingGroupKey { get; set; }

        public int AccountKey { get; set; }

        public int DebtCounsellingStatusKey { get; set; }

        public DateTime? PaymentReceivedDate { get; set; }

        public double? PaymentReceivedAmount { get; set; }

        public DateTime? DiaryDate { get; set; }

        public string ReferenceNumber { get; set; }

        public void SetKey(int key)
        {
            this.DebtCounsellingKey =  key;
        }
    }
}