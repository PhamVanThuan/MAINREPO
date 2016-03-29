using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Projections.ComcorpProgress.Model;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination;

namespace SAHL.Services.EventProjection.Projections.ComcorpProgress
{
    public partial class ComcorpProgressLiveReply : IServiceProjector<ApplicationNTUPendedLegacyEvent, ICommunicationsServiceClient>
    {
        public void Handle(ApplicationNTUPendedLegacyEvent @event, IServiceRequestMetadata metadata, ICommunicationsServiceClient service)
        {
            if (this.IsComcorpApplication(@event.ApplicationKey))
            {
                ComcorpApplicationInformationDataModel dataModel = this.GetComcorpApplication(@event.ApplicationKey);
                SendComcorpLiveReplyCommand liveReply = new SendComcorpLiveReplyCommand(
                    @event.Id,
                    @event.ApplicationKey.ToString(),
                    dataModel.ReservedAccountKey.ToString(),
                    dataModel.Reference,
                    "Cancelled/Withdrawn",//description  or free text
                    @event.Date,
                    9,
                    dataModel.OfferedAmount,
                    dataModel.RegisteredAmount,
                    dataModel.RequestedAmount
                );
                service.PerformCommand(liveReply, metadata);
            }
        }
    }
}