using SAHL.Core.Services;
using SAHL.Core.X2.Providers;

namespace SAHL.X2Engine2.Commands
{
    public class SaveContextualDataCommand : ServiceCommand
    {
        public IX2ContextualDataProvider ContextualData { get; protected set; }

        public long InstanceId { get; protected set; }

        public SaveContextualDataCommand(IX2ContextualDataProvider contextualData, long instanceId)
        {
            this.ContextualData = contextualData;
            this.InstanceId = instanceId;
        }
    }
}