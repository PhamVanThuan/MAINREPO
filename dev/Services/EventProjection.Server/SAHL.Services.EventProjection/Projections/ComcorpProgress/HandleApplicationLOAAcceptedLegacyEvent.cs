using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Projections.ComcorpProgress.Model;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.ApplicationProgress;

namespace SAHL.Services.EventProjection.Projections.ComcorpProgress
{
    public partial class ComcorpProgressLiveReply : IServiceProjector<AppProgressLOAAcceptedLegacyEvent, ICommunicationsServiceClient>
    {
        public void Handle(AppProgressLOAAcceptedLegacyEvent @event, IServiceRequestMetadata metadata, ICommunicationsServiceClient service)
        {
            if (this.IsComcorpApplication(@event.ApplicationKey))
            {
                ComcorpApplicationInformationDataModel dataModel = this.GetComcorpApplication(@event.ApplicationKey);
                SendComcorpLiveReplyCommand liveReply_accept = new SendComcorpLiveReplyCommand(
                    @event.Id,
                    @event.ApplicationKey.ToString(),
                    dataModel.ReservedAccountKey.ToString(),
                    dataModel.Reference,
                    "Accept Grant",//description  or free text
                    @event.Date,
                    18,
                    dataModel.OfferedAmount,
                    dataModel.RegisteredAmount,
                    dataModel.RequestedAmount
                );
                service.PerformCommand(liveReply_accept, metadata);
            }
        }
    }
}