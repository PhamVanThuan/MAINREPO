using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.X2;
using SAHL.Core.X2.Providers;

namespace SAHL.X2Engine2.Commands
{
    public class RefreshInstanceActivitySecurityCommand : ServiceCommand
    {
        public InstanceDataModel Instance { get; protected set; }

        public IX2ContextualDataProvider ContextualDataProvider { get; protected set; }

        public IX2Map Map { get; protected set; }

        public RefreshInstanceActivitySecurityCommand(InstanceDataModel instance, IX2ContextualDataProvider contextualDataProvider, IX2Map map)
        {
            this.Instance = instance;
            this.ContextualDataProvider = contextualDataProvider;
            this.Map = map;
        }
    }
}