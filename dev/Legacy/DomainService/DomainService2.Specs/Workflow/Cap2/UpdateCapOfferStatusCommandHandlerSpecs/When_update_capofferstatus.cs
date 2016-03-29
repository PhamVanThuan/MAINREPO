using DomainService2.Workflow.Cap2;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.DataAccess;

namespace DomainService2.Specs.Workflow.Cap2.UpdateCapOfferStatusCommandHandlerSpecs
{
    [Subject(typeof(UpdateCapOfferStatusCommandHandler))]
    public class When_update_capofferstatus : DomainServiceSpec<UpdateCapOfferStatusCommand, UpdateCapOfferStatusCommandHandler>
    {
        static ICastleTransactionsService service;

        Establish context = () =>
            {
                service = An<ICastleTransactionsService>();
                IUIStatementService uistatementservice = An<IUIStatementService>();

                uistatementservice.WhenToldTo(x => x.GetStatement(Param.IsAny<string>(), Param.IsAny<string>()))
                    .Return("");

                command = new UpdateCapOfferStatusCommand(1111, 1);
                handler = new UpdateCapOfferStatusCommandHandler(service, uistatementservice);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_execute = () =>
            {
                service.WasToldTo(x => x.ExecuteNonQueryOnCastleTran(Param.IsAny<string>(), Param.IsAny<SAHL.Common.Globals.Databases>(), Param.IsAny<ParameterCollection>())).OnlyOnce();
            };
    }
}