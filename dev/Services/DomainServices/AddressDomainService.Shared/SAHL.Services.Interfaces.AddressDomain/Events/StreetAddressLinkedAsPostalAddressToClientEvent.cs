using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.AddressDomain.Events
{
    public class StreetAddressLinkedAsPostalAddressToClientEvent : Event
    {
        public StreetAddress StreetAddressModel { get; protected set; }
        public int ClientKey { get; protected set; }
        public StreetAddressLinkedAsPostalAddressToClientEvent(DateTime date, int genericKey, StreetAddress streetAddressModel, int clientKey)
            : base(date, genericKey, (int)GenericKeyType.Address)
        {
            this.StreetAddressModel = streetAddressModel;
            this.ClientKey = clientKey;
        }
    }
}
