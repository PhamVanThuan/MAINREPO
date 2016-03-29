using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.DebtCounselling.UpdateHearingDetailStatusCommandHandlerSpecs
{
    [Subject(typeof(UpdateHearingDetailStatusToInactiveCommandHandler))]
    public class When_inactive_hearing_detail_case_exists : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static UpdateHearingDetailStatusToInactiveCommand command;
        protected static UpdateHearingDetailStatusToInactiveCommandHandler handler;
        protected static IDebtCounsellingRepository debtCounsellingRepository;
        protected static ILookupRepository lookupRepository;
        protected static IDebtCounselling debtCounselling;
        protected static IHearingDetail hearingDetail;
        protected static IGeneralStatus generalStatus;

        // Arrange
        Establish context = () =>
        {
            IEventList<IHearingDetail> hearingDetails = new EventList<IHearingDetail>();

            debtCounsellingRepository = An<IDebtCounsellingRepository>();
            lookupRepository = An<ILookupRepository>();
            debtCounselling = An<IDebtCounselling>();
            hearingDetail = An<IHearingDetail>();
            generalStatus = An<IGeneralStatus>();

            generalStatus.WhenToldTo(x => x.Key).Return((int)GeneralStatuses.Inactive);
            hearingDetail.WhenToldTo(x => x.GeneralStatus).Return(generalStatus);

            hearingDetails.Add(messages, hearingDetail);

            debtCounselling.WhenToldTo(x => x.HearingDetails).Return(hearingDetails);

            debtCounsellingRepository.WhenToldTo(x => x.GetDebtCounsellingByKey(Param<int>.IsAnything)).Return(debtCounselling);

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