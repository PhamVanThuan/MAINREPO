using System.Collections.Generic;
using DomainService2.Specs.DomainObjects;
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
    public class When_send_litigation_reminder_email_naturalperson : WithFakes
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
        static ILegalEntityNaturalPerson client;
        static ILegalEntityNaturalPerson recipient;

        static IDebtCounsellorOrganisationNode dcNode;
        static IReadOnlyEventList<ILegalEntity> debtCounsellors;
        static ILegalEntity topDebtCounsellor;
        static IDebtCounsellorDetail debtCounsellorDetail;

        static ILegalEntityType legalEntityType;
        static ILegalEntityNaturalPerson legalEntityNaturalPerson;
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
            topDebtCounsellor = An<ILegalEntity>();
            account = An<IAccount>();
            recipient = An<ILegalEntityNaturalPerson>();

            clients = new List<ILegalEntity>();
            client = An<ILegalEntityNaturalPerson>();
            clients.Add(client);

            dcNode = An<IDebtCounsellorOrganisationNode>();
            debtCounsellorDetail = An<IDebtCounsellorDetail>();
            legalEntityType = An<ILegalEntityType>();
            legalEntityNaturalPerson = An<ILegalEntityNaturalPerson>();

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
            recipient.WhenToldTo(x => x.EmailAddress).Return("test@email.com");
            debtCounsellor.WhenToldTo(x => x.GetLegalName(LegalNameFormat.Full)).Return("Mr Debt Counsellor");

            controlRepository.WhenToldTo(x => x.GetControlByDescription(SAHL.Common.Constants.ControlTable.HALOEmailAddress).ControlText).Return("halo@sahomeloans.com");

            debtCounsellingRepository.WhenToldTo(x => x.GetTopDebtCounsellorCompanyNodeForDebtCounselling(Param<int>.IsAnything)).Return(dcNode);

            debtCounsellors = new StubReadOnlyEventList<ILegalEntity>(new ILegalEntity[] { topDebtCounsellor });
            dcNode.WhenToldTo(x => x.LegalEntities).Return(debtCounsellors);
            topDebtCounsellor.WhenToldTo(x => x.DebtCounsellorDetail).Return(debtCounsellorDetail);
            debtCounsellorDetail.WhenToldTo(x => x.NCRDCRegistrationNumber).Return("123");

            debtCounsellor.WhenToldTo(x => x.EmailAddress).Return("dc@test.com");
            debtCounsellor.WhenToldTo(x => x.HomePhoneCode).Return("031");
            debtCounsellor.WhenToldTo(x => x.HomePhoneNumber).Return("5551234");
            debtCounsellor.WhenToldTo(x => x.WorkPhoneCode).Return("031");
            debtCounsellor.WhenToldTo(x => x.WorkPhoneNumber).Return("5551234");
            debtCounsellor.WhenToldTo(x => x.CellPhoneNumber).Return("123");

            client.WhenToldTo(x => x.LegalEntityType).Return(legalEntityType);
            client.WhenToldTo(x => x.IDNumber).Return("1235");
            client.WhenToldTo(x => x.GetLegalName(LegalNameFormat.Full)).Return("Mr Natural Person Client");
            legalEntityType.WhenToldTo(x => x.Key).Return((int)SAHL.Common.Globals.LegalEntityTypes.NaturalPerson);

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
        It should_call_the_messageservce_method_to_send_email = () =>
        {
            messageService.WasToldTo(x => x.SendEmailInternal(Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<bool>()));
        };
    }
}