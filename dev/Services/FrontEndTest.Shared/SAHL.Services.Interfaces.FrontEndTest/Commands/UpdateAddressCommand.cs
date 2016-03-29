using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class UpdateAddressCommand : ServiceCommand, IFrontEndTestCommand
    {
        public AddressDataModel Address { get; protected set; }

        public UpdateAddressCommand(AddressDataModel address)
        {
            this.Address = address;
        }
    }
}