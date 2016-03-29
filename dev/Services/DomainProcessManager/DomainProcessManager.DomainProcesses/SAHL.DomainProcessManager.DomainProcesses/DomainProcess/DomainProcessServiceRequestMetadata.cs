using SAHL.Core;
using SAHL.Core.Services;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess
{
    public class DomainProcessServiceRequestMetadata : ServiceRequestMetadata
    {
        public DomainProcessServiceRequestMetadata(Guid domainProcessId, Guid commandCorrelationId)
            : base()
        {
            this.Add(CoreGlobals.DomainProcessIdName, domainProcessId.ToString());
            this.Add(DomainProcessManagerGlobals.CommandCorrelationKey, commandCorrelationId.ToString());
        }

        public Guid CommandCorrelationId { get { return Guid.Parse(this[DomainProcessManagerGlobals.CommandCorrelationKey]); } }
    }

    public static class DomainProcessManagerGlobals
    {
        public static string CommandCorrelationKey { get { return "CommandCorrelationId"; } }
    }
}