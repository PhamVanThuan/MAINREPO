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
    public class When_email_address_null : DomainServiceSpec<SendReminderEMailCommand, SendReminderEMailCommandHandler>
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

            service.WhenToldTo(x => x.ExecuteQueryOnCastleTran(Param.IsAny<string>(), Param.IsAny<Databases>(), Param.IsAny<ParameterCollection>())).Return(dsEmailAddress);

            command = new SendReminderEMailCommand(Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<string>());
            handler = new SendReminderEMailCommandHandler(messageService, service);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_not_send_email = () =>
        {
            messageService.WasNotToldTo(x => x.SendEmailInternal(Param.IsAny<string>(), Param.IsAny<string>(),
                                                              Param.IsAny<string>(), Param.IsAny<string>(),
                                                              Param.IsAny<string>(), Param.IsAny<string>()));
        };
    }
}