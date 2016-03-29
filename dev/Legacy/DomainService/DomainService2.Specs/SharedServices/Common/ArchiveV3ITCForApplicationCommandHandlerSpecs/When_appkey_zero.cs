using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Service;

namespace DomainService2.Specs.SharedServices.Common.ArchiveV3ITCForApplicationCommandHandlerSpecs
{
    [Subject(typeof(ArchiveV3ITCForApplicationCommandHandler))]
    public class When_appkey_zero : DomainServiceSpec<ArchiveV3ITCForApplicationCommand, ArchiveV3ITCForApplicationCommandHandler>
    {
        protected static ICastleTransactionsService service;
        protected static IUIStatementService uistatementservice;

        Establish context = () =>
        {
            service = An<ICastleTransactionsService>();
            uistatementservice = An<IUIStatementService>();

            uistatementservice.WhenToldTo(x => x.GetStatement(Param.IsAny<string>(), Param.IsAny<string>())).Return("");
            command = new ArchiveV3ITCForApplicationCommand(0);
            handler = new ArchiveV3ITCForApplicationCommandHandler(service, uistatementservice);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_not_execute_statement = () =>
        {
            service.WasNotToldTo(x => x.ExecuteNonQueryOnCastleTran("", SAHL.Common.Globals.Databases.TwoAM, null));
        };
    }
}