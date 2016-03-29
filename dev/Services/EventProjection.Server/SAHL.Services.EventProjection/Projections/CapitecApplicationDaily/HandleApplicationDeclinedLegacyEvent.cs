using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination;
using System;

namespace SAHL.Services.EventProjection.Projections.CapitecApplicationDaily
{
    public partial class CapitecApplicationDailyReport : ITableProjector<ApplicationDeclinedLegacyEvent, SAHL.Core.Data.Models.Capitec.ApplicationDataModel>
    {
        public void Handle(ApplicationDeclinedLegacyEvent @event, IServiceRequestMetadata metadata)
        {
            this.UpdateApplicationStatus(@event.ApplicationKey, Guid.Parse(ApplicationStatusEnumDataModel.DECLINE), @event.Date);
        }
    }
}