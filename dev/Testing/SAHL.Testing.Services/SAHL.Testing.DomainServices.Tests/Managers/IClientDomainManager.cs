using SAHL.Core.SystemMessages;

namespace SAHL.Testing.Services.Tests.Managers
{
    public interface IClientDomainManager
    {
        ISystemMessageCollection AddPendingClientDomicilium(int clientAddressKey, int legalentityKey);
    }
}