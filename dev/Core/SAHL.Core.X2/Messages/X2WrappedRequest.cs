using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;
using SAHL.Core.Services;

namespace SAHL.Core.X2.Messages
{
    public class X2WrappedRequest : IX2Request
    {
        public X2WrappedRequest(IX2Request x2Request)
        {
            this.RequestType = X2RequestType.WrappedRequest;
            this.X2Request = x2Request;
        }

        public IX2Request X2Request { get; set; }

        public Guid Id
        {
            get { return this.X2Request.CorrelationId; }
        }

        public X2RequestType RequestType
        {
            get;
            protected set;
        }

        public Guid CorrelationId
        {
            get { return this.X2Request.CorrelationId; }
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