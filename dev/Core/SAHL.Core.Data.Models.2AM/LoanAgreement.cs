using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LoanAgreementDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LoanAgreementDataModel(DateTime agreementDate, double amount, string userName, int bondKey, DateTime? changeDate)
        {
            this.AgreementDate = agreementDate;
            this.Amount = amount;
            this.UserName = userName;
            this.BondKey = bondKey;
            this.ChangeDate = changeDate;
		
        }
		[JsonConstructor]
        public LoanAgreementDataModel(int loanAgreementKey, DateTime agreementDate, double amount, string userName, int bondKey, DateTime? changeDate)
        {
            this.LoanAgreementKey = loanAgreementKey;
            this.AgreementDate = agreementDate;
            this.Amount = amount;
            this.UserName = userName;
            this.BondKey = bondKey;
            this.ChangeDate = changeDate;
		
        }		

        public int LoanAgreementKey { get; set; }

        public DateTime AgreementDate { get; set; }

        public double Amount { get; set; }

        public string UserName { get; set; }

        public int BondKey { get; set; }

        public DateTime? ChangeDate { get; set; }

        public void SetKey(int key)
        {
            this.LoanAgreementKey =  key;
        }
    }
}