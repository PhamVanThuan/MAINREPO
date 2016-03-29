using DomainService2.Workflow.Origination.ApplicationCapture;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;

namespace DomainService2.Specs.Workflow.Origination.ApplicationCapture.IsEstateAgentInRoleCommandHandlerSpecs
{
    [Subject(typeof(IsEstateAgentInRoleCommandHandler))]
    public class When_user_is_estate_agent_and_in_role : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static IsEstateAgentInRoleCommand command;
        protected static IsEstateAgentInRoleCommandHandler handler;

        Establish context = () =>
        {
            //this is an Estate Agent
            var isEstateAgent = true;
            //The user is an Admin user
            var roles = new string[] { SAHL.Common.OrganisationStructure.Admin, SAHL.Common.OrganisationStructure.Manager };

            ICommon common = An<ICommon>();
            IOrganisationStructureRepository organisationStructureRepository = An<IOrganisationStructureRepository>();

            messages = An<IDomainMessageCollection>();

            var user = An<IADUser>();
            var userOrganisationStructure = An<IUserOrganisationStructure>();
            var organisationStructure = An<IOrganisationStructure>();
            var userOrganisationStructures = An<EventList<IUserOrganisationStructure>>();

            userOrganisationStructure.WhenToldTo(x => x.OrganisationStructure).Return(organisationStructure);
            organisationStructure.WhenToldTo(x => x.Description).Return(SAHL.Common.OrganisationStructure.Admin);
            userOrganisationStructures.Add(messages, userOrganisationStructure);
            user.WhenToldTo(x => x.UserOrganisationStructure).Return(userOrganisationStructures);

            command = new IsEstateAgentInRoleCommand(Param.IsAny<string>(), roles);
            handler = new IsEstateAgentInRoleCommandHandler(common, organisationStructureRepository);

            common.WhenToldTo(x => x.CheckIfUserIsPartOfOrgStructure(Param<IDomainMessageCollection>.IsAnything, Param.Is<SAHL.Common.Globals.OrganisationStructure>(SAHL.Common.Globals.OrganisationStructure.EstateAgent), Param.IsAny<string>()))
                .Return(isEstateAgent);

            organisationStructureRepository.WhenToldTo(x => x.GetAdUserForAdUserName(Param.IsAny<string>())).Return(user);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_true = () =>
        {
            command.Result.ShouldBeTrue();
        };
    }
}