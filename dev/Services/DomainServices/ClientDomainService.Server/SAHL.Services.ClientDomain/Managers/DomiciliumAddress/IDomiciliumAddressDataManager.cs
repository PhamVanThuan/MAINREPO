using SAHL.Core.Data.Models._2AM;
using System.Collections.Generic;

namespace SAHL.Services.ClientDomain.Managers.DomiciliumAddress
{
    public interface IDomiciliumAddressDataManager
    {
        IEnumerable<LegalEntityAddressDataModel> FindExistingActiveClientAddress(int clientAddressKey);

        IEnumerable<LegalEntityDomiciliumDataModel> FindExistingClientPendingDomicilium(int clientAddressKey);

        int SavePendingDomiciliumAddress(LegalEntityDomiciliumDataModel clientAddressAsPendingDomicilium);

        bool CheckIsAddressTypeAResidentialAddress(int clientAddressKey);

        bool IsClientAddressPendingDomicilium(int clientAddressKey);

        bool IsClientAddressActiveDomicilium(int clientAddressKey, int clientKey);
    }
}