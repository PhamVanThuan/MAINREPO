using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2;

namespace SAHL.Config.Services.X2.Server.CommandHandler
{
    public class RebuildInstanceCommandHandler : IServiceCommandHandler<X2RequestForSecurityRecalc>
    {
        private IX2Engine engine;

        public RebuildInstanceCommandHandler(IX2Engine engine)
        {
            this.engine = engine;
        }

        public ISystemMessageCollection HandleCommand(X2RequestForSecurityRecalc command, IServiceRequestMetadata metadata)
        {
            var response = engine.ReceiveRequest(command);
            command.Result = response;
            return SystemMessageCollection.Empty();
        }
    }
}