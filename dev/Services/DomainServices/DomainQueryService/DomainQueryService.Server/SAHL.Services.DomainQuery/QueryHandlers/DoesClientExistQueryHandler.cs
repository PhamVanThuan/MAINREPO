using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using SAHL.Core.SystemMessages;
using SAHL.Services.DomainQuery.Managers.Client;
using SAHL.Services.Interfaces.DomainQuery.Model;

namespace SAHL.Services.DomainQuery.QueryHandlers
{
    public class DoesClientExistQueryHandler : IServiceQueryHandler<DoesClientExistQuery>
    {
        private IClientDataManager clientDataManager;
        public DoesClientExistQueryHandler(IClientDataManager clientDataManager)
        {
            this.clientDataManager = clientDataManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleQuery(DoesClientExistQuery query)
        {
            var systemMessages = SystemMessageCollection.Empty();
            var clientExists = clientDataManager.IsClientOnOurSystem(query.ClientKey);
            var queryResult = new DoesClientExistQueryResult();
            queryResult.ClientExists = clientExists;
            queryResult.ClientID = query.ClientKey;
            query.Result = new ServiceQueryResult<DoesClientExistQueryResult>(new List<DoesClientExistQueryResult>() { queryResult });
            return systemMessages;
        }
    }
}
