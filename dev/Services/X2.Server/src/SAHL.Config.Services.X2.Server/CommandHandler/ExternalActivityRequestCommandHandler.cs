using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2;

namespace SAHL.Config.Services.X2.Server.CommandHandler
{
    public class ExternalActivityRequestCommandHandler : IServiceCommandHandler<X2ExternalActivityRequest>
    {
        private IX2Engine engine;

        public ExternalActivityRequestCommandHandler(IX2Engine engine)
        {
            this.engine = engine;
        }

        public ISystemMessageCollection HandleCommand(X2ExternalActivityRequest command, IServiceRequestMetadata metadata)
        {
            var response = engine.ReceiveExternalActivityRequest(command);
            command.Result = response;
            return SystemMessageCollection.Empty();
        }
    }
}