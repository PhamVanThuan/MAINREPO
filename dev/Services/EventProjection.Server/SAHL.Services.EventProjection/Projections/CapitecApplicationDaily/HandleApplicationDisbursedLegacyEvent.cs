using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.ApplicationProgress;
using System;

namespace SAHL.Services.EventProjection.Projections.CapitecApplicationDaily
{
    public partial class CapitecApplicationDailyReport : ITableProjector<AppProgressDisbursedLegacyEvent, SAHL.Core.Data.Models.Capitec.ApplicationDataModel>
    {
        public void Handle(AppProgressDisbursedLegacyEvent @event, IServiceRequestMetadata metadata)
        {
            this.Update(@event.ApplicationKey, Guid.Parse(ApplicationStatusEnumDataModel.IN_PROGRESS), Guid.Parse(ApplicationStageTypeEnumDataModel.LOAN_REGISTERED), 
                @event.CommissionableADUser, @event.Date);
        }
    }
}