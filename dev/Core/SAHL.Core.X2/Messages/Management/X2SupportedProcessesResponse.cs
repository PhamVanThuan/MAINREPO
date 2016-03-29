using System;
using System.Collections.Generic;

namespace SAHL.Core.X2.Messages.Management
{
    public class X2SupportedProcessesResponse : X2Response
    {
        public Dictionary<string, List<string>> SupportedProcesses { get; set; }

        public X2SupportedProcessesResponse(Guid requestID, string message, long instanceId, Dictionary<string, List<string>> supportedProcesses, bool isErrorResponse = false)
            : base(requestID, message, instanceId, isErrorResponse)
        {
            this.SupportedProcesses = supportedProcesses;
        }
    }
}