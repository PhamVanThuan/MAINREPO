using System.Collections.Generic;
using System.Linq;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.Origination.Credit
{
    public class CreditResubCommandHandler : IHandlesDomainServiceCommand<CreditResubCommand>
    {
        private IApplicationRepository applicationRepo;

        public CreditResubCommandHandler(IApplicationRepository applicationRepo)
        {
            this.applicationRepo = applicationRepo;
        }

        public void Handle(IDomainMessageCollection messages, CreditResubCommand command)
        {
            List<int> roles = new List<int>
            {
                (int)OfferRoleTypes.CreditExceptionsD,
                (int)OfferRoleTypes.CreditManagerD,
                (int)OfferRoleTypes.CreditSupervisorD,
                (int)OfferRoleTypes.CreditUnderwriterD
            };

            IEventList<IApplicationRole> applicationRoles = applicationRepo.GetApplicationRolesForKey(command.ApplicationKey);

            IApplicationRole arMostSen =
                applicationRoles
                .Where
                        (x => roles.Contains(x.ApplicationRoleType.Key))
                .OrderBy
                        (x => x.ApplicationRoleType.Key).FirstOrDefault();

            if (arMostSen != null)
                applicationRepo.CreateAndSaveApplicationRole_WithoutRules(command.ApplicationKey, arMostSen.ApplicationRoleType.Key, arMostSen.LegalEntityKey);
        }
    }
}