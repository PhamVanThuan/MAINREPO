using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.Core.X2.Messages.Management;
using SAHL.X2Engine2;

namespace SAHL.Config.Services.X2.Server.CommandHandler
{
    public class NodeManagementCommandHandler : IServiceCommandHandler<X2NodeManagementMessage>
    {
        IX2Engine engine;

        public NodeManagementCommandHandler(IX2Engine engine)
        {
            this.engine = engine;
        }

        public ISystemMessageCollection HandleCommand(X2NodeManagementMessage command, IServiceRequestMetadata metadata)
        {
            var response = engine.ReceiveManagementMessage(command);
            command.Result = response;
            return SystemMessageCollection.Empty();
        }
    }
}