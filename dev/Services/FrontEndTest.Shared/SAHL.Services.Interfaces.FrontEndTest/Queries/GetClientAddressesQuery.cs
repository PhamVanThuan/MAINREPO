using System.ComponentModel.DataAnnotations;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetClientAddressesQuery : ServiceQuery<GetClientAddressesQueryResult>, IFrontEndTestQuery, ISqlServiceQuery<GetClientAddressesQueryResult>
    {
        public GetClientAddressesQuery(int ClientKey)
        {
            this.ClientKey = ClientKey;
        }

        [Required]
        public int ClientKey { get; protected set; }
    }
}