using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class LegalEntityDomicilium
    {
        public int LegalEntityDomiciliumKey { get; set; }

        public int LegalEntityAddressKey { get; set; }

        public GeneralStatusEnum GeneralStatusKey { get; set; }

        public DateTime? ChangeDate { get; set; }

        public int ADUserKey { get; set; }

        public int OfferRoleDomiciliumKey { get; set; }

        public int OfferRoleKey { get; set; }

        public int LegalEntityKey { get; set; }

        public string FormattedAddress { get; set; }

        public int AddressKey { get; set; }

        public string DelimitedAddress { get; set; }
    }
}