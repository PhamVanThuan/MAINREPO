using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class CreateContextualDataCommandHandler : IServiceCommandHandler<CreateContextualDataCommand>
    {
        public ISystemMessageCollection HandleCommand(CreateContextualDataCommand command, IServiceRequestMetadata metadata)
        {
            // TODO: Get the map to load its statement provider.
            command.ContextualData.InsertData(command.Instance.ID, command.MapVariables);
            return new SystemMessageCollection();
        }
    }
}