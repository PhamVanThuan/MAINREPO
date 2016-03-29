using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.EventPublisher.Commands
{
    public class PublishEventCommand : ServiceCommand
    {
        public PublishEventCommand(int eventKey)
        {
            this.EventKey = eventKey;
        }

        public int EventKey { get; protected set; }
    }
}