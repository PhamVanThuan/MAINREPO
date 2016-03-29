using Capitec.Core.Identity;
using System.Security.Principal;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Commands;
using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.CommandHandlers
{
    public class ChangePasswordCommandHandler : IServiceCommandHandler<ChangePasswordCommand>
    {
        private IUserManager userManager;
        private IHostContext hostContext;

        public ChangePasswordCommandHandler(IUserManager userManager, IHostContext hostContext)
        {
            this.userManager = userManager;
            this.hostContext = hostContext;
        }

        public ISystemMessageCollection HandleCommand(ChangePasswordCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            var user = this.hostContext.GetUser();
            CapitecIdentity capitecIdentity = user.Identity as CapitecIdentity;
            if (capitecIdentity != null)
            {
                if (capitecIdentity.UserId == Guid.Empty)
                    messages.AddMessage(new SystemMessage("Bad Guid", SystemMessageSeverityEnum.Error));
                else if (string.IsNullOrEmpty(command.Password))
                    messages.AddMessage(new SystemMessage("Bad Password", SystemMessageSeverityEnum.Error));
                else
                    this.userManager.ChangePassword(capitecIdentity.UserId, command.Password);
            }
            return messages;
        }
    }
}