using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.DomainProcessManager.DomainProcesses.Managers.Client
{
    public interface IClientDataManager
    {
        int? GetEmployerKey(string employerName);

        int GetClientKeyForClientAddress(int legalEntityAddressKey);
    }
}