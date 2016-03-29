using System.Data;
using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;

namespace DomainService2.Specs.SharedServices.Common.IsValuationInProgressCommandHandlerSpecs
{
    [Subject(typeof(IsValuationInProgressCommandHandler))]
    internal class When_Valuation_Is_In_Progress : DomainServiceSpec<IsValuationInProgressCommand, IsValuationInProgressCommandHandler>
    {
        private static ICastleTransactionsService castleTransactionsService;
        Establish context = () =>
        {
            castleTransactionsService = An<ICastleTransactionsService>();

            DataSet dsClone = new DataSet();
            dsClone.Tables.Add(new DataTable());
            dsClone.Tables[0].Columns.Add(new DataColumn());
            dsClone.Tables[0].Columns.Add(new DataColumn());
            dsClone.Tables[0].Columns.Add(new DataColumn());
            DataRow row = dsClone.Tables[0].NewRow();
            dsClone.Tables[0].Rows.Add(row);

            castleTransactionsService.WhenToldTo(x => x.ExecuteQueryOnCastleTran(Param.IsAny<string>(), Param.IsAny<Databases>(), Param.IsAny<ParameterCollection>())).Return(dsClone);

            handler = new IsValuationInProgressCommandHandler(castleTransactionsService);
            command = new IsValuationInProgressCommand(Param<long>.IsAnything, Param<int>.IsAnything);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_Valuation_is_in_progress = () =>
        {
            command.Result.ShouldBeTrue();
            messages.Count.ShouldBeGreaterThan(0);
        };
    }
}