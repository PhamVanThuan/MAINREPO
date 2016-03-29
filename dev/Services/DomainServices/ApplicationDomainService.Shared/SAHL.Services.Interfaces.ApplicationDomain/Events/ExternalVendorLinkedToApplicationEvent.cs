using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;

namespace SAHL.Services.Interfaces.ApplicationDomain.Events
{
    public class ExternalVendorLinkedToApplicationEvent : Event
    {
        public int VendorKey { get; protected set; }

        public string VendorCode { get; protected set; }

        public int OrganisationStructureKey { get; protected set; }

        public int ClientKey { get; protected set; }

        public int GeneralStatusKey { get; protected set; }

        public int ApplicationNumber { get; private set; }

        public ExternalVendorLinkedToApplicationEvent(int applicationNumber, DateTime date, int vendorKey, string vendorCode, int organisationStructureKey, 
            int clientKey, int generalStatusKey ) 
            : base(date) 
        {
            this.VendorKey = vendorKey;
            this.VendorCode = vendorCode;
            this.OrganisationStructureKey = organisationStructureKey;
            this.ClientKey = clientKey;
            this.GeneralStatusKey = generalStatusKey;
            this.ApplicationNumber = applicationNumber;
        }
    }
}
