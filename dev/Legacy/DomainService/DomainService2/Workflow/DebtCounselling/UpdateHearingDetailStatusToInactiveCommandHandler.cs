using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.DebtCounselling
{
    public class UpdateHearingDetailStatusToInactiveCommandHandler : IHandlesDomainServiceCommand<UpdateHearingDetailStatusToInactiveCommand>
    {
        private IDebtCounsellingRepository DebtcounsellingRepository;
        private ILookupRepository LookupRepository;

        public UpdateHearingDetailStatusToInactiveCommandHandler(IDebtCounsellingRepository debtcounsellingRepository, ILookupRepository lookupRepository)
        {
            this.DebtcounsellingRepository = debtcounsellingRepository;
            this.LookupRepository = lookupRepository;
        }

        public void Handle(IDomainMessageCollection messages, UpdateHearingDetailStatusToInactiveCommand command)
        {
            bool success = false;

            IDebtCounselling debtCounselling = DebtcounsellingRepository.GetDebtCounsellingByKey(command.DebtCounsellingKey);

            if (debtCounselling != null)
            {
                if (debtCounselling.HearingDetails != null && debtCounselling.HearingDetails.Count > 0)
                {
                    foreach (IHearingDetail hearingDetail in debtCounselling.HearingDetails)
                    {
                        if (hearingDetail.GeneralStatus.Key == (int)GeneralStatuses.Active)
                        {
                            hearingDetail.GeneralStatus = LookupRepository.GeneralStatuses[GeneralStatuses.Inactive];
                            DebtcounsellingRepository.SaveHearingDetail(hearingDetail);
                        }
                    }
                }

                success = true;
            }

            command.Result = success;
        }
    }
}