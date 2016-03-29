using DomainService2.Workflow.Origination.ApplicationCapture;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using X2DomainService.Interface.Common;

namespace DomainService2.Specs.Workflow.Origination.ApplicationCapture.IsEstateAgentInRoleCommandHandlerSpecs
{
    [Subject(typeof(IsEstateAgentInRoleCommandHandler))]
    public class When_user_is_not_estate_agent : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static IsEstateAgentInRoleCommand command;
        protected static IsEstateAgentInRoleCommandHandler handler;

        Establish context = () =>
        {
            var isEstateAgent = false;
            ICommon common = An<ICommon>();
            IOrganisationStructureRepository organisationStructureRepository = An<IOrganisationStructureRepository>();

            messages = An<IDomainMessageCollection>();
            command = new IsEstateAgentInRoleCommand(Param.IsAny<string>(), Param.IsAny<string>());
            handler = new IsEstateAgentInRoleCommandHandler(common, organisationStructureRepository);

            common.WhenToldTo(x => x.CheckIfUserIsPartOfOrgStructure(Param<IDomainMessageCollection>.IsAnything, Param.Is<OrganisationStructure>(OrganisationStructure.EstateAgent), Param.IsAny<string>()))
                .Return(isEstateAgent);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_false = () =>
        {
            command.Result.ShouldBeFalse();
        };
    }
}