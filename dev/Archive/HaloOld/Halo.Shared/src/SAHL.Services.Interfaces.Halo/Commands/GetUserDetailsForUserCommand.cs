using SAHL.Core.Services;
using SAHL.Core.UI.UserState.Models;
using System;

namespace SAHL.Services.Interfaces.Halo.Commands
{
    public class GetUserDetailsForUserCommand : ServiceCommand, IServiceCommandWithReturnedData<IUserDetails>, IHaloServiceCommand
    {
        public GetUserDetailsForUserCommand(string userName)
        {
            this.UserName = userName;
        }

        public string UserName { get; protected set; }

        public IUserDetails Result { get; set; }
    }
}