using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.DebtCounselling.DebtCounsellingCheckCourtDetailsAndCourtOrderReason
{
    public class DebtCounsellingCheckCourtDetailsAndCourtOrderReasonBase : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingCheckCourtDetailsAndCourtOrderReason>
    {
        protected static IReasonRepository reasonRepository;

        public static void SetUpDebtCounsellingCase(HearingAppearanceTypes hearingAppearanceType, HearingTypes hearingType, List<IReason> reasons)
        {
            //Set up reasons
            reasonRepository = An<IReasonRepository>();

            IReadOnlyEventList<IReason> readonlyReasons= new ReadOnlyEventList<IReason>(reasons);

            reasonRepository.WhenToldTo(x => x.GetReasonByGenericKeyAndReasonDefinitionKey(Param.IsAny<int>(), Param.IsAny<ReasonDefinitions>())).Return(readonlyReasons);

            businessRule = new BusinessModel.Rules.DebtCounselling.DebtCounsellingCheckCourtDetailsAndCourtOrderReason(reasonRepository);

            GeneralStatuses generalStatus = GeneralStatuses.Active;

            //Set up Debt counselling case
            IDebtCounselling debtCounsellingCase = An<IDebtCounselling>();
            IGeneralStatus hearingDetailGeneralStatus = An<IGeneralStatus>();
            IEventList<IHearingDetail> hearingDetails = null;

            IHearingDetail debtCounsellingHearingDetail = An<IHearingDetail>();
            IHearingType debtCounsellingHearingType = An<IHearingType>();
            IHearingAppearanceType debtCounsellingHearingAppearanceType = An<IHearingAppearanceType>();

            hearingDetails = new EventList<IHearingDetail>(new List<IHearingDetail> { debtCounsellingHearingDetail });

            hearingDetailGeneralStatus.WhenToldTo(x => x.Key).Return((int)generalStatus);
            debtCounsellingHearingType.WhenToldTo(x => x.Key).Return((int)hearingType);
            debtCounsellingHearingDetail.WhenToldTo(x => x.GeneralStatus).Return(hearingDetailGeneralStatus);
            debtCounsellingHearingDetail.WhenToldTo(x => x.HearingType).Return(debtCounsellingHearingType);
            debtCounsellingCase.WhenToldTo(x => x.HearingDetails).Return(hearingDetails);
            debtCounsellingHearingDetail.WhenToldTo(x => x.HearingAppearanceType).Return(debtCounsellingHearingAppearanceType);
            debtCounsellingHearingAppearanceType.WhenToldTo(x => x.Key).Return((int)hearingAppearanceType);

            parameters = new object[] { debtCounsellingCase };
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingCheckCourtDetailsAndCourtOrderReason>.startrule.Invoke();
        }

    }
}
