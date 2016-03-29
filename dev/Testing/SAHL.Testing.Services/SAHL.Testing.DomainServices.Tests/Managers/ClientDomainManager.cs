using SAHL.Core;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System.Collections.Generic;

namespace SAHL.Testing.Services.Tests.Managers
{
    public class ClientDomainManager : IClientDomainManager
    {
        private IClientDomainServiceClient client;

        public ClientDomainManager(IIocContainer container)
        {
            client = container.GetInstance<IClientDomainServiceClient>();
        }

        public ISystemMessageCollection AddPendingClientDomicilium(int clientAddressKey, int legalentityKey)
        {
            var model = new ClientAddressAsPendingDomiciliumModel(clientAddressKey, legalentityKey);
            var guid = CombGuid.Instance.Generate();
            var addPendingClientDomicilium = new AddClientAddressAsPendingDomiciliumCommand(model, guid);
            Dictionary<string, string> metadataDictionary = new Dictionary<string, string>();
            metadataDictionary.Add("UserName", @"SAHL\HaloUser");
            ServiceRequestMetadata metaData = new ServiceRequestMetadata(metadataDictionary);
            return client.PerformCommand(addPendingClientDomicilium, metaData);
        }
    }
}