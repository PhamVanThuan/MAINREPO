using Capitec.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Capitec.Commands;

namespace SAHL.Services.Capitec.CommandHandlers
{
    public class LoginCommandHandler : IServiceCommandHandler<LoginCommand>
    {
        private IAuthenticationManager authenticationManager;

        public LoginCommandHandler(IAuthenticationManager authenticationManager)
        {
            this.authenticationManager = authenticationManager;
        }

        public ISystemMessageCollection HandleCommand(LoginCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            this.authenticationManager.Login(command.Username, command.Password, "");

            return messages;
        }
    }
}