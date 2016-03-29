using System;

namespace DomainService2.Workflow.DebtCounselling
{
    public class GetReviewDateCommand : StandardDomainServiceCommand
    {
        public GetReviewDateCommand(int debtCounsellingKey)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
        }

        public int DebtCounsellingKey
        {
            get;
            protected set;
        }

        public DateTime? ReviewDateResult
        {
            get;
            set;
        }
    }
}