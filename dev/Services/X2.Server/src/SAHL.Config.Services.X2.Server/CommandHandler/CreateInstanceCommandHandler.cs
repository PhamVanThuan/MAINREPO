using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2;

namespace SAHL.Config.Services.X2.Server.CommandHandler
{
    public class CreateInstanceCommandHandler : IServiceCommandHandler<X2CreateInstanceRequest>
    {
        IX2Engine engine;

        public CreateInstanceCommandHandler(IX2Engine engine)
        {
            this.engine = engine;
        }

        public ISystemMessageCollection HandleCommand(X2CreateInstanceRequest command, IServiceRequestMetadata metadata)
        {
            var response = engine.ReceiveRequest(command);
            command.Result = response;
            return SystemMessageCollection.Empty();
        }
    }
}