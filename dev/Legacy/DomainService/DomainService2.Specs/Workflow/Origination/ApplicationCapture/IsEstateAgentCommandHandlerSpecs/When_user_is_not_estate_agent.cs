using DomainService2.Workflow.Origination.ApplicationCapture;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using X2DomainService.Interface.Common;

namespace DomainService2.Specs.Workflow.Origination.ApplicationCapture.IsEstateAgentCommandHandlerSpecs
{
    [Subject(typeof(IsEstateAgentCommandHandler))]
    public class When_user_is_not_estate_agent : WithFakes
    {
        protected static IsEstateAgentCommandHandler handler;
        protected static IsEstateAgentCommand command;
        protected static IDomainMessageCollection messages;
        //Arrange
        Establish context = () =>
        {
            var isEstateAgent = false;
            var common = An<ICommon>();
            messages = An<IDomainMessageCollection>();

            command = new IsEstateAgentCommand(Param.IsAny<string>());
            handler = new IsEstateAgentCommandHandler(common);

            common.WhenToldTo(x => x.CheckIfUserIsPartOfOrgStructure(Param<IDomainMessageCollection>.IsAnything, Param<OrganisationStructure>.IsAnything, Param<string>.IsAnything)).Return(isEstateAgent);
        };

        //Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        //Assert
        It should_return_false = () =>
        {
            command.Result.ShouldBeFalse();
        };
    }
}