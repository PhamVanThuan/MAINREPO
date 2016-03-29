using SAHL.Core.Data.Models._2AM;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class LinkLegalEntityAddressCommand : ServiceCommand, IFrontEndTestCommand
    {
        public int LegalEntityKey { get; protected set; }

        public int AddressKey { get; protected set; }

        public AddressType AddressType { get; protected set; }

        public LinkLegalEntityAddressCommand(int legalEntityKey, int addressKey, AddressType addressType)
        {
            this.LegalEntityKey = legalEntityKey;
            this.AddressKey = addressKey;
            this.AddressType = addressType;
        }
    }
}