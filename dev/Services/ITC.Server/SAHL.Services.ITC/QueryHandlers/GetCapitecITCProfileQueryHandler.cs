using System.Collections.Generic;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.Interfaces.ITC.Queries;
using SAHL.Services.ITC.Managers.Itc;

namespace SAHL.Services.ITC.QueryHandlers
{
    public class GetCapitecITCProfileQueryHandler : IServiceQueryHandler<GetCapitecITCProfileQuery>
    {
        private IItcManager itcManager;

        public GetCapitecITCProfileQueryHandler(IItcManager itcManager)
        {
            this.itcManager = itcManager;
        }

        public ISystemMessageCollection HandleQuery(GetCapitecITCProfileQuery query)
        {
            var systemMessages = SystemMessageCollection.Empty();
            var itcProfile = itcManager.GetITCProfile(query.ItcID);
            var results = new List<GetITCProfileQueryResult>();
            if (itcProfile != null)
            {
                results.Add(new GetITCProfileQueryResult { ITCProfile = itcProfile });
            }
            query.Result = new ServiceQueryResult<GetITCProfileQueryResult>(results);
            return systemMessages;
        }
    }
}