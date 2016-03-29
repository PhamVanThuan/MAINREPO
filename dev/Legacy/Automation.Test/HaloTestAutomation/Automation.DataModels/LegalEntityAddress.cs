using Common.Enums;

namespace Automation.DataModels
{
    public class LegalEntityAddress
    {
        public LegalEntityAddress()
        {
            this.Address = new Address();
            this.LegalEntity = new LegalEntity();
        }

        public GeneralStatusEnum GeneralStatusKey { get; set; }

        public AddressTypeEnum AddressTypeKey { get; set; }

        public string AddressTypeDescription { get; set; }

        public LegalEntity LegalEntity { get; set; }

        public Address Address { get; set; }

        public int AddressKey { get; set; }

        public int LegalEntityKey { get; set; }

        public string DelimitedAddress { get; set; }

        public string FormattedAddress { get; set; }

        public AddressFormatEnum AddressFormatKey { get; set; }

        public int LegalEntityAddressKey { get; set; }
    }
}