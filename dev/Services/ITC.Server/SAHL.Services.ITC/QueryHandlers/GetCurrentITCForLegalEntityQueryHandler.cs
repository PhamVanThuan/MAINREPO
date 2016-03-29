using System.Collections.Generic;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.Interfaces.ITC.Queries;
using SAHL.Services.ITC.Managers.Itc;

namespace SAHL.Services.ITC.QueryHandlers
{
    public class GetCurrentITCForLegalEntityQueryHandler : IServiceQueryHandler<GetCurrentITCForLegalEntityQuery>
    {
        private IItcManager itcManager;

        public GetCurrentITCForLegalEntityQueryHandler(IItcManager itcManager)
        {
            this.itcManager = itcManager;
        }

        public ISystemMessageCollection HandleQuery(GetCurrentITCForLegalEntityQuery query)
        {
            var systemMessages = SystemMessageCollection.Empty();

            var itcProfile = itcManager.GetCurrentItcProfileForLegalEntity(query.IdentityNumber);
            var itcProfileResult = new List<GetCurrentITCForLegalEntityQueryResult>();
            if (itcProfile != null)
            {
                itcProfileResult.Add(new GetCurrentITCForLegalEntityQueryResult { ItcProfile = itcProfile });
            }
            query.Result = new ServiceQueryResult<GetCurrentITCForLegalEntityQueryResult>(itcProfileResult);

            return systemMessages;
        }
    }
}