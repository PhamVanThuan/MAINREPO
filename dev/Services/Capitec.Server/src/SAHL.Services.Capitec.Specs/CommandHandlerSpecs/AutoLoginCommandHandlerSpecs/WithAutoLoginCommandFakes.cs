using Capitec.Core.Identity;
using Machine.Fakes;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.CommandHandlers;
using SAHL.Services.Capitec.Managers.Security;

using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.AutoLoginCommandHandlerSpecs
{
    public class WithAutoLoginCommandFakes : WithFakes
    {
        protected static AutoLoginCommand command;
        protected static AutoLoginCommandHandler commandHandler;
        protected static ISecurityDataManager securityDataServices;
        protected static IAuthenticationManager authenticationManager;
        protected static IUserDataManager userDataManager;
        protected static ISecurityManager securityManager;
        protected static IPasswordManager passwordManager;
        protected static ISystemMessageCollection messages;
    }
}
