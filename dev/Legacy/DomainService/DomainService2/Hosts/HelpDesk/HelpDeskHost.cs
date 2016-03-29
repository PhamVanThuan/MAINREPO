using DomainService2.Workflow.HelpDesk;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.HelpDesk;

namespace DomainService2.Hosts.HelpDesk
{
    public class HelpDeskHost : HostBase, IHelpDesk
    {
        public HelpDeskHost(ICommandHandler commandHandler)
            : base(commandHandler)
        {
        }

        public bool X2AutoArchive2AM_Update(IDomainMessageCollection messages, int helpDeskQueryKey)
        {
            var command = new X2AutoArchive2AM_UpdateCommand(helpDeskQueryKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public string CreateRequest(IDomainMessageCollection messages, int legalEntityKey)
        {
            var command = new CreateRequestCommand(legalEntityKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }
    }
}