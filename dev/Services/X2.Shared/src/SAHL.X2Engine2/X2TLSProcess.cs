using SAHL.Core.Logging;
using SAHL.Core.X2;
using SAHL.Core.X2.AppDomain;

namespace SAHL.X2Engine2
{
    /// <summary>
    /// X2 Thread Local Storage Process
    /// </summary>
    public class X2TLSProcess : X2TLSProcessBase
    {
        public X2TLSProcess(IX2Process process, IAppDomainProcessProxy proxy, ILogger metrics)
            : base(process, proxy, metrics)
        {

        }
    }
}