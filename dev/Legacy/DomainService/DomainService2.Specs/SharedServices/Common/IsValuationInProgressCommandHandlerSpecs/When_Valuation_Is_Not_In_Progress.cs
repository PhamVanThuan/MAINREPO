using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;

namespace DomainService2.Specs.SharedServices.Common.IsValuationInProgressCommandHandlerSpecs
{
    [Subject(typeof(IsValuationInProgressCommandHandler))]
    internal class When_Valuation_Is_Not_In_Progress : DomainServiceSpec<IsValuationInProgressCommand, IsValuationInProgressCommandHandler>
    {
        private static ICastleTransactionsService castleTransactionsService;
        Establish context = () =>
        {
            castleTransactionsService = An<ICastleTransactionsService>();
            castleTransactionsService.WhenToldTo(x => x.ExecuteQueryOnCastleTran(Param.IsAny<string>(), Param.IsAny<Databases>(), Param.IsAny<ParameterCollection>())).Return(new System.Data.DataSet());
            handler = new IsValuationInProgressCommandHandler(castleTransactionsService);
            command = new IsValuationInProgressCommand(Param<long>.IsAnything, Param<int>.IsAnything);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_Valuation_is_not_in_progress = () =>
        {
            command.Result.ShouldBeFalse();
            messages.Count.ShouldBeLessThan(1);
        };
    }
}