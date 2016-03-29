using DomainService2.Workflow.Cap2;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.DataAccess;

namespace DomainService2.Specs.Workflow.Cap2.OnCompleteActivity_Promotion_ClientCommandHandlerSpecs
{
    [Subject(typeof(OnCompleteActivity_Promotion_ClientCommandHandler))]
    public class When_accept_capoffer : DomainServiceSpec<OnCompleteActivity_Promotion_ClientCommand, OnCompleteActivity_Promotion_ClientCommandHandler>
    {
        static ICastleTransactionsService service;

        Establish context = () =>
            {
                service = An<ICastleTransactionsService>();
                IUIStatementService uistatementservice = An<IUIStatementService>();

                uistatementservice.WhenToldTo(x => x.GetStatement(Param.IsAny<string>(), Param.IsAny<string>()))
                    .Return("");

                command = new OnCompleteActivity_Promotion_ClientCommand(1, 1, 1, 1, 1, "");
                handler = new OnCompleteActivity_Promotion_ClientCommandHandler(service, uistatementservice);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_execute = () =>
            {
                service.WasToldTo(x => x.ExecuteQueryOnCastleTran(Param.IsAny<string>(), Param.IsAny<SAHL.Common.Globals.Databases>(), Param.IsAny<ParameterCollection>()));
            };
    }
}