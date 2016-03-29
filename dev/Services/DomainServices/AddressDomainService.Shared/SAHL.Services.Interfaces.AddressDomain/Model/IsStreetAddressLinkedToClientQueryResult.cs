using System;
using System.Linq;

namespace SAHL.Services.Interfaces.AddressDomain.Model
{
    public class IsStreetAddressLinkedToClientQueryResult
    {
        public bool AddressIsLinkedToClientAddress { get; set; }
        public int? ClientAddressKey { get; set; }
    }
}