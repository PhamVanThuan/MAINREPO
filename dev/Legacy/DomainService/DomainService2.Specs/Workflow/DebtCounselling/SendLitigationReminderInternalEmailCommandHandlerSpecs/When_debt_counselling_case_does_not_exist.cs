using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Logging;
using SAHL.Common.Service.Interfaces;
using System.Collections.Generic;

namespace DomainService2.Specs.Workflow.DebtCounselling.SendLitigationReminderInternalEmailCommandHandlerSpecs
{
    [Subject(typeof(SendLitigationReminderInternalEmailCommandHandler))]
    public class When_debt_counselling_case_does_not_exist : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static SendLitigationReminderInternalEmailCommand command;
        protected static SendLitigationReminderInternalEmailCommandHandler handler;
        protected static IDebtCounsellingRepository debtCounsellingRepository;
        protected static IControlRepository controlRepository;
        protected static IMessageService messageService;
        protected static ILogger logger;
        protected static IDebtCounselling debtCounselling;

        // Arrange
        Establish context = () =>
        {
            debtCounsellingRepository = An<IDebtCounsellingRepository>();
            controlRepository = An<IControlRepository>();
            messageService = An<IMessageService>();
            logger = An<ILogger>();

            debtCounselling = null;

            debtCounsellingRepository.WhenToldTo(x => x.GetDebtCounsellingByKey(Param<int>.IsAnything)).Return(debtCounselling);

            command = new SendLitigationReminderInternalEmailCommand(Param<int>.IsAnything);
            handler = new SendLitigationReminderInternalEmailCommandHandler(debtCounsellingRepository, controlRepository, messageService, logger);
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