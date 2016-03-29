using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class HOCReassurance_SnapshotDataModel :  IDataModel
    {
        public HOCReassurance_SnapshotDataModel(DateTime? monthEndDate, int policyNumber, int loanNumber, double? insuredAmount, decimal? monthlyPremium, decimal? reassurancePremium, decimal? sASRIAAmount, int? spvKey, double? hOCAdministrationFee, double? hOCProRataPremium, double? totalHOCPremium, double? postedHOCPremium)
        {
            this.MonthEndDate = monthEndDate;
            this.PolicyNumber = policyNumber;
            this.LoanNumber = loanNumber;
            this.InsuredAmount = insuredAmount;
            this.MonthlyPremium = monthlyPremium;
            this.ReassurancePremium = reassurancePremium;
            this.SASRIAAmount = sASRIAAmount;
            this.SpvKey = spvKey;
            this.HOCAdministrationFee = hOCAdministrationFee;
            this.HOCProRataPremium = hOCProRataPremium;
            this.TotalHOCPremium = totalHOCPremium;
            this.PostedHOCPremium = postedHOCPremium;
		
        }		

        public DateTime? MonthEndDate { get; set; }

        public int PolicyNumber { get; set; }

        public int LoanNumber { get; set; }

        public double? InsuredAmount { get; set; }

        public decimal? MonthlyPremium { get; set; }

        public decimal? ReassurancePremium { get; set; }

        public decimal? SASRIAAmount { get; set; }

        public int? SpvKey { get; set; }

        public double? HOCAdministrationFee { get; set; }

        public double? HOCProRataPremium { get; set; }

        public double? TotalHOCPremium { get; set; }

        public double? PostedHOCPremium { get; set; }
    }
}