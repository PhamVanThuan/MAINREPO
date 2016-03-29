using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.DomainMessages;

namespace DomainService2.SharedServices.Common
{
    public class CheckPropertyExistsCommandHandler : IHandlesDomainServiceCommand<CheckPropertyExistsCommand>
    {
        IPropertyRepository propertyRepository;

        public CheckPropertyExistsCommandHandler(IPropertyRepository propertyRepository)
        {
            this.propertyRepository = propertyRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, CheckPropertyExistsCommand command)
        {
            IProperty property = propertyRepository.GetPropertyByApplicationKey(command.ApplicationKey);
            if (property == null)
            {
                messages.Add(new Error("No property exists to do a valuation.", "No property exists to do a valuation."));
                command.Result = false;
                return;
            }
            command.Result = true;
        }
    }
}