using SAHL.Core.Identity;
using System;
using System.Collections.Generic;
using SAHL.Core.Services;

namespace SAHL.Core.X2.Messages
{
    public class X2BundledRequest : IX2Request
    {
        public X2BundledRequest(IEnumerable<IX2Request> requests)
        {
            Requests = requests;
            RequestType = X2RequestType.BundledRequest;
            CorrelationId = CombGuid.Instance.Generate();
        }

        public IEnumerable<IX2Request> Requests 
        { 
            get; 
            set; 
        }

        public X2RequestType RequestType
        {
            get;
            protected set;
        }

        public Guid CorrelationId
        {
            get;
            protected set;
        }

        public Dictionary<string, string> MapVariables
        {
            get;
            protected set;
        }

        public long InstanceId
        {
            get;
            protected set;
        }

        public bool IgnoreWarnings
        {
            get;
            protected set;
        }

        public string UserName
        {
            get;
            protected set;
        }

        public object Data
        {
            get;
            protected set;
        }

        public Guid Id
        {
            get;
            protected set;
        }

        public X2Response Result
        {
            get;
            set;
        }

        public IServiceRequestMetadata ServiceRequestMetadata
        {
            get
            {
                return null;
            }
        }
    }
}