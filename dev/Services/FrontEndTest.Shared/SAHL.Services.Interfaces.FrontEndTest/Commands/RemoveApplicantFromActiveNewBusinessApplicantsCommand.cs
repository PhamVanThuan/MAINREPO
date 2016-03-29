using SAHL.Core.Services;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class RemoveApplicantFromActiveNewBusinessApplicantsCommand : ServiceCommand, IFrontEndTestCommand
    {
        public RemoveApplicantFromActiveNewBusinessApplicantsCommand(int offerRoleKey)
        {
            this.OfferRoleKey = offerRoleKey;
        }
        public int OfferRoleKey { get; protected set; }
    }
}