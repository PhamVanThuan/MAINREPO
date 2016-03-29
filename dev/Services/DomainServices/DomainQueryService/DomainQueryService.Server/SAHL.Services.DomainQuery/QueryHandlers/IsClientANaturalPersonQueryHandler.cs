using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DomainQuery.Managers.Client;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DomainQuery.QueryHandlers
{
    public class IsClientANaturalPersonQueryHandler : IServiceQueryHandler<IsClientANaturalPersonQuery>
    {
        private Managers.Client.IClientDataManager clientDataManager;

        public IsClientANaturalPersonQueryHandler(IClientDataManager clientDataManager)
        {
            // TODO: Complete member initialization
            this.clientDataManager = clientDataManager;
        }
        public int ClientKey { get; protected set; }

        public Core.SystemMessages.ISystemMessageCollection HandleQuery(Interfaces.DomainQuery.Queries.IsClientANaturalPersonQuery query)
        {
            var systemMessages = SystemMessageCollection.Empty();
            bool isClientANaturalPerson = clientDataManager.IsClientANaturalPerson(query.ClientKey);
            query.Result = new ServiceQueryResult<IsClientANaturalPersonQueryResult>(new List<IsClientANaturalPersonQueryResult>() { new IsClientANaturalPersonQueryResult { ClientIsANaturalPerson = isClientANaturalPerson } });
            return systemMessages;
        }
    }
}
