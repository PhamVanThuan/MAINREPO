using System.Collections.Generic;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class PublishBundledRequestCommand : ServiceCommand
    {
        public List<IX2BundledNotificationCommand> Commands { get; protected set; }

        public PublishBundledRequestCommand()
        {
            this.Commands = new List<IX2BundledNotificationCommand>();
        }

        public PublishBundledRequestCommand(List<IX2BundledNotificationCommand> commands)
        {
            this.Commands = commands;
        }
    }
}