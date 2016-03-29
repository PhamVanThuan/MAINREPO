using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.AddressDomain.Events
{
    public class StreetAddressLinkedAsResidentialAddressToClientEvent : Event
    {
        public StreetAddress StreetAddressModel { get; protected set; }

        public int ClientKey { get; protected set; }

        public StreetAddressLinkedAsResidentialAddressToClientEvent(DateTime date, int genericKey, StreetAddress streetAddressModel, int clientKey)
            : base(date, genericKey, (int)GenericKeyType.Address)
        {
            this.ClientKey = clientKey;
            this.StreetAddressModel = streetAddressModel;
        }
    }
}