using System;
using System.Reflection;

namespace SAHL.Core.X2.AppDomain
{
    public class AppDomainProcessProxyFactory : IAppDomainProcessProxyFactory
    {
        public IAppDomainProcessProxy Create(System.AppDomain newDomain)
        {
            Type proxyType = typeof(AppDomainProcessProxy);
            Assembly proxyAssembly = proxyType.Assembly;
            AppDomainProcessProxy proxy = (AppDomainProcessProxy)newDomain.CreateInstanceAndUnwrap(proxyAssembly.FullName, proxyType.FullName);
            return proxy;
        }
    }
}