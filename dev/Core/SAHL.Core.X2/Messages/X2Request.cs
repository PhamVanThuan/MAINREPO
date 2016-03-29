using SAHL.Core.Services;
using System;
using System.Collections.Generic;

namespace SAHL.Core.X2.Messages
{
    public class X2Request : IX2Request
    {
        public X2Request(Guid correlationID, X2RequestType requestType, long? instanceId, bool ignoreWarnings, IServiceRequestMetadata serviceRequestMetadata,
            Dictionary<string, string> mapVariables = null, object data = null)
        {
            this.CorrelationId = correlationID;
            this.RequestType = requestType;
            this.InstanceId = instanceId ?? 0;
            this.IgnoreWarnings = ignoreWarnings;
            this.ServiceRequestMetadata = serviceRequestMetadata;
            this.MapVariables = mapVariables;
            this.Data = data;
        }

        public Guid Id { get; set; }

        public X2RequestType RequestType { get; protected set; }

        public Guid CorrelationId { get; protected set; }

        
        public IServiceRequestMetadata ServiceRequestMetadata { get; protected set; }

        public System.Collections.Generic.Dictionary<string, string> MapVariables { get; protected set; }

        public long InstanceId { get; protected set; }

        public bool IgnoreWarnings { get; protected set; }

        public object Data { get; protected set; }

        public X2Response Result { get; set; }
    }
}