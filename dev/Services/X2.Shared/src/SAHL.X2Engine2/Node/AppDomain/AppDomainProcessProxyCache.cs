using System;
using System.Collections.Concurrent;
using SAHL.Core.X2.AppDomain;

namespace SAHL.X2Engine2.Node.AppDomain
{
    public class AppDomainProcessProxyCache : IAppDomainProcessProxyCache
    {
        private static ConcurrentDictionary<int, IAppDomainProcessProxy> processCache = new ConcurrentDictionary<int, IAppDomainProcessProxy>();
        private static object syncObj = new object();

        public bool ContainsProxy(int processId)
        {
            return processCache.ContainsKey(processId);
        }

        public void AddProxy(int processId, IAppDomainProcessProxy proxy)
        {
            if (!processCache.TryAdd(processId, proxy))
            {
                throw new Exception(string.Format("Could not add to process proxy cache, processId: {0}", processId));
            }
        }

        public IAppDomainProcessProxy GetProxy(int processId)
        {
            return processCache[processId];
        }
    }
}