using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class SaveContextualDataCommandHandler : IServiceCommandHandler<SaveContextualDataCommand>
    {
        public ISystemMessageCollection HandleCommand(SaveContextualDataCommand command, IServiceRequestMetadata metadata)
        {
            // TODO: Get the map to load its statement provider.
            command.ContextualData.SaveData(command.InstanceId);
            return new SystemMessageCollection();
        }
    }
}