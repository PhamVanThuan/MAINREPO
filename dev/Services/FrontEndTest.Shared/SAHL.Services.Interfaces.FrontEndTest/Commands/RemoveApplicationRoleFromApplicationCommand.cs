using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class RemoveApplicationRoleFromApplicationCommand : ServiceCommand, IFrontEndTestCommand
    {
        public int OfferRoleKey { get; protected set; }

        public RemoveApplicationRoleFromApplicationCommand(int offerRoleKey)
        {
            this.OfferRoleKey = offerRoleKey;
        }
    }
}