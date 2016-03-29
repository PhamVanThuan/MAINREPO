using SAHL.Core.Logging;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2;
using SAHL.Core.X2.AppDomain;
using System;

namespace SAHL.X2Engine2
{
    public class X2TLSLegacyProcess : X2TLSProcessBase, IX2TLSLegacyProcess
    {
        public X2TLSLegacyProcess(IX2Process process, IAppDomainProcessProxy proxy, ILogger metrics)
            : base(process, proxy, metrics)
        {
        }

        public void ClearCache(ISystemMessageCollection messages, Object data)
        {
            var legacyProcess = this.GetProcess as IX2LegacyProcess;
            legacyProcess.ClearCache(messages,data);
        }
    }
}