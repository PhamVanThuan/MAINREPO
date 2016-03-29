using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.AddressDomain.Commands
{
    public class RemoveKeyForGuidCommand : IAddressDomainInternalCommand
    {
        public Guid TrackingGuid {get; protected set;}
        public RemoveKeyForGuidCommand(Guid trackingGuid)
        {
            TrackingGuid = trackingGuid;
        }
    }
}
