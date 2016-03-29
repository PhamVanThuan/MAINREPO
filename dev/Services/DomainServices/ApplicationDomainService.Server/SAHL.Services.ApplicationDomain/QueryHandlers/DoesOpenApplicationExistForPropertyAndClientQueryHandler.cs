using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.QueryHandlers
{
    public class DoesOpenApplicationExistForPropertyAndClientQueryHandler : IServiceQueryHandler<DoesOpenApplicationExistForPropertyAndClientQuery>
    {
        private IApplicationDataManager applicationDataManager;
        public DoesOpenApplicationExistForPropertyAndClientQueryHandler(IApplicationDataManager applicationDataManager)
        {
            this.applicationDataManager = applicationDataManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleQuery(DoesOpenApplicationExistForPropertyAndClientQuery query)
        {
            var systemMessages = SystemMessageCollection.Empty();
            var openApplicationExists = applicationDataManager.DoesOpenApplicationExistForPropertyAndClient(query.PropertyKey, query.ClientIDNumber);
            query.Result = new ServiceQueryResult<bool>(new List<bool> { openApplicationExists });

            return systemMessages;
        }
    }
}
