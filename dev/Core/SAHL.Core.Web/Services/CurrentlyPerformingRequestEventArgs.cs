using SAHL.Core.Services;
using System;

namespace SAHL.Core.Web.Services
{
    public class CurrentlyPerformingRequestEventArgs : EventArgs
    {
        public Type RequestType
        { get; protected set; }

        public IServiceRequestMetadata Metadata
        { get; protected set; }

        public CurrentlyPerformingRequestEventArgs(Type requestType, IServiceRequestMetadata metadata)
        {
            this.RequestType = requestType;
            this.Metadata = metadata;
        }
    }
}