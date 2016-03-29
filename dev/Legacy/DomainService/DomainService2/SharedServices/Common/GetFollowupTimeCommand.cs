using System;

namespace DomainService2.SharedServices.Common
{
    public class GetFollowupTimeCommand : StandardDomainServiceCommand
    {
        public GetFollowupTimeCommand(int memoKey)
        {
            this.MemoKey = memoKey;
        }

        public int MemoKey
        {
            get;
            protected set;
        }

        public DateTime Result
        {
            get;
            set;
        }
    }
}