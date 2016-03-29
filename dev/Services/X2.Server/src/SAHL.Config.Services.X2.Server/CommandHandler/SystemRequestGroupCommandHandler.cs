using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2;

namespace SAHL.Config.Services.X2.Server.CommandHandler
{
    public class SystemRequestGroupCommandHandler : IServiceCommandHandler<X2SystemRequestGroup>
    {
        IX2Engine engine;

        public SystemRequestGroupCommandHandler(IX2Engine engine)
        {
            this.engine = engine;
        }

        public ISystemMessageCollection HandleCommand(X2SystemRequestGroup command, IServiceRequestMetadata metadata)
        {
            var response = engine.ReceiveSystemRequest(command);
            command.Result = response;
            return SystemMessageCollection.Empty();
        }
    }
}