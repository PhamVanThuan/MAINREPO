using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.AddressDomain.Commands
{
    public class RemoveAddressKeyGuidMapCommand : IAddressDomainInternalCommand
    {
        public Guid MappingGuid {get; protected set;}
        public RemoveAddressKeyGuidMapCommand(Guid mappingGuid)
        {
            MappingGuid = mappingGuid;
        }
    }
}
