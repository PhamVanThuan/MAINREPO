using SAHL.Core.Services;
using System;
using System.Collections.Generic;

namespace SAHL.Core.X2.Messages
{
    public class X2NotificationOfNewScheduledActivityRequest : IX2RequestForExistingInstance
    {
        public X2NotificationOfNewScheduledActivityRequest(long instanceId, int activityId, Dictionary<string, string> mapVariables = null, object data = null)
        {
            this.InstanceId = instanceId;
            this.ActivityId = activityId;
            this.RequestType = X2RequestType.Timer;
            this.CorrelationId = Guid.NewGuid();
            this.MapVariables = mapVariables;
            this.IgnoreWarnings = true;

            this.ServiceRequestMetadata = new ServiceRequestMetadata(new Dictionary<string, string>() 
                            {
                                { Services.ServiceRequestMetadata.HEADER_USERNAME, "X2" }
                            });

            this.Data = data;
        }

        public long InstanceId
        {
            get;
            protected set;
        }

        public Guid Id { get; set; }

        public bool IgnoreWarnings { get; protected set; }

        public IServiceRequestMetadata ServiceRequestMetadata { get; protected set; }

        public object Data { get; protected set; }

        public int ActivityId { get; protected set; }

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

        public X2Response Result
        {
            get;
            set;
        }
    }
}