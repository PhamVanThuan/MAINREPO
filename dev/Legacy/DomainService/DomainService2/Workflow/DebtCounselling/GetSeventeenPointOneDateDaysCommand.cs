using System;

namespace DomainService2.Workflow.DebtCounselling
{
    public class GetSeventeenPointOneDateDaysCommand : StandardDomainServiceCommand
    {
        public GetSeventeenPointOneDateDaysCommand(int debtCounsellingKey, int days)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
            this.Days = days;
        }

        public int DebtCounsellingKey
        {
            get;
            protected set;
        }

        public int Days
        {
            get;
            protected set;
        }

        public DateTime? SeventeenPointOneDatePlusDaysResult
        {
            get;
            set;
        }
    }
}