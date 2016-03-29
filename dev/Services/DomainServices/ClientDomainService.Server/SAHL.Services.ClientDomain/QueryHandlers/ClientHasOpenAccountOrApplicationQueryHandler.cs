using SAHL.Core.Services;
using SAHL.Services.Interfaces.ClientDomain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.Interfaces.ClientDomain.Models;

namespace SAHL.Services.ClientDomain.QueryHandlers
{
    public class ClientHasOpenAccountOrApplicationQueryHandler : IServiceQueryHandler<ClientHasOpenAccountOrApplicationQuery>
    {
        private IClientDataManager clientDataManager;
        public ClientHasOpenAccountOrApplicationQueryHandler(IClientDataManager clientDataManager)
        {
            this.clientDataManager = clientDataManager;
        }
        public ISystemMessageCollection HandleQuery(ClientHasOpenAccountOrApplicationQuery query)
        {
            var systemMessages = SystemMessageCollection.Empty();
            var clientHasOpenAccount = clientDataManager.FindOpenAccountKeysForClient(query.ClientKey).Any();
            var clientHasOpenApplication = clientDataManager.FindOpenApplicationNumbersForClient(query.ClientKey).Any();

            query.Result = new ServiceQueryResult<ClientHasOpenAccountOrApplicationQueryResult>(
                new ClientHasOpenAccountOrApplicationQueryResult[]
                {
                    new ClientHasOpenAccountOrApplicationQueryResult
                    {
                         ClientIsAlreadyAnSAHLCustomer = clientHasOpenAccount || clientHasOpenApplication
                    }
                }
            );

            return systemMessages;
        }

        public void HandleQuery(object query)
        {
            throw new NotImplementedException();
        }
    }
}
