using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class GuaranteeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public GuaranteeDataModel(int accountKey, double limitedAmount, DateTime issueDate, byte statusNumber, DateTime? cancelledDate)
        {
            this.AccountKey = accountKey;
            this.LimitedAmount = limitedAmount;
            this.IssueDate = issueDate;
            this.StatusNumber = statusNumber;
            this.CancelledDate = cancelledDate;
		
        }
		[JsonConstructor]
        public GuaranteeDataModel(int guaranteeKey, int accountKey, double limitedAmount, DateTime issueDate, byte statusNumber, DateTime? cancelledDate)
        {
            this.GuaranteeKey = guaranteeKey;
            this.AccountKey = accountKey;
            this.LimitedAmount = limitedAmount;
            this.IssueDate = issueDate;
            this.StatusNumber = statusNumber;
            this.CancelledDate = cancelledDate;
		
        }		

        public int GuaranteeKey { get; set; }

        public int AccountKey { get; set; }

        public double LimitedAmount { get; set; }

        public DateTime IssueDate { get; set; }

        public byte StatusNumber { get; set; }

        public DateTime? CancelledDate { get; set; }

        public void SetKey(int key)
        {
            this.GuaranteeKey =  key;
        }
    }
}