using System;

namespace DomainService2.Workflow.LoanAdjustments
{
    public class ApproveTermChangeRequestCommand : StandardDomainServiceCommand
    {
        public ApproveTermChangeRequestCommand(int accountKey, long instanceID, bool ignorewarnings)
            : base(ignorewarnings)
        {
            this.AccountKey = accountKey;
            this.InstanceID = instanceID;
        }

        public Int64 InstanceID
        {
            get;
            protected set;
        }

        public int AccountKey { get; set; }

        public bool Result { get; set; }
    }
}