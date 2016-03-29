using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Globals;

namespace DomainService2.Specs.SharedServices.Common.CheckIfUserIsPartOfOrgStructureCommandHandlerSpecs
{
    [Subject(typeof(CheckIfUserIsPartOfOrgStructureCommandHandler))]
    internal class When_CheckIfUserIsPartOfOrgStructureCommandHandled_Returns_False : DomainServiceSpec<CheckIfUserIsPartOfOrgStructureCommand, CheckIfUserIsPartOfOrgStructureCommandHandler>
    {
        public static ICastleTransactionsService castleTransactionService;

        Establish context = () =>
        {
            castleTransactionService = An<ICastleTransactionsService>();

            command = new CheckIfUserIsPartOfOrgStructureCommand(Param<OrganisationStructure>.IsAnything, Param<string>.IsAnything);
            handler = new CheckIfUserIsPartOfOrgStructureCommandHandler(castleTransactionService);
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_return_false = () =>
        {
            command.Result.ShouldBeFalse();
        };
    }
}