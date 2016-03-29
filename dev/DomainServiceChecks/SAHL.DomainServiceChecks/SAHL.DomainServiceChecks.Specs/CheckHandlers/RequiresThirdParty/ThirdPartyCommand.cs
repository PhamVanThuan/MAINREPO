using SAHL.DomainServiceChecks.Checks;
using System;
using System.Linq;

namespace SAHL.DomainServiceCheck.Specs.CheckHandlers.RequiresThirdParty
{
    public class ThirdPartyCommand : IRequiresThirdParty
    {
        public Guid ThirdPartyId { get; protected set; }

        public Guid Id { get; protected set; }

        public ThirdPartyCommand(Guid Id, Guid ThirdPartyId)
        {
            this.Id = Id;
            this.ThirdPartyId = ThirdPartyId;
        }
    }
}