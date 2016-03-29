using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.DebtCounselling.UpdateHearingDetailStatusCommandHandlerSpecs
{
    [Subject(typeof(UpdateHearingDetailStatusToInactiveCommandHandler))]
    public class When_hearing_detail_does_not_exist : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static UpdateHearingDetailStatusToInactiveCommand command;
        protected static UpdateHearingDetailStatusToInactiveCommandHandler handler;
        protected static IDebtCounsellingRepository debtCounsellingRepository;
        protected static ILookupRepository lookupRepository;
        protected static IDebtCounselling debtCounselling;
        protected static IEventList<IHearingDetail> hearingDetails;
        protected static IHearingDetail hearingDetail;

        // Arrange
        Establish context = () =>
        {
            debtCounsellingRepository = An<IDebtCounsellingRepository>();
            debtCounselling = An<IDebtCounselling>();
            hearingDetails = null;

            debtCounsellingRepository.WhenToldTo(x => x.GetDebtCounsellingByKey(Param<int>.IsAnything)).Return(debtCounselling);

            debtCounselling.WhenToldTo(x => x.HearingDetails).Return(hearingDetails);

            command = new UpdateHearingDetailStatusToInactiveCommand(Param<int>.IsAnything);
            handler = new UpdateHearingDetailStatusToInactiveCommandHandler(debtCounsellingRepository, lookupRepository);
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
        It should_not_save_hearing_detail = () =>
        {
            debtCounsellingRepository.WasNotToldTo(x => x.SaveHearingDetail(hearingDetail));
        };
    }
}