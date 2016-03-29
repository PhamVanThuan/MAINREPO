using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.Specs.Workflow.DebtCounselling.SendNotificationsCancellatonRegistered_EXTCommandHandlerSpecs
{
    [Subject(typeof(SendNotificationsCancellatonRegistered_EXTCommandHandler))]
    public class When_recoveriesproposalreceivedstagetransition_not_exists : WithFakes
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

            ILegalEntity debtCounsellor = An<ILegalEntity>();
            ICorrespondenceTemplate correspondenceTemplate = An<ICorrespondenceTemplate>();
            correspondenceTemplate.WhenToldTo(x => x.Subject).Return("test");
            correspondenceTemplate.WhenToldTo(x => x.Template).Return("test");

            debtCounsellingRepository.WhenToldTo(x => x.GetDebtCounsellingByKey(Param.IsAny<int>())).Return(debtCounsellingCase);
            debtCounsellingCase.WhenToldTo(x => x.DebtCounsellor).Return(debtCounsellor);
            debtCounsellor.WhenToldTo(x => x.EmailAddress).Return(Param.IsAny<string>());

            //command.RecoveriesProposalReceivedStageTransitionExists = true;

            commonRepository.WhenToldTo(x => x.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.MortgageLoanCancelledDontContinuePaying)).Return(correspondenceTemplate);

            messageService.WhenToldTo(x => x.SendEmailExternal(Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>(),
                                                               Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(),
                                                               Param.IsAny<string>(), Param.IsAny<string>())).Return(true);

            messages = new DomainMessageCollection();
            command = new SendNotificationsCancellatonRegistered_EXTCommand(1, false);
            handler = new SendNotificationsCancellatonRegistered_EXTCommandHandler(debtCounsellingRepository, commonRepository, messageService);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_send_notification = () =>
        {
            debtCounsellingRepository.WasToldTo(x => x.SendNotification(Param.IsAny<IDebtCounselling>()));
        };
    }
}