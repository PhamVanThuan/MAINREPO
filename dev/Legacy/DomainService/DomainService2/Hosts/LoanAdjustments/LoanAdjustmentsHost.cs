using DomainService2.Workflow.LoanAdjustments;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.LoanAdjustments;

namespace DomainService2.Hosts.LoanAdjustments
{
    public class LoanAdjustmentsHost : HostBase, ILoanAdjustments
    {
        public LoanAdjustmentsHost(ICommandHandler commandHandler)
            : base(commandHandler)
        {
        }

        public bool ApproveTermChangeRequest(IDomainMessageCollection messages, int accountKey, long instanceID, bool ignoreWarnings)
        {
            ApproveTermChangeRequestCommand command = new ApproveTermChangeRequestCommand(accountKey, instanceID, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckIfCanApproveTermChangeRules(IDomainMessageCollection messages, int accountKey, long instanceID, bool ignoreWarnings)
        {
            CheckIfCanApproveTermChangeRulesCommand command = new CheckIfCanApproveTermChangeRulesCommand(accountKey, instanceID, ignoreWarnings);
            base.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }
    }
}