using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainProcessManagerProxy.Models;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.DomainProcessManagerProxy.Commands
{
    public class StartPayAttorneyProcessCommand : ServiceCommand, IDomainProcessManagerProxyCommand
    {
        public IEnumerable<ThirdPartyPaymentModel> ThirdPartyPayments { get; set; }

        public StartPayAttorneyProcessCommand(IEnumerable<ThirdPartyPaymentModel> thirdPartyPayments)
        {
            this.ThirdPartyPayments = thirdPartyPayments;
        }
    }
}
