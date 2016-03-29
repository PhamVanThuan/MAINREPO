using System.Linq;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.Origination.Credit
{
    public class IsReviewRequiredCommandHandler : IHandlesDomainServiceCommand<IsReviewRequiredCommand>
    {
        private IX2Repository X2Repository;
        private IOrganisationStructureRepository organisationStructureRepository;

        public IsReviewRequiredCommandHandler(IX2Repository X2Repository, IOrganisationStructureRepository organisationStructureRepository)
        {
            this.X2Repository = X2Repository;
            this.organisationStructureRepository = organisationStructureRepository;
        }

        public void Handle(IDomainMessageCollection messages, IsReviewRequiredCommand command)
        {
            string userName = this.X2Repository.GetUserWhoPerformedActivity(command.InstanceID, command.ActivityName);

            IApplicationRoleType applicationRoleType = this.organisationStructureRepository.GetApplicationRoleTypeByKey((int)OfferRoleTypes.CreditUnderwriterD);

            //Is this user an Underwriter
            var underwriters = applicationRoleType.OfferRoleTypeOrganisationStructures
                .Where
                    (x => x.ADUsers
                        .Any(y => y.ADUserName == userName))
                .FirstOrDefault();

            if (underwriters != null)
                command.Result = true;
            else
                command.Result = false;
        }
    }
}