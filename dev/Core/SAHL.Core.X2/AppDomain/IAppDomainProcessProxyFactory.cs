namespace SAHL.Core.X2.AppDomain
{
    public interface IAppDomainProcessProxyFactory
    {
        IAppDomainProcessProxy Create(System.AppDomain newDomain);
    }
}