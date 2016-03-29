using System;
using System.Data;
using DomainService2.Workflow.HelpDesk;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.HelpDesk.CreateRequestCommandHandlerSpecs
{
    [Subject(typeof(CreateRequestCommandHandler))]
    public class When_legalentity_null : DomainServiceSpec<CreateRequestCommand, CreateRequestCommandHandler>
    {
        protected static ICastleTransactionsService castleTransactionsService;
        protected static Exception exception;

        Establish context = () =>
        {
            castleTransactionsService = An<ICastleTransactionsService>();

            DataSet dsLegalEntity = new DataSet();
            dsLegalEntity.Tables.Add(new DataTable());
            dsLegalEntity.Tables[0].Columns.Add(new DataColumn("LegalEntityLegalName"));

            castleTransactionsService.WhenToldTo(x => x.ExecuteQueryOnCastleTran(Param.IsAny<string>(), Param.IsAny<Databases>(), Param.IsAny<ParameterCollection>())).Return(dsLegalEntity);

            command = new CreateRequestCommand(Param.IsAny<int>());
            handler = new CreateRequestCommandHandler(castleTransactionsService);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => handler.Handle(messages, command));
        };

        It should_return_exception = () =>
        {
            exception.ShouldNotBeNull();
        };
    }
}