using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.AddressDataManager;

namespace SAHL.DomainServiceChecks.CheckHandlers
{
    public class RequiresAddressHandler : IDomainCommandCheckHandler<IRequiresAddress>
    {
        private IAddressDataManager addressDataManager;

        public RequiresAddressHandler(IAddressDataManager addressDataManager)
        {
            this.addressDataManager = addressDataManager;
        }

        public ISystemMessageCollection HandleCheckCommand(IRequiresAddress command)
        {
            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();

            return systemMessages;
        }
    }
}