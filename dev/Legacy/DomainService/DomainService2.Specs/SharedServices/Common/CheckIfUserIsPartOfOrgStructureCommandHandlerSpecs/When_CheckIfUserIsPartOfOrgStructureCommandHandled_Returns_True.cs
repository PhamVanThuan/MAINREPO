using System.Data;
using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Globals;

namespace DomainService2.Specs.SharedServices.Common.CheckIfUserIsPartOfOrgStructureCommandHandlerSpecs
{
    [Subject(typeof(CheckIfUserIsPartOfOrgStructureCommandHandler))]
    internal class When_CheckIfUserIsPartOfOrgStructureCommandHandled_Returns_True : DomainServiceSpec<CheckIfUserIsPartOfOrgStructureCommand, CheckIfUserIsPartOfOrgStructureCommandHandler>
    {
        public static ICastleTransactionsService castleTransactionService;

        Establish context = () =>
        {
            castleTransactionService = An<ICastleTransactionsService>();
            DataSet dsClone = new DataSet();
            dsClone.Tables.Add(new DataTable());
            dsClone.Tables[0].Columns.Add(new DataColumn());
            DataRow row = dsClone.Tables[0].NewRow();
            dsClone.Tables[0].Rows.Add(row);
            castleTransactionService.WhenToldTo(x => x.ExecuteQueryOnCastleTran(Param.IsAny<string>(), Param.IsAny<Databases>(), Param.IsAny<SAHL.Common.DataAccess.ParameterCollection>())).Return(dsClone);
            command = new CheckIfUserIsPartOfOrgStructureCommand(Param<OrganisationStructure>.IsAnything, Param<string>.IsAnything);
            handler = new CheckIfUserIsPartOfOrgStructureCommandHandler(castleTransactionService);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_return_true = () =>
        {
            command.Result.ShouldBeTrue();
        };
    }
}