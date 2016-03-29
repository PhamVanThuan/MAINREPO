using DomainService2.Workflow.Cap2;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Cap2;

namespace DomainService2.Hosts.Cap2
{
    public class Cap2Host : HostBase, ICap2
    {
        public Cap2Host(ICommandHandler commandHandler)
            : base(commandHandler)
        {
        }

        public bool CheckReadvanceDoneRules(IDomainMessageCollection messages, bool ignoreWarnings, int applicationKey)
        {
            CheckReadvanceDoneRulesCommand command = new CheckReadvanceDoneRulesCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand<CheckReadvanceDoneRulesCommand>(messages, command);
            return command.Result;
        }

        public void UpdateCapOfferStatus(IDomainMessageCollection messages, int applicationKey, int statusKey)
        {
            UpdateCapOfferStatusCommand command = new UpdateCapOfferStatusCommand(applicationKey, statusKey);
            this.CommandHandler.HandleCommand<UpdateCapOfferStatusCommand>(messages, command);
        }

        public bool IsLANotRequired(IDomainMessageCollection messages, int applicationKey)
        {
            IsLANotRequiredCommand command = new IsLANotRequiredCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool IsCreditCheckRequired(IDomainMessageCollection messages, int applicationKey)
        {
            IsCreditCheckRequiredCommand command = new IsCreditCheckRequiredCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }
    }
}