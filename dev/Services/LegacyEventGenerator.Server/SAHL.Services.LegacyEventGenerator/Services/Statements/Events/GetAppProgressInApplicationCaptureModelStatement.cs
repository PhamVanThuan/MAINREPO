using SAHL.Core.Data;
using SAHL.Services.LegacyEventGenerator.Services.Models.Events;

namespace SAHL.Services.LegacyEventGenerator.Services.Statements.Events
{
    public class GetAppProgressInApplicationCaptureModelStatement : ISqlStatement<AppProgressInApplicationCaptureModel>
    {
        public int StageTransitionCompositeKey { get; protected set; }

        public GetAppProgressInApplicationCaptureModelStatement(int stageTransitionCompositeKey)
        {
            this.StageTransitionCompositeKey = stageTransitionCompositeKey;
        }

        // TODO PUT THIS BACK WHEN MARCHUAN HAS GOT A ATTRIBUTE TO BYPASS THE CONVENTION
        // (nolock)
        public string GetStatement()
        {
            return @"select oi.OfferInformationKey, wl.ADUserName AssignedADUser, ad.ADUserName CommissionableADUser
                    from [2am].[dbo].[StageTransitionComposite] stc
                    join [2am].[dbo].[OfferInformation] oi on oi.OfferKey = stc.[GenericKey]
                    join (select max(oi.[OfferInformationKey]) OfferInformationKey
                            from [2am].[dbo].[OfferInformation] oi
                            join [2am].[dbo].[StageTransitionComposite] stc on stc.[GenericKey] = oi.OfferKey
                            where stc.StageTransitionCompositeKey = @StageTransitionCompositeKey
                            and oi.[OfferInsertDate] <= stc.[TransitionDate]) latest_oi on latest_oi.OfferInformationKey = oi.OfferInformationKey
                    left join [2am].[dbo].[OfferRole] ofr on ofr.[OfferKey] = oi.OfferKey
                        and ofr.[OfferRoleTypeKey] = 100
                    left join [2am].[dbo].[ADUser] ad on ad.[LegalEntityKey] = ofr.[LegalEntityKey]
                    left join [x2].[X2DATA].[Application_Capture] ac on ac.[ApplicationKey] = ofr.OfferKey
                    left join [x2].[x2].Instance i on i.id = ac.InstanceID
                        and i.ParentInstanceID is null
                    left join [X2].[X2].[WorkList] wl on wl.[InstanceID] = ac.[InstanceID]
                    where stc.StageTransitionCompositeKey = @StageTransitionCompositeKey";
        }
    }
}