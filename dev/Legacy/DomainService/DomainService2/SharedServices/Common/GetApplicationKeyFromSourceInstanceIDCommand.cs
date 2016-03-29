using System;

namespace DomainService2.SharedServices.Common
{
    public class GetApplicationKeyFromSourceInstanceIDCommand : StandardDomainServiceCommand
    {
        public GetApplicationKeyFromSourceInstanceIDCommand(Int64 instanceID)
        {
            this.InstanceID = instanceID;
        }

        public Int64 InstanceID
        {
            get;
            protected set;
        }

        public int ApplicationKeyResult
        {
            get;
            set;
        }
    }
}