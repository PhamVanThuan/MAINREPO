using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BatchAmountsDataModel :  IDataModel
    {
        public BatchAmountsDataModel(DateTime dateToday, int financialServiceKey, double? installmentAmount, double? fixedDebitOrderAmount, double? hOC, double? proRataHOC, double? regent, double? life, double? other, double? currentBalance, string underCancellation, DateTime? loanOpendate, string badBankDetail)
        {
            this.DateToday = dateToday;
            this.FinancialServiceKey = financialServiceKey;
            this.InstallmentAmount = installmentAmount;
            this.FixedDebitOrderAmount = fixedDebitOrderAmount;
            this.HOC = hOC;
            this.ProRataHOC = proRataHOC;
            this.Regent = regent;
            this.Life = life;
            this.Other = other;
            this.CurrentBalance = currentBalance;
            this.UnderCancellation = underCancellation;
            this.LoanOpendate = loanOpendate;
            this.BadBankDetail = badBankDetail;
		
        }		

        public DateTime DateToday { get; set; }

        public int FinancialServiceKey { get; set; }

        public double? InstallmentAmount { get; set; }

        public double? FixedDebitOrderAmount { get; set; }

        public double? HOC { get; set; }

        public double? ProRataHOC { get; set; }

        public double? Regent { get; set; }

        public double? Life { get; set; }

        public double? Other { get; set; }

        public double? CurrentBalance { get; set; }

        public string UnderCancellation { get; set; }

        public DateTime? LoanOpendate { get; set; }

        public string BadBankDetail { get; set; }
    }
}