using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.AddressDomain.Queries
{
    public class GetKeyForGuidQuery : IAddressDomainInternalQuery
    {
        public Guid TrackingGuid { get; protected set; }
        public GetKeyForGuidQuery(Guid trackingGuid)
        {
            TrackingGuid = trackingGuid;
        }
    }
}
