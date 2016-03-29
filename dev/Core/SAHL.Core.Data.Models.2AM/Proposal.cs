using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ProposalDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ProposalDataModel(int proposalTypeKey, int proposalStatusKey, int debtCounsellingKey, bool hOCInclusive, bool lifeInclusive, int aDUserKey, DateTime createDate, DateTime? reviewDate, bool? accepted, bool monthlyServiceFee)
        {
            this.ProposalTypeKey = proposalTypeKey;
            this.ProposalStatusKey = proposalStatusKey;
            this.DebtCounsellingKey = debtCounsellingKey;
            this.HOCInclusive = hOCInclusive;
            this.LifeInclusive = lifeInclusive;
            this.ADUserKey = aDUserKey;
            this.CreateDate = createDate;
            this.ReviewDate = reviewDate;
            this.Accepted = accepted;
            this.MonthlyServiceFee = monthlyServiceFee;
		
        }
		[JsonConstructor]
        public ProposalDataModel(int proposalKey, int proposalTypeKey, int proposalStatusKey, int debtCounsellingKey, bool hOCInclusive, bool lifeInclusive, int aDUserKey, DateTime createDate, DateTime? reviewDate, bool? accepted, bool monthlyServiceFee)
        {
            this.ProposalKey = proposalKey;
            this.ProposalTypeKey = proposalTypeKey;
            this.ProposalStatusKey = proposalStatusKey;
            this.DebtCounsellingKey = debtCounsellingKey;
            this.HOCInclusive = hOCInclusive;
            this.LifeInclusive = lifeInclusive;
            this.ADUserKey = aDUserKey;
            this.CreateDate = createDate;
            this.ReviewDate = reviewDate;
            this.Accepted = accepted;
            this.MonthlyServiceFee = monthlyServiceFee;
		
        }		

        public int ProposalKey { get; set; }

        public int ProposalTypeKey { get; set; }

        public int ProposalStatusKey { get; set; }

        public int DebtCounsellingKey { get; set; }

        public bool HOCInclusive { get; set; }

        public bool LifeInclusive { get; set; }

        public int ADUserKey { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? ReviewDate { get; set; }

        public bool? Accepted { get; set; }

        public bool MonthlyServiceFee { get; set; }

        public void SetKey(int key)
        {
            this.ProposalKey =  key;
        }
    }
}