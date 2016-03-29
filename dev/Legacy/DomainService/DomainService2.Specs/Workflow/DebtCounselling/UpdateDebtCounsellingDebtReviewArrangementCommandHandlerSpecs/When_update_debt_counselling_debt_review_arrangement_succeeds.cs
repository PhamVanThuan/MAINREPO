using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.DebtCounselling.UpdateDebtCounsellingDebtReviewArrangementCommandHandlerSpecs
{
    [Subject(typeof(UpdateDebtCounsellingDebtReviewArrangementCommandHandler))]
    public class When_update_debt_counselling_debt_review_arrangement_succeeds : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static UpdateDebtCounsellingDebtReviewArrangementCommand command;
        protected static UpdateDebtCounsellingDebtReviewArrangementCommandHandler handler;
        protected static IDebtCounsellingRepository debtCounsellingRepository;
        protected static string msg = string.Empty;
        protected static ICommonRepository commonRepository;


        // Arrange
        Establish context = () =>
        {
            commonRepository = An<ICommonRepository>();

            debtCounsellingRepository = An<IDebtCounsellingRepository>();

            debtCounsellingRepository.WhenToldTo(x => x.UpdateDebtCounsellingDebtReviewArrangement(Param<int>.IsAnything, Param<string>.IsAnything)).Return(true);

            messages = new DomainMessageCollection();
            command = new UpdateDebtCounsellingDebtReviewArrangementCommand(Param<int>.IsAnything, Param<string>.IsAnything);
            handler = new UpdateDebtCounsellingDebtReviewArrangementCommandHandler(debtCounsellingRepository, commonRepository);
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_return_true = () =>
        {
            command.Result.ShouldBeTrue();
        };

        // Assert
        It should_return_no_error_message = () =>
        {
            messages.Count.Equals(0);
        };
    }
}