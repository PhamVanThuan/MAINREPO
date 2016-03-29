using System.Collections.Generic;
using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Logging;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.Specs.Workflow.DebtCounselling.SendLitigationReminderInternalEmailCommandHandlerSpecs
{
    [Subject(typeof(SendLitigationReminderInternalEmailCommandHandler))]
    public class When_email_address_was_not_found_or_invalid : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static SendLitigationReminderInternalEmailCommand command;
        protected static SendLitigationReminderInternalEmailCommandHandler handler;
        protected static IDebtCounsellingRepository debtCounsellingRepository;
        protected static IControlRepository controlRepository;
        protected static IMessageService messageService;
        protected static ILogger logger;
        protected static IDebtCounselling debtCounselling;
        protected static ILegalEntity debtCounsellor;
        protected static IAccount account;

        static string eFolderID;
        static string eStageName;
        static IADUser eworkUser;

        static IList<ILegalEntity> clients;
        static ILegalEntity client;
        static ILegalEntityNaturalPerson recipient;

        // Arrange
        Establish context = () =>
        {
            MockRepository mocks = new MockRepository();
            debtCounsellingRepository = mocks.StrictMock<IDebtCounsellingRepository>();

            controlRepository = An<IControlRepository>();
            messageService = An<IMessageService>();
            logger = An<ILogger>();

            debtCounselling = An<IDebtCounselling>();
            debtCounsellor = An<ILegalEntity>();
            account = An<IAccount>();
            recipient = An<ILegalEntityNaturalPerson>();

            clients = new List<ILegalEntity>();
            client = An<ILegalEntity>();
            clients.Add(client);

            debtCounsellingRepository.WhenToldTo(x => x.GetDebtCounsellingByKey(Param<int>.IsAnything)).Return(debtCounselling);
            debtCounselling.WhenToldTo(x => x.DebtCounsellor).Return(debtCounsellor);
            debtCounselling.WhenToldTo(x => x.Account).Return(account);
            debtCounselling.WhenToldTo(x => x.Clients).Return(clients);
            account.WhenToldTo(x => x.Key).Return(Param<int>.IsAnything);

            eFolderID = "";
            eStageName = "";
            eworkUser = mocks.StrictMock<IADUser>();
            eworkUser.Expect(x => x.ADUserName).Return("");
            debtCounsellingRepository.Expect(x => x.GetEworkDataForLossControlCase(account.Key, out eStageName, out eFolderID, out eworkUser)).OutRef("aaa", "bbb", eworkUser);

            eworkUser.WhenToldTo(x => x.LegalEntity).Return(recipient);
            recipient.WhenToldTo(x => x.EmailAddress).Return("");

            command = new SendLitigationReminderInternalEmailCommand(Param<int>.IsAnything);
            handler = new SendLitigationReminderInternalEmailCommandHandler(debtCounsellingRepository, controlRepository, messageService, logger);

            mocks.ReplayAll();
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_write_an_message_to_the_common_log_file = () =>
        {
            logger.WasToldTo(x => x.LogFormattedWarning(Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<Dictionary<string,object>>(), Param.IsAny<object[]>()));
        };
    }
}