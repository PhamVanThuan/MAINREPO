using System.Collections.Generic;
using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.DebtCounselling.UpdateDebtCounsellingStatusCommandHandlerSpecs
{
    [Subject(typeof(UpdateDebtCounsellingStatusCommandHandler))]
    public class When_debt_counselling_case_exists : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static UpdateDebtCounsellingStatusCommand command;
        protected static UpdateDebtCounsellingStatusCommandHandler handler;
        protected static IDebtCounsellingRepository debtCounsellingRepository;
        protected static ILookupRepository lookupRepository;
        protected static IDebtCounselling debtCounselling;
        protected static IDebtCounsellingStatus debtCounsellingStatus;

        // Arrange
        Establish context = () =>
        {
            debtCounsellingRepository = An<IDebtCounsellingRepository>();
            lookupRepository = An<ILookupRepository>();
            debtCounselling = An<IDebtCounselling>();
            debtCounsellingStatus = An<IDebtCounsellingStatus>();

            debtCounsellingRepository.WhenToldTo(x => x.GetDebtCounsellingByKey(Param<int>.IsAnything)).Return(debtCounselling);

            debtCounselling.WhenToldTo(x => x.DebtCounsellingStatus).Return(debtCounsellingStatus);

            IDictionary<DebtCounsellingStatuses, IDebtCounsellingStatus> debtcounsellingStatuses = new Dictionary<DebtCounsellingStatuses, IDebtCounsellingStatus>();
            debtcounsellingStatuses.Add(DebtCounsellingStatuses.Closed, debtCounsellingStatus);
            lookupRepository.WhenToldTo(x => x.DebtCounsellingStatuses).Return(debtcounsellingStatuses);

            command = new UpdateDebtCounsellingStatusCommand(Param<int>.IsAnything, (int)SAHL.Common.Globals.DebtCounsellingStatuses.Closed);
            handler = new UpdateDebtCounsellingStatusCommandHandler(debtCounsellingRepository, lookupRepository);
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
        It should_update_and_save_debtcounselling_record = () =>
        {
            debtCounsellingRepository.WasToldTo(x => x.SaveDebtCounselling(debtCounselling));
        };
    }
}