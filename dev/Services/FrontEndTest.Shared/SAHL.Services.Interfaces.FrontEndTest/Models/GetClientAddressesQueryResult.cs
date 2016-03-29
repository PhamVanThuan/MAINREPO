using System;

namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class GetClientAddressesQueryResult
    {
        public int LegalEntityAddressKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int AddressKey { get; set; }

        public int AddressTypeKey { get; set; }

        public DateTime EffectiveDate { get; set; }

        public int GeneralStatusKey { get; set; }

        public int AddressFormatKey { get; set; }
    }
}