using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.Specs.Workflow.DebtCounselling.SendNotificationsCancellatonRegistered_EXTCommandHandlerSpecs
{
    [Subject(typeof(SendNotificationsCancellatonRegistered_EXTCommandHandler))]
    public class When_debtcounsellor_not_exists : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static SendNotificationsCancellatonRegistered_EXTCommand command;
        protected static SendNotificationsCancellatonRegistered_EXTCommandHandler handler;
        protected static IDebtCounsellingRepository debtCounsellingRepository;
        protected static IDebtCounselling debtCounsellingCase;
        protected static ICommonRepository commonRepository;
        protected static IMessageService messageService;

        Establish context = () =>
            {
                debtCounsellingRepository = An<IDebtCounsellingRepository>();
                commonRepository = An<ICommonRepository>();
                messageService = An<IMessageService>();
                debtCounsellingCase = An<IDebtCounselling>();
                IAccount account = An<IAccount>();

                debtCounsellingCase.WhenToldTo(x => x.Account).Return(account);
                account.WhenToldTo(x => x.Key).Return(Param.IsAny<int>());

                ILegalEntity debtCounsellor = null;

                debtCounsellingRepository.WhenToldTo(x => x.GetDebtCounsellingByKey(Param.IsAny<int>())).Return(debtCounsellingCase);
                debtCounsellingCase.WhenToldTo(x => x.DebtCounsellor).Return(debtCounsellor);

                messages = new DomainMessageCollection();
                command = new SendNotificationsCancellatonRegistered_EXTCommand(1, true);
                handler = new SendNotificationsCancellatonRegistered_EXTCommandHandler(debtCounsellingRepository, commonRepository, messageService);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_throw_error = () =>
            {
                messages.Count.Equals(1);
            };
    }
}