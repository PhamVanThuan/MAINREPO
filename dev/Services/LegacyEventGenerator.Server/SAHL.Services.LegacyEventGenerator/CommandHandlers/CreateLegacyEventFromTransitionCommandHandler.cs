using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Commands;

namespace SAHL.Services.LegacyEventGenerator.CommandHandlers
{
    public class CreateLegacyEventFromTransitionCommandHandler : IServiceCommandHandler<CreateLegacyEventFromTransitionCommand>
    {
        public CreateLegacyEventFromTransitionCommandHandler()
        {
        }

        public ISystemMessageCollection HandleCommand(CreateLegacyEventFromTransitionCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            return messages;
        }
    }
}