using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.DebtCounselling.UpdateDebtCounsellingStatusCommandHandlerSpecs
{
    [Subject(typeof(UpdateDebtCounsellingStatusCommandHandler))]
    public class When_debt_counselling_case_does_not_exist : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static UpdateDebtCounsellingStatusCommand command;
        protected static UpdateDebtCounsellingStatusCommandHandler handler;
        protected static IDebtCounsellingRepository debtCounsellingRepository;
        protected static ILookupRepository lookupRepository;
        protected static IDebtCounselling debtCounselling;

        // Arrange
        Establish context = () =>
        {
            debtCounsellingRepository = An<IDebtCounsellingRepository>();
            lookupRepository = An<ILookupRepository>();
            debtCounselling = null;

            debtCounsellingRepository.WhenToldTo(x => x.GetDebtCounsellingByKey(Param<int>.IsAnything)).Return(debtCounselling);

            command = new UpdateDebtCounsellingStatusCommand(Param<int>.IsAnything, (int)SAHL.Common.Globals.DebtCounsellingStatuses.Closed);
            handler = new UpdateDebtCounsellingStatusCommandHandler(debtCounsellingRepository, lookupRepository);
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_return_false = () =>
        {
            command.Result.ShouldBeFalse();
        };

        // Assert
        It should_not_update_and_save_debtcounselling_record = () =>
        {
            debtCounsellingRepository.WasNotToldTo(x => x.SaveDebtCounselling(debtCounselling));
        };
    }
}