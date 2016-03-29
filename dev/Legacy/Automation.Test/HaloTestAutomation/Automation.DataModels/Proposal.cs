using Common.Enums;
using System;
using System.Collections.Generic;

namespace Automation.DataModels
{
    public sealed class Proposal : IComparable<Proposal>
    {
        public Proposal()
        {
            this.ProposalItems = new List<ProposalItem>();
        }

        public int ProposalKey { get; set; }

        public ProposalTypeEnum ProposalTypeKey { get; set; }

        public ProposalStatusEnum ProposalStatusKey { get; set; }

        public int DebtCounsellingKey { get; set; }

        public bool? HOCInclusive { get; set; }

        public bool? LifeInclusive { get; set; }

        public int ADUserKey { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ReviewDate { get; set; }

        public bool Accepted { get; set; }

        public List<ProposalItem> ProposalItems { get; set; }

        public int CompareTo(Proposal other)
        {
            if (this.ProposalTypeKey != other.ProposalTypeKey)
                return 0;
            if (this.ProposalTypeKey != other.ProposalTypeKey)
                return 0;
            if (this.ProposalStatusKey != other.ProposalStatusKey)
                return 0;
            if (this.DebtCounsellingKey != other.DebtCounsellingKey)
                return 0;
            if (this.HOCInclusive != other.HOCInclusive)
                return 0;
            if (this.LifeInclusive != other.LifeInclusive)
                return 0;
            if (this.ADUserKey != other.ADUserKey)
                return 0;
            if (this.CreateDate.Date != other.CreateDate.Date)
                return 0;
            if (this.ReviewDate.Date != other.ReviewDate.Date)
                return 0;
            if (this.Accepted != other.Accepted)
                return 0;
            return 1;
        }

        public string HOC { get; set; }

        public string Life { get; set; }

        public string Note { get; set; }
    }
}