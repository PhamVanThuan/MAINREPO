using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetClientAssetsAndLiabilitiesQuery : ServiceQuery<GetClientAssetsAndLiabilitiesQueryResult>, IFrontEndTestQuery, ISqlServiceQuery<GetClientAssetsAndLiabilitiesQueryResult>
    {
        [Required]
        public int ClientKey { get; protected set; }

        public GetClientAssetsAndLiabilitiesQuery(int clientKey)
        {
            this.ClientKey = clientKey;
        }
    }
}