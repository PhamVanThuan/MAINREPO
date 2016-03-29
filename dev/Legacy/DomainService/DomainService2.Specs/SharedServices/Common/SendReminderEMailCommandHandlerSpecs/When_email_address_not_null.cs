using System.Data;
using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.Specs.SharedServices.Common.SendReminderEMailCommandHandlerSpecs
{
    [Subject(typeof(SendReminderEMailCommandHandler))]
    public class When_email_address_not_null : DomainServiceSpec<SendReminderEMailCommand, SendReminderEMailCommandHandler>
    {
        protected static IMessageService messageService;
        protected static ICastleTransactionsService service;

        Establish context = () =>
            {
                messageService = An<IMessageService>();
                service = An<ICastleTransactionsService>();

                DataSet dsEmailAddress = new DataSet();
                dsEmailAddress.Tables.Add(new DataTable());
                dsEmailAddress.Tables[0].Columns.Add(new DataColumn());
                DataRow row = dsEmailAddress.Tables[0].NewRow();
                dsEmailAddress.Tables[0].Rows.Add(row);

                service.WhenToldTo(x => x.ExecuteQueryOnCastleTran(Param.IsAny<string>(), Param.IsAny<Databases>(), Param.IsAny<ParameterCollection>())).Return(dsEmailAddress);

                command = new SendReminderEMailCommand(Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<string>());
                handler = new SendReminderEMailCommandHandler(messageService, service);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_send_email = () =>
            {
                messageService.WasToldTo(x => x.SendEmailInternal(Param.IsAny<string>(), Param.IsAny<string>(),
                                                                  Param.IsAny<string>(), Param.IsAny<string>(),
                                                                  Param.IsAny<string>(), Param.IsAny<string>()));
            };
    }
}