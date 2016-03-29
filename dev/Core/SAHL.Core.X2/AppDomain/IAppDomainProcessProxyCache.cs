namespace SAHL.Core.X2.AppDomain
{
    public interface IAppDomainProcessProxyCache
    {
        bool ContainsProxy(int processId);

        void AddProxy(int processId, IAppDomainProcessProxy proxy);

        IAppDomainProcessProxy GetProxy(int processId);
    }
}