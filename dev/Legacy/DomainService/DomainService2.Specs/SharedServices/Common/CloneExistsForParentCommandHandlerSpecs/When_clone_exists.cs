using System.Collections.Generic;
using System.Data;
using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Globals;

namespace DomainService2.Specs.SharedServices.Common.CloneExistsForParentCommandHandlerSpecs
{
    [Subject(typeof(CloneExistsForParentCommandHandler))]
    public class When_clone_exists : DomainServiceSpec<CloneExistsForParentCommand, CloneExistsForParentCommandHandler>
    {
        protected static ICastleTransactionsService service;
        protected static IUIStatementService uistatementservice;

        Establish context = () =>
            {
                uistatementservice = An<IUIStatementService>();
                service = An<ICastleTransactionsService>();

                DataSet dsClone = new DataSet();
                dsClone.Tables.Add(new DataTable());
                dsClone.Tables[0].Columns.Add(new DataColumn());
                DataRow row = dsClone.Tables[0].NewRow();
                dsClone.Tables[0].Rows.Add(row);

                uistatementservice.WhenToldTo(x => x.GetStatement(Param.IsAny<string>(), Param.IsAny<string>())).Return("");

                service.WhenToldTo(x => x.ExecuteQueryOnCastleTran(Param.IsAny<string>(), Param.IsAny<Databases>(), Param.IsAny<SAHL.Common.DataAccess.ParameterCollection>())).Return(dsClone);

                command = new CloneExistsForParentCommand(1, new List<string> { "test" });
                handler = new CloneExistsForParentCommandHandler(service, uistatementservice);
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